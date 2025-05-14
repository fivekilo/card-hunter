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

    private bool isWaitingForPlayerAction = false; 
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
        FindAllEnemies();

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
                break;

            case BattleState.NotBegin:
                EndBattle();
                break;
        }
    }

    private IEnumerator HandEnemyTurn()
    {
        foreach(var enemy in _enemies)
        {
            if (enemy != null)
            {
                yield return enemy.TakeTurn();
            }
        }
        ChangeState(BattleState.PlayerDraw);
    }

    private IEnumerator PlayerDrawPhase()
    {
        Debug.Log("draw waiting");
        yield return new WaitForSeconds(5f);
     //   OnPositionChanged?.Invoke(new(3, 4));
        Debug.Log("waiting complete");
      //  Action<Vector2Int> callback = OnPositionChanged.Invoke;
      //  StartCoroutine(mapmanager.MoveCommand(GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }), Player.PlayerGridPos, new Vector2Int(1, 1) , callback));
      
        DrawCard(GameConfig.InitialHandCardNum);
        ChangeState(BattleState.PlayerTurn);
    }

    private void StartPlayerTurn()
    {
        
        Player.ModifyCost(Player.GetComponent<PlayerInfo>().MaxCost);


        isWaitingForPlayerAction = true;

       // UIManager.Instance.SetEndTurnButtonActive(true);

    }
    public void OnEndTurnButtonClicked()
    {
        if (!isWaitingForPlayerAction) return;

        Debug.Log("玩家结束回合");

        isWaitingForPlayerAction = false;

        ChangeState(BattleState.EnemyTurn);
    }

   
    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn) return;


        ChangeState(BattleState.EnemyTurn);
    }
    public void PlayCard(Card card)
    {

        if (currentState != BattleState.PlayerTurn || isWaitingForPlayerAction == false) return;
       // Debug.Log(card.Cost);
        if (Player.curCost < card.Cost)return;
        //LockCards();
        Player.ModifyCost(Player.curCost - card.Cost);
        Debug.Log("还剩" + Player.curCost.ToString() + "费！");
        if(card.Move != null)
        {
            //  OnPositionChange?.Invoke(this, new PlayerWantMoveEventArgs(GetAdjacent(card.Move) , /*card.MoveDistance*/1));
            Action<Vector2Int> callback = OnPositionChanged.Invoke;
            StartCoroutine(mapmanager.MoveCommand(GetAdjacent(card.Move), Player.PlayerGridPos, card.MoveLength, callback));
        }
        cardManager.RemoveCardFromHand(card, hand);
        UpdateCards();
    } 
    public void UpdateCards()
    {
        foreach(Card card in hand)
        {
            
            if(Player.curCost < card.Cost || Player.curBladeNum < -card.DeltaBladeNum || Player.curBladeLevel < -card.DeltaBladeLevel)
            {
                card.CBuse = false;
            }
        }
    }
    //public void LockCards()
    //{
    //    foreach (Card card in hand)
    //    {
    //            card.CBuse = false;
    //    }
    //}

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

    }
}
