using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
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
    public PlayerBuff playerBuff;
    public EnemyBuff enemyBuff;
    public MapManager mapmanager;
    public BladegasSlotController BladeLevelSlot;
    public Button Endbutton;
    public Button Movebutton;
    public CardManager cardManager;
    public TextMeshProUGUI UserIndicator;
    public TextMeshProUGUI Decknum;
    public TextMeshProUGUI Dpnum;
    public AudioManager AudioManager;
    public SharedData data;
    // public int i = 1;
    private List<Card> InitialDeck = new(); 
    private List<Card> deck = new ();      
    [SerializeField]
    private List<Card> discardPile = new (); 
    private List<Card> hand = new ();    
    
    public List<EnemyAIController> _enemies = new ();

    public delegate void BattleEvent(BattleState state);
    public event BattleEvent OnBattleStateChanged;

    public GameObject BattleComplete;

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


    private void UpdatedeckNum()
    {
        Decknum.text = deck.Count.ToString();
        Dpnum.text = discardPile.Count.ToString();
    }

    public IEnumerator WaitBattleComplete()
    {
        FindAllEnemies();
        Debug.Log("现在人0血量为" + Player.curHealth);
        Debug.Log("现在怪物0血量为" + _enemies[0]._currentHealth);
        yield return new WaitUntil(() => _enemies[0]._currentHealth == 0 || Player.curHealth == 0);
        Debug.Log("现在人血量为" + Player.curHealth);
        Debug.Log("现在怪物血量为" + _enemies[0]._currentHealth);
        GameObject battlecomplete=Instantiate(BattleComplete, Vector3.zero, Quaternion.identity);
        battlecomplete.transform.SetParent(transform);
        Button button = battlecomplete.GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
         //   data.Complete = false;
        });
        if (Player.curHealth == 0)
        {
            battlecomplete.GetComponentInChildren<TextMeshProUGUI>().text = "战斗结束了！你输了！";
        }
        else if (_enemies[0]._currentHealth == 0)
        {
            battlecomplete.GetComponentInChildren<TextMeshProUGUI>().text = "战斗结束了！你赢了！";
        }
    }

    public Vector2Int GenerateSpawn()
    {
        int attemp = 0;
        Vector2Int res = new();
        System.Random rand = new System.Random();
        while (attemp < 100)
        {
            int xplusy = rand.Next(0 , GameConfig.size - 2);
            int x = rand.Next(0, xplusy + 1);
            if(mapmanager.isObstacle(new(x , xplusy - x)) == false)
            {
                res = new(x, xplusy - x);
                Debug.Log("生成在" + x + " " + (xplusy - x));
                break;
            }
            attemp++;
        }
        return res;
    }
    private void Start()
    {
        
        Player = GetComponentInChildren<PlayerInfo>();
        mapmanager = GetComponentInChildren<MapManager>();
        playerBuff = GetComponentInChildren<PlayerBuff>();
        enemyBuff = GetComponentInChildren<EnemyBuff>();


        OnBladeGasChange += Player.ModifyBladeNum;
        OnBladeGasChange += BladeLevelSlot.ShowBladeGas;

        OnBladeLevelChange += BladeLevelSlot.ShowBladeLevel;
        OnBladeLevelChange += Player.ModifyBladeLevel;

        OnPositionChanged += Player.ModifyPos;
        OnPositionChanged += Player.GetComponent<PlayerShow>().ModifyPos;
        OnPositionChanged += CheckContent;

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
     /*   foreach (Card card in discardPile)
        {
            CardController cardController = card.GetComponent<CardController>();
            cardController.OnCardUsed += PlayCard;
        }*/
        
    }

    private void InitializeBattle()
    {
        UserIndicator.text = "初始化中";
        OnPositionChanged?.Invoke(GenerateSpawn()); //生成出生位置
        OnBladeGasChange?.Invoke(8);
        OnBladeLevelChange?.Invoke(1);

        //人物初始化
        Player.money = data.playerinfo.money;
        Player.MaxCost = data.playerinfo.MaxCost;
        Player.MaxHealth = data.playerinfo.MaxHealth;

        //怪物初始化
        FindAllEnemies();

        StartCoroutine(WaitBattleComplete());
        InitializeDeck();
  //      data
        ChangeState(BattleState.PlayerDraw);
        Decknum.text = deck.Count.ToString();
        Dpnum.text = "0";
    }
    // 初始化函数

    public void InitializeDeck()
    {
        foreach (int id in data.playerinfo.deck)
        {
            Card newcard = cardManager.CreateCard(id, cardManager.transform);
            discardPile.Add(newcard);
            CardIntoHand(newcard);
        }
       /* for (int i = 1; i <= 3; i++)
        {
            Card newcard = cardManager.CreateCard(20  ,cardManager.transform );
            discardPile.Add(newcard);
            CardIntoHand(newcard);
        }
        Card newcard1 = cardManager.CreateCard(21, cardManager.transform);
        discardPile.Add(newcard1);
        CardIntoHand(newcard1);

        Card newcard2 = cardManager.CreateCard(33, cardManager.transform);
        discardPile.Add(newcard2);
        CardIntoHand(newcard2);

        Card newcard3 = cardManager.CreateCard(32, cardManager.transform);
        discardPile.Add(newcard3);
        CardIntoHand(newcard3);
       */
        ShuffleDeck(discardPile);
    }

    public void FindAllEnemies()
    {
        _enemies = new List<EnemyAIController>(FindObjectsOfType<EnemyAIController>());
    }

    public void CardIntoHand(Card card) //只有初始化 衍生物 boss塞牌才调用 用于绑定事件
    {
        CardController cardController = card.GetComponent<CardController>();
        cardController.OnCardUsed += PlayCard;
    }
    public void DrawCard(int num)
    {
        for (int i = 0;i < num;i ++)
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
            Debug.Log("抽牌调用开始");
            cardManager.AddCardToHand(drawnCard, hand);
            Debug.Log("抽牌调用结束");
            UpdatedeckNum();
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
        UserIndicator.text = "怪物回合开始了......";
        Debug.Log("怪物回合开始了!");

        yield return new WaitForSeconds(2f);
        FindAllEnemies();
        foreach(var enemy in _enemies)
         {
             if (enemy != null)
             {
                //判断麻痹效果
                if(enemy.enemybuff.Wound > 0)
                {
                    enemy.enemybuff.ModifyWound(enemy.enemybuff.Wound - 1);
                }
                yield return enemy.TakeTurn();
             }
        }
        UserIndicator.text = "怪物回合结束了!";
        Debug.Log("怪物回合结束了!");
        ChangeState(BattleState.PlayerDraw);
    }

    //变招前预检测
    private void BeforeChangeskill()
    {
        FindAllEnemies();
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                //判断是不是会变招的怪:蛮颚龙，岩贼龙，冰咒龙
                if ((enemy.ID==3 && (float)enemy._currentHealth * 1.25 < (float)enemy._maxHealth)
                    ||(enemy.ID == 5)|| (enemy.ID == 7))
                    enemy.skillSystem.ChangeSkillinRealtime(Player.PlayerGridPos);
            }
        }
    }

    private IEnumerator PlayerDrawPhase()
    {
        Debug.Log("draw waiting");
        UserIndicator.text = "抽牌阶段";
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
        Player.ModifyDefence(0);
        playerBuff.ModifyJQ(playerBuff.JQ - 1);
        playerBuff.ModifyBigJu(playerBuff.BigJu - 1);
        
        if(playerBuff.Numbness > 0)
        {
            playerBuff.ModifyNumbness(playerBuff.Numbness - 1);
            Player.ModifyCost(Player.curCost - 2);
        }

        if(playerBuff.DL > 0)
        {
            playerBuff.ModifyDL(playerBuff.DL - 1);
            DrawCard(1);
            Player.ModifyCost(Player.curCost + 1);
        }

        if(playerBuff.ExCost > 0)
        {
            Player.ModifyCost(Player.curCost + 1);
            playerBuff.ModifyExCost(playerBuff.ExCost - 1);
        }

        UserIndicator.text = "玩家回合";

        // UIManager.Instance.SetEndTurnButtonActive(true);

    }
    public void OnEndTurnButtonClicked()
    {
        if (isWaitingForPlayerChoose == true || currentState != BattleState.PlayerTurn) return;

        Debug.Log("玩家结束回合了");
        UserIndicator.text = "玩家回合结束";

        EndPlayerTurn();
        ChangeState(BattleState.EnemyTurn);
    }
    public void OnMoveButtonClicked()
    {
        if (currentState != BattleState.PlayerTurn || playerBuff.CantMove > 0) return;
        int MoveCost = Player.Situation + 1;
        if (Player.curCost < MoveCost) return;
        Debug.Log("移动指令被触发了");
        Player.ModifyCost(Player.curCost - MoveCost);
        isWaitingForPlayerChoose = true;
            Action<Vector2Int> callback1 = OnPositionChanged.Invoke;
            Action<Vector2Int> callback2 = OnDirectionChanged.Invoke;
            Action callback3 = BeforeChangeskill;//把他当成回调函数传出才能正确的使用新坐标！
            StartCoroutine(mapmanager.MoveCommand(GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }), Player.PlayerGridPos, new Vector2Int(1,1), callback1 , callback2,callback3));
        Player.ModifySituation(0);
    }

    public void CheckContent(Vector2Int Pos) 
    {
        bool exist = false;
        GameConfig.Content type = mapmanager.StepContent(Pos, out exist);
        if (exist == false) return;
        switch (type)
        {
            case GameConfig.Content.LuCao:
                Player.ModifyHealth(Player.curHealth + 10);
                mapmanager.GetHexagon(Pos).GetComponent<Hexagon>().ContentRemove();
                break;
            case GameConfig.Content.Trap:
                playerBuff.ModifyCantMove(playerBuff.CantMove + 1);
                mapmanager.GetHexagon(Pos).GetComponent<Hexagon>().ContentRemove();
                break;
            case GameConfig.Content.Frog:
                playerBuff.ModifyNumbness(playerBuff.Numbness + 1);
                mapmanager.GetHexagon(Pos).GetComponent<Hexagon>().ContentRemove();
                break;
            case GameConfig.Content.DuCao:
                playerBuff.ModifyPoison(playerBuff.Poison + 3);
                mapmanager.GetHexagon(Pos).GetComponent<Hexagon>().ContentRemove();
                break;
            case GameConfig.Content.NaiLiBug:
                playerBuff.ModifyExCost(playerBuff.ExCost + 2);
                mapmanager.GetHexagon(Pos).GetComponent<Hexagon>().ContentRemove();
                break;
        }
    }
    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn) return;

        if(playerBuff.Poison > 0)
        {
            playerBuff.ModifyPoison(playerBuff.Poison - 1);
            Player.ModifyHealth(Player.curHealth - (int)(Player.MaxHealth * 0.02));
        }

        if(playerBuff.CantMove > 0)
        {
            playerBuff.ModifyCantMove(playerBuff.CantMove - 1);
        }


        foreach(Card card in hand)
        {
            card.transform.position += new Vector3(10000, 0, 0);
        }
        foreach (Card card in hand)
        {
            if(card.Nothingness == false)
            discardPile.Add(card);
            else
            {
                CardController cardController = card.GetComponent<CardController>();
                cardController.OnCardUsed -= PlayCard;
                cardManager.ReturnCardToPool(card);
            }
        }
        hand.Clear();
    //    Debug.Log("回合结束时手牌数为:" + hand.Count);
        cardManager.UpdateCardPositions(hand);
        UpdatedeckNum();
    }
    public IEnumerator ConsumeCoRoutine(Card card)
    {
        if(card.Sequence == false)
        {
            if (card.AttackDirection != null)
            {
                UserIndicator.text = "请选择攻击方向";
                isWaitingForPlayerChoose = true;
                Action<Vector2Int> callback = OnDirectionChanged.Invoke;
                List<int> newDir = card.AttackDirection;
                List<int> AllDir = new List<int> { 0, 1, 2, 3, 4, 5 };
                if (Player.Situation == 0) newDir = AllDir;
                yield return StartCoroutine(mapmanager.AttackCommand(GetAdjacent(newDir), Player.PlayerGridPos, new(1, card.AttackLength), callback, card));
                if (card.EnterState == 1 || (card.EnterState == 0 && Player.Situation == 0))
                    Player.ModifySituation(0);
                else if (card.EnterState == 2 || (card.EnterState == 0 && Player.Situation == 1))
                    Player.ModifySituation(1);
            }
        }
        if (card.Move != null && playerBuff.CantMove == 0)
        {
            //  OnPositionChange?.Invoke(this, new PlayerWantMoveEventArgs(GetAdjacent(card.Move) , /*card.MoveDistance*/1));
            isWaitingForPlayerChoose = true;
            Action<Vector2Int> callback1 = OnPositionChanged.Invoke;
            Action<Vector2Int> callback2 = OnDirectionChanged.Invoke;
            Action callback3 = BeforeChangeskill;
            List<int> newDir = card.Move;
            List<int> AllDir = new List<int> { 0, 1, 2, 3, 4, 5 };
            if (Player.Situation == 0) newDir = AllDir;
            if (card.EnterState == 1 || (card.EnterState == 0 && Player.Situation == 0))
                Player.ModifySituation(0);
            else if (card.EnterState == 2 || (card.EnterState == 0 && Player.Situation == 1))
                Player.ModifySituation(1);
            yield return StartCoroutine(mapmanager.MoveCommand(GetAdjacent(newDir), Player.PlayerGridPos, card.MoveLength, callback1, callback2, callback3));
            BeforeChangeskill();
        }

        if (card.AttackDirection != null && card.Sequence == true)
        {
            UserIndicator.text = "请选择攻击方向";
            isWaitingForPlayerChoose = true;
            Action<Vector2Int> callback = OnDirectionChanged.Invoke;
            List<int> newDir = card.AttackDirection;
            List<int> AllDir = new List<int> { 0, 1, 2, 3, 4, 5 };
            if (Player.Situation == 0) newDir = AllDir;
            yield return StartCoroutine(mapmanager.AttackCommand(GetAdjacent(newDir), Player.PlayerGridPos, new(1, card.AttackLength), callback , card));
            if (card.EnterState == 1 || (card.EnterState == 0 && Player.Situation == 0))
                Player.ModifySituation(0);
            else if (card.EnterState == 2 || (card.EnterState == 0 && Player.Situation == 1))
                Player.ModifySituation(1);
        }

        if (card.Buff != null)
        {
            foreach (int Buf_id in card.Buff)
            {
                switch (Buf_id)
                {
                    case 1:
                        playerBuff.ModifyPower(playerBuff.Power + 1);
                        break;
                    case 2:
                        playerBuff.ModifyJQ(playerBuff.JQ + 1);
                        break;
                    case 3:
                        playerBuff.ModifyBuffer(playerBuff.Buffer + 1);
                        break;
                    case 4:
                        playerBuff.ModifyBigJu(playerBuff.BigJu + 1);
                        break;
                    case 5:
                        playerBuff.ModifyPoison(playerBuff.Poison + 1);
                        break;
                    case 6:
                        playerBuff.ModifyNumbness(playerBuff.Numbness + 1);
                        break;
                    case 7:
                        playerBuff.ModifyCantMove(playerBuff.CantMove + 1);
                        break;
                    case 8:
                        playerBuff.ModifyDL(playerBuff.DL + 1);
                        break;
                    case 9:
                        playerBuff.ModifyExCost(playerBuff.ExCost + 1);
                        break;
                    case 10:
                        playerBuff.ModifyBladeShield(playerBuff.BladeShield + 1);
                        break;
                    case 11:
                        playerBuff.ModifyJD(playerBuff.JD + 1);
                        break;
                    case 12:
                        playerBuff.ModifyWoundManage(playerBuff.WoundManage + 1);
                        break;
                    case 13:
                        playerBuff.ModifyRedBladeCrazy(playerBuff.RedBladeCrazy + 1);
                        break;
                    case 14:
                        playerBuff.ModifyNextDL(playerBuff.NextDL + 1);
                        break;
                    case 15:
                        //特判了,下一次伤害的额外值
                        break;
                }
            }

        }
        if(card.DeltaHealth != 0)
        {
            Player.ModifyHealth(Player.curHealth + card.DeltaHealth);
        }

        if(card.DeltaCost != 0)
        {
            Player.ModifyCost(Player.curCost + card.DeltaCost);
        }

        if (card.DeltaBladeNum >= 0)
        {
            Player.ModifyBladeNum(Player.curBladeNum + card.DeltaBladeNum);
            if (card.cardNum == 32)
            {
                playerBuff.ModifyNextDamage(Player.curBladeNum * 2);
                Player.ModifyBladeNum(0);
            }
            OnBladeGasChange?.Invoke(Player.curBladeNum);
        }

        if (card.DeltaBladeLevel != 0)
        {
            if(card.DeltaBladeLevel > 0 && playerBuff.BladeShield > 0 && Player.curBladeLevel < 3) Player.ModifyDefence(Player.Defence + 4);
            Player.ModifyBladeLevel(Player.curBladeLevel + card.DeltaBladeLevel);
            OnBladeLevelChange?.Invoke(Player.curBladeLevel);
        }
        if (card.Derivation != 0)
        {
            Card newCard = cardManager.CreateCard(card.Derivation, cardManager.transform);
            Debug.Log("条件为" + (card.cardNum) + " " + Player.curBladeLevel);
            if (hand.Count < GameConfig.MaxHandCardNum && 
                (card.cardNum != 20 || Player.curBladeLevel != 0) && 
                (card.cardNum != 24 || Player.curBladeLevel != 0))
            {
                hand.Add(newCard);
                CardIntoHand(newCard);
                cardManager.UpdateCardPositions(hand);
            }
        }
       
       if(card.DrawCard != 0)
       {
            DrawCard(card.DrawCard);
       }

       if(card.Defence != 0 && (playerBuff.RedBladeCrazy == 0 || Player.curBladeLevel != 3))
       {
            Player.ModifyDefence(Player.Defence + card.Defence);
       }

       
      // AudioManager.PlayCardPlaySound(card.cardNum);
       cardManager.RemoveCardFromHand(card, hand);
       card.transform.position += new Vector3(10000, 0, 0);
        if (card.Consumption == true)//消耗判断
        {
            CardController cardController = card.GetComponent<CardController>();
            cardController.OnCardUsed -= PlayCard;
            cardManager.ReturnCardToPool(card);
        }
        else discardPile.Add(card);
        UpdatedeckNum();
       UserIndicator.text = "玩家回合";
      

       if(card.cardNum == 11 || card.cardNum == 22)
       {
            if(playerBuff.JD > 0)
            playerBuff.ModifyNextDL(playerBuff.NextDL + 1);
       }

       if(card.cardNum == 21)
       {
            playerBuff.ModifyNextDL(0);
       }

       if(card.cardNum == 22)
       {
            OnEndTurnButtonClicked();
       }

    }
  /*  public int CalculateAttack(Card card)
    {
        float BladeLevelBuff = 1;
        switch (Player.curBladeLevel)
        {
            case 0:
                BladeLevelBuff = 1;
                break;
            case 1:
                BladeLevelBuff = 1.1f;
                break;
            case 2:
                BladeLevelBuff = 1.3f;
                break;
            case 3:
                BladeLevelBuff = 1.6f;
                break;
        }
        int res = 1;
        res = (int)((card.Attack.x + playerBuff.Power )* BladeLevelBuff) * card.Attack.y;
        return res;
    }*/
    public int CalculateAttack(Vector2Int Attack , EnemyAIController enemy)
    {
        float BladeLevelBuff = 1;
        switch (Player.curBladeLevel)
        {
            case 0:
                BladeLevelBuff = 1;
                break;
            case 1:
                BladeLevelBuff = 1.1f;
                break;
            case 2:
                BladeLevelBuff = 1.3f;
                break;
            case 3:
                BladeLevelBuff = (playerBuff.RedBladeCrazy > 0 ? 2.1f : 1.6f);
                break;
        }
        int res = 1;
        res = (int)((Attack.x + playerBuff.Power) * BladeLevelBuff * Attack.y * (enemy.enemybuff.Wound > 0 ? 1.5f : 1.0f));
        
        return res;
    }
    public void PlayCard(Card card)
    {
        if (currentState != BattleState.PlayerTurn ) return;
        if (Player.curCost < card.Cost)return;

        Player.ModifyCost(Player.curCost - card.Cost);
        Debug.Log("还剩" + Player.curCost.ToString() + "费！");
        


        StartCoroutine(ConsumeCoRoutine(card));
        
    } 
    public void UpdateCards()
    {
        foreach(Card card in hand)
        {
            if(card.cardNum == 21)
            {
                if (playerBuff.NextDL > 0)
                    card.ChangeCost(1);
                else card.ChangeCost(2);
            }

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
    public void AttackConsume(Card card) //伤害结算
    {
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
            List<EnemyAIController> AttackedMonster = new();
            int Length = card.AttackLength;
            List<int> AttackRange = card.AttackRange;
            foreach (int Dir_id in AttackRange)
            {
                for (int i = 0; i <= Length; i++)
                {
                    int newDir = (Dir_id + PlayerDirId) % 6;
                    Vector2Int nowPos = Player.PlayerGridPos + new Vector2Int(dx[newDir] * i, dy[newDir] * i);
                    foreach (EnemyAIController enemyAI in _enemies)
                    {
                        if (enemyAI.GetCurrentGridPos() == nowPos && !AttackedMonster.Contains(enemyAI))
                        {
                            AttackedMonster.Add(enemyAI);
                        }
                    }
                }
            }
            foreach (EnemyAIController enemy in AttackedMonster)
            {
                Vector2Int Attack = card.Attack;
                if (card.cardNum == 21)
                {
                    Attack.y += Player.curBladeLevel;
                }
                int atknum = CalculateAttack(Attack, enemy);
                if(atknum > 0)
                {
                    atknum += playerBuff.NextDamage;
                    playerBuff.ModifyNextDamage(0);
                }
                enemy.ReduceHealth(atknum);
                if (card.cardNum == 35 && enemy.enemybuff.Wound >= 3)
                {
                    enemy.enemybuff.ModifyWound(enemy.enemybuff.Wound - 3);
                    enemy.ReduceHealth(15);
                }
                if (card.Wound > 0)
                {
                    enemy.enemybuff.ModifyWound(enemy.enemybuff.Wound + card.Wound + playerBuff.WoundManage);
                }
            }
        }
    }
    //给怪物检测攻击范围内有无玩家
    public List<PlayerInfo> GetTargetsInRange(List<Vector2Int> actualrangepos)
    {
        List<PlayerInfo> players = new List<PlayerInfo>();
        foreach (Vector2Int pos in actualrangepos)
        {
            if (Player.PlayerGridPos == pos)
                players.Add(Player);
        }
        return players;
    }
    //怪物对玩家造成伤害
    public void ApplyDamage(PlayerInfo target, int damage, EnemyAIController origin)
    {
        if(playerBuff.BigJu > 0)
        {
            playerBuff.ModifyBigJu(playerBuff.BigJu - 1);
            bool CanMove = true;
            Vector2Int newPos = Player.PlayerGridPos + Player.Direction;
            if (mapmanager.isObstacle(newPos) == true) CanMove = false;
            FindAllEnemies();
            foreach (EnemyAIController enemy in _enemies)
            {
                if (enemy.GetCurrentGridPos() == newPos) CanMove = false;
            }
            if (CanMove) OnPositionChanged?.Invoke(newPos);
            origin.ReduceHealth(CalculateAttack(new(5 , 4) , origin));
            OnBladeLevelChange?.Invoke(Player.curBladeLevel + 1);
            AudioManager.PlayCardSpecialSound(22);
            return;
        }

        if(playerBuff.JQ > 0) //见切判断
        {
            playerBuff.ModifyJQ(playerBuff.JQ - 1);
            Card newCard = cardManager.CreateCard(5 , cardManager.transform);
            if (hand.Count < GameConfig.MaxHandCardNum)
            {
                hand.Add(newCard);
                CardIntoHand(newCard);
                cardManager.UpdateCardPositions(hand);
            }
            OnBladeGasChange?.Invoke(GameConfig.MaxBladeNum );
         //   Player.ModifyBladeNum(GameConfig.MaxBladeNum - Player.curBladeNum);
            return;
        }
        if(playerBuff.Buffer > 0) //缓冲判断
        {
            playerBuff.ModifyBuffer(playerBuff.Buffer - 1);
            return;
        }
        int temphealth = Player.curHealth - Math.Max(damage - Player.Defence , 0);
        Player.ModifyDefence(Math.Max(Player.Defence - damage, 0));
        Player.ModifyHealth(temphealth);
    }

    //怪物对玩家施加debuff
    public void ApplyDebuff(PlayerInfo target, GameConfig.EnemyDebuff debuff, EnemyAIController origin)
    {
        Debug.Log("玩家被施加了负面效果！");
        if (debuff == GameConfig.EnemyDebuff.CantMove) 
            playerBuff.CantMove++;
        else if  (debuff == GameConfig.EnemyDebuff.Numbness)
            playerBuff.Numbness++;
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

    //检测玩家是否在当前的攻击范围里
    public bool PlayerInRange(List<Vector2Int> actualrangepos)
    {
        bool res = false;
        foreach (Vector2Int pos in actualrangepos)
        {
            if (pos == Player.PlayerGridPos)
            {
                res = true;
            }
        }
        return res;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCards();
    }
}
