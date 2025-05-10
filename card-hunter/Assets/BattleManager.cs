using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum BattleState
{
    NotBegin,        //战斗未开始时
    PlayerTurn,     // 玩家出牌阶段
    PlayerDraw,      // 玩家抽牌阶段
    EnemyTurn        // 敌人行动阶段
}
public class PlayerWantMoveEventArgs : EventArgs
{
    public int Length{ get; }  // 位移长度
    public List<Vector2Int> Adjacent { get; }  // 相邻位置

    public PlayerWantMoveEventArgs(List<Vector2Int>v , int _Length)
    {
        Adjacent = v;
        Length = _Length;
    }
}
public class BattleManager : MonoBehaviour
{
    public BattleState currentState = BattleState.NotBegin;
    public PlayerInfo Player;

    private List<Card> InitialDeck = new(); //初始卡组
    private List<Card> deck = new ();      // 牌库
    private List<Card> discardPile = new (); // 弃牌堆
    private List<Card> hand = new ();      // 手牌

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged; //状态改变

    public delegate void PositionChangeHandler(object sender, PlayerWantMoveEventArgs e); //将要改变位置
    public event PositionChangeHandler OnPositionChange;  //玩家移动时让地图显示可移动的区域

    public delegate void PositionChangedHandler(object sender, Vector2Int newPos); //位置已经发生改变
    public event PositionChangedHandler OnPositionChanged;

    public delegate void BladeLevelUpHandler(object sender);
    public event BladeLevelUpHandler OnBladeLevelUp; //气刃等级提升

    private bool isWaitingForPlayerAction = false; //等待玩家操作
    public UnityEvent EndTurnClicked; //回合结束按钮按下
    private void Start()
    {

        InitializeBattle();
    }

    // 初始化战斗
    private void InitializeBattle()
    {
        // 初始化玩家和敌人
        Player.Initialize();
        InitializeDeck();
        // 开始玩家回合
        ChangeState(BattleState.PlayerDraw);
    }

    public void InitializeDeck()
    {
        //添加卡牌，再洗牌
        ShuffleDeck();

    }
    public void DrawCard(int num)
    {
        //抽牌动画处理 tbd
        for(int i = 0;i < num;i ++)
        {
            if(deck.Count == 0)
            {
                if (discardPile.Count == 0) break;
                deck.AddRange(discardPile);
                discardPile.Clear();
                ShuffleDeck();
            }
            if (deck.Count >= GameConfig.MaxHandCardNum) break;
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            hand.Add(drawnCard);
        }
    }
    public void ShuffleDeck()
    {
        for(int i = 0;i < deck.Count;i ++)
        {
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    // 改变战斗状态
    private void ChangeState(BattleState newState)
    {
        currentState = newState;
        OnBattleStateChanged?.Invoke(currentState);

        switch (currentState)
        {
            case BattleState.PlayerDraw:
                StartCoroutine(PlayerDrawPhase());
                break;

            case BattleState.PlayerTurn:
                StartPlayerTurn();
                break;

            case BattleState.EnemyTurn:
                //怪物行动
                break;

            case BattleState.NotBegin:
                EndBattle();
                break;
        }
    }

    // 玩家抽牌阶段
    private IEnumerator PlayerDrawPhase()
    {
        // 抽牌动画或延迟
        yield return new WaitForSeconds(0.5f);

        DrawCard(GameConfig.InitialHandCardNum);
        ChangeState(BattleState.PlayerTurn);
    }

    // 玩家回合开始
    private void StartPlayerTurn()
    {
        // 重置玩家能量
        Player.ModifyCost(Player.MaxCost - Player.curCost);

        Debug.Log("玩家回合开始 - 等待操作");

        // 设置等待标志
        isWaitingForPlayerAction = true;

        // 这里可以启用玩家交互UI
       // UIManager.Instance.SetEndTurnButtonActive(true);
    }
    public void OnEndTurnButtonClicked()
    {
        if (!isWaitingForPlayerAction) return;

        Debug.Log("玩家结束回合");

        // 清除等待标志
        isWaitingForPlayerAction = false;

        // 切换到敌人回合
        ChangeState(BattleState.EnemyTurn);
    }

    // 玩家结束回合
    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        //在这里结算回合

        ChangeState(BattleState.EnemyTurn);
    }
    public bool PlayCard(Card card)
    {
        if (currentState != BattleState.PlayerTurn || isWaitingForPlayerAction == false) return false;
        //       if(Player.curCost <= card.cost)return false; 费用不够的话

        //如果移动不为0
        {
            OnPositionChange?.Invoke(this, new PlayerWantMoveEventArgs(GetAdjacent() , /*card.MoveDistance*/1));
        }


        return true;
    } 
    public void EndBattle()
    {
        //怪物死亡进入战斗结算
    }
    public bool CheckPosIsValid(Vector2Int v)
    {
        if (v.x < 0 || v.x >= GameConfig.size || v.y < 0 || v.y >= GameConfig.size) return false;
        return true;
    }
    public List<Vector2Int> GetAdjacent()
    {
        List<Vector2Int> res = new();
        int[] dx = { 1, -1, 0, 0, -1, 1 };
        int[] dy = { 0, 0, 1, -1, 1, -1 };
        int x = Player.PlayerGridPos.x;
        int y = Player.PlayerGridPos.y;
        for(int i = 0;i < 6;i ++)
        {
            if (CheckPosIsValid(new Vector2Int(x + dx[i], y + dy[i])))res.Add(new Vector2Int(x + dx[i], y + dy[i]));
        }
        return res;
    }

    public 
    // Update is called once per frame
    void Update()
    {
        if (Player.curBladeNum >= GameConfig.MaxBladeNum)
            OnBladeLevelUp?.Invoke(this);
    }
}
