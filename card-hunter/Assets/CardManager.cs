using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CardManager : MonoBehaviour
{
    public float cardSpacing = 100f; // 卡牌间距
   // public float moveDuration = 0.3f; // 移动动画时间
    public GameObject card;
   // public CardPool cardpool;
    public int originalsortingOrder;
    //private List<Card> cards;
    public Card CreateCard(int cardNum, Transform parent)
    {
        GameObject newCard = Instantiate(card,parent);
        //GameObject newCard = cardpool.GetCard(cardNum);
        newCard.transform.position = new Vector3(0, -300, 0) + transform.position;
        Card carddata = newCard.GetComponent<Card>();
        carddata.cardNum = cardNum;
        carddata.cardInit();
        return carddata;
        // Start is called before the first frame update
    }
    public void AddCardToHand(Card card,List<Card> cardsInHand)
    {
        if (!cardsInHand.Contains(card))
        {
            cardsInHand.Add(card);
            //card.transform.SetParent(transform);
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

    private void UpdateCardPositions(List<Card> cardsInHand)
    {
        float totalWidth = (cardsInHand.Count - 1) * cardSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            Card card = cardsInHand[i];
            card.GetComponentInChildren<Canvas>().sortingOrder = i;
            float ratio = (float)i / (cardsInHand.Count - 1);
            float xPos = startX + i * cardSpacing;
            // 移动卡牌
            card.transform.position = new Vector3(xPos, 0, 0)+transform.position;

        }
    }
    void Start()
    {
        originalsortingOrder = 0;
     //   cardpool = GetComponent<CardPool>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
