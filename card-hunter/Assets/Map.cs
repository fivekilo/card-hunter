using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    private GameObject[,] Hexs;
    private int size = GameConfig.size;
    public void Initialize(GameObject[,] Hexs)//´«ÈëµØÍ¼
    {
        this.Hexs = Hexs;
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
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Vector2Int pos=new Vector2Int(x, y);
                if (GetHex(pos).tag == "Obstacle")
                {
                    res.Add(pos);
                }
            }
        }
        return res;
    }
}
