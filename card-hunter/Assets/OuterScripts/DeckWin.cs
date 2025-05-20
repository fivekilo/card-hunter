using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckWin : MonoBehaviour
{
    public GameObject cardWin;
    public event Action<int> DeleteConfirm;
    private List<GameObject> Cards=new List<GameObject>();
    private int ChosedNum=-1;

    public void Show(List<int>deck)
    {
        int x = -GameConfig.XBound;
        int y = GameConfig.YBound;
        int idx = 0;
        foreach(int i in deck)
        {
            GameObject card = Instantiate(cardWin, new Vector3(x,y,0)+transform.position, Quaternion.identity,transform);
            Cards.Add(card);
            card.GetComponent<Card>().cardNum = i;
            card.GetComponent<Card>().cardInit();
            card.GetComponent<CardWin>().num = idx;
            idx++;
            if (x + GameConfig.Xdelta <= GameConfig.XBound)
            {
                x = x + GameConfig.Xdelta;
            }
            else//����
            {
                x = -GameConfig.XBound;
                y -= GameConfig.Ydelta;
            }
        }
    }

    public void ShowDelete(List<int>deck)
    {
        Show(deck);
        //��ѡ���¼�
        foreach(GameObject card in Cards)
        {
            card.GetComponent<CardWin>().Clicked += ClickHandler;
        }
        //��ȷ�ϰ�ť
        transform.Find("Btn").gameObject.GetComponent<ConfirmBtn>().Confirm += ConfirmHandle;
    }

    private void ConfirmHandle()
    {
        if (ChosedNum != -1)
        {
            DeleteConfirm?.Invoke(ChosedNum);
        }
    }

    private void ClickHandler(int num)
    {
        //ѡ�п����Ч��
        foreach (GameObject card in Cards)
        {
            card.transform.localScale = GameConfig.normalScale;
        }
        Cards[num].transform.localScale = GameConfig.hoverScale;
        //��¼����
        ChosedNum = Cards[num].GetComponent<Card>().cardNum;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0 && Cards[0].transform.localPosition.y >= -GameConfig.YBound)
        {
            
            foreach (GameObject card in Cards)
            {
                card.transform.position += new Vector3(0, -20, 0);
            }
        }
        else if(scrollInput < 0 && Cards[Cards.Count - 1].transform.localPosition.y <= GameConfig.YBound)
        {
            foreach (GameObject card in Cards)
            {
                card.transform.position += new Vector3(0, 20, 0);
            }
        }
    }
}
