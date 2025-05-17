using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SharedData shareddata;
    public GameObject camp;
    public GameObject WorldMap;
    public GameObject Player;
    private bool CommissionAvailable=true;
    private RogueMod RM;
    private List<Commission> AcceptedCommission;
    private event Func<Commission,IEnumerator> ArriveBattleField;
    private event Action ArriveCamp;
    private int PlayerProgress = 0;//玩家进度,标识行程
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
    private void AcceptCommission()//接取委托（生成随机委托,生成随机事件,完成去程,处理去程中的随机事件）
    {
        //点击后解绑，防止多次接取委托
        camp.GetComponent<Camp>().ClickEvent -= AcceptCommission;

        List<Commission> commissions = GameConfig.Commissions;
        List<Commission>selected= RM.ChooseCommission(commissions, 1);
        //显示任务面板,传回所选委托
        Commission commission = GameConfig.Commissions[0];
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
    private IEnumerator EventHandle(Event e)//处理随机事件
    {
        Debug.Log("handling"+e.id+e.text);
        yield return new WaitForSeconds(1);
    }
    private IEnumerator BattleEnter(Commission c)//进入战斗
    {
        Debug.Log("进行了与" + c.monster + "的战斗");
        yield return new WaitForSeconds(1);//调用战斗,返回战斗结果

        //战斗结束，返回营地。返回函数与营地绑定
        camp.GetComponent<Camp>().ClickEvent+=BackToCamp;
    }
    private void CampEnter()
    {
        Debug.Log("回到营地了");
        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
    }
    void Start()
    {
        //初始化Rogue Mod
        RM = new RogueMod();
        RM.ArrangeEvent(GameConfig.Events,GameConfig.EventAmountBounds);

        camp.GetComponent<Camp>().ClickEvent += AcceptCommission;
        Player.GetComponent<PlayerMove>().encounterEvent += EventHandle;
        ArriveBattleField += BattleEnter;
        ArriveCamp += CampEnter;
        ShowStartMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
