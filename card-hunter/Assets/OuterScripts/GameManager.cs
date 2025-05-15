using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject camp;
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
        //显示任务面板,传回所选委托
        List<Commission> commissions = new List<Commission>();
        
    }
    private void EventHandle(Event e)//处理随机事件
    {

    }
    private void BattleEnter(Commission c)//进入战斗
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
