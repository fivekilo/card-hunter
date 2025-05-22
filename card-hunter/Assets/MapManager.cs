using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject Hex;
    public Vector2Int ClickedPos;
    private Map map=new Map();
    private const double ObstacleRate = GameConfig.ObstacleRate;//�ϰ������ɸ���
    private const int ObstacleSup = GameConfig.ObstacleSup;//�ϰ�����������
    int size = GameConfig.size;//��ͼ�ߴ�
    private bool MoveComplete = false;
    public AudioManager audioManager;
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
    restart:
        int count = 0;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2Int pos= new Vector2Int(x, y);
                if (count >= ObstacleSup)
                {
                    goto check;
                }
                double r = random.NextDouble();
                if (r < ObstacleRate)
                {
                    map.GetHex(pos).GetComponent<Hexagon>().ObstacleAdd();
                    count++;
                }
            }
        }
    check:
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
                else if (hex.tag == "Content")
                {
                    hex.GetComponent<Hexagon>().ChangeColor(Color.white);
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
    public IEnumerator MoveCommand(List<Vector2Int> directions, Vector2Int player,Vector2Int length,Action<Vector2Int>callback1 , Action<Vector2Int> callback2, Action callback3)//�ƶ�ָ��
    {
        
        //��û���Խ���ϰ��﹦��
        List<Vector2Int> ObPosition = map.GetObstacles();
        Vector2Int D = new Vector2Int();
        List<Vector2Int>accessible = new List<Vector2Int>();
        BattleManager battleManager = GetComponentInParent<BattleManager>();
        battleManager.FindAllEnemies();
        List<EnemyAIController> enemies = battleManager._enemies;
        List<Vector2Int> enemyPos = new();
        foreach(EnemyAIController enemyAI in enemies)
        {
            enemyPos.Add(enemyAI._currentGridPos);
        }
        battleManager.UserIndicator.text = "��ѡ���ƶ��ĵؿ�";
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
                if (ObPosition.Contains(player + D *i) || !checkPosValid(player + D * i))
                {
                    break;
                }
                else
                {
                    if(!enemyPos.Contains(player + D * i))
                    accessible.Add(player + D * i);
                }
            }
        }
        if (accessible.Count == 0)
        {
            Debug.Log("�������ƶ��ķ�Χ��");
            battleManager.isWaitingForPlayerChoose = false;
            yield break;
        }

        foreach(Vector2Int pos in accessible)
        {
            map.ChangeColor(pos, Color.blue);
        }

        ClickedPos = new Vector2Int(-1, -1);
        yield return new WaitUntil(()=>accessible.Contains(ClickedPos));
        foreach(Vector2Int pos in accessible)//��ɫ����
        {
            map.RollbackColor(pos);
        }
        battleManager.isWaitingForPlayerChoose = false;
        callback2(GetNewDir(ClickedPos, player));
        callback1(ClickedPos);
        callback3();
        //MoveComplete = true;
        battleManager.UserIndicator.text = "��һغ�";
    }

    public IEnumerator AttackCommand(List<Vector2Int> directions, Vector2Int player, Vector2Int length, Action<Vector2Int> callback , Card card)//�ƶ�ָ��
    {

       /* yield return new WaitUntil(() => MoveComplete == true);
        MoveComplete = false;*/
        //��û���Խ���ϰ��﹦��
        List<Vector2Int> ObPosition = map.GetObstacles();
        Vector2Int D = new Vector2Int();
        List<Vector2Int> accessible = new List<Vector2Int>();
        BattleManager battleManager = GetComponentInParent<BattleManager>();
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
                if (ObPosition.Contains(player + D * i) || !checkPosValid(player + D * i))
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
            Debug.Log("�����ڹ����ķ�Χ��");
            battleManager.isWaitingForPlayerChoose = false;
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
            map.RollbackColor(pos);
        }
        
        Vector2Int Direction = GetNewDir(ClickedPos , player);
        //��ת��
        callback(Direction);

        /*Ѱ�ҹ����ķ�Χ�������ɫ*/
        List<Vector2Int> Attacked = new();
        int[] dx = { 1, 0, -1, -1, 0, 1 };
        int[] dy = { 0, 1, 1, 0, -1, -1 };
        int PlayerDirId = -1;
        for (int i = 0; i < 6; i++)
        {
            if (Direction.x == dx[i] && Direction.y == dy[i]) { PlayerDirId = i; break; }
        }
        int Length = card.AttackLength;
        List<int> AttackRange = card.AttackRange;
        foreach (int Dir_id in AttackRange)
        {
            for (int i = 1; i <= Length; i++)
            {
                int newDir = (Dir_id + PlayerDirId) % 6;
                Vector2Int nowPos = battleManager.Player.PlayerGridPos + new Vector2Int(dx[newDir] * i, dy[newDir] * i);
                if(checkPosValid(nowPos))
                Attacked.Add(nowPos);
            }
        }
        foreach (Vector2Int pos in Attacked)
        {
            map.ChangeColor(pos, Color.yellow);
        }
        audioManager.PlayCardPlaySound(card.cardNum);
        yield return new WaitForSeconds(1f);
        foreach (Vector2Int pos in Attacked)
        {
            map.RollbackColor(pos);
        }
        battleManager.AttackConsume(card);
        battleManager.isWaitingForPlayerChoose = false;
    }

    //��ָ�����꣨��ͼ���꣩�ĸ��ӱ�ɫ
    public void ChangeColorByPos(List<Vector2Int> PosSet,Color color) 
    {
        foreach (Vector2Int pos in PosSet) 
        {   
            if (map.GetHex(pos).tag != "Obstacle")
                map.ChangeColor(pos, color);
        }
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
    public GameObject GetHexagon(Vector2Int pos)
    {
        return map.GetHex(pos);
    }
    
    public GameConfig.Content StepContent(Vector2Int pos,out bool exist)
    {
        Hexagon hex = map.GetHex(pos).GetComponent<Hexagon>();
        GameConfig.Content content = hex.content;
        if (map.GetHex(pos).tag=="Content")
        {
            hex.ContentRemove();
            exist = true;
            return content;
        }
        else
        {
            exist = false;
            return content;
        }
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

    public bool checkPosValid(Vector2Int Pos)
    {
        if (Pos.x < 0 || Pos.x >= GameConfig.size || Pos.y < 0 || Pos.y >= GameConfig.size) return false;
        return true;
    }
    public bool isObstacle(Vector2Int pos)
    {
        if (map.GetHex(pos).tag == "Obstacle") return true;
        return false;
    }

    //���������ı����Ҫ��
    public void AddMonsterContent(Vector2Int pos, GameConfig.Content content)
    {
        //�����ﻹҪ����ǲ���ǽ��
        if (isObstacle(pos)) return;
        Hexagon hexagon =map.GetHex(pos).GetComponent<Hexagon>();
        hexagon.ContentChange(content);
    }

    //�����������ɺ��Ƴ��ϰ���
    public void AddMonsterObstacles(Vector2Int pos)
    {
        //�����ﻹҪ����ǲ���ǽ��
        if (isObstacle(pos)) return;
        map.GetHex(pos).GetComponent<Hexagon>().ObstacleAddIcicle();
    }
    public void RemoveMonsterObstacles(Vector2Int pos)
    {
        map.GetHex(pos).GetComponent<Hexagon>().ObstacleRemove();
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
