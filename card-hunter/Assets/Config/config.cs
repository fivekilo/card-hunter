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
        "气刃斩1" ,//3
        "气刃斩2",//4
        "气刃大回旋" ,//5
        "翻滚",//6
        "见切",//7
        "格挡",//8
        "纵斩",//9
        "剑鞘攻击",//10
        "居合拔刀斩",//11
        "蓄意破坏",//12
        "侧步",//13
        "弱点突刺",//14
        "",//15
        "",//16
        "",//17
        "",//18
        "",//19
        "气刃突刺",//20
        "气刃兜割",//21
        "居合拔刀气刃斩",//22
        "飞翔爪攻击"//23
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
        "xiaojv",//11
        "Intentional destruction",//12
        "Direct slash",//13
        "Direct slash",//14
        "Direct slash",//15
        "Direct slash",//16
        "Direct slash",//17
        "Direct slash",//18
        "Direct slash",//19
        "The gas blade pierces",//20
        "Gas blade pocket cutting",//21
        "dajv",//22
        "Flying claw attack",//23
        }.AsReadOnly();
    public static IReadOnlyList<string> CardText = new List<string>
    { "", //卡牌描述
        "造成6点伤害，回复2格气刃槽" , //1
        "向前移动一格，造成4点伤害，回复1格气刃槽" ,//2
        "消耗1格气刃槽,造成7点伤害,转向",//3
        "消耗2格气刃槽，造成两次4点伤害，转向，消耗，虚无",//4
        "消耗2格气刃槽,对前方三格范围内的敌人造成10点伤害，提升气刃等级，转向，消耗，虚无",//5
        "向任意方向移动一格,进入自由态",//6
        "限连携，消耗1格气刃槽,从后方三格中选择移动一格，获得一层免伤buff，若下回合开始时见切前角色所处地块或角色被攻击，则获得【气刃大回旋】，回满气刃槽。",//7
        "获得3点格挡",//8
        "造成8点伤害，恢复3格气刃槽后退或水平（前方左右）移动一格，进入自由态。",//9
        "造成5点伤害,获得5点格挡,回复1气刃槽转向",//10
        "造成两次4点伤害，抽1张牌，回复2气刃槽，转向。",//11
        "造成2点伤害,造成2层【伤口】。",//12
        "向左边或右边位移一格（无前后），不改变状态",//13
        "造成6点伤害,回复2气刃槽, 如果有伤口，则降低2层【伤口】造成10点伤害,转向",//14
        "",//15
        "",//16
        "",//17
        "",//18
        "",//19
        "向前移动一格，造成8伤害 恢复1气刃槽。如果有气刃等级，将【气刃兜割】加入手卡",//20
        "限连携态，根据气刃等级的不同造成不同伤害。白刃：4*5 黄刃：5*5 红刃 6*5。下一回合开始获得1费抽一张牌。",//21
        "自由选择方向后结束你的回合。获得1层【大居合】buff，受到伤害消耗该buff会向人物所面对的方向位移1格，造成5*4点伤害，提升气刃等级。",//22
        "移动两格，造成12点伤害，造成4层【伤口】 ，回复3格气刃槽",//23
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
        0,//11
        0,//12
        2,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
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
        1,//11
        0,//12
        0,//13
        1,//14
        1,//15
        1,//16
        1,//17
        1,//18
        1,//19
        1,//20
        1,//21
        2,//22
        2,//23
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
        null,//11
        null,//12
        new List<int>{1,2,4,5},//13
        null,//14
        null,//15
        null,//16
        null,//17
        null,//18
        null,//19
        new List<int>{0},//20
        null,//21
        null,//22
        new List<int>{0 }//23
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
        new Vector2Int(0,0),//11
        new Vector2Int(0,0),//12
        new Vector2Int(1,1),//13
        new Vector2Int(0,0),//14
        new Vector2Int(0,0),//15
        new Vector2Int(0,0),//16
        new Vector2Int(0,0),//17
        new Vector2Int(0,0),//18
        new Vector2Int(0,0),//19
        new Vector2Int(1,1),//20
        new Vector2Int(0,0),//21
        new Vector2Int(0,0),//22
        new Vector2Int(1,2),//23
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
        0,//11
        0,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        21,//20
        0,//21
        0,//22
        0,//23
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
        false,//11
        false,//12
        false,//13
        false,//14
        false,//15
        false,//16
        false,//17
        false,//18
        false,//19
        false,//20
        true,//21
        false,//22
        false,//23
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
        1,//11
        0,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
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
        false,//11
        false,//12
        false,//13
        false,//14
        false,//15
        false,//16
        false,//17
        false,//18
        false,//19
        false,//20
        false,//21
        false,//22
        false,//23
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
        2,//12
        2,//13
        1,//14
        2,//15
        2,//16
        2,//17
        2,//18
        2,//19
        2,//20
        1,//21
        2,//22
        2,//23
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
        2,//11
        2,//12
        0,//13
        2,//14
        2,//15
        2,//16
        2,//17
        2,//18
        2,//19
        2,//20
        2,//21
        2,//22
        2,//23
    }.AsReadOnly();

    public static IReadOnlyList<List<int>> Buff = new List<List<int>>
    {null,//Buff列表 2 见切
        null,//1
        null,//2
        null,//3
        null,//4
        null,//5
        null,//6
        new List<int>{2 },//7
        null,//8
        null,//9
        null,//10
        null,//11
        null,//12
        null,//13
        null,//14
        null,//15
        null,//16
        null,//17
        null,//18
        null,//19
        null,//20
        null,//21
        new List<int>{4},//22
        null,//23
    }.AsReadOnly();

    public static IReadOnlyList<int> Wound = new List<int>
    {0,//施加伤口层数
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
        2,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        4,//23
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
        new List<int>{0,1,5},//11
        new List<int>{0},//12
        null,//13
        new List<int>{0},//14
        null,//15
        null,//16
        null,//17
        null,//18
        null,//19
        new List<int>{0},//20
        new List<int>{0},//21
        new List<int>{0,1,2,3,4,5},//22
        new List<int>{0},//23
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
        1,//11
        1,//12
        0,//13
        1,//14
        1,//15
        1,//16
        1,//17
        1,//18
        1,//19
        1,//20
        1,//21
        1,//22
        1,//23
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
        new Vector2Int(2 , 1),//12
        new Vector2Int(0 , 0),//13
        new Vector2Int(6 , 1),//14
        new Vector2Int(0 , 0),//15
        new Vector2Int(0 , 0),//16
        new Vector2Int(0 , 0),//17
        new Vector2Int(0 , 0),//18
        new Vector2Int(0 , 0),//19
        new Vector2Int(8 , 1),//20
        new Vector2Int(5 , 3),//21
        new Vector2Int(0 , 0),//22
        new Vector2Int(12,1),//23
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
        0,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
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
        0,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
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
        0,//12
        0,//13
        2,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        1,//20
        0,//21
        0,//22
        3,//23
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
        0,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        -1,//21
        0,//22
        0,//23
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
        0,//12
        0,//13
        0,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
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
        true,//12
        true,//13
        true,//14
        true,//15
        true,//16
        true,//17
        true,//18
        true,//19
        true,//20
        true,//21
        true,//22
        true,//23
    }.AsReadOnly();
    
    public enum MoveType { None, Forward, Backward}
    //怪物给自己加的buff:转向
    public enum EnemyBuff { None, TurntoPlayer }
    //怪物给玩家加的debuff:无法移动，麻痹，震慑
    public enum EnemyDebuff { None, CantMove, Numbness, Deterrence }
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
        public EnemyBuff getbuff;
        public EnemyDebuff pushdebuff;  //怪物的特殊效果
        public int HPchange;//回复生命值数量（填入负值来回复生命）
        public int armor;//获得护甲数量
    }
    public static IReadOnlyList<EnemySkillConfig> EnemySkills = new List<EnemySkillConfig>
    {
        new EnemySkillConfig
        {
            skillID = 0,
            skillName = "力竭倒地",
            range = new List<Vector2Int>{},
            damage =0,
            hittimes = 0,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 1,
            skillName = "爪击",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(0, 1), new Vector2Int(1, -1) },
            damage =5,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 2,
            skillName = "二连咬",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(2, 0)},
            damage =4,
            hittimes = 2,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.TurntoPlayer,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
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
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
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
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = -20,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 5,
            skillName = "浓痰喷射",
            range = new List<Vector2Int>{new Vector2Int(2,0), new Vector2Int(3, 0), new Vector2Int(2, 1), new Vector2Int(3, -1)},
            damage =5,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.CantMove,
            HPchange = 0,
            armor=0
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
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 7,
            skillName = "飞挠",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(0,1),new Vector2Int(2,0),new Vector2Int(1,-1)},
            damage =9,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 8,
            skillName = "飞扑扫尾",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(1,1),
                new Vector2Int(2,1),new Vector2Int(2,-1),new Vector2Int(3,1)},
            damage =8,
            hittimes = 1,
            moveType = MoveType.Forward,
            moveDistance = 2,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 9,
            skillName = "眩鸟的闪光",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(0,1),
                new Vector2Int(0,2),new Vector2Int(0,3),new Vector2Int(1,1),new Vector2Int(1,2),new Vector2Int(2,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1),new Vector2Int(3,-1),new Vector2Int(2,-2),new Vector2Int(3,-2),
                new Vector2Int(3,-3),},
            damage =0,
            hittimes = 0,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.Numbness,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 10,
            skillName = "龙吼",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(0,1),new Vector2Int(0,-1),new Vector2Int(1,-1),new Vector2Int(-1,1)},
            damage =3,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.Deterrence,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 11,
            skillName = "二连前咬",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(0,1),new Vector2Int(1,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1)},
            damage =5,
            hittimes = 2,
            moveType = MoveType.Forward,
            moveDistance = 1,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None, 
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 12,
            skillName = "昂头喷火",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(4,0),new Vector2Int(1,1),
                new Vector2Int(2,-1)},
            damage =9,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        },
        new EnemySkillConfig
        {
            skillID = 13,
            skillName = "冲撞撕咬",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(4,0)},
            damage =6,
            hittimes = 2,
            moveType = MoveType.Forward,
            moveDistance = 4,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0
        }
    }.AsReadOnly();

    //GameManager
    public const int CommissionAmount = 7;

    //DeckWin 画图参数
    public const int XBound = 720, YBound = 270;//X,Y界限坐标
    public const int Xdelta = 440, Ydelta = 540;//两卡间隔

    //RouteRender参数
    public static IReadOnlyList<string> Destinies = new List<string> { "Camp","Forest", "Desert", "Valcano" };
    public const int RoutePointNum = 2;//路径上的节点
    public const int PointDistance = 1;//节点间距
    public const float MoveDuration = 1;//移动时长
    public static readonly Vector3 CampRestPos = new Vector3(-0.6819376f, -1.417444f, 0);

    //RogueMod
    public static readonly List<Commission> Commissions = new List<Commission> { new Commission(0, "大贼龙", 1, 1) };
    public static readonly List<Event> Events = new List<Event> {
        new Event(1,"剑术大师","剑术大师小作文","3",
            new List<Choice>{
                new Choice(1,"[夯实基础]删一张牌",1,0,0,0,new List<int>(),0),
                new Choice(2,"[学习技巧]获得蓝色稀有度的一张牌",0,1,0,0,new List<int>{1,2,3,6 },0),
                new Choice(3,"[祖传秘技]花费？？金币，获得一张金色稀有度的卡牌",0,1,-20,0,new List<int>{1,2,3,6 },0)
            })
    };
    public static readonly Vector2Int EventAmountBounds = new Vector2Int(1, 1);//每层总事件数限制
    public const int EventPerTour = 2;
}