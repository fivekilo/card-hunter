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
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / (GameConfig.MoveDuration*distance))-new Vector3(0,0,1);//play���ڵ�ͼ��
                elapsedTime += Time.deltaTime;
            }
            yield return null; // �ȴ���һ֡
        }

        transform.position = pos - new Vector3(0, 0, 1); // ȷ������λ��׼ȷ
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
