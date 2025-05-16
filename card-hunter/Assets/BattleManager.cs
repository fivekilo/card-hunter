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
    NotBegin,  
    PlayerTurn,  
    PlayerDraw,    
    EnemyTurn     
}
public class PlayerWantMoveEventArgs : EventArgs
{
    public int Length{ get; }  
    public List<Vector2Int> Adjacent { get; } 
    public PlayerWantMoveEventArgs(List<Vector2Int> v, int _Length)
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
    public Button Movebutton;
    public CardManager cardManager;
   // public int i = 1;
    private List<Card> InitialDeck = new(); 
    private List<Card> deck = new ();      
    [SerializeField]
    private List<Card> discardPile = new (); 
    private List<Card> hand = new ();    
    
    private List<EnemyAIController> _enemies = new ();

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged; 

    

    public delegate void PositionChangedHandler(Vector2Int newPos);
    public event PositionChangedHandler OnPositionChanged;

    public delegate void BladeLevelChangeHandler(int NewBladeLevel);
    public event BladeLevelChangeHandler OnBladeLevelChange; 

    public delegate void BladeGasChangeHandler(int NewBladeNum);
    public event BladeGasChangeHandler OnBladeGasChange; 

    public delegate void DirectionChangedHandler(Vector2Int newDir);
    public event DirectionChangedHandler OnDirectionChanged; 

    public bool isWaitingForPlayerChoose = false;
    public UnityEvent EndTurnClicked; 

    private void Start()
    {

        Player = GetComponentInChildren<PlayerInfo>();
        mapmanager = GetComponentInChildren<MapManager>();
        OnBladeGasChange += Player.ModifyBladeNum;
        OnBladeGasChange += BladeLevelSlot.ShowBladeGas;

        OnBladeLevelChange += BladeLevelSlot.ShowBladeLevel;
        OnBladeLevelChange += Player.ModifyBladeLevel;

        OnPositionChanged += Player.ModifyPos;
        OnPositionChanged += Player.GetComponent<PlayerShow>().ModifyPos;

        OnDirectionChanged += Player.GetComponent<PlayerShow>().ModifyDirection;
        OnDirectionChanged += Player.ModifyDirection;

        Endbutton.onClick.AddListener(OnEndTurnButtonClicked);
        Movebutton.onClick.AddListener(OnMoveButtonClicked);
        /*  Endbutton.onClick.AddListener(() => {
             Card newcard= cardManager.CreateCard(i, cardManager.transform);
              cardManager.AddCardToHand(newcard , hand);
              i = i % 8 + 1;
          });*/
        InitializeBattle();
        foreach (Card card in discardPile)
        {
            CardController cardController = card.GetComponent<CardController>();
            cardController.OnCardUsed += PlayCard;
        }
        
    }

    private void InitializeBattle()
    {
        OnBladeGasChange?.Invoke(5);
        OnBladeLevelChange?.Invoke(2);

        InitializeDeck();

        ChangeState(BattleState.PlayerDraw);
    }

    public void InitializeDeck()
    {
        for (int i = 1; i <= 8; i++)
        {
            Card newcard = cardManager.CreateCard(i,cardManager.transform );
            discardPile.Add(newcard); 
        }
        ShuffleDeck(discardPile);
    }

    public void FindAllEnemies()
    {
        _enemies = new List<EnemyAIController>(FindObjectsOfType<EnemyAIController>());
    }

