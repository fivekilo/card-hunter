using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyAIController : MonoBehaviour
{
    //加载基本事项
    private BattleManager _battleManager;
    public Vector2Int _currentGridPos { get; set; }
    private MapManager _mapManager;
    public PlayerInfo _player;
    public TextMeshProUGUI text;
    public Transform arrowtransform;
    public EnemyBuff enemybuff;
    [Header("基础属性")]
    [SerializeField] public string name = "";
    [SerializeField] public int _maxHealth = 100;
    [SerializeField] public int _currentHealth;
    [SerializeField] protected int moveRange = 2;//每回合最大移动距离
    [SerializeField] protected int detectionRange = 4;//检测玩家的最大范围
    [SerializeField] public int armor = 0;//初始护甲
    private float moveInterval = 0.3f; // 移动动画间隔
    //方向及方向转换
    public int direction=0;
    private List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };

    public EnemySkillSystem skillSystem;
    [SerializeField] private List<int> selfSkills = new List<int>();//自身技能组（需要预先在inspector里设置好！）

    void Start()
    {
        _battleManager = GetComponentInParent<BattleManager>();
        _mapManager = _battleManager.mapmanager;
        _currentGridPos = new(GameConfig.size/2,GameConfig.size/2); 
        //初始化位置
        Vector3 InitialPos = _mapManager.GetHexagon(_currentGridPos).transform.position;
        InitialPos.z = -5;
        transform.position = InitialPos;
        _player= FindObjectOfType<PlayerInfo>();
        _currentHealth = _maxHealth;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{_currentHealth}/{_maxHealth}";
        arrowtransform = transform.Find("Arrow");
        //传入技能和buff
        skillSystem = GetComponent<EnemySkillSystem>();
        skillSystem.availableSkills = selfSkills;
        enemybuff = GetComponent<EnemyBuff>();
    }
   
    public IEnumerator TakeTurn()//执行回合
    {

        //先出上一回合指定的招式
        yield return skillSystem.ExecuteCurrentSkill(1);
        //再判断移动
        if (ShouldMoveToPlayer())
        {
            List<Vector2Int> path = CalculatePath();
            if(path.Count==0 )//没生成路径相当于没找到玩家
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
        //展示要出的下一招
        skillSystem.SelectNextSkill();
    }

    bool ShouldMoveToPlayer()
    {
        if (_player == null) return false;
        int distance = (int)Vector2Int.Distance(_currentGridPos, _player.PlayerGridPos);
        return distance <= detectionRange;
    }
    //寻路算法
    private List<Vector2Int> CalculatePath()
    {
        // BFS寻路算法
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

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom)//辅助生成路径
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = _player.PlayerGridPos;
        while(current !=_currentGridPos)
        {
            path.Add(current);
            current = cameFrom[current];
            // 防止无限循环
            if (path.Count > 100) break;
        }
        path.Reverse();
        return TrimPathToMoveRange(path);
    }
    //行走范围不能超过最大移动距离
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

    //沿着路径走或随机游荡
    IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        foreach (var pos in path)
        {
            if (!_battleManager.CheckPosIsValid(pos)) break;
            //先更新方向再动
            Vector2Int directionvector = pos - _currentGridPos;
            int newdirection = StdVector.FindIndex(vector => vector==directionvector);
            ChangeDirection(newdirection);
            if (pos == _player.PlayerGridPos) break;//如果要走到玩家那一格了就刹车
            UpdatePosition(pos);
            yield return new WaitForSeconds(moveInterval);//等待一定时长
        }
    }
    IEnumerator WanderRandomly()
    {

        //先找出周围可以移动的点
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

    //更新坐标位置
    public void UpdatePosition(Vector2Int newPos)
    {
        _currentGridPos = newPos;
        Vector3 newPos3 = _mapManager.GetHexagon(_currentGridPos).transform.position;
        newPos3.z = -5;
        transform.position = newPos3;
    }
    
    //加减血量
    public void ReduceHealth(int num)
    {
        //优先抵扣护甲
        if(armor>0)
        {
            int deltaarmor = armor-Mathf.Clamp(armor - num, 0, 100);
            armor = Mathf.Clamp(armor - num, 0, 100);
            num = num - deltaarmor;
        }
        _currentHealth = Mathf.Clamp(_currentHealth - num, 0, _maxHealth);
        if (num>=0) 
            Debug.Log("怪物被打了" + num.ToString() + "血");
        else
            Debug.Log("怪物回复生命值了！");
        text.text = $"{_currentHealth}/{_maxHealth}";
        //特判：蛮颚龙的进暴怒和退暴怒
        if(name=="蛮颚龙"&& (float)_currentHealth * 5 < (float)_maxHealth)//退暴怒
        {
            StartCoroutine(skillSystem.ExecuteCurrentSkill(-1));
            skillSystem.nextSkillID = 0;
            Debug.Log("蛮颚龙退出暴怒状态了！");
        }
        else if (name == "蛮颚龙" && (float)_currentHealth * 1.25 < (float)_maxHealth)//进暴怒
        {
            selfSkills.Add(10);
            Debug.Log("蛮颚龙进入暴怒状态了！");
        }
    }

    public Vector2Int GetCurrentGridPos() // 公共方法供MapManager调用
    {
        return _currentGridPos;
    }

    //调转方向(待编写)
    public void ChangeDirection(int newdirection)
    {
        arrowtransform.rotation = Quaternion.Euler(0, 0, 60*newdirection);//调转箭头方向
        direction = newdirection;
    }

    // Update is called once per frame
    void Update()
    {
        //以防变身更新了技能组，不断传入新技能
        skillSystem = GetComponent<EnemySkillSystem>();
        skillSystem.availableSkills = selfSkills;
    }
}
