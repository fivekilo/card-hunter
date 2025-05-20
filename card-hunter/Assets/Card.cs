using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    //public string cardId;//创建的卡牌对象名称
    public Canvas parentCanvas;
    public string cardText;
    public string cardName;
    public string cardType;

    public int cardNum { get; set; }
    public List<int> Move; //移动方向 正前方0 逆时针标号 不移动为null
    public Vector2Int MoveLength;//移动长度 下上限
    public int Derivation;//派生卡牌
    public bool Consumption;//消耗
    public int DrawCard;//抽牌
    public bool Nothingness;//虚无
    public int OnlyLState;//0:限自由态 1:限连携态 2:不限
    public int EnterState;//1:进入自由态 2:进入连携态
    public List<int> Buff;//Buff
    public int Wound;//Wound
    public List<int> AttackDirection; //攻击方向
    public List<int> AttackRange;//攻击范围
    public int AttackLength;//攻击长度
    public int Cost;//费用
    public Vector2Int Attack;//攻击
    public int Defence;//格挡
    public int DeltaCost;//回费
    public int DeltaBladeNum;//回气刃槽
    public int DeltaBladeLevel; //提升气刃等级
    public int DeltaHealth; //生命值变化
    public bool Sequence;//0先攻击再移动 1先移动再攻击 默认为true
    //private TextMeshProUGUI cardTextBox;
    //private TextMeshProUGUI cardNameBox;
    //private TextMeshProUGUI cardTypeBox;
    public bool CBuse;//能否被使用 默认否
    private void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = GameConfig.CardName[cardNum];
        cardText = GameConfig.CardText[cardNum];
        switch (GameConfig.CardType[cardNum])
        {
            case 0:
                cardType = "攻击";
                break;
            case 1:
                cardType = "防御";
                break;
            case 2:
                cardType = "移动";
                break;
            case 3:
                cardType = "能力";
                break;
        }
    }
     private void cardTextsInit (int cardNum){
        TextMeshProUGUI cardTypeBox = GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI cardTextBox = GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI cardNameBox = GetComponentsInChildren<TextMeshProUGUI>()[2];
        TextMeshProUGUI cardCost = GetComponentsInChildren<TextMeshProUGUI>()[3];
        FindText(cardNum, ref cardName,ref cardText,ref cardType);
        cardTypeBox.text = cardType;
        cardTextBox.text = cardText;
        cardNameBox.text = cardName;
        cardCost.text = Cost.ToString();
    }
    private void cardFrameworkInit(int cardNum)
    {
        Sprite newSprite;
        //SpriteRenderer cardFramework= GetComponentsInChildren<SpriteRenderer>()[0];
        Image cardFramework = GetComponentsInChildren<Image>()[0];
        newSprite = Resources.Load<Sprite>(GameConfig.CardImageName[cardNum]);
        cardFramework.sprite = newSprite;
    }
    private void cardPropertyInit(int cardNum)
    {
        Move=GameConfig.Move[cardNum]; //移动方向
        MoveLength = GameConfig.MoveLength[cardNum];//移动长度 下上限
        Derivation = GameConfig.Derivation[cardNum];//派生卡牌 0为不派生
        Consumption = GameConfig.Consumption[cardNum];//消耗
        DrawCard = GameConfig.DrawCard[cardNum];//抽牌
        Nothingness = GameConfig.Nothingness[cardNum];//虚无
        OnlyLState = GameConfig.OnlyLState[cardNum];//0:不限 1:限自由态 2:限连携态
        EnterState = GameConfig.EnterState[cardNum];//1:进入自由态 2:进入连携态
        Buff = GameConfig.Buff[cardNum];//Buff
        Wound = GameConfig.Wound[cardNum];//Wound
        AttackDirection = GameConfig.AttackDirection[cardNum]; //攻击方向
        AttackLength = GameConfig.AttackLength[cardNum];//攻击长度
        Cost = GameConfig.Cost[cardNum];//费用
        Attack = GameConfig.Attack[cardNum];//攻击
        Defence = GameConfig.Defence[cardNum];//格挡
        DeltaCost = GameConfig.DeltaCost[cardNum];//回费
        DeltaBladeNum = GameConfig.DeltaBladeNum[cardNum];//回气刃槽
        DeltaBladeLevel = GameConfig.DeltaBladeLevel[cardNum]; //提升气刃等级
        DeltaHealth = GameConfig.DeltaHealth[cardNum]; //生命值变化
        Sequence = GameConfig.Sequence[cardNum];
    }
    public void cardInit()
    {        
        cardPropertyInit(cardNum);
        cardTextsInit(cardNum);
        cardFrameworkInit(cardNum);

        CBuse = true;
    }
    
 
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
}
