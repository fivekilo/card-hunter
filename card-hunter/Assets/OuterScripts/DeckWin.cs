using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckWin : MonoBehaviour
{
    public GameObject cardWin;
    private List<GameObject> Cards=new List<GameObject>();
    public void Show(List<int>deck)
    {
        int x = -GameConfig.XBound;
        int y = GameConfig.YBound;
        foreach(int i in deck)
        {
            GameObject card = Instantiate(cardWin, new Vector3(x,y,0)+transform.position, Quaternion.identity,transform);
            Cards.Add(card);
            card.GetComponent<Card>().cardNum = i;
            card.GetComponent<Card>().cardInit();
            card.GetComponent<CardWin>().num = card.GetComponent<Card>().cardNum;
            if (x + GameConfig.Xdelta <= GameConfig.XBound)
            {
                x = x + GameConfig.Xdelta;
            }
            else//»»ÐÐ
            {
                x = -GameConfig.XBound;
                y -= GameConfig.Ydelta;
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0 && Cards[0].transform.position.y >= GameConfig.YBound)
        {
            
            foreach (GameObject card in Cards)
            {
                card.transform.position += new Vector3(0, -10, 0);
            }
        }
        else if(scrollInput < 0 && Cards[Cards.Count - 1].transform.position.y <= GameConfig.YBound)
        {
            foreach (GameObject card in Cards)
            {
                card.transform.position += new Vector3(0, 10, 0);
            }
        }
    }
}
