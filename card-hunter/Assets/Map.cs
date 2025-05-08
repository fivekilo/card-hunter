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
}
