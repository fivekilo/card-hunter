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
        print(v);
        mapManager.ModifyHex(v , "PlayerDir1");
    }
    public void ModifyDirection(Vector2Int newDir)
    {
        //�ı���ͼ
    }
    public void ModifyPos(Vector2Int newPos)
    {
        //�ı���ͼλ��
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
