using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject Hex;
    private Map map=new Map();
    private const double ObstacleRate = GameConfig.ObstacleRate;//障碍物生成概率
    private const int ObstacleSup = GameConfig.ObstacleSup;//障碍物生成上限
    int size = GameConfig.size;//地图尺寸


    public void spawn()//地图初始化
    {
        GameObject[,] Hexs=new GameObject[size,size];
        for(int x=0;x<size;x++){
            for(int y=0;y<size;y++){
                Vector3 pos=new Vector3(3*(x+y)/2.0f,math.sqrt(3)*(y-x)/2,0);
                GameObject GO=Instantiate(Hex,pos,Quaternion.identity);
                GO.transform.SetParent(transform);
                Hexs[x,y]=GO;
            }
        }
        map.Initialize(Hexs);
    }
    public List<Vector2Int> GetNearby(Vector2Int v)//返回邻接的六边形坐标
    {
        List<Vector2Int> vectors = new List<Vector2Int>();

        Vector2Int[] movement = new Vector2Int[6];//六个方向的位移
        movement[0] = new Vector2Int(1, 0); movement[1] = new Vector2Int(-1, 0); movement[2] = new Vector2Int(0, 1); movement[3] = new Vector2Int(0, -1);
        movement[4] = new Vector2Int(1, -1); movement[5] = new Vector2Int(-1, 1);

        //越界检测
        if (v.x < 0 || v.y < 0 || v.x >= size || v.y >= size)
        {
            throw new IndexOutOfRangeException("给定坐标越界");
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
    private void ObstacleGenerate()//障碍物生成
    {
        System.Random random = new System.Random();
        int count = 0;
    restart:
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2Int pos= new Vector2Int(x, y);
                if (count >= ObstacleSup)
                {
                    return;
                }
                double r = random.NextDouble();
                if (r < ObstacleRate)
                {
                    map.GetHex(pos).tag = "Obstacle";
                    map.AddObstacle(pos);
                    map.GetHex(pos).GetComponent<Hexagon>().ChangeColor(Color.grey);//目前为变色效果
                    count++;
                }
            }
        }
        if (count < 3)//太少障碍物重新生成
        {
            goto restart;
        }
    }
    private void ContentGenerate()//要素生成
    {
        System.Random rnd = new System.Random();
        List<int> ContentCount = Enumerable.Repeat(0, GameConfig.ContentAmount.Count()).ToList();//已经生成的要素数量
        for (int x = 0;x < size;x++) 
        {
            for(int y = 0; y < size; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (map.GetHex(pos).tag == "Obstacle")
                {
                    continue;
                }
                double r=rnd.NextDouble();//判定是否生成的随机数
                if (r < GameConfig.ContentRate)
                {
                    int sum = GameConfig.ContentAmount.Sum() - ContentCount.Sum();
                    double _r = rnd.NextDouble();//判定种类的随机数
                    foreach (GameConfig.Content content in Enum.GetValues(typeof(GameConfig.Content)))
                    {
                        double rate = (GameConfig.ContentAmount[(int)content]-ContentCount[(int)content])/(double)sum;
                        if (_r < rate)
                        {
                            ContentCount[(int)content] += 1;
                            Hexagon hexagon= map.GetHex(pos).GetComponent<Hexagon>();
                            hexagon.ContentChange(content);
                        }
                        else
                        {
                            _r -= rate;
                        }
                    }
                }
            }
        }
    }
    private void BackgroundGenerate()//背景颜色生成
    {
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject hex = map.GetHex(pos);
                if (hex.tag != "Obstacle" && hex.tag!="Content")
                {
                    ColorUtility.TryParseHtmlString("#aaa58f", out Color color);
                    hex.GetComponent<Hexagon>().ChangeColor(color);
                }
            }
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
                Vector2Int pos = new Vector2Int(x, y);
                if (map.GetHex(pos).tag == "Obstacle" || visited.Contains(new Vector2Int(x, y)))
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
                    //广度搜索
                    while (Q.Count > 0)
                    {
                        Vector2Int v=Q.Dequeue();
                        List<Vector2Int> near=GetNearby(v);
                        near.ForEach(x => {
                            if (map.GetHex(x).tag != "Obstacle")
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
    public Vector2Int MoveCommand(List<Vector2Int> directions, int length)//移动指令
    {
        return Vector2Int.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        size = 7;
        spawn();
        ObstacleGenerate();
        ContentGenerate();
        BackgroundGenerate();
        Debug.Log(CheckConnectivity());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
