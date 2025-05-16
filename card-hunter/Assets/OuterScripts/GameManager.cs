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
    private bool ComAccept=false;//是否可以接取委托
    private void ShowStartMenu()//显示起始界面
    {

    }
    private void GameStart()//游戏启动
    {

    }
    private void Save()//存档
    {

    }
    private void Exit()//退出游戏
    {

    }
    private void AcceptCommission()//接取委托（生成随机委托,生成随机事件）
    {
        List<Commission> commissions = GameConfig.Commissions;
        List<Commission>selected= RM.ChooseCommission(commissions, 1);
        //显示任务面板,传回所选委托
        Commission commission = GameConfig.Commissions[0];
        //去程：生成随机事件
        List<Event>events = new List<Event>();
        //RM.EventGenerate(events, 1);
        //生成路径
        List<Vector3> points= WorldMap.GetComponent<RouteRender>().plotRoute(commission.place);
        //让玩家沿路径移动
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
    private void EventHandle(Event e)//处理随机事件
    {

    }
    private void BattleEnter(Commission c)//进入战斗
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
