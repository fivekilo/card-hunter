using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardNum { get; set; }
    //public int cardRarity { get; set; }
    public string cardText;
    public string cardName;
    public string cardType;
    private bool isDragging = false;
    private Vector3 originalPosition;
    //Card(int cardNUm) { this.cardNum = cardNum; }
    private SpriteRenderer cardFramework;
    private SpriteRenderer cardImage;
    private TextMeshProUGUI cardTextBox;
    private TextMeshProUGUI cardNameBox;
    private TextMeshProUGUI cardTypeBox;
    void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = "�����п���";
        cardText = "�����п�������";
        cardType = "��1";
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
    }
    int GetcardRarity(int cardNum)
    {
        return cardNum;
    }
    void cardFrameworkInit(int cardNum)
    {
        Sprite newSprite;
        int cardRarity = GetcardRarity(cardNum);
        GameObject Target = GameObject.Find("cardFramework");
        cardFramework = Target.GetComponent<SpriteRenderer>();
        Target = GameObject.Find("cardImage");
        cardImage= Target.GetComponent<SpriteRenderer>();
        newSprite = Resources.Load<Sprite>("3");
        cardImage.sprite = newSprite;
        if (cardRarity == 0)
        {
            newSprite = Resources.Load<Sprite>("cardbottom");
            cardFramework.sprite = newSprite;
        }
        else if (cardRarity ==1)
        {
            newSprite = Resources.Load<Sprite>("cardbottom_blue");
            cardFramework.sprite = newSprite;
        }
        else if(cardRarity == 2)
        {
            newSprite = Resources.Load<Sprite>("cardbottom_god");
            cardFramework.sprite = newSprite;
        }
    }
    public void cardInit()
    {
        cardTextsInit(cardNum);
        cardFrameworkInit(cardNum);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Card clicked: " + cardNum);
        // ���Ʊ����ʱ���߼�
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        // ��߿��Ʋ㼶��ȷ���������������Ϸ�
        GetComponent<SpriteRenderer>().sortingOrder += 10;
    }

    // ��ק��
    public void OnDrag(PointerEventData eventData)
    {
        // ����Ļ����ת��Ϊ��������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0; // ȷ��z����Ϊ0
        transform.position = mousePos;
    }

    // ������ק
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        GetComponent<SpriteRenderer>().sortingOrder -= 10;

        // ����Ƿ��������Ч����
        if (!IsValidDropZone())
        {
            // ���������Ч���򣬷���ԭλ
            transform.position = originalPosition;
        }
    }

    private bool IsValidDropZone()
    {
        // ����ʵ�ּ���Ƿ�����Ч����������߼�
        // ����ʹ�����߼�����������
        return true; // ��ʱ����true
    }
    //Start is called before the first frame update
    void Start()
    {
        //cardInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
