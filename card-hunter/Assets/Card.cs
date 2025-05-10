using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{

    public Canvas parentCanvas;
    //public int cardRarity { get; set; }
    public string cardText;
    public string cardName;
    public string cardType;

    //卡片基本属性
    public int cardNum { get; set; }
    public Vector2Int Move; //引发的位移
    public int Cost;
    public int Attack;
    public int Defence;
    public int DeltaCost;//卡牌的回费效果
    public int DeltaBladeNum;//引发的气刃槽改变
    public int DeltaBladeLevel; //引发的气刃等级改变
    public int DeltaHealth; //引发的血量变化
    public Card(string cardText, string cardName, string cardType, int cardNum, Vector2Int move, int cost, int attack, int defence, int deltaCost, int deltaBladeNum, int deltaBladeLevel, int deltaHealth)
    {
        this.cardText = cardText;
        this.cardName = cardName;
        this.cardType = cardType;
        this.cardNum = cardNum;
        Move = move;
        Cost = cost;
        Attack = attack;
        Defence = defence;
        DeltaCost = deltaCost;
        DeltaBladeNum = deltaBladeNum;
        DeltaBladeLevel = deltaBladeLevel;
        DeltaHealth = deltaHealth;
    }

    private bool isDragging = false;
    private Vector3 originalPosition;
    private TextMeshProUGUI cardTextBox;
    private TextMeshProUGUI cardNameBox;
    private TextMeshProUGUI cardTypeBox;
    void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = "测试中卡名";
        cardText = "测试中卡牌描述";
        cardType = "类1";
    }
     void cardTextsInit (int cardNum){
       
        FindText(cardNum, ref cardName,ref cardText,ref cardType);
        GameObject Target = GameObject.Find("cardText");
        cardTextBox = Target.GetComponent<TextMeshProUGUI>();
        Target = GameObject.Find("cardName");
        cardNameBox= Target.GetComponent<TextMeshProUGUI>();
        Target = GameObject.Find("cardType");
        cardTypeBox= Target.GetComponent<TextMeshProUGUI>();
        cardTypeBox.text = cardType;
        cardTextBox.text = cardText;
        cardNameBox.text = cardName;
        Target = GameObject.Find("Canvas");
        parentCanvas = Target.GetComponent<Canvas>();
    }
    int GetcardRarity(int cardNum)
    {
        return cardNum;
    }
    void cardFrameworkInit(int cardNum)
    {
        Sprite newSprite;
        int cardRarity = GetcardRarity(cardNum);
        //SpriteRenderer cardFramework= GetComponentsInChildren<SpriteRenderer>()[0];
        Image cardFramework = GetComponentsInChildren<Image>()[0];
        switch (cardRarity)
        {
            case 0:
                newSprite = Resources.Load<Sprite>("cardbottom");
                break;
            case 1:
                newSprite = Resources.Load<Sprite>("cardbottom_blue");
                break;
            case 2:
                newSprite = Resources.Load<Sprite>("cardbottom_gold");
                break;
            default:
                newSprite = Resources.Load<Sprite>("cardbottom");
                break;
        }

                cardFramework.sprite = newSprite;
    }
    public void cardInit()
    {
        cardTextsInit(cardNum);
        cardFrameworkInit(cardNum);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card clicked: " + cardNum);
        // 卡牌被点击时的逻辑
    }


    //Start is called before the first frame update
    void Start()
    {
        //cardInit();
        //Debug.Log("Card clicked: " + cardNum);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
