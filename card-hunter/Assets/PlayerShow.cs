using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShow : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform Arrow;
    void Start()
    {
        PlayerInfo Player = GetComponent<PlayerInfo>();
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        MapManager mapManager = battleManager.mapmanager;
        Vector3 InitialPos = mapManager.GetHexagon(new Vector2Int(0 , 0)).transform.position;
        //Arrow = GetComponentInChildren<Transform>();
        Arrow = transform.Find("Arrow");
        InitialPos.z = -5;
        Player.transform.position = InitialPos;
    }
   /* public void ModifyDirection(Vector2Int newDir)
    {
        //�ı���ͼ
        Debug.Log("ת���ˣ�");
        PlayerInfo Player = GetComponent<PlayerInfo>();
        Vector2Int Dir = Player.Direction;
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int Dir_id = 0;
        for(int i = 0;i < 6;i ++)
        {
            if (newDir == new Vector2Int(dx[i], dy[i])) Dir_id = i;
        }
        //ѡ���µ�ͼƬ����
        string newsource = "PlayerDir/" + "PlayerDir" + Dir_id.ToString();
        ModifyPlayerImage(newsource);
    }*/
    public void ModifyDirection(Vector2Int Dir)
    {
        PlayerInfo Player = GetComponent<PlayerInfo>();
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int Dir_id = -1;
        int now_id = -1;
        for (int i = 0; i < 6; i++)
        {
            if (Dir == new Vector2Int(dx[i], dy[i])) Dir_id = i;
            if (Player.Direction == new Vector2Int(dx[i], dy[i])) now_id = i;
        }
        if (Dir_id == -1 || now_id == -1)
        {
            Debug.Log("PlayerShow���������µķ���");
            return;
        }
        Vector3 newAngle = Arrow.eulerAngles;
        newAngle.z += (60f * (Dir_id - now_id));
        Arrow.eulerAngles = newAngle;
    }
    public void ModifyPos(Vector2Int newPos)
    {
        //�ı���ͼλ��
        PlayerInfo Player = GetComponent<PlayerInfo>();
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        MapManager mapManager = battleManager.mapmanager;
        Vector3 Pos = mapManager.GetHexagon(newPos).transform.position;
        Pos.z = -5;
        Player.transform.position = Pos;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
