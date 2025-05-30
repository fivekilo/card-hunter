using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private float cardSpacing = 150f; // ���Ƽ��
    public float arcHeight = 50f;    // ���θ߶�
    public float maxRotation = 15f;  // �����ת�Ƕ�
   // public float moveDuration = 0.3f; // �ƶ�����ʱ��
    public GameObject card;
    public CardPool cardpool;
    public int originalsortingOrder;
    //private List<Card> cards;
    public Card CreateCard(int cardNum, Transform parent)
    {
        ////GameObject newCard = Instantiate(card,parent);
        //GameObject newCard = cardpool.GetCard(cardNum);
        //newCard.transform.position = new Vector3(0, -300, 0) + transform.position;
        //Card carddata = newCard.GetComponent<Card>();
        ////carddata.cardNum = cardNum;
        ////carddata.cardInit();
        //return carddata;
        //// Start is called before the first frame update
        ///
        if (cardpool == null)
        {
            Debug.Log("CardPool is not assigned!");
            return null;
        }

        // 2. 从卡牌池获取卡牌
        GameObject newCard = cardpool.GetCard(cardNum);
        if (newCard == null)
        {
            Debug.Log($"Card {cardNum} not found in pool!");
            return null;
        }

        // 3. 设置卡牌位置
        newCard.transform.position = new Vector3(0, -300, 0) + transform.position;

        // 4. 获取 Card 组件
        Card carddata = newCard.GetComponent<Card>();
        if (carddata == null)
        {
            Debug.Log("No Card component on the card prefab!");
            return null;
        }

        return carddata;
    }
    public void ReturnCardToPool(Card card)
    {
        cardpool.ReturnCard(card.gameObject);
    }
    public void AddCardToHand(Card card, List<Card> cardsInHand)
    {
        if (!cardsInHand.Contains(card))
        {
            cardsInHand.Add(card);
            UpdateCardPositions(cardsInHand);
        }
    }
    public void RemoveCardFromHand(Card card, List<Card> cardsInHand)
    {
        if (cardsInHand.Contains(card))
        {
            cardsInHand.Remove(card);
            UpdateCardPositions(cardsInHand);
        }
    }

    public void UpdateCardPositions(List<Card> cardsInHand)
    {
        float totalWidth = (cardsInHand.Count - 1) * cardSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Card card = cardsInHand[i];
            card.GetComponentInChildren<Canvas>().sortingOrder = i;
            float xPos = startX + i * cardSpacing;
            card.transform.position = new Vector3(xPos, 0, 0)+transform.position;
        }
    }
    private void Awake()
    {
        //cardpool = GetComponent<CardPool>();
    }
    void Start()
    {
        originalsortingOrder = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
