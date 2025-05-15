using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CardManager : MonoBehaviour
{
    public float cardSpacing = 100f; // ���Ƽ��
    public float arcHeight = 50f;    // ���θ߶�
    public float maxRotation = 15f;  // �����ת�Ƕ�
   // public float moveDuration = 0.3f; // �ƶ�����ʱ��
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
            //float ratio = (float)i / (cardsInHand.Count - 1);
            float xPos = startX + i * cardSpacing;

            // ���㻡��λ��
            //float yPos = -arcHeight * Mathf.Sin(ratio * Mathf.PI);

            // ������ת�Ƕ�
            //float rotation = Mathf.Lerp(-maxRotation, maxRotation, ratio);

            // ʹ�ö����ƶ�����
            //card.transform.Translate(new Vector3(xPos, 0, 0));
            card.transform.position = new Vector3(xPos, 0, 0)+transform.position;
            //card.transform.Rotate(new Vector3(0, 0, rotation));

            // ���ò㼶��ʹ�м俨����������
            //card.transform.SetSiblingIndex(i);
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
