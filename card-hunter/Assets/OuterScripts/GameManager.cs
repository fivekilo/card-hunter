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
    private GameObject DW;//���鴰��
    private GameObject CB;//ί����
    private GameObject SM;//��ʼ�˵�
    private GameObject Camp;//Ӫ�ز˵�
    private GameObject SP;//�̵����
    private List<Commission> AcceptedCommission;
    private event Func<Commission,IEnumerator> ArriveBattleField;
    private event Action ArriveCamp;
    private int PlayerProgress = 0;//��ҽ���,��ʶ�г�
    private List<column> CardsInShop = new List<column>();
    private void ShowStartMenu()//��ʾ��ʼ����
    {
        //�ƿ�����ͷ
        Camera.transform.position = GameConfig.CameraNew;

        SM=Instantiate(StartMenu,Vector3.zero,Quaternion.identity);
        SM.transform.Find("Background/Start").GetComponent<ConfirmBtn>().Confirm += GameStart;
        SM.transform.Find("Background/Exit").GetComponent<ConfirmBtn>().Confirm += Exit;
    }
    private void EnterCamp()
    {
        Camp = Instantiate(InCamp, Vector3.zero, Quaternion.identity);
    }
    private void GameStart()//��Ϸ����
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
    private void Save()//�浵
    {

    }
    private void Exit()//�˳���Ϸ
    {
        Application.Quit();
    }
    private void GetCommission()
    {
        //�ƻؾ�ͷ
        Camera.transform.position = GameConfig.CameraDefault;
        Destroy(Camp);

        List<Commission> commissions = GameConfig.Commissions;
        List<Commission> selected = RM.ChooseCommission(commissions, 1);
        CB = Instantiate(Commissionboard,Vector3.zero,Quaternion.identity);
        CB.GetComponent<CommissionBoard>().Init(selected);
        //����ͷ�ƶ���ֹ�󴥱���
        Camera.transform.position = GameConfig.CameraNew;
        //�󶨹رմ��ڰ�ť
        CB.GetComponent<CommissionBoard>().Confirm += AcceptCommission;
    }
    private void AcceptCommission(Commission commission)//�ѽ�ȡί�У���������¼�,���ȥ��,����ȥ���е�����¼���
    {
        //����ί���� �ƻ�����ͷ ��󴥷��¼�
        Destroy(CB);
        Camera.transform.position = GameConfig.CameraDefault;
        camp.GetComponent<Camp>().ClickEvent -= GetCommission;


        //ȥ�̣���ȡ����¼�
        List<Event> events = RM.GetEvents(PlayerProgress);
        //����·��
        List<Vector3> points= WorldMap.GetComponent<RouteRender>().plotRoute(commission.place);
        //�������·���ƶ�
        StartCoroutine(PlayerGo(points,events,commission));
        PlayerProgress++;
    }
    private void BackToCamp()
    {
        //�������,��ֹ��η���Ӫ��
        camp.GetComponent<Camp>().ClickEvent -= BackToCamp;

        //���̣���ȡ����¼�
        List<Event> events = RM.GetEvents(PlayerProgress);
        //����·��
        List<Vector3> points = WorldMap.GetComponent<RouteRender>().plotRoute(0);
        //�������·���ƶ�
        StartCoroutine(PlayerGo(points, events, null));
        PlayerProgress++;
    }
    private IEnumerator PlayerGo(List<Vector3> points,List<Event> events,Commission commission)//�����г�Ŀ���ί��,ʹ������һ���г�,�����г��д��������events ��ί�д���Ϊnull,Ϊ����Ӫ��
    {
        PlayerMove PM = Player.GetComponent<PlayerMove>();
        //���¼���·�����ƥ��
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
    private IEnumerator EventChoose(Event e)//չʾ����¼�����
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
        });//�ȴ�AbleToMove(�ӿ���ɾ������)
        for(int i = 0; i < AbleToMove.Count; i++)//�ص�AbleToMove
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
        //������л�ԭ��λ��
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
        else//����ɾ��
        {
            AbleToMove[1] = true;
        }
        if (choice.modifydeck > 0)
        {
            //���ƺ���
            GameObject AW = Instantiate(AddCardWin, Vector3.zero, Quaternion.identity);
            AW.GetComponent<AddCardWindow>().AddCard(AddCard,choice.CardsID);//�������ӵĿ��Ʒ�Χ
        }
        else//�������
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
            //���ƺ���
        }
        if (choice.equipment > 0)
        {
            //���װ��
        }
        
    }
    private IEnumerator BattleEnter(Commission c)//����ս��
    {
        shareddata.commission = c;
        Debug.Log("��������" + c.monster + "��ս��");
        HideScene();
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
        yield return new WaitUntil(()=>shareddata.Complete);//����ս��,����ս�����
        SceneManager.UnloadSceneAsync("SampleScene");
        ShowScene();

        //ս������������Ӫ�ء����غ�����Ӫ�ذ�
        camp.GetComponent<Camp>().ClickEvent+=BackToCamp;
    }
    private void CampEnter()
    {
        Debug.Log("�ص�Ӫ����");
        camp.GetComponent<Camp>().ClickEvent += GetCommission;
    }
    private void CheckDeck()//�鿴����
    {
        //���ֹͣ�ƶ�
        Player.GetComponent<PlayerMove>().Stop = true;
        DW = Instantiate(DeckWin, Vector3.zero, Quaternion.identity);
        DW.GetComponent<DeckWin>().Show(shareddata.playerinfo.deck);
        //����ͷ�ƶ���ֹ�󴥱���
        Camera.transform.position = GameConfig.CameraNew;
        //�󶨹رմ��ڰ�ť
        DW.GetComponent<Transform>().Find("Btn").GetComponent<ConfirmBtn>().Confirm += CloseDeck;
    }
    private void CloseDeck()//�رտ��鴰��
    {
        Destroy(DW);
        //������л�ԭ��λ��
        Camera.transform.position = GameConfig.CameraDefault;
        //��Ҽ����ƶ�
        Player.GetComponent<PlayerMove>().Stop = false;
    }
    private void DeleteCard()
    {
        DW = Instantiate(DeckWin, Vector3.zero, Quaternion.identity);
        DW.GetComponent<DeckWin>().ShowDelete(shareddata.playerinfo.deck);
        //��ɾ���¼�
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
        //��ʼ��Rogue Mod
        RM = new RogueMod();
        RM.ArrangeEvent(GameConfig.Events,GameConfig.EventAmountBounds);

        //��ʼ��SharedData
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
