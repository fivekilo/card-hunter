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
        Vector3 InitialPos = mapManager.GetVector3(new Vector2Int(0 , 0));
        InitialPos.z = -5;
        Player.transform.position = InitialPos;
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
        Vector3 Pos = mapManager.GetVector3(newPos);
        Pos.z = -5;
        Player.transform.position = Pos;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
