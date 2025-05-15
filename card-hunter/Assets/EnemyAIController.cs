using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    //���ػ�������
    public BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    public MapManager _mapManager;
    public PlayerInfo _player;
    public TextMeshProUGUI text;
    [Header("��������")]
    [SerializeField] protected int _maxHealth = 100;
    [SerializeField] protected int _currentHealth;
    [SerializeField] protected int moveRange = 2;//ÿ�غ�����ƶ�����
    [SerializeField] protected int detectionRange = 4;//�����ҵ����Χ
    [SerializeField] private float moveInterval = 0.3f; // �ƶ��������
    
    void Start()
    {
        _battleManager = GetComponentInParent<BattleManager>();
        _mapManager = _battleManager.mapmanager;
        _currentGridPos = new(GameConfig.size/2,GameConfig.size/2); 
        Vector3 InitialPos = _mapManager.GetVector3(_currentGridPos);
        InitialPos.z = -5;
        transform.position = InitialPos;
        _player= FindObjectOfType<PlayerInfo>();
        _currentHealth = _maxHealth;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
   
    public IEnumerator TakeTurn()//ִ�лغ�
    {

        //�ȳ���һ�غϽ�������ʽ(����д)
        //���ж��ƶ�
        if (ShouldMoveToPlayer())
        {
            List<Vector2Int> path = CalculatePath();
            if(path.Count==0 )//û����·���൱��û�ҵ����
            {
                yield return WanderRandomly();
            }
            else
                yield return MoveAlongPath(path);
        }
        else
        {
            yield return WanderRandomly();
        }
        //����һ�У�����д��
    }

    bool ShouldMoveToPlayer()
    {
        if (_player == null) return false;
        int distance = (int)Vector2Int.Distance(_currentGridPos, _player.PlayerGridPos);
        return distance <= detectionRange;
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
            if (foundplayer == true)
                break;

            foreach (Vector2Int neighbor in _mapManager.GetNearby(current))
            {
                if (neighbor == _player.PlayerGridPos)
                {
                    camefrom[neighbor] = current;
                    foundplayer = true;
                    break;
                }
                if (!camefrom.ContainsKey(neighbor)&& _battleManager.CheckPosIsValid(neighbor)&& !_mapManager.IsPositionOccupied(neighbor))
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
            // ��ֹ����ѭ��
            if (path.Count > 100) break;
        }
        path.Reverse();
        return TrimPathToMoveRange(path);
    }
    //���߷�Χ���ܳ�������ƶ�����
    List<Vector2Int> TrimPathToMoveRange(List<Vector2Int> path)
    {
        List<Vector2Int> trimmedPath = new List<Vector2Int>();
        int stepsTaken = 0;

        foreach (var pos in path)
        {
            if (stepsTaken >= moveRange) break;
            trimmedPath.Add(pos);
            stepsTaken++; 
        }
        return trimmedPath;
    }

    //����·���߻�����ε�
    IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        foreach (var pos in path)
        {
            if (!_battleManager.CheckPosIsValid(pos)) break;
            if (pos == _player.PlayerGridPos) break;//���Ҫ�ߵ������һ���˾�ɲ��
            UpdatePosition(pos);
            yield return new WaitForSeconds(moveInterval);//�ȴ�һ��ʱ��
        }
    }
    IEnumerator WanderRandomly()
    {

        //���ҳ���Χ�����ƶ��ĵ�
        List<Vector2Int> possibleMoves = _mapManager.GetNearby(_currentGridPos)
            .FindAll(pos => !_mapManager.IsPositionOccupied(pos));
        if (possibleMoves.Count > 0)
        {
            Vector2Int target = possibleMoves[Random.Range(0, possibleMoves.Count)];
            UpdatePosition(target);
            yield return new WaitForSeconds(moveInterval);
        }
    }

    //��������λ��
    void UpdatePosition(Vector2Int newPos)
    {
        _currentGridPos = newPos;
        Vector3 newPos3 = _mapManager.GetVector3(_currentGridPos);
        newPos3.z = -5;
        transform.position = newPos3;
    }
    
    //�Ӽ�Ѫ��
   /* public void AddHealth(int num)
    {
        _currentHealth += num;
    }*/
    public void ReduceHealth(int num)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - num, 0, _maxHealth);
        text.text = $"{_currentHealth}/{_maxHealth}";
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
