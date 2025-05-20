using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAIController : MonoBehaviour
{
    //���ػ�������
    private BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    private MapManager _mapManager;
    public PlayerInfo _player;
    public TextMeshProUGUI text;
    public Transform arrowtransform;
    public EnemyBuff enemybuff;
    [Header("��������")]
    [SerializeField] public string name = "";
    [SerializeField] public int _maxHealth = 100;
    [SerializeField] public int _currentHealth;
    [SerializeField] protected int moveRange = 2;//ÿ�غ�����ƶ�����
    [SerializeField] protected int detectionRange = 4;//�����ҵ����Χ
    [SerializeField] public int armor = 0;//��ʼ����
    private float moveInterval = 0.3f; // �ƶ��������
    //���򼰷���ת��
    public int direction=0;
    private List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };

    public EnemySkillSystem skillSystem;
    [SerializeField] private List<int> selfSkills = new List<int>();//�������飨��ҪԤ����inspector�����úã���

    void Start()
    {
        _battleManager = GetComponentInParent<BattleManager>();
        _mapManager = _battleManager.mapmanager;
        _currentGridPos = new(GameConfig.size/2,GameConfig.size/2); 
        //��ʼ��λ��
        Vector3 InitialPos = _mapManager.GetHexagon(_currentGridPos).transform.position;
        InitialPos.z = -5;
        transform.position = InitialPos;
        _player= FindObjectOfType<PlayerInfo>();
        _currentHealth = _maxHealth;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{_currentHealth}/{_maxHealth}";
        arrowtransform = transform.Find("Arrow");
        //���뼼�ܺ�buff
        skillSystem = GetComponent<EnemySkillSystem>();
        skillSystem.availableSkills = selfSkills;
        enemybuff = GetComponent<EnemyBuff>();
    }
   
    public IEnumerator TakeTurn()//ִ�лغ�
    {

        //�ȳ���һ�غ�ָ������ʽ
        yield return skillSystem.ExecuteCurrentSkill(1);
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
        //չʾҪ������һ��
        skillSystem.SelectNextSkill();
    }

    bool ShouldMoveToPlayer()
    {
        if (_player == null) return false;
        int distance = (int)Vector2Int.Distance(_currentGridPos, _player.PlayerGridPos);
        return distance <= detectionRange;
    }
    //Ѱ·�㷨
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
            //�ȸ��·����ٶ�
            Vector2Int directionvector = pos - _currentGridPos;
            int newdirection = StdVector.FindIndex(vector => vector==directionvector);
            ChangeDirection(newdirection);
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
            Vector2Int directionvector = target - _currentGridPos;
            int newdirection = StdVector.FindIndex(vector => vector == directionvector);
            ChangeDirection(newdirection);
            UpdatePosition(target);
            yield return new WaitForSeconds(moveInterval);
        }
    }

    //��������λ��
    public void UpdatePosition(Vector2Int newPos)
    {
        _currentGridPos = newPos;
        Vector3 newPos3 = _mapManager.GetHexagon(_currentGridPos).transform.position;
        newPos3.z = -5;
        transform.position = newPos3;
    }
    
    //�Ӽ�Ѫ��
    public void ReduceHealth(int num)
    {
        //���ȵֿۻ���
        if(armor>0)
        {
            int deltaarmor = armor-Mathf.Clamp(armor - num, 0, 100);
            armor = Mathf.Clamp(armor - num, 0, 100);
            num = num - deltaarmor;
        }
        _currentHealth = Mathf.Clamp(_currentHealth - num, 0, _maxHealth);
        if (num>=0) 
            Debug.Log("���ﱻ����" + num.ToString() + "Ѫ");
        else
            Debug.Log("����ظ�����ֵ�ˣ�");
        text.text = $"{_currentHealth}/{_maxHealth}";
        //���У�������Ľ���ŭ���˱�ŭ
        if(name=="�����"&& (float)_currentHealth * 5 < (float)_maxHealth)//�˱�ŭ
        {
            StartCoroutine(skillSystem.ExecuteCurrentSkill(-1));
            skillSystem.nextSkillID = 0;
            Debug.Log("������˳���ŭ״̬�ˣ�");
        }
        else if (name == "�����" && (float)_currentHealth * 1.25 < (float)_maxHealth)//����ŭ
        {
            selfSkills.Add(10);
            Debug.Log("��������뱩ŭ״̬�ˣ�");
        }
    }

    public Vector2Int GetCurrentGridPos() // ����������MapManager����
    {
        return _currentGridPos;
    }

    //��ת����(����д)
    public void ChangeDirection(int newdirection)
    {
        arrowtransform.rotation = Quaternion.Euler(0, 0, 60*newdirection);//��ת��ͷ����
        direction = newdirection;
    }

    // Update is called once per frame
    void Update()
    {
        //�Է���������˼����飬���ϴ����¼���
        skillSystem = GetComponent<EnemySkillSystem>();
        skillSystem.availableSkills = selfSkills;
    }
}
