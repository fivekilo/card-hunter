using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{
    public SharedData shareddata;
    public GameObject camp;
    public GameObject WorldMap;
    public GameObject Player;
    public GameObject EventWin;
    public GameObject AddCardWin;
    public GameObject DeckBtn;
    public GameObject DeckWin;
    public GameObject Camera;
    public GameObject Commissionboard;
    public GameObject StartMenu;
    public GameObject InCamp;
    public GameObject ExitBtn;
    public GameObject Shop;
    public event Action<Choice> Chosed;
    public event Action<int> AddCard;
    private List<bool> AbleToMove=new List<bool> {false,false };
    private PlayerInfo playerinfo=new PlayerInfo();
    private RogueMod RM;
    private GameObject DW;//卡组窗口
    private GameObject CB;//委托栏
    private GameObject SM;//开始菜单
    private GameObject Camp;//营地菜单
    private GameObject SP;//商店界面
    private List<Commission> AcceptedCommission;
    private event Func<Commission,IEnumerator> ArriveBattleField;
    private event Action ArriveCamp;
    private int PlayerProgress = 0;//玩家进度,标识行程
    private List<column> CardsInShop = new List<column>();
    private void ShowStartMenu()//显示起始界面
    {
        //移开摄像头
        Camera.transform.position = GameConfig.CameraNew;

        SM=Instantiate(StartMenu,Vector3.zero,Quaternion.identity);
        SM.transform.Find("Background/Start").GetComponent<ConfirmBtn>().Confirm += GameStart;
        SM.transform.Find("Background/Exit").GetComponent<ConfirmBtn>().Confirm += Exit;
    }
    private void EnterCamp()
    {
        Camp = Instantiate(InCamp, Vector3.zero, Quaternion.identity);
    }
    private void GameStart()//游戏启动
    {
        Destroy(SM);
        shareddata.playerinfo.MaxHealth = GameConfig.InitialHealth;
        shareddata.playerinfo.curHealth = GameConfig.InitialHealth;
        shareddata.playerinfo.MaxCost = GameConfig.InitialCost;
        shareddata.playerinfo.curCost = GameConfig.InitialCost;
        shareddata.playerinfo.Direction = new(1, 0);
        Camp=Instantiate(InCamp,Vector3.zero,Quaternion.identity);
        Camp.transform.Find("Commission").GetComponent<ConfirmBtn>().Confirm += GetCommission;
        Camp.transform.Find("Shop").GetComponent <ConfirmBtn>().Confirm += OpenCardShop;
        Camp.transform.Find("Forge").GetComponent<ConfirmBtn>().Confirm += OpenEquipShop;
    }
    private void Save()//存档
    {

    }
    private void Exit()//退出游戏
    {
        Application.Quit();
    }
    private void GetCommission()
    {
        //移回镜头
        Camera.transform.position = GameConfig.CameraDefault;
        Destroy(Camp);

        List<Commission> commissions = GameConfig.Commissions;
        List<Commission> selected = RM.ChooseCommission(commissions, 1);
        CB = Instantiate(Commissionboard,Vector3.zero,Quaternion.identity);
        CB.GetComponent<CommissionBoard>().Init(selected);
        //摄像头移动防止误触背景
        Camera.transform.position = GameConfig.CameraNew;
        //绑定关闭窗口按钮
        CB.GetComponent<CommissionBoard>().Confirm += AcceptCommission;
    }
    private void AcceptCommission(Commission commission)//已接取委托（生成随机事件,完成去程,处理去程中的随机事件）
    {
        //销毁委托栏 移回摄像头 解绑触发事件
        Destroy(CB);
        Camera.transform.position = GameConfig.CameraDefault;
        camp.GetComponent<Camp>().ClickEvent -= GetCommission;


        //去程：获取随机事件
        List<Event> events = RM.GetEvents(PlayerProgress);
        //生成路径
        List<Vector3> points= WorldMap.GetComponent<RouteRender>().plotRoute(commission.place);
        //让玩家沿路径移动
        StartCoroutine(PlayerGo(points,events,commission));
        PlayerProgress++;
    }
    private void BackToCamp()
    {
        //点击后解绑,防止多次返回营地
        camp.GetComponent<Camp>().ClickEvent -= BackToCamp;

        //返程：获取随机事件
        List<Event> events = RM.GetEvents(PlayerProgress);
        //生成路径
        List<Vector3> points = WorldMap.GetComponent<RouteRender>().plotRoute(0);
        //让玩家沿路径移动
        StartCoroutine(PlayerGo(points, events, null));
        PlayerProgress++;
    }
    private IEnumerator PlayerGo(List<Vector3> points,List<Event> events,Commission commission)//传入行程目标的委托,使玩家完成一个行程,并在行程中处理给定的events 若委托传入为null,为返回营地
    {
        PlayerMove PM = Player.GetComponent<PlayerMove>();
        //将事件与路段随机匹配
        System.Random rand = new System.Random();
        Dictionary<int,Event> Road_Event= new Dictionary<int,Event>();
        foreach(Event e in events)
        {
            int ri;
            do
            {
                ri = rand.Next(points.Count - 1);
            }
            while (Road_Event.ContainsKey(ri));
            Road_Event[ri] = e;
        }

        for(int i = 0; i < points.Count; i++)
        {
            Event e=null;
            if (Road_Event.ContainsKey(i - 1))
            {
                e= Road_Event[i-1];
            }
            else
            {
                e= null;
            }
            yield return PM.MoveTo(points[i], e);
        }
        if (commission != null)
        {
            StartCoroutine(ArriveBattleField?.Invoke(commission));
        }
        else
        {
            ArriveCamp?.Invoke();
        }

    }
    private IEnumerator EventChoose(Event e)//展示随机事件窗口
    {
        GameObject EW = Instantiate(EventWin, Vector3.zero, Quaternion.identity);
        yield return StartCoroutine(EW.GetComponent<EventData>().EventInit(e, Chosed));
        yield return new WaitUntil(() =>
        {
            bool res=true;
            foreach(bool b in AbleToMove)
            {
                res = res && b;
            }
            return res;
        });//等待AbleToMove(加卡或删卡窗口)
        for(int i = 0; i < AbleToMove.Count; i++)//回调AbleToMove
        {
            AbleToMove[i] = false;
        }
    }
    private void HideScene()
    {
        Scene scene = SceneManager.GetSceneByName("WorldMap");
        foreach(GameObject G in scene.GetRootGameObjects())
        {
            if (G != this.gameObject)
            {
                G.SetActive(false);
            }
        }
        transform.Find("WorldMap").gameObject.SetActive(false);
    }
    private void ShowScene()
    {
        Scene scene = SceneManager.GetSceneByName("WorldMap");
        foreach (GameObject G in scene.GetRootGameObjects())
        {
            if (G != this.gameObject)
            {
                G.SetActive(true);
            }
            transform.Find("WorldMap").gameObject.SetActive(true);
        }
    }
    private void AddToDeck(int CardNum)
    {
        shareddata.playerinfo.deck.Add(CardNum);
        AbleToMove[0] = true;
    }
    private void DeleteFromDeck(int CardNum)
    {
        Destroy(DW);
        //摄像机切回原本位置
        Camera.transform.position = GameConfig.CameraDefault;
        shareddata.playerinfo.deck.Remove(CardNum);
        AbleToMove[1] = true;
    }
    private void ChoiceHandle(Choice choice)
    {
        if (choice.modifydeck < 0)
        {
            DeleteCard();
        }
        else//无需删牌
        {
            AbleToMove[1] = true;
        }
        if (choice.modifydeck > 0)
        {
            //加牌函数
            GameObject AW = Instantiate(AddCardWin, Vector3.zero, Quaternion.identity);
            AW.GetComponent<AddCardWindow>().AddCard(AddCard,choice.CardsID);//传入可添加的卡牌范围
        }
        else//无需加牌
        {
            AbleToMove[0] = true;
        }
        if (choice.money != 0)
        {
            shareddata.playerinfo.money += choice.money;
        }
        if (choice.health != 0)
        {
            shareddata.playerinfo.curHealth += choice.health;
        }
        if (choice.CardsID.Count > 0)
        {
            //添牌函数
        }
        if (choice.equipment > 0)
        {
            //添加装备
        }
        
    }
    private IEnumerator BattleEnter(Commission c)//进入战斗
    {
        shareddata.commission = c;
        Debug.Log("进行了与" + c.monster + "的战斗");
        HideScene();
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
        yield return new WaitUntil(()=>shareddata.Complete);//调用战斗,返回战斗结果
        SceneManager.UnloadSceneAsync("SampleScene");
        ShowScene();

        //战斗结束，返回营地。返回函数与营地绑定
        camp.GetComponent<Camp>().ClickEvent+=BackToCamp;
    }
    private void CampEnter()
    {
        Debug.Log("回到营地了");
        camp.GetComponent<Camp>().ClickEvent += GetCommission;
    }
    private void CheckDeck()//查看卡组
    {
        //玩家停止移动
        Player.GetComponent<PlayerMove>().Stop = true;
        DW = Instantiate(DeckWin, Vector3.zero, Quaternion.identity);
        DW.GetComponent<DeckWin>().Show(shareddata.playerinfo.deck);
        //摄像头移动防止误触背景
        Camera.transform.position = GameConfig.CameraNew;
        //绑定关闭窗口按钮
        DW.GetComponent<Transform>().Find("Btn").GetComponent<ConfirmBtn>().Confirm += CloseDeck;
    }
    private void CloseDeck()//关闭卡组窗口
    {
        Destroy(DW);
        //摄像机切回原本位置
        Camera.transform.position = GameConfig.CameraDefault;
        //玩家继续移动
        Player.GetComponent<PlayerMove>().Stop = false;
    }
    private void DeleteCard()
    {
        DW = Instantiate(DeckWin, Vector3.zero, Quaternion.identity);
        DW.GetComponent<DeckWin>().ShowDelete(shareddata.playerinfo.deck);
        //绑定删卡事件
        DW.GetComponent<DeckWin>().DeleteConfirm += DeleteFromDeck;
    }

    private void OpenEquipShop()
    {
        SP = Instantiate(Shop, Vector3.zero, Quaternion.identity);
        SP.GetComponent<Shop>().Init(GameConfig.EquipmentsCol);
        SP.GetComponent<Shop>().Exit += ExitShop;
        SP.GetComponent<Shop>().Purchase += PurchaseHandle;
    }
    private void OpenCardShop()
    {
        SP=Instantiate(Shop, Vector3.zero, Quaternion.identity);
        SP.GetComponent<Shop>().Init(CardsInShop);
        SP.GetComponent<Shop>().Exit += ExitShop;
        SP.GetComponent <Shop>().Purchase += PurchaseHandle;
    }
    private void PurchaseHandle(int id,bool IsCard)
    {
        if(IsCard)
        {
            AddToDeck(id);
        }
        else
        {
            shareddata.playerinfo.Equipments.Add(id);
        }
    }
    private void ExitShop()
    {
        Destroy(SP);
    }

    void Start()
    {
        //初始化Rogue Mod
        RM = new RogueMod();
        RM.ArrangeEvent(GameConfig.Events,GameConfig.EventAmountBounds);

        //初始化SharedData
        shareddata.playerinfo = playerinfo;
        shareddata.Complete = false;
        camp.GetComponent<Camp>().ClickEvent += GetCommission;
        Player.GetComponent<PlayerMove>().encounterEvent += EventChoose;
        ArriveBattleField += BattleEnter;
        ArriveCamp += CampEnter;
        Chosed += ChoiceHandle;
        AddCard += AddToDeck;
        DeckBtn.GetComponent<Btn>().Clicked += CheckDeck;
        ExitBtn.GetComponent<Btn>().Clicked += Exit;
        
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
