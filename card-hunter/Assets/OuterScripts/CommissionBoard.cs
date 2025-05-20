using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class CommissionBoard : MonoBehaviour
{
    public GameObject Leaflet;
    public event Action<Commission> Confirm;
    private List<GameObject> leaves = new List<GameObject>();
    private List<Commission> Commissions = new List<Commission>();
    private int ChosedNum = -1;
    public void Init(List<Commission>commissions)
    {
        Commissions = commissions;
        if (commissions.Count > 3)
        {
            throw new System.Exception("ί��������");
        }
        int x = 0;
        foreach(Commission c in commissions)
        {
            GameObject leaf = Instantiate(Leaflet, new Vector3(GameConfig.BoardX+x*GameConfig.BoardX, 0, 0) + transform.position, Quaternion.identity, transform);
            leaves.Add(leaf);
            leaf.GetComponent<Leaflet>().Init(c);
            leaf.GetComponent<Leaflet>().num=x++;
            leaf.GetComponent<Leaflet>().Clicked += ClickHandle;
        }
    }

    private void ClickHandle(int num)
    {
        //ѡ�п����Ч��
        foreach (GameObject _ in leaves)
        {
            _.transform.localScale = new Vector3(1,1,1);
        }
        leaves[num].transform.localScale = new Vector3(1.1f,1.1f,1);
        //��¼����
        ChosedNum = leaves[num].GetComponent<Leaflet>().num;
    }
    private void ConfirmHandle()
    {
        if (ChosedNum != -1)
        {
            Confirm?.Invoke(Commissions[ChosedNum]);
        }
    }

    void Start()
    {
        transform.Find("Btn").GetComponent<ConfirmBtn>().Confirm += ConfirmHandle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
