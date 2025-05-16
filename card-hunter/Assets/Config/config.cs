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
        "直斩" ,//1
        "踏步斩",//2
        "气刃斩Ⅰ" ,//3
        "气刃斩Ⅱ",//4
        "气刃大回旋" ,//5
        "翻滚",//6
        "见切",//7
        "格挡",//8
        "纵斩",//9
        "剑鞘攻击",//10
        "居合拔刀斩"
    }.AsReadOnly();
    public static IReadOnlyList<string> CardImageName=new List<string> 
    {"",//卡图名称
        "Direct slash" ,//1
        "Step slash",//2
        "Bladegas slash1",//3
        "Bladegas slash2",//4
        "Bladegas big spin",//5
        "Roll",//6
        "Mind's Eye Slash",//7
        "Block",//8
        "Vertical slash",//9
        "Scabbard attack",//10
        "xiaojv"//11
        }.AsReadOnly();
    public static IReadOnlyList<string> CardText = new List<string>
    { "", //卡牌描述
        "造成6点伤害，回复2格气刃槽" , //1
        "向前移动一格，造成4点伤害，回复1格气刃槽" ,//2
        "消耗1格气刃槽,造成7点伤害,转向",//3
        "消耗2格气刃槽，造成两次4点伤害，转向，消耗，虚无",//4
        "消耗2格气刃槽,对前方三格范围内的敌人造成10点伤害，提升气刃等级，转向，消耗，虚无",//5
        "向任意方向移动一格,进入自由态",//6
        "消耗1格气刃槽,从后方三格中选择移动一格，获得一层免伤buff，若下回合开始时角色或见切前角色所处地块被攻击，则获得【气刃大回旋】，回满气刃槽。",//7
        "获得3点格挡",//8
        "造成8点伤害，恢复3格气刃槽后退或水平（前方左右）移动一格，进入自由态。",//9
        "造成5点伤害,获得5点格挡,回复1气刃槽转向",//10
        "造成两次4点伤害，抽1张牌，回复2气刃槽，转向。"//11

    }.AsReadOnly();
    public static IReadOnlyList<int> CardType = new List<int> 
    { 3,//卡牌类型
        0,//1 
        0,//2 
        0, //3
        0,//4
        0,//5
        2,//6
        2,//7
        1,//8
        0,//9
        0,//10
        0//11
    }.AsReadOnly();//0 攻击 1防御 2移动 3能力
    public static IReadOnlyList<int> Cost = new List<int>
    { 3,//费用
        1,//1
        1,//2
        1,//3
        1,//4
        1,//5
        1,//6
        2,//7
        0,//8
        1,//9
        1,//10
        1//11
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
        null,//8
        new List<int>{1,3,5},//9
        null,//10
        null//11
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
        new Vector2Int(0,0),//8
        new Vector2Int(1,1),//9
        new Vector2Int(0,0),//10
        new Vector2Int(0,0)//11
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
        0,//8
        0,//9
        0,//10
        0//11
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
        false,//8
        false,//9
        false,//10
        false //11
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
        0,//8
        0,//9
        0,//10
        1//11
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
        false,//8
        false,//9
        false,//10
        false//11
    }.AsReadOnly();

    public static IReadOnlyList<int> OnlyLState = new List<int>
    {2,//限制形态使用 0自由 1连携 2不限
        2,//1
        2,//2
        2,//3
        2,//4
        2,//5
        2,//6
        1,//7
        2,//8
        2,//9
        2,//10
        2,//11
    }.AsReadOnly();

    public static IReadOnlyList<int> EnterState = new List<int>
    {0,//1:进入自由态 2:进入连携态
        2,//1
        2,//2
        2,//3
        2,//4
        2,//5
        1,//6
        2,//7
        0,//8
        2,//9
        2,//10
        2//11
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
        null,//8
        null,//9
        null,//10
        null//11
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
        null,//8
        null,//9
        null,//10
        null,//11
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
        null,//8
        new List<int>{0},//9
        new List<int>{0,1,5},//10
        new List<int>{0,1,5}//11

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
        0,//8
        1,//9
        1,//10
        1//11
    }.AsReadOnly();

    public static IReadOnlyList<Vector2Int> Attack = new List<Vector2Int>
    {new Vector2Int(0, 0),//造成伤害
        new Vector2Int(6 , 1),//1
        new Vector2Int (4, 1),//2
        new Vector2Int (7, 1),//3
        new Vector2Int (4, 2),//4
        new Vector2Int (10, 1),//5
        new Vector2Int(0 , 0),//6
        new Vector2Int(0 , 0),//7
        new Vector2Int(0 , 0),//8
        new Vector2Int(8 , 1),//9
        new Vector2Int(5 , 1),//10
        new Vector2Int(4 , 2),//11
    }.AsReadOnly();

    public static IReadOnlyList<int> Defence = new List<int>
    {0,//格挡
        0,//1
        0,//2
        0,//3
        0,//4
        0,//5
        0,//6
        0,//7
        3,//8
        0,//9
        5,//10
        0,//11
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
        0,//8
        0,//9
        0,//10
        0,//11
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
        0,//8
        3,//9
        1,//10
        2,//11
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
        0,//8
        0,//9
        0,//10
        0,//11
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
        0,//8
        0,//9
        0,//10
        0,//11
    }.AsReadOnly();

    public static IReadOnlyList<bool> Sequence = new List<bool>
    {true,//0先攻击再移动 1先移动再攻击 默认为true
        true,//1
        true,//2
        true,//3
        true,//4
        true,//5
        true,//6
        true,//7
        true,//8
        false,//9
        true,//10
        true,//11
    }.AsReadOnly();
    
    public enum MoveType { None, Forward, Backward}
    public enum EnemyBuff { None, TurntoPlayer }//怪物给自己加的buff
    public enum EnemyDebuff { None, CantMove }//怪物给玩家加的debuff
    //招式信息内部类
    public class EnemySkillConfig
    {
        public int skillID;
        public string skillName;
        public List<Vector2Int> range;//范围(填写对于怪物相对xy轴的相对向量)
        public int damage;
        public int hittimes;//伤害次数
        public MoveType moveType;
        public int moveDistance;    //移动方式和距离
        public EnemyBuff enemybuff;
        public EnemyDebuff enemydebuff;  //怪物的特殊效果
        public int HPchange;//回复生命值数量（填入负值来回复生命）
    }
    public static IReadOnlyList<EnemySkillConfig> EnemySkills = new List<EnemySkillConfig>
    {
        new EnemySkillConfig
        {
            skillID = 1,
            skillName = "爪击",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(0, 1), new Vector2Int(1, -1) },
            damage =5,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            enemybuff = EnemyBuff.None,
            enemydebuff = EnemyDebuff.None,
            HPchange = 0
        },
        new EnemySkillConfig
        {
            skillID = 2,
            skillName = "二连前咬",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(0, 1), new Vector2Int(1, -1) },
            damage =3,
            hittimes = 2,
            moveType = MoveType.None,
            moveDistance = 0,
            enemybuff = EnemyBuff.TurntoPlayer,
            enemydebuff = EnemyDebuff.None,
            HPchange = 0
        },
        new EnemySkillConfig
        {
            skillID = 3,
            skillName = "龙车",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(2, 0), new Vector2Int(3, 0) },
            damage =8,
            hittimes = 1,
            moveType = MoveType.Forward,
            moveDistance = 3,
            enemybuff = EnemyBuff.None,
            enemydebuff = EnemyDebuff.None,
            HPchange = 0
        },
        new EnemySkillConfig
        {
            skillID = 4,
            skillName = "进食",
            range = new List<Vector2Int>(),
            damage =0,
            hittimes = 0,
            moveType = MoveType.None,
            moveDistance = 0,
            enemybuff = EnemyBuff.None,
            enemydebuff = EnemyDebuff.None,
            HPchange = -20
        },
        new EnemySkillConfig
        {
            skillID = 5,
            skillName = "浓痰喷射",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(0, 1), new Vector2Int(1, -1) },
            damage =4,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            enemybuff = EnemyBuff.None,
            enemydebuff = EnemyDebuff.CantMove,
            HPchange = 0
        },
        new EnemySkillConfig
        {
            skillID = 6,
            skillName = "下压",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(0,1),new Vector2Int(1,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1)},
            damage =9,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            enemybuff = EnemyBuff.None,
            enemydebuff = EnemyDebuff.None,
            HPchange = 0
        }
    }.AsReadOnly();

    public static IReadOnlyList<string> Destinies = new List<string> {"Forest","Desert","Valcano"};
    public const int RoutePointNum= 2;//路径上的节点
    public const int PointDistance = 1;//节点间距
    public const float MoveDuration = 1;//移动时长

    //RogueMod
    public static readonly List<Commission> Commissions= new List<Commission>{ new Commission(0, "大贼龙", 1,0) };
}