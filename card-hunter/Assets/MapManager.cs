using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject Hex;
    private GameObject [,]Hexs;
    private const double ObstacleRate = GameConfig.ObstacleRate;//障碍物生成概率
    private const int ObstacleSup = GameConfig.ObstacleSup;//障碍物生成上限
    int size = GameConfig.size;//地图尺寸
    public void spawn()//地图初始化
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

    public void AddImage(string image,Vector2Int pos)//在指定格子添加图像
    {
        int x=pos.x;int y = pos.y;
        SpriteRenderer spriteRenderer = Hexs[x,y].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=Resources.Load<Sprite>(image);
    } 

    private void ObstacleGenerate()//障碍物生成
    {
        System.Random random = new System.Random();
        int count = 0;
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
                    ChangeColor(Color.grey, new Vector2Int(x,y));//目前为变色效果
                    count++;
                }
            }
        }
    }
    public void ChangeColor(Color color,Vector2Int pos)//改变指定格子颜色
    {
        int x, y;
        x=pos.x; y=pos.y;
        Renderer renderer = Hexs[x,y].GetComponent<Renderer>();
        renderer.material.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        size = 7;
        spawn();
        ObstacleGenerate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
