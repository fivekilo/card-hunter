using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool EventHandling=false;
    public IEnumerator MoveTo(Vector3 pos)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        float distance=Vector3.Distance(startPos, pos);
        while (elapsedTime < GameConfig.MoveDuration*distance)
        {
            if (!EventHandling)
            {
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / (GameConfig.MoveDuration*distance))-new Vector3(0,0,1);//play置于地图上
                elapsedTime += Time.deltaTime;
            }
            yield return null; // 等待下一帧
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
