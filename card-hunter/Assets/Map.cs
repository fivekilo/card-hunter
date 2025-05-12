using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private GameObject[,] Hexs;
    public List<GameObject> Obstacles { get; set; }
    public void AddObstacle(Vector2Int pos)
    {
        Obstacles.Add(Hexs[pos.x, pos.y]);
    }
    public void Initialize(GameObject[,] Hexs)//´«ÈëµØÍ¼
    {
        this.Hexs = Hexs;
        Obstacles = new List<GameObject>();
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
