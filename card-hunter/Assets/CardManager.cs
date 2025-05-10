using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CardManager : MonoBehaviour
{
    public GameObject card;
    private List<Card> cards;
    public void CreateCard(int cardNum, Transform parent)
    {
        GameObject newCard = Instantiate(card, parent);
        cards.Add( newCard.GetComponent<Card>());
        cards[cards.Count].cardNum = cardNum;
        cards[cards.Count].cardInit();
        // Start is called before the first frame update
    }
        void Start()
    {
        CreateCard(1, transform);
        CreateCard(2, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
