using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public delegate IEnumerator EncounterEvent(Event e);
    public event EncounterEvent encounterEvent;
    public IEnumerator MoveTo(Vector3 pos,Event e)//这段路需处理事件e
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        float distance=Vector3.Distance(startPos, pos);
        if (e == null)//无需处理事件
        {
            while (elapsedTime < GameConfig.MoveDuration * distance)
            {
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / (GameConfig.MoveDuration * distance)) - new Vector3(0, 0, 1);//play置于地图上
                elapsedTime += Time.deltaTime;
                yield return null; // 等待下一帧
            }
        }
        else
        {
            System.Random rand = new System.Random();
            bool trigger = false;
            float EncounterTime = (float)rand.NextDouble()* GameConfig.MoveDuration * distance-0.01f;//留出一点时间,防止事件被跳过
            while (elapsedTime < GameConfig.MoveDuration * distance)
            {
                if (elapsedTime > EncounterTime&&!trigger)//触发事件并等待
                {
                    yield return encounterEvent?.Invoke(e);
                    trigger = true;//防止多次触发
                }
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / (GameConfig.MoveDuration * distance)) - new Vector3(0, 0, 1);//play置于地图上
                elapsedTime += Time.deltaTime;
                yield return null; // 等待下一帧
            }
        }

        transform.position = pos - new Vector3(0, 0, 1); // 确保最终位置准确
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
