using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private GameObject[,] Hexs;
    private int size = GameConfig.size;
    public List<GameObject> Obstacles { get; set; }
    public void AddObstacle(Vector2Int pos)
    {
        Obstacles.Add(Hexs[pos.x, pos.y]);
    }
    public void Initialize(GameObject[,] Hexs)//´«ÈëµØÍ¼
    {
        this.Hexs = Hexs;
        Obstacles = new List<GameObject>();
        for(int x=0; x < size; x++)
        {
            for(int y=0; y<size; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GetHex(pos).GetComponent<Hexagon>().pos = pos;
            }
        }
    }
    public GameObject GetHex(Vector2Int pos)
    {
        return Hexs[pos.x, pos.y];
    }
    public void ChangeColor(Vector2Int pos,Color color)
    {
        GetHex(pos).GetComponent<Hexagon>().ChangeColor(color);
    }
    public List<Vector2Int> GetObstacles()
    {
        List<Vector2Int> res=new List<Vector2Int>();
        foreach(var i in Obstacles)
        {
            res.Add(i.GetComponent<Hexagon>().pos);
        }
        return res;
    }
}
