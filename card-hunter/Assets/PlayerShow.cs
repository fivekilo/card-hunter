using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo Player = GetComponent<PlayerInfo>();
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        MapManager mapManager = battleManager.mapmanager;
        Vector2Int v = Player.PlayerGridPos;
        Vector2Int InitialPos = mapManager.
    }
    public void ModifyDirection(Vector2Int newDir)
    {
        //改变贴图
    }
    public void ModifyPos(Vector2Int newPos)
    {
        //改变贴图位置
        PlayerInfo Player = GetComponent<PlayerInfo>();
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        MapManager mapManager = battleManager.mapmanager;
        Vector2Int v = Player.PlayerGridPos;
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
