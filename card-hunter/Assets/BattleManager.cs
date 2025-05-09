using System;
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
public class PlayerWantMoveEventArgs : EventArgs
{
    public int Length{ get; }  // λ�Ƴ���
    public List<Vector2Int> Adjacent { get; }  // ����λ��

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

    private List<Card> InitialDeck = new(); //��ʼ����
    private List<Card> deck = new ();      // �ƿ�
    private List<Card> discardPile = new (); // ���ƶ�
    private List<Card> hand = new ();      // ����

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged; //״̬�ı�

    public delegate void PositionChangeHandler(object sender, PlayerWantMoveEventArgs e); //��Ҫ�ı�λ��
    public event PositionChangeHandler OnPositionChange;  //����ƶ�ʱ�õ�ͼ��ʾ���ƶ�������

    public delegate void PositionChangedHandler(object sender, Vector2Int newPos); //λ���Ѿ������ı�
    public event PositionChangedHandler OnPositionChanged;

    public delegate void BladeLevelUpHandler(object sender);
    public event BladeLevelUpHandler OnBladeLevelUp; //���еȼ�����

    private bool isWaitingForPlayerAction = false; //�ȴ���Ҳ���
    public UnityEvent EndTurnClicked; //�غϽ�����ť����
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

    }
    public void DrawCard(int num)
    {
        //���ƶ������� tbd
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
    public bool PlayCard(Card card)
    {
        if (currentState != BattleState.PlayerTurn || isWaitingForPlayerAction == false) return false;
        //       if(Player.curCost <= card.cost)return false; ���ò����Ļ�

        //����ƶ���Ϊ0
        {
            OnPositionChange?.Invoke(this, new PlayerWantMoveEventArgs(GetAdjacent() , /*card.MoveDistance*/1));
        }


        return true;
    } 
    public void EndBattle()
    {
        //������������ս������
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
