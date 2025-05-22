using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

public class column
{
    public bool IsCard;
    public int ID;
    public string image;
    public int Money;
    public column(bool IsCard,int ID, string image, int money) 
    {
        this.IsCard = IsCard;
        this.ID = ID;
        this.image = image;
        Money = money;
    }
}

public class Shop : MonoBehaviour
{
    public GameObject Column;
    public SharedData sharedData;
    public List<GameObject> Cols=new List<GameObject>();
    public event Action<int, bool> Purchase;
    public event Action Exit;
    public void Init(List<column> columns)
    {
        int y = GameConfig.ShopY;
        int x = GameConfig.ShopX;
        int idx = 0;
        transform.Find("Exit").GetComponent<ConfirmBtn>().Confirm += ExitHandle;
        foreach (column c in columns)
        {
            GameObject g=Instantiate(Column,new Vector3(x,y,0),Quaternion.identity);
            g.transform.parent= transform;
            g.transform.position += transform.position;
            Cols.Add(g);
            //设置图片
            Image image= g.transform.Find("Image").GetComponent<Image>();
            Sprite loadedSprite = Resources.Load<Sprite>(c.image);
            image.sprite = loadedSprite;
            //设置文本
            g.transform.Find("MoneyText").GetComponent<TextMeshProUGUI>().text = c.Money.ToString();
            if (!c.IsCard)//是装备
            {
                g.transform.Find("StuffText").GetComponent<TextMeshProUGUI>().text = GameConfig.Material[c.ID] + "装备";
                g.transform.Find("MaterialText").GetComponent<TextMeshProUGUI>().text = "需要" + GameConfig.Material[c.ID] +  "1个";
            }
            else
            {
                g.transform.Find("StuffText").GetComponent<TextMeshProUGUI>().text = GameConfig.CardName[c.ID];
            }
            g.GetComponent<ColumnWin>().num = idx++;
            g.GetComponent<ColumnWin>().c = c;

            y += GameConfig.ShopDeltaY;
            g.GetComponent<ColumnWin>().Clicked += ClickHandle;
        }
    }

    private void ClickHandle(column column,int num)
    {
        if (column.IsCard && sharedData.playerinfo.money >= column.Money)//买卡
        {
            Purchase?.Invoke(column.ID, column.IsCard);
            sharedData.playerinfo.money -= column.Money;
            GameObject c = Cols[num];
            Cols.Remove(c);
            Destroy(c);
            for (int i = num; i < Cols.Count; i++)
            {
                Cols[i].transform.position -= new Vector3(0, GameConfig.ShopDeltaY, 0);
                Cols[i].GetComponent<ColumnWin>().num--;
            }
        }

        if(!column.IsCard && sharedData.playerinfo.money >= column.Money && sharedData.playerinfo.Material[column.ID] > 0)
        {
            Purchase?.Invoke(column.ID, column.IsCard);
            sharedData.playerinfo.money -= column.Money;
            sharedData.playerinfo.Material[column.ID] -= 1;
            GameObject c = Cols[num];
            Cols.Remove(c);
            Destroy(c);
            for (int i = num; i < Cols.Count; i++)
            {
                Cols[i].transform.position -= new Vector3(0, GameConfig.ShopDeltaY, 0);
                Cols[i].GetComponent<ColumnWin>().num--;
            }
        }

    }

    private void ExitHandle()
    {
        Exit.Invoke();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0 && Cols[0].transform.localPosition.y >= -GameConfig.YBound)
        {

            foreach (GameObject card in Cols)
            {
                card.transform.position += new Vector3(0, -20, 0);
            }
        }
        else if (scrollInput < 0 && Cols[Cols.Count - 1].transform.localPosition.y <= GameConfig.YBound)
        {
            foreach (GameObject card in Cols)
            {
                card.transform.position += new Vector3(0, 20, 0);
            }
        }
    }
}
