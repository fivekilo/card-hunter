using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject camp;
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
        List<Commission> commissions = new List<Commission>();
        List<Commission>selected= RM.ChooseCommission(commissions, 3);
        //��ʾ�������,������ѡί��
        Commission commission = GameConfig.Commissions[0];
        //ȥ�̣���������¼�
        List<Event>events = new List<Event>();
        RM.EventGenerate(events, 2);
        
    }
    private void EventHandle(Event e)//��������¼�
    {

    }
    private void BattleEnter(Commission c)//����ս��
    {

    }
    void Start()
    {
        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
