using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CardManager : MonoBehaviour
{
    public GameObject card;

    public void CreateCard(int cardNum, Transform parent)
    {
        GameObject newCard = Instantiate(card, parent);
        Card cardScript = newCard.GetComponent<Card>();
        cardScript.cardNum = cardNum;
        cardScript.cardInit();
        // Start is called before the first frame update
    }
        void Start()
    {
       // CreateCard(0, transform);
        //CreateCard(1,transform);
        CreateCard(2, transform);
       // Debug.Log("Card clicked: ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
