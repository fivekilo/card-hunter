using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject Hex;
    private GameObject [,]Hexs;
    private const double ObstacleRate = GameConfig.ObstacleRate;//�ϰ������ɸ���
    private const int ObstacleSup = GameConfig.ObstacleSup;//�ϰ�����������
    int size = GameConfig.size;//��ͼ�ߴ�


    public void spawn()//��ͼ��ʼ��
    {
        Hexs=new GameObject[size,size];
        for(int x=0;x<size;x++){
            for(int y=0;y<size;y++){
                Vector3 pos=new Vector3(3*(x+y)/2.0f,math.sqrt(3)*(y-x)/2,0);
                GameObject GO=Instantiate(Hex,pos,Quaternion.identity);
                GO.transform.SetParent(transform);
                Hexs[x,y]=GO;
            }
        }
    }
    public List<Vector2Int> GetNearby(Vector2Int v)//�����ڽӵ�����������
    {
        List<Vector2Int> vectors = new List<Vector2Int>();

        Vector2Int[] movement = new Vector2Int[6];//���������λ��
        movement[0] = new Vector2Int(1, 0); movement[1] = new Vector2Int(-1, 0); movement[2] = new Vector2Int(0, 1); movement[3] = new Vector2Int(0, -1);
        movement[4] = new Vector2Int(1, -1); movement[5] = new Vector2Int(-1, 1);

        //Խ����
        if (v.x < 0 || v.y < 0 || v.x >= size || v.y >= size)
        {
            throw new IndexOutOfRangeException("��������Խ��");
        }

        for (int i = 0; i < 6; i++)
        {
            Vector2Int near = v + movement[i];
            if (near.x < 0 || near.y < 0 || near.x >= size || near.y >= size)
            {
                continue;
            }
            else
            {
                vectors.Add(near);
            }
        }
        return vectors;
    }
    public void AddImage(string image,Vector2Int pos)//��ָ���������ͼ��
    {
        int x=pos.x;int y = pos.y;
        SpriteRenderer spriteRenderer = Hexs[x,y].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=Resources.Load<Sprite>(image);
    } 
    public void ChangeColor(Color color,Vector2Int pos)//�ı�ָ��������ɫ
    {
        int x, y;
        x=pos.x; y=pos.y;
        Renderer renderer = Hexs[x,y].GetComponent<Renderer>();
        renderer.material.color = color;
    }
    private void ObstacleGenerate()//�ϰ�������
    {
        System.Random random = new System.Random();
        int count = 0;
    restart:
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (count >= ObstacleSup)
                {
                    return;
                }
                double r = random.NextDouble();
                if (r < ObstacleRate)
                {
                    Hexs[x, y].tag = "Obstacle";
                    ChangeColor(Color.grey, new Vector2Int(x, y));//ĿǰΪ��ɫЧ��
                    count++;
                }
            }
        }
        if (count < 3)//̫���ϰ�����������
        {
            goto restart;
        }
    }
    private bool CheckConnectivity()
    {
        List<Vector2Int> visited = new List<Vector2Int>();
        bool Enter=false;
        Queue<Vector2Int> Q = new Queue<Vector2Int>();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (Hexs[x, y].tag == "Obstacle" || visited.Contains(new Vector2Int(x, y)))
                {
                    continue;
                }
                else
                {
                    if (Enter)
                    {
                        return false;
                    }
                    Q.Enqueue(new Vector2Int(x, y));
                    visited.Add(new Vector2Int(x, y));
                    Enter = true;
                    //�������
                    while (Q.Count > 0)
                    {
                        Vector2Int v=Q.Dequeue();
                        List<Vector2Int> near=GetNearby(v);
                        near.ForEach(x => {
                            if (Hexs[x.x, x.y].tag != "Obstacle")
                            {
                                if (!visited.Contains(x))
                                {
                                    Q.Enqueue(x);
                                    visited.Add(x);
                                }
                            }
                        });
                    }
                }
            }
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        size = 7;
        spawn();
        ObstacleGenerate();
        Debug.Log(CheckConnectivity());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
