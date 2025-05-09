using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int cardNum { get; set; }
    public Canvas parentCanvas;
    //public int cardRarity { get; set; }
    public string cardText;
    public string cardName;
    public string cardType;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private TextMeshProUGUI cardTextBox;
    private TextMeshProUGUI cardNameBox;
    private TextMeshProUGUI cardTypeBox;
    void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = "≤‚ ‘÷–ø®√˚";
        cardText = "≤‚ ‘÷–ø®≈∆√Ë ˆ";
        cardType = "¿‡1";
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
        // ø®≈∆±ªµ„ª˜ ±µƒ¬ﬂº≠
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
