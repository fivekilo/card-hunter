using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject camp;
    public GameObject WorldMap;
    public GameObject Player;
    private RogueMod RM;
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
        List<Commission> commissions = GameConfig.Commissions;
        List<Commission>selected= RM.ChooseCommission(commissions, 1);
        //��ʾ�������,������ѡί��
        Commission commission = GameConfig.Commissions[0];
        //ȥ�̣���������¼�
        List<Event> events = RM.GetEvents(PlayerProgress);
        //����·��
        List<Vector3> points= WorldMap.GetComponent<RouteRender>().plotRoute(commission.place);
        //�������·���ƶ�
        StartCoroutine(PlayerGo(points,events));
        PlayerProgress++;
    }
    private IEnumerator PlayerGo(List<Vector3> points,List<Event> events)//ʹ������һ���г�,�����г��д��������events
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
    }
    private IEnumerator EventHandle(Event e)//��������¼�
    {
        Debug.Log("handling"+e.id);
        yield return new WaitForSeconds(1);
    }
    private void BattleEnter(Commission c)//����ս��
    {

    }
    void Start()
    {
        //��ʼ��Rogue Mod
        RM = new RogueMod();
        RM.ArrangeEvent(GameConfig.Events,GameConfig.EventAmountBounds);

        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
        Player.GetComponent<PlayerMove>().encounterEvent += EventHandle;
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
