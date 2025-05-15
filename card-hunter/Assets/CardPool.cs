using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    public GameObject cardPrefab;
    public int copiesPerCard = 1; // 每种卡牌保留的副本数
    public int initialCardTypes = 8; // 初始卡牌种类数量
    private Dictionary<int, Queue<GameObject>> cardPools;
    //private Dictionary<string, Card> cardDataMap;

    void Awake()
    {
        InitializeCardPools();
    }

    private void InitializeCardPools()
    {
        cardPools = new Dictionary<int, Queue<GameObject>>();
        //cardDataMap = new Dictionary<string, Card>();

        // 初始化示例卡牌数据
        for (int i = 1; i < initialCardTypes; i++)
        {
            RegisterCardType(i);
        }
    }

    public void RegisterCardType(int cardnum)
    {
        //if (cardPools.ContainsKey(cardData.cardId)) return;

        Queue<GameObject> cardQueue = new Queue<GameObject>();

        for (int i = 0; i < copiesPerCard; i++)
        {
            GameObject card = Instantiate(cardPrefab,transform);
            //card.transform.SetParent(transform);
            card.SetActive(false);

            // 设置卡牌数据
            Card carddata = card.GetComponent<Card>();
            if (carddata != null)
            {
                carddata.cardNum = cardnum;
                carddata.cardInit();
            }

            cardQueue.Enqueue(card);
        }

        cardPools.Add(cardnum, cardQueue);
        //cardDataMap.Add(carddata.cardId, cardData);
    }

    public GameObject GetCard(int cardnum)
    {
        if (!cardPools.ContainsKey(cardnum))
        {
            Debug.LogWarning($"Card ID {cardnum} not found in pool!");
            return null;
        }

        Queue<GameObject> pool = cardPools[cardnum];

        if (pool.Count > 0)
        {
            GameObject card = pool.Dequeue();
            card.SetActive(true);
            return card;
        }
        else
        {
            // 动态创建新卡牌（超出预设副本数）
            GameObject newCard = Instantiate(cardPrefab);
            Card newCardData = newCard.GetComponent<Card>();
            newCardData.cardNum=cardnum;
            newCardData.cardInit();
            return newCard;
        }
    }

    public void ReturnCard(GameObject card)
    {
        Card cardData = card.GetComponent<Card>();
        if (cardData == null) return;

        int cardNum = cardData.cardNum;

        if (cardPools.ContainsKey(cardNum))
        {
            card.SetActive(false);
            cardPools[cardNum].Enqueue(card);

        }
        else
        {
            Debug.LogWarning($"Returning unregistered card: {cardNum}");
            Destroy(card);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
