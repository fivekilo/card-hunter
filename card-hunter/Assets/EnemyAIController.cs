using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;


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
    [SerializeField] public int ID ;
    [SerializeField] public string name = "";
    [SerializeField] public int _maxHealth = 100;
    [SerializeField] public int _currentHealth =100;
    [SerializeField] protected int moveRange = 2;//每回合最大移动距离
    [SerializeField] protected int detectionRange = 4;//检测玩家的最大范围
    [SerializeField] public int armor = 0;//初始护甲
    private float moveInterval = 0.3f; // 移动动画间隔
    public bool havechangedskill=false;//特殊怪物拥有的是否变招
    //方向及方向转换
    public int direction=0;
    private List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };

    public EnemySkillSystem skillSystem;
    [SerializeField] public List<int> selfSkills = new List<int>();//自身技能组（需要预先在inspector里设置好！）

    //形态指示器：蛮颚龙，雷狼龙，冰咒龙
    public int enemystate = 0;
    public bool isdead = false;//是否死亡
    public int TurnCount = 0;//回合计数器
    public int FrozenTurnCount = 0;//冰咒龙极寒形态回合计数器

    void Start()
    {
        _battleManager = GetComponentInParent<BattleManager>();
        _mapManager = _battleManager.mapmanager;
        _currentGridPos = GenerateSpawn(); 
        //初始化位置
        Vector3 InitialPos = _mapManager.GetHexagon(_currentGridPos).transform.position;
        InitialPos.z = -5;
        transform.position = InitialPos;
        _player= FindObjectOfType<PlayerInfo>();
        _currentHealth = _maxHealth;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"{_currentHealth}/{_maxHealth} 护甲：{armor}";
        arrowtransform = transform.Find("Arrow");
        //传入技能和buff
        skillSystem = GetComponent<EnemySkillSystem>();
        skillSystem.availableSkills = selfSkills;
        enemybuff = GetComponent<EnemyBuff>();
    }

    public Vector2Int GenerateSpawn()
    {
        int attemp = 0;
        Vector2Int res = new();
        System.Random rand = new System.Random();
        while (attemp < 100)
        {
            int x = rand.Next(0, 6);
            int y = rand.Next(0, 6);
            if (x + y < 7|| _mapManager.isObstacle(new(x, y)) == true) continue;
            res = new(x, y);
            Debug.Log("怪物生成在" + x + " " + y);
            break;
            attemp++;
        }
        return res;
    }

    public IEnumerator TakeTurn()//执行回合
    {
        TurnCount++;//回合计数+1
        if (ID == 7)
            FrozenTurnCount++;

        if (isdead == true)
        {
            havechangedskill = false;//重置变招函数
            yield return skillSystem.ExecuteCurrentSkill(1);
            yield break;
        }//判定怪物死亡

        havechangedskill = false;//重置变招函数
        //先出上一回合指定的招式
        yield return skillSystem.ExecuteCurrentSkill(-1);
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
        //展示要出的下一招（被麻痹了选0）
        if(enemybuff.Numbness>0)
        {
            skillSystem.SelectNextSkill(0);
            enemybuff.ModifyNumbness(enemybuff.Numbness-1);
        }
        else
            skillSystem.SelectNextSkill(-1);
        //判定中毒状态
        if (enemybuff.Poison > 0)
        {
            ReduceHealth((int)(_currentHealth*0.02));
            enemybuff.ModifyPoison(enemybuff.Poison-1);
        }

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

    //更新坐标位置并判断是否踩到地形要素
    public void UpdatePosition(Vector2Int newPos)
    {
        _currentGridPos = newPos;
        Vector3 newPos3 = _mapManager.GetHexagon(_currentGridPos).transform.position;
        newPos3.z = -5;
        transform.position = newPos3;
        bool exist = false;
        GameConfig.Content type = _mapManager.StepContent(newPos, out exist);
        if (exist == false) return;
        switch (type)
        {
            case GameConfig.Content.Trap:
                enemybuff.ModifyNumbness(enemybuff.Numbness + 1);
                _mapManager.GetHexagon(newPos).GetComponent<Hexagon>().ContentRemove();
                break;
            case GameConfig.Content.Frog:
                enemybuff.ModifyNumbness(enemybuff.Numbness + 1);
                _mapManager.GetHexagon(newPos).GetComponent<Hexagon>().ContentRemove();
                break;
            case GameConfig.Content.DuCao:
                enemybuff.ModifyPoison(enemybuff.Poison + 3);
                _mapManager.GetHexagon(newPos).GetComponent<Hexagon>().ContentRemove();
                break;
        }
    }
    
    //加减血量
    public void ReduceHealth(int num)
    {
        //特判：冰咒龙究极冰暴时免疫伤害
        if (skillSystem.nextSkillID==34 && ID == 7 && enemystate == 1)
        {
            num = 0;
            Debug.Log("冰咒龙高高升天，猎人伤害不到它！");
        }

        //优先抵扣护甲
        if (armor>0 && num>0)
        {
            int deltaarmor = armor-Mathf.Clamp(armor - num, 0, 999);
            armor = Mathf.Clamp(armor - num, 0, 100);
            num = num - deltaarmor;
        }
        //特判：冰咒龙的进冰和退冰
        if (armor==0 && ID ==7 && enemystate==1&&skillSystem.nextSkillID!=33)
        {
            enemystate = 0;
            StartCoroutine(skillSystem.ExecuteCurrentSkill(-1));
            skillSystem.nextSkillID = 0;
            FrozenTurnCount = -1;//保证是每5回合进入一次
            bool isRemoved1=selfSkills.Remove(34);
            bool isRemoved2 = selfSkills.Remove(35);
            bool isRemoved3 = selfSkills.Remove(36);
            Debug.Log("冰咒龙退出极寒之冰形态了！");
        }

        _currentHealth = Mathf.Clamp(_currentHealth - num, 0, _maxHealth);
        if (num>=0) 
            Debug.Log("怪物被打了" + num.ToString() + "血");
        else
            Debug.Log("怪物回复生命值了！");
        text.text = $"{_currentHealth}/{_maxHealth} 护甲：{armor}";
        if (_currentHealth == 0)
        {
            isdead = true;
            Debug.Log("怪物死亡了！");
            return;
        }
        //在怪物存活时进行特判
        //特判：蛮颚龙的进暴怒和退暴怒
        if(ID==3&& (float)_currentHealth * 5 < (float)_maxHealth && enemystate == 1)//退暴怒
        {
            enemystate = 0;
            StartCoroutine(skillSystem.ExecuteCurrentSkill(-1));
            skillSystem.nextSkillID = 0;
            Debug.Log("蛮颚龙退出暴怒状态了！");
        }
        else if (ID == 3 && (float)_currentHealth * 1.25 < (float)_maxHealth && (float)_currentHealth * 5 > (float)_maxHealth &&enemystate==0)//进暴怒
        {
            enemystate = 1;
            selfSkills.Add(10);
            Debug.Log("蛮颚龙进入暴怒状态了！");
        }
    }

    //改变护甲
    public IEnumerator ChangeArmor(int deltaarmor)
    {
        armor = Mathf.Clamp(armor + deltaarmor, 0, 999);
        Debug.Log($"怪物获得了{deltaarmor}点护甲！");
        text.text = $"{_currentHealth}/{_maxHealth} 护甲：{armor}";
        yield return new WaitForSeconds(0.2f);
    }

    public Vector2Int GetCurrentGridPos() // 公共方法供MapManager调用
    {
        return _currentGridPos;
    }

    //调转方向
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
