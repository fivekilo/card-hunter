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
    private bool ComAccept=false;//�Ƿ���Խ�ȡί��
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
    private void AcceptCommission()//��ȡί�У��������ί��,��������¼���
    {
        List<Commission> commissions = GameConfig.Commissions;
        List<Commission>selected= RM.ChooseCommission(commissions, 1);
        //��ʾ�������,������ѡί��
        Commission commission = GameConfig.Commissions[0];
        //ȥ�̣���������¼�
        List<Event>events = new List<Event>();
        //RM.EventGenerate(events, 1);
        //����·��
        List<Vector3> points= WorldMap.GetComponent<RouteRender>().plotRoute(commission.place);
        //�������·���ƶ�
        StartCoroutine(PlayerGo(points));
    }
    private IEnumerator PlayerGo(List<Vector3> points)
    {
        PlayerMove PM = Player.GetComponent<PlayerMove>();
        foreach (Vector3 p in points)
        {
            yield return StartCoroutine(PM.MoveTo(p));
        }
    }
    private void EventHandle(Event e)//��������¼�
    {

    }
    private void BattleEnter(Commission c)//����ս��
    {

    }
    void Start()
    {
        RM = new RogueMod();
        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
