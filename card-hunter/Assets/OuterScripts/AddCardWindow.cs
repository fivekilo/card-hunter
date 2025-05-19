using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddCardWindow : MonoBehaviour
{
    private int ChosedNum;
    private Action<int> addCard;
    private List<GameObject> Cards=new List<GameObject>();
    public void AddCard(Action<int>addCard)//�ӿں���
    {
        this.addCard = addCard;
        //���ȡ������
        HashSet<int> Set = new HashSet<int>();
        System.Random rand = new System.Random();
        while (Set.Count < 3)
        {
            Set.Add(rand.Next(1, GameConfig.CardName.Count));
        }
        List<int> CardNum = Set.ToList();
        for(int i = 1; i < 4; i++)
        {
            Cards.Add(transform.Find("Panel/card"+i).gameObject);
        }
        //���Ӵ������͵���¼� ��ʼ������
        for(int i=0;i<3;i++)
        {
            string name = "card " + i;
            Debug.Log(name);
            Cards[i].GetComponent<Card>().cardNum = CardNum[i];
            Cards[i].GetComponent<Card>().cardInit();
            Cards[i].GetComponent<CardWin>().Clicked += ClickHandler;
        }
        //����ȷ�Ϻ���
        transform.Find("Panel/ConfirmBtn").gameObject.GetComponent<ConfirmBtn>().Confirm += Confirm;
    }
    private void Confirm()
    {
        addCard?.Invoke(ChosedNum);
        //���ٵ�ǰ����
        Destroy(this.gameObject);
    }
    private void ClickHandler(int num)
    {
        //ѡ�п���ɫЧ��
        foreach(GameObject card in Cards)
        {
            card.transform.Find("card Canvas/cardImage").gameObject.GetComponent<Image>().color = Color.white;
        }
        Cards[num].transform.Find("card Canvas/cardImage").gameObject.GetComponent<Image>().color = Color.blue;
        //��¼����
        ChosedNum = num;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
