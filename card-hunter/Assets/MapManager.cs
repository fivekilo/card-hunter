using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class MapManager : MonoBehaviour
{
    public GameObject Hex;
    public Vector2Int ClickedPos;
    private Map map=new Map();
    private const double ObstacleRate = GameConfig.ObstacleRate;//�ϰ������ɸ���
    private const int ObstacleSup = GameConfig.ObstacleSup;//�ϰ�����������
    int size = GameConfig.size;//��ͼ�ߴ�


    public void spawn()//��ͼ��ʼ��
    {
        GameObject[,] Hexs=new GameObject[size,size];
        for(int x=0;x<size;x++){
            for(int y=0;y<size;y++){
                Vector3 pos=new Vector3(3*(x+y)/2.0f,math.sqrt(3)*(y-x)/2,0);
                GameObject GO=Instantiate(Hex,pos,Quaternion.identity);
                GO.transform.SetParent(transform);
                GO.GetComponent<Hexagon>().AddImage("background");
                Hexs[x,y]=GO;
            }
        }
        map.Initialize(Hexs);
    }
    public List<Vector2Int> GetNearby(Vector2Int v)//�����ڽӵ�����������
    {
        List<Vector2Int> vectors = new List<Vector2Int>();

        Vector2Int[] movement = new Vector2Int[6];//���������λ��
        movement[0] = new Vector2Int(1, 0); movement[1] = new Vector2Int(-1, 0); movement[2] = new Vector2Int(0, 1); movement[3] = new Vector2Int(0, -1);
        movement[4] = new Vector2Int(1, -1); movement[5] = new Vector2Int(-1, 1);

        //Խ����
        if (v.x < 0 || v.y < 0 || v.x >= size || v.y >= size)
        {
            throw new IndexOutOfRangeException("��������Խ��");
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
    private void ObstacleGenerate()//�ϰ�������
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
                    map.GetHex(pos).AddComponent<Hexagon>().ObstacleAdd();
                    count++;
                }
            }
        }
        if (count < 3)//̫���ϰ�����������
        {
            goto restart;
        }
        if (!CheckConnectivity())//����ͨ��ɾ��������
        {
            List<Vector2Int>Obstacles=map.GetObstacles();
            foreach(Vector2Int ob in Obstacles)
            {
                map.GetHex(ob).GetComponent<Hexagon>().ObstacleRemove();
            }
            goto restart;
        }
    }
    private void ContentGenerate()//Ҫ������
    {
        System.Random rnd = new System.Random();
        List<int> ContentCount = Enumerable.Repeat(0, GameConfig.ContentAmount.Count()).ToList();//�Ѿ����ɵ�Ҫ������
        for (int x = 0;x < size;x++) 
        {
            for(int y = 0; y < size; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (map.GetHex(pos).tag == "Obstacle")
                {
                    continue;
                }
                double r=rnd.NextDouble();//�ж��Ƿ����ɵ������
                if (r < GameConfig.ContentRate)
                {
                    int sum = GameConfig.ContentAmount.Sum() - ContentCount.Sum();
                    double _r = rnd.NextDouble();//�ж�����������
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
    private void BackgroundGenerate()//������ɫ����
    {
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject hex = map.GetHex(pos);
                if (hex.tag != "Obstacle" && hex.tag!="Content")
                {
                    ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
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
                    //�������
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
    public IEnumerator MoveCommand(List<Vector2Int> directions, Vector2Int player,Vector2Int length,Action<Vector2Int>callback1 , Action<Vector2Int> callback2)//�ƶ�ָ��
    {
        //��û���Խ���ϰ��﹦��
        List<Vector2Int> ObPosition = map.GetObstacles();
        Vector2Int D = new Vector2Int();
        List<Vector2Int>accessible = new List<Vector2Int>();
        foreach (Vector2Int pos in ObPosition)
        {
            if (directions.Contains(pos))
            {
                directions.Remove(pos);
            }
        }
        //����ɴ�ĸ���
        foreach (Vector2Int direction in directions)
        {
            D = direction - player;
            for(int i = length[0]; i <= length[1]; i++)
            {
                if (ObPosition.Contains(player + D *i))
                {
                    break;
                }
                else
                {
                    accessible.Add(player + D * i);
                }
            }
        }
        if (accessible.Count == 0)
        {
            yield break;
        }

        foreach(Vector2Int pos in accessible)
        {
            map.ChangeColor(pos, Color.blue);
        }

        ClickedPos = new Vector2Int(-1, -1);
        yield return new WaitUntil(()=>accessible.Contains(ClickedPos));
        foreach(Vector2Int pos in accessible)
        {
            ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
            map.ChangeColor(pos, color);
        }
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        battleManager.isWaitingForPlayerChoose = false;
        callback2(GetNewDir(ClickedPos, player));
        callback1(ClickedPos);
        

    }

    public IEnumerator AttackCommand(List<Vector2Int> directions, Vector2Int player, Vector2Int length, Action<Vector2Int> callback)//�ƶ�ָ��
    {
        //��û���Խ���ϰ��﹦��
        List<Vector2Int> ObPosition = map.GetObstacles();
        Vector2Int D = new Vector2Int();
        List<Vector2Int> accessible = new List<Vector2Int>();
        foreach (Vector2Int pos in ObPosition)
        {
            if (directions.Contains(pos))
            {
                directions.Remove(pos);
            }
        }
        //����ɴ�ĸ���
        foreach (Vector2Int direction in directions)
        {
            D = direction - player;
            for (int i = length[0]; i <= length[1]; i++)
            {
                if (ObPosition.Contains(player + D * i))
                {
                    break;
                }
                else
                {
                    accessible.Add(player + D * i);
                }
            }
        }
        if (accessible.Count == 0)
        {
            yield break;
        }

        foreach (Vector2Int pos in accessible)
        {
            map.ChangeColor(pos, Color.red);
        }

        ClickedPos = new Vector2Int(-1, -1);
        yield return new WaitUntil(() => accessible.Contains(ClickedPos));
        foreach (Vector2Int pos in accessible)
        {
            ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
            map.ChangeColor(pos, color);
        }
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        battleManager.isWaitingForPlayerChoose = false;
        Vector2Int Direction = GetNewDir(ClickedPos , player);
        callback(Direction);
    }
    public Vector2Int GetNewDir(Vector2Int ClickedPos , Vector2Int player)
    {
        Vector2Int Dir = ClickedPos - player;
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int Dir_id = -1;
        Vector2Int res = new(-1, -1);
        for (int i = 0; i < 6; i++)
        {
            for (int j = 1; j <= GameConfig.size; j++)
            {
                if (j * dx[i] == Dir.x && j * dy[i] == Dir.y) { Dir_id = i; break; }
            }
        }
        if (Dir_id == -1)
        {
            Debug.Log("AttackCommand���������·���");
        }
        else
        {
            Vector2Int Direction = new Vector2Int(dx[Dir_id], dy[Dir_id]);
            res = Direction;
        }
        return res;
    }
    public Vector3 GetVector3(Vector2Int pos)
    {
        return map.GetHex(pos).transform.position;
    }

    public bool IsPositionOccupied(Vector2Int pos)//����ͼĳһ���Ƿ�ռ��
    {
        //������λ��
        PlayerInfo player = FindObjectOfType<PlayerInfo>();
        if (player != null && player.PlayerGridPos == pos) return true;
        //�����������
        foreach(EnemyAIController enemy in FindObjectsOfType<EnemyAIController>())
        {
            if (enemy.GetCurrentGridPos() == pos) return true;
        }
        //���ǽ��
        if(map.GetHex(pos).tag == "Obstacle")return true;
        return false;
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
