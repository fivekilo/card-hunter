using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camp : MonoBehaviour
{
    public event Action ClickEvent;
    public Vector3 EndPoint(int num)//输入编号返回端点 1-4 左上开始顺时针
    {
        Vector2 size = GetComponent<SpriteRenderer>().bounds.size;
        Vector3[]ends = new Vector3[5];
        ends[0] = transform.position;
        ends[1]=transform.position+new Vector3(-size.x/2,size.y/2,0);
        ends[2] = transform.position + new Vector3(size.x / 2, size.y / 2, 0);
        ends[3] = transform.position + new Vector3(size.x / 2, -size.y / 2, 0);
        ends[4] = transform.position + new Vector3(-size.x / 2, -size.y / 2, 0);
        return ends[num];
    }
    private void OnMouseDown()
    {
        ClickEvent?.Invoke();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
