using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum BattleState
{
    NotBegin,        //ս��δ��ʼʱ
    PlayerTurn,     // ��ҳ��ƽ׶�
    PlayerDraw,      // ��ҳ��ƽ׶�
    EnemyTurn        // �����ж��׶�
}
public class BattleManager : MonoBehaviour
{
    public BattleState currentState = BattleState.NotBegin;
    public PlayerInfo Player;

    // private List<Card> deck = new List<Card>();      // �ƿ�
    // private List<Card> discardPile = new List<Card>(); // ���ƶ�
    // private List<Card> hand = new List<Card>();      // ����

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged;

    private bool isWaitingForPlayerAction = false;
    public UnityEvent EndTurnClicked;
    private void Start()
    {
        InitializeBattle();
    }

    // ��ʼ��ս��
    private void InitializeBattle()
    {
        // ��ʼ����Һ͵���
        Player.Initialize();
        InitializeDeck();
        // ��ʼ��һغ�
        ChangeState(BattleState.PlayerDraw);
    }

    public void InitializeDeck()
    {
        //��ӿ��ƣ���ϴ��
        ShuffleDeck();

        //������ƶ�
    }
    public void DrawCard(int num)
    {

    }
    public void ShuffleDeck()
    {

    }
    // �ı�ս��״̬
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
                //�����ж�
                break;

            case BattleState.NotBegin:
                EndBattle();
                break;
        }
    }

    // ��ҳ��ƽ׶�
    private IEnumerator PlayerDrawPhase()
    {
        // ���ƶ������ӳ�
        yield return new WaitForSeconds(0.5f);

        DrawCard(GameConfig.InitialHandCardNum);
        ChangeState(BattleState.PlayerTurn);
    }

    // ��һغϿ�ʼ
    private void StartPlayerTurn()
    {
        // �����������
        Player.ModifyCost(Player.MaxCost - Player.curCost);

        Debug.Log("��һغϿ�ʼ - �ȴ�����");

        // ���õȴ���־
        isWaitingForPlayerAction = true;

        // �������������ҽ���UI
       // UIManager.Instance.SetEndTurnButtonActive(true);
    }
    public void OnEndTurnButtonClicked()
    {
        if (!isWaitingForPlayerAction) return;

        Debug.Log("��ҽ����غ�");

        // ����ȴ���־
        isWaitingForPlayerAction = false;

        // �л������˻غ�
        ChangeState(BattleState.EnemyTurn);
    }

    // ��ҽ����غ�
    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        //���������غ�

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
        //������������ս������
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
