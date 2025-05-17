using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public delegate IEnumerator EncounterEvent(Event e);
    public event EncounterEvent encounterEvent;
    public IEnumerator MoveTo(Vector3 pos,Event e)//���·�账���¼�e
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        float distance=Vector3.Distance(startPos, pos);
        if (e == null)//���账���¼�
        {
            while (elapsedTime < GameConfig.MoveDuration * distance)
            {
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / (GameConfig.MoveDuration * distance)) - new Vector3(0, 0, 1);//play���ڵ�ͼ��
                elapsedTime += Time.deltaTime;
                yield return null; // �ȴ���һ֡
            }
        }
        else
        {
            System.Random rand = new System.Random();
            bool trigger = false;
            float EncounterTime = (float)rand.NextDouble()* GameConfig.MoveDuration * distance-0.01f;//����һ��ʱ��,��ֹ�¼�������
            while (elapsedTime < GameConfig.MoveDuration * distance)
            {
                if (elapsedTime > EncounterTime&&!trigger)//�����¼����ȴ�
                {
                    yield return encounterEvent?.Invoke(e);
                    trigger = true;//��ֹ��δ���
                }
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / (GameConfig.MoveDuration * distance)) - new Vector3(0, 0, 1);//play���ڵ�ͼ��
                elapsedTime += Time.deltaTime;
                yield return null; // �ȴ���һ֡
            }
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