    public void DrawCard(int num)
    {
        for(int i = 0;i < num;i ++)
        {
            if(deck.Count == 0)
            {
                if (discardPile.Count == 0) break;
                deck.AddRange(discardPile);
                discardPile.Clear();
          //      ShuffleDeck(deck);
            }
            if (hand.Count >= GameConfig.MaxHandCardNum) break;
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            cardManager.AddCardToHand(drawnCard, hand);
        }
      //  Debug.Log(hand.Count);
    }
    public void ShuffleDeck(List<Card> cards)
    {
        for (int i = 0;i < cards.Count;i ++)
        {
            int randomIndex = UnityEngine.Random.Range(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
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
                StartCoroutine(HandEnemyTurn());
                break;

            case BattleState.NotBegin:
                EndBattle();
                break;
        }
    }

    private IEnumerator HandEnemyTurn()
    {
        Debug.Log("怪物回合开始了!");
        yield return new WaitForSeconds(5f);
        FindAllEnemies();
        foreach(var enemy in _enemies)
         {
             if (enemy != null)
             {
                 yield return enemy.TakeTurn();
             }
        }
        Debug.Log("怪物回合结束了!");
        ChangeState(BattleState.PlayerDraw);
    }

    private IEnumerator PlayerDrawPhase()
    {
        Debug.Log("draw waiting");
        yield return new WaitForSeconds(2f);
     //   OnPositionChanged?.Invoke(new(3, 4));
        Debug.Log("waiting complete");
        //  Action<Vector2Int> callback = OnPositionChanged.Invoke;
        //  StartCoroutine(mapmanager.MoveCommand(GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }), Player.PlayerGridPos, new Vector2Int(1, 1) , callback));
        Debug.Log(hand.Count);
        DrawCard(GameConfig.InitialHandCardNum);
        ChangeState(BattleState.PlayerTurn);
    }

    private void StartPlayerTurn()
    {
        
        Player.ModifyCost(Player.GetComponent<PlayerInfo>().MaxCost);



       // UIManager.Instance.SetEndTurnButtonActive(true);

    }
    public void OnEndTurnButtonClicked()
    {
        if (isWaitingForPlayerChoose == true || currentState != BattleState.PlayerTurn) return;

        Debug.Log("玩家结束回合了");


        EndPlayerTurn();
        ChangeState(BattleState.EnemyTurn);
    }
    public void OnMoveButtonClicked()
    {
        if (currentState != BattleState.PlayerTurn) return;
        int MoveCost = Player.Situation + 1;
        if (Player.curCost < MoveCost) return;
        
        Player.ModifyCost(Player.curCost - MoveCost);
        isWaitingForPlayerChoose = true;
            Action<Vector2Int> callback1 = OnPositionChanged.Invoke;
            Action<Vector2Int> callback2 = OnDirectionChanged.Invoke;
            StartCoroutine(mapmanager.MoveCommand(GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }), Player.PlayerGridPos, new Vector2Int(1,1), callback1 , callback2));
        Player.ModifySituation(0);

    }


    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        discardPile.AddRange(hand);
        foreach(Card card in hand)
        {
            card.transform.position += new Vector3(10000, 0, 0);
        }
        hand.Clear();
        Debug.Log("回合结束时手牌数为:" + hand.Count);
        cardManager.UpdateCardPositions(hand);
    }
    public IEnumerator MoveAndAttackCoRoutine(Card card)
    {
        if (card.Move != null)
        {
            //  OnPositionChange?.Invoke(this, new PlayerWantMoveEventArgs(GetAdjacent(card.Move) , /*card.MoveDistance*/1));
            isWaitingForPlayerChoose = true;
            Action<Vector2Int> callback1 = OnPositionChanged.Invoke;
            Action<Vector2Int> callback2 = OnDirectionChanged.Invoke;
            Player.ModifySituation(0);
            yield return StartCoroutine(mapmanager.MoveCommand(GetAdjacent(card.Move), Player.PlayerGridPos, card.MoveLength, callback1, callback2));
        }

        if (card.AttackDirection != null)
        {
            isWaitingForPlayerChoose = true;
            Action<Vector2Int> callback = OnDirectionChanged.Invoke;
            Player.ModifySituation(1);
            yield return StartCoroutine(mapmanager.AttackCommand(GetAdjacent(card.AttackDirection), Player.PlayerGridPos, new(0, card.AttackLength), callback));
        }
        if (card.DeltaBladeNum != 0)
        {
            Player.ModifyBladeNum(Player.curBladeNum + card.DeltaBladeNum);
            OnBladeGasChange?.Invoke(Player.curBladeNum);
        }

        if (card.DeltaBladeLevel != 0)
        {
            Player.ModifyBladeLevel(Player.curBladeLevel + card.DeltaBladeLevel);
            OnBladeLevelChange?.Invoke(Player.curBladeLevel);
        }

        if (card.AttackDirection != null)
        {
            int[] dx = { 1, 0, -1, -1, 0, 1 };
            int[] dy = { 0, 1, 1, 0, -1, -1 };
            int PlayerDirId = -1;
            for (int i = 0; i < 6; i++)
            {
                if (Player.Direction.x == dx[i] && Player.Direction.y == dy[i]) { PlayerDirId = i; break; }
            }
            FindAllEnemies();
            foreach (int Dir_id in card.AttackDirection)
            {
                for (int i = 0; i <= card.AttackLength; i++)
                {
                    int newDir = (Dir_id + PlayerDirId) % 6;
                    Vector2Int nowPos = Player.PlayerGridPos + new Vector2Int(dx[newDir] * i, dy[newDir] * i);
                    foreach (EnemyAIController enemyAI in _enemies)
                    {
                        if (enemyAI.GetCurrentGridPos() == nowPos)
                        {
                            enemyAI.ReduceHealth(card.Attack.x * card.Attack.y);
                        }
                    }
                }
            }
        }
        
        
    }
    public void PlayCard(Card card)
    {
        if (currentState != BattleState.PlayerTurn ) return;
        if (Player.curCost < card.Cost)return;

        Player.ModifyCost(Player.curCost - card.Cost);
        Debug.Log("还剩" + Player.curCost.ToString() + "费！");

        /*   if(card.Move != null)
           {
               isWaitingForPlayerChoose = true;
               Action<Vector2Int> callback1 = OnPositionChanged.Invoke;
               Action<Vector2Int> callback2 = OnDirectionChanged.Invoke;
               StartCoroutine(mapmanager.MoveCommand(GetAdjacent(card.Move), Player.PlayerGridPos, card.MoveLength, callback1 , callback2));
           }

           if(card.AttackDirection != null)
           {
               isWaitingForPlayerChoose = true;
               Action<Vector2Int> callback = OnDirectionChanged.Invoke;
               StartCoroutine(mapmanager.AttackCommand(GetAdjacent(card.AttackDirection), Player.PlayerGridPos, new(0 , card.AttackLength), callback));
           }*/
        
        
        StartCoroutine(MoveAndAttackCoRoutine(card));
        cardManager.RemoveCardFromHand(card, hand);
        card.transform.position += new Vector3(10000, 0, 0);
        discardPile.Add(card);
        /*   if(card.DeltaBladeNum != 0)
           {
               Player.ModifyBladeNum(Player.curBladeNum + card.DeltaBladeNum);
               OnBladeGasChange?.Invoke(Player.curBladeNum);
           }

           if(card.DeltaBladeLevel != 0)
           {
               Player.ModifyBladeLevel(Player.curBladeLevel + card.DeltaBladeLevel);
               OnBladeLevelChange?.Invoke(Player.curBladeLevel);
           }

           if (card.AttackDirection != null)
           {
               int[] dx = { 1, 0, -1, -1, 0, 1 };
               int[] dy = { 0, 1, 1, 0, -1, -1 };
               FindAllEnemies();
               foreach (int Dir_id in card.AttackDirection)
               {
                   for(int i = 0;i <= card.AttackLength;i ++)
                   {
                       Vector2Int nowPos = Player.PlayerGridPos + new Vector2Int(dx[Dir_id] * i, dy[Dir_id] * i);
                       foreach (EnemyAIController enemyAI in _enemies)
                       {
                           if(enemyAI.GetCurrentGridPos() == nowPos)
                           {
                               enemyAI.ReduceHealth(card.Attack.x * card.Attack.y);
                           }
                       }
                   }
               }
           }

           cardManager.RemoveCardFromHand(card, hand);
           card.transform.position += new Vector3(10000, 0, 0);
           discardPile.Add(card);*/
    } 
    public void UpdateCards()
    {
        foreach(Card card in hand)
        {
            
            if(Player.curCost < card.Cost || 
                Player.curBladeNum < -card.DeltaBladeNum ||
                Player.curBladeLevel < -card.DeltaBladeLevel || 
                isWaitingForPlayerChoose || 
                currentState != BattleState.PlayerTurn||
                (card.OnlyLState!=2&&Player.Situation!=card.OnlyLState)
                )
            {
                card.CBuse = false;
            }
            else
            {
                card.CBuse = true;

            }
        }
    }
    public void LockCards()
    {
        foreach (Card card in hand)
        {
            card.CBuse = false;
        }
    }

    public void EndBattle()
    {
        
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
        for (int i = 0; i < 6; i++)
        {
            if (Player.Direction == new Vector2Int(dx[i], dy[i])) dir_id = i;
        }

        for (int i = 0; i < Dir.Count; i++)
        {
            int newDir = (dir_id + Dir[i]) % 6;
            if (CheckPosIsValid(new Vector2Int(x + dx[newDir], y + dy[newDir]))) res.Add(new Vector2Int(x + dx[newDir], y + dy[newDir]));
        }
        return res;
    }

    public
    // Update is called once per frame
    void Update()
    {
        UpdateCards();
    }
}
