using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using UnityEngine.UIElements;

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
    public MapManager mapmanager;
    public BladegasSlotController BladeLevelSlot;
    public Button Endbutton;
    public CardManager cardManager;
    public int i = 1;//������
    private List<Card> InitialDeck = new(); //��ʼ����
    private List<Card> deck = new ();      // �ƿ�
    private List<Card> discardPile = new (); // ���ƶ�
    private List<Card> hand = new ();      // ����
    
    private List<EnemyAIController> _enemies = new ();//����

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged; //״̬�ı�

    //public delegate void PositionChangeHandler(object sender, PlayerWantMoveEventArgs e); //��Ҫ�ı�λ��
    //public event PositionChangeHandler OnPositionChange;  //����ƶ�ʱ�õ�ͼ��ʾ���ƶ�������

    public delegate void PositionChangedHandler(Vector2Int newPos); //λ���Ѿ������ı�
    public event PositionChangedHandler OnPositionChanged;

    public delegate void BladeLevelChangeHandler(int NewBladeLevel);
    public event BladeLevelChangeHandler OnBladeLevelChange; //���еȼ��ı�

    public delegate void BladeGasChangeHandler(int NewBladeNum);
    public event BladeGasChangeHandler OnBladeGasChange; //���ı�

    public delegate void DirectionChangedHandler(Vector2Int newDir);
    public event DirectionChangedHandler OnDirectionChanged; //����ı�

    private bool isWaitingForPlayerAction = false; //�ȴ����ѡ��λ��
    public UnityEvent EndTurnClicked; //�غϽ�����ť����
    private void Start()
    {

        Player = GetComponentInChildren<PlayerInfo>();
        mapmanager = GetComponentInChildren<MapManager>();
        //��ʼ�¼�����:
        OnBladeGasChange += Player.ModifyBladeNum;
        OnBladeGasChange += BladeLevelSlot.ShowBladeGas;

        OnBladeLevelChange += BladeLevelSlot.ShowBladeLevel;
        OnBladeLevelChange += Player.ModifyBladeLevel;

        OnPositionChanged += Player.ModifyPos;
        OnPositionChanged += Player.GetComponent<PlayerShow>().ModifyPos;
        OnDirectionChanged += Player.GetComponent<PlayerShow>().ModifyDirection;
        Endbutton.onClick.AddListener(() => {
           Card newcard= cardManager.CreateCard(i, cardManager.transform);//���������ɿ���
            cardManager.AddCardToHand(newcard);
            i = i % 8 + 1;
        });
        InitializeBattle();


    }

    // ��ʼ��ս��
    private void InitializeBattle()
    {
        OnBladeGasChange?.Invoke(5);
        OnBladeLevelChange?.Invoke(2);
        // ��ʼ����Һ͵���
        InitializeDeck();
        FindAllEnemies();
        // ��ʼ��һغ�
        ChangeState(BattleState.PlayerDraw);
    }

    public void InitializeDeck()
    {
        //���ӿ��ƣ���ϴ��
        ShuffleDeck();
    }

    public void FindAllEnemies()
    {
        //���ع���
        _enemies = new List<EnemyAIController>(FindObjectsOfType<EnemyAIController>());
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
            yield return new WaitForSeconds(0.5f);
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
        Debug.Log("������ƽ׶�");
        yield return new WaitForSeconds(5f);
     //   OnPositionChanged?.Invoke(new(3, 4));
        Debug.Log("����ǰ�ȴ����");
        Action<Vector2Int> callback = OnPositionChanged.Invoke;
        StartCoroutine(mapmanager.MoveCommand(GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }), Player.PlayerGridPos, new Vector2Int(1, 1) , callback));
       
        yield return StartCoroutine(DrawCard(GameConfig.InitialHandCardNum));
        ChangeState(BattleState.PlayerTurn);
    }

    // ��һغϿ�ʼ
    private void StartPlayerTurn()
    {
        // �����������
        Player.ModifyCost(Player.GetComponent<PlayerInfo>().MaxCost - Player.GetComponent<PlayerInfo>().curCost);

        Debug.Log("��һغϿ�ʼ - �ȴ�����");
       
        ChangeState(BattleState.EnemyTurn);
    }
    public void OnEndTurnButtonClicked()
    {
        //������Ƿ�Ϸ�
        Debug.Log("��ҽ����غ�");



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
        if (currentState != BattleState.PlayerTurn) return false;
        if(Player.curCost <= card.Cost)return false; 

        if(card.Move.Count != 0)
        {
            Action<Vector2Int> callback = OnPositionChanged.Invoke;
            isWaitingForPlayerAction = true; //��ʼ�ȴ����ѡ��λ��
            StartCoroutine(mapmanager.MoveCommand(GetAdjacent(card.Move), Player.PlayerGridPos, card.MoveLength, callback));
            isWaitingForPlayerAction = false; //���ѡ��λ�ý���
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
    public List<Vector2Int> GetAdjacent(List<int> Dir)
    {
        List<Vector2Int> res = new();
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int x = Player.PlayerGridPos.x;
        int y = Player.PlayerGridPos.y;
        int dir_id = 0;
        for(int i = 0;i < 6;i ++)
        {
            if (Player.Direction == new Vector2Int(dx[i], dy[i])) dir_id = i;
        }
        
        for (int i = 0; i < Dir.Count; i++)
        {
            int newDir = (dir_id + Dir[i]) % 6;
            if (CheckPosIsValid(new Vector2Int(x +  dx[newDir], y +  dy[newDir]))) res.Add(new Vector2Int(x +  dx[newDir], y + dy[newDir]));
        }
        return res;
    }

    public 
    // Update is called once per frame
    void Update()
    {
       
    }
}
