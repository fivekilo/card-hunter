using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public const double ObstacleRate = 0.15;//障碍物生成概率
    public const double ContentRate = 0.2;//地图要素生成概率
    public const int ObstacleSup = 10;//障碍物生成上限
    public const int size = 7;
    public const int InitialHealth = 80; //血量
    public const int InitialCost = 3; //初始费用
    public const int InitialHandCardNum = 4; //初始手牌数
    public const int MaxHandCardNum = 8;
    public const int MaxBladeLevel = 3; //最大气刃等级
    public const int MaxBladeNum = 50;//最大气刃值
    public const string BackgroundColor = "#aaa58f";//背景颜色
    //地图要素
    public enum Content
    {
        LuCao,
        Trap,
        Frog,
        DuCao,
        NaiLiBug
    }
    public static IReadOnlyList<int> ContentAmount = new List<int> {1,1,2,2,1}.AsReadOnly();
    public static IReadOnlyList<string> CardName = new List<string> {"",//卡牌名称
        "直斩" ,
        "踏步斩",
        "气刃斩Ⅰ" ,
        "气刃斩Ⅱ",
        "气刃大回旋" ,
        "翻滚",
        "见切",
        "格当"
    }.AsReadOnly();
    public static IReadOnlyList<string> CardImageName=new List<string> 
    {"",//卡图名称
        "Direct slash" ,
        "Step slash",
        "Bladegas slash1",
        "Bladegas slash2",
        "Bladegas big spin",
        "Roll",
        "Mind's Eye Slash",
        "Block"
        }.AsReadOnly();
    public static IReadOnlyList<string> CardText = new List<string>
    { "", //卡牌描述
        "造成6点伤害，回复2格气刃槽" , 
        "向前移动一格，造成4点伤害，回复1格气刃槽" ,
        "消耗1格气刃槽,造成7点伤害,转向",
        "消耗2格气刃槽，造成两次4点伤害，转向，消耗，虚无",
        "消耗2格气刃槽,对前方三格范围内的敌人造成10点伤害，提升气刃等级，转向，消耗，虚无",
        "向任意方向移动一格,进入自由态",
        "从后方三格中选择移动一格，获得一层免伤buff，若下回合开始时角色所处地块或发动见切前角色所处地块被攻击，则获得【气刃大回旋】，回满气刃槽。",
        "获得3点格当"
    }.AsReadOnly();
    public static IReadOnlyList<int> CardType = new List<int> 
    { 3,//卡牌类型
        0, 
        0, 
        0, 
        0,
        0,
        2,
        2,
        1
    }.AsReadOnly();//0 攻击 1防御 2移动 3能力
    public static IReadOnlyList<int> Cost = new List<int>
    { 3,//卡牌类型
        1,
        1,
        1,
        1,
        1,
        1,
        2,
        0
    }.AsReadOnly();
    public static IReadOnlyList<List<int>> Move = new List<List<int>>
    {null,//移动方向
        null,//1
        new List<int>{0},//2
        null,//3
        null,//4
        null,//5
        new List<int>{0,1,2,3,4,5},//6
        new List<int>{2,3,4},//7
        null//8

    }.AsReadOnly();

    public static IReadOnlyList<Vector2Int> MoveLength = new List<Vector2Int>
    {
        new Vector2Int(0,0),//移动长度 下上限
        new Vector2Int(0,0),//1
        new Vector2Int(1,1),//2
        new Vector2Int(0,0),//3
        new Vector2Int(0,0),//4
        new Vector2Int(0,0),//5
        new Vector2Int(1,1),//6
        new Vector2Int(1,1),//7
        new Vector2Int(0,0)//8

    }.AsReadOnly();

    public static IReadOnlyList<int> Derivation = new List<int>
    {0,//派生卡牌
        0,//1
        0,//2
        4,//3
        5,//4
        0,//5
        0,//6
        5,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<bool> Consumption = new List<bool>
    {true,//消耗属性
        false,//1
        false,//2
        false,//3
        true,//4
        true,//5
        false,//6
        false,//7
        false//8
    }.AsReadOnly();

    public static IReadOnlyList<int> DrawCard = new List<int>
    {0,//牌效抽卡
        0,//1
        0,//2
        0,//3
        0,//4
        0,//5
        0,//6
        0,//7
        0//8
    }.AsReadOnly();

    public static IReadOnlyList<bool> Nothingness = new List<bool>
    {true,//虚无属性
        false,//1
        false,//2
        false,//3
        true,//4
        true,//5
        false,//6
        false,//7
        false
    }.AsReadOnly();

    public static IReadOnlyList<int> OnlyLState = new List<int>
    {0,//限制形态使用 0不限 1限自由 2限制连携
        0,//1
        0,//2
        0,//3
        0,//4
        0,//5
        0,//6
        0,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<int> EnterState = new List<int>
    {0,//1:进入自由态 2:进入连携态
        2,//1
        2,//2
        2,//3
        2,//4
        2,//5
        1,//6
        0,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<List<int>> Buff = new List<List<int>>
    {null,//Buff列表 1 见切
        null,//1
        null,//2
        null,//3
        null,//4
        null,//5
        null,//6
        new List<int>{1 },//7
        null//8
    }.AsReadOnly();

    public static IReadOnlyList<List<int>> DeBuff = new List<List<int>>
    {null,//施加deBuff列表
        null,//1
        null,//2
        null,//3
        null,//4
        null,//5
        null,//6
        null,//7
        null
    }.AsReadOnly();

    public static IReadOnlyList<List<int>> AttackDirection = new List<List<int>>
    {null,//攻击方向
        new List<int>{0},//1
        new List<int>{0},//2
        new List<int>{0,1,5},//3
        new List<int>{0,1,5},//4
        new List<int>{0,1,5},//5
        null,//6
        null,//7
        null
    }.AsReadOnly();

    public static IReadOnlyList<int> AttackLength = new List<int>
    {0,//攻击长度
        1,//1
        1,//2
        1,//3
        1,//4
        1,//5
        0,//6
        0,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<List<int>> Attack = new List<List<int>>
    {null,//造成伤害
        new List<int>{6},//1
        new List<int>{4},//2
        new List<int>{7},//3
        new List<int>{4,4},//4
        new List<int>{10},//5
        null,//6
        null,//7
        null
    }.AsReadOnly();

    public static IReadOnlyList<int> Defence = new List<int>
    {0,//格挡
        0,//1
        0,//2
        0,//3
        0,//4
        0,//5
        0,//6
        3,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<int> DeltaCost = new List<int>
    {0,//回费
        0,//1
        0,//2
        0,//3
        0,//4
        0,//5
        0,//6
        0,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<int> DeltaBladeNum = new List<int>
    {0,//改变气刃槽 正增负减
        2,//1
        1,//2
        -1,//3
        -2,//4
        -2,//5
        0,//6
        -1,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<int> DeltaBladeLevel = new List<int>
    {0,//改变气刃等级 正增负减
        0,//1
        0,//2
        0,//3
        0,//4
        1,//5
        0,//6
        0,//7
        0
    }.AsReadOnly();

    public static IReadOnlyList<int> DeltaHealth = new List<int>
    {0,//改变血量
        0,//1
        0,//2
        0,//3
        0,//4
        0,//5
        0,//6
        0,//7
        0
    }.AsReadOnly();

    public enum MoveType { None, Forward, Backward}
    //招式信息内部类
    public class EnemySkillConfig
    {
        public int skillID;
        public string skillName;
        public List<int>directions; //攻击方向 正前方0 逆时针标号 不移动为null
        public List<Vector2Int> range;//范围
        public int damage;
        public int hittimes;//伤害次数
        public MoveType moveType;
        public int moveDistance;    //移动方式和距离
        public List<int> debuffIDs;
    }

    //RogueMod
    public static readonly List<Commission> Commissions= new List<Commission>{ new Commission(0, "大贼龙", 1) };
}