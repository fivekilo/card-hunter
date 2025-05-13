using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    //���ػ�������
    protected BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    public MapManager _mapManager;
    private PlayerInfo _player;
    [Header("��������")]
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int moveRange=3;//ÿ�غ�����ƶ�����
    [SerializeField] protected int detectionRange=6;//�����ҵ����Χ

    void Start()
    {
        _battleManager = FindObjectOfType<BattleManager>();
        _currentGridPos= Vector2Int.FloorToInt(transform.position); // ��ʼλ�����������
        _mapManager = FindObjectOfType<MapManager>();
        _player= FindObjectOfType<PlayerInfo>();
    }

    public virtual void TakeTurn()//ִ�лغ�
    {
        //�ȳ���һ�غϽ�������ʽ(����д)
        //���ж��ƶ�
        //if(ShouldMoveToPlayer())
        //{
        //    List<Vector2Int> path = CalculatePath();
        //}
        //����һ�У�����д��
    }

    //Ѱ·�㷨������д��
    private List<Vector2Int> CalculatePath()
    {
        // BFSѰ·�㷨
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> camefrom = new Dictionary<Vector2Int, Vector2Int>();
        queue.Enqueue(_currentGridPos);
        camefrom[_currentGridPos] = Vector2Int.zero;
        bool foundplayer = false;
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == _player.PlayerGridPos)
            {
                foundplayer = true;
                break;
            }

                foreach (Vector2Int neighbor in _battleManager.GetAdjacent(new List<int> { 0, 1, 2, 3, 4, 5 }))
            {
                if(!camefrom.ContainsKey(neighbor)&& _battleManager.CheckPosIsValid(neighbor)&& !_mapManager.IsPositionOccupied(neighbor))
                {
                    queue.Enqueue(neighbor);
                    camefrom[neighbor] = current;
                }
            }
        }

        return foundplayer ? ReconstructPath(camefrom) : new List<Vector2Int>();
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom)//��������·��
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = _player.PlayerGridPos;
        while(current !=_currentGridPos)
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }

    public Vector2Int GetCurrentGridPos() // ����������MapManager����
    {
        return _currentGridPos;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
