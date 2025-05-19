using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public event Action<Choice> Chosed;
    public event Action<int> AddCard;
    private List<bool> AbleToMove=new List<bool> {false,false };
    private PlayerInfo playerinfo=new PlayerInfo();
    private RogueMod RM;
    private GameObject DW;//���鴰��
    private List<Commission> AcceptedCommission;
    private event Func<Commission,IEnumerator> ArriveBattleField;
    private event Action ArriveCamp;
    private int PlayerProgress = 0;//��ҽ���,��ʶ�г�
    private void ShowStartMenu()//��ʾ��ʼ����
    {

    }
    private void GameStart()//��Ϸ����
    {

    }
    private void Save()//�浵
    {

    }
    private void Exit()//�˳���Ϸ
    {

    }
    private void AcceptCommission()//��ȡί�У��������ί��,��������¼�,���ȥ��,����ȥ���е�����¼���
    {
        //������󣬷�ֹ��ν�ȡί��
        camp.GetComponent<Camp>().ClickEvent -= AcceptCommission;

        List<Commission> commissions = GameConfig.Commissions;
        List<Commission>selected= RM.ChooseCommission(commissions, 1);
        //��ʾ�������,������ѡί��
        Commission commission = GameConfig.Commissions[0];
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
    private void AddToDeck(int CardNum)
    {
        shareddata.playerinfo.deck.Add(CardNum);
        AbleToMove[0] = true;
    }
    private void ChoiceHandle(Choice choice)
    {
        if (choice.DeleteCard > 0)
        {
            //ɾ�ƺ���
            AbleToMove[1] = true;
        }
        else//����ɾ��
        {
            AbleToMove[1] = true;
        }
        if (choice.AddCard > 0)
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
        Debug.Log("��������" + c.monster + "��ս��");
        yield return new WaitForSeconds(1);//����ս��,����ս�����

        //ս������������Ӫ�ء����غ�����Ӫ�ذ�
        camp.GetComponent<Camp>().ClickEvent+=BackToCamp;
    }
    private void CampEnter()
    {
        Debug.Log("�ص�Ӫ����");
        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
    }
    private void CheckDeck()//�鿴����
    {
        DW = Instantiate(DeckWin, Vector3.zero, Quaternion.identity);
        DW.GetComponent<DeckWin>().Show(shareddata.playerinfo.deck);
        //�İ�Ϊ�رմ���
        DeckBtn.GetComponent<Btn>().Clicked -= CheckDeck;
        DeckBtn.GetComponent<Btn>().Clicked += CloseDeck;
    }
    private void CloseDeck()//�رտ��鴰��
    {
        Destroy(DW);
        //�İ�Ϊ��������
        DeckBtn.GetComponent<Btn>().Clicked += CheckDeck;
        DeckBtn.GetComponent<Btn>().Clicked -= CloseDeck;
    }

    void Start()
    {
        //��ʼ��Rogue Mod
        RM = new RogueMod();
        RM.ArrangeEvent(GameConfig.Events,GameConfig.EventAmountBounds);

        //��ʼ��SharedData
        shareddata.playerinfo = playerinfo;

        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
        Player.GetComponent<PlayerMove>().encounterEvent += EventChoose;
        ArriveBattleField += BattleEnter;
        ArriveCamp += CampEnter;
        Chosed += ChoiceHandle;
        AddCard += AddToDeck;
        DeckBtn.GetComponent<Btn>().Clicked += CheckDeck;
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
