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
public class BattleManager : MonoBehaviour
{
    public BattleState currentState = BattleState.NotBegin;
    public PlayerInfo Player;

    // private List<Card> deck = new List<Card>();      // 牌库
    // private List<Card> discardPile = new List<Card>(); // 弃牌堆
    // private List<Card> hand = new List<Card>();      // 手牌

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged;

    private bool isWaitingForPlayerAction = false;
    public UnityEvent EndTurnClicked;
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

        //清空弃牌堆
    }
    public void DrawCard(int num)
    {

    }
    public void ShuffleDeck()
    {

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
   /* public bool PlayCard(Card card)
    {
        if (currentState != BattleState.PlayerTurn || isWaitingForPlayerAction == false) return false;
        if(Player.curCost <= card.cost)return false;
        return true;
    } */
    public void EndBattle()
    {
        //怪物死亡进入战斗结算
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
