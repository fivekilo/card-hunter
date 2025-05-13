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
        PlayerInfo Player = GetComponent<PlayerInfo>();
        Vector2Int Dir = Player.Direction;
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int Dir_id = 0;
        for(int i = 0;i < 6;i ++)
        {
            if (newDir == new Vector2Int(dx[i], dy[i])) Dir_id = i;
        }
        //选择新的图片名称
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
    public void ModifyPlayerImage(string newImage)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(newImage);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
