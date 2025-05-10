using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
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

    //��Ƭ��������
    public int cardNum { get; set; }
    public List<int> Move; //������λ��
    public Vector2Int MoveLength;//λ�Ƶľ���,0���ǲ��ƶ� ������
    public int Derivation;//�����Ŀ��Ʊ��
    public bool MoveTurn;//�ƶ��Ƿ���ת��
    public bool Consumption;//����
    public int DrawCard;//�鼸����
    public bool Nothingness;//����
    public int OnlyLState;//0:���� 1:������̬ 2:����Я̬
    public int EnterState;//1:������̬ 2:����Я̬
    public List<int> Buff;//��õ�Buff�����ܶ����
    public List<int> DeBuff;//��õ�DeBuff
    public List<int> AttackDirection; //��������
    public int AttackLength;//��������
    public int Cost;//����
    public List<int> Attack;//�˺�
    public int Defence;//��
    public int DeltaWound;
    public int DeltaCost;//���ƵĻط�Ч��
    public int DeltaBladeNum;//���������в۸ı�
    public int DeltaBladeLevel; //���������еȼ��ı�
    public int DeltaHealth; //������Ѫ���仯

    private bool isDragging = false;
    private Vector3 originalPosition;
    //private TextMeshProUGUI cardTextBox;
    //private TextMeshProUGUI cardNameBox;
    //private TextMeshProUGUI cardTypeBox;
    void FindText(int cardNum,ref string cardName,ref string cardText,ref string cardType)
    
    {
        cardName = "�����п���";
        cardText = "�����п�������";
        cardType = "��1";
    }
     void cardTextsInit (int cardNum){
        TextMeshProUGUI cardTypeBox = GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextMeshProUGUI cardTextBox = GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI cardNameBox = GetComponentsInChildren<TextMeshProUGUI>()[2];
        FindText(cardNum, ref cardName,ref cardText,ref cardType);
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
        // ���Ʊ����ʱ���߼�
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
