using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public static List<int> normal = new List<int>() { 9,9,9,10,10,10,11,11,11,12,12,12,13,13,13,14,14,14,15,15,15,16,16,16,17,17,17,18,18,18,19,19,19,20,20,22,22,23,23,24,24,25,25,26,26,27,27,28,28,29,29,32,32,33,34,35,37,};
    public static List<int> rare = new List<int>() { 20, 20, 22, 22, 23, 23, 24, 24, 25, 25, 26, 26, 27, 27, 28, 28, 29, 29, 32, 32, 33, 34, 35, 37, };
    public static List<int> gold = new List<int>() { 33, 34, 35, 37, };
    public const double ObstacleRate = 0.15;//障碍物生成概率
    public const double ContentRate = 0.2;//地图要素生成概率
    public const int ObstacleSup = 10;//障碍物生成上限
    public const int size = 7;
    public const int InitialHealth = 80; //血量
    public const int InitialCost = 3; //初始费用
    public const int InitialHandCardNum = 4; //初始手牌数
    public const int MaxHandCardNum = 8;
    public const int MaxBladeLevel = 3; //最大气刃等级
    public const int MaxBladeNum = 8;//最大气刃值
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
        "战场冥想",//15
        "精准防御",//16
        "拔刀二连斩",//17
        "简易气刃释放",//18
        "看破斩·旋",//19
        "气刃突刺",//20
        "气刃兜割",//21
        "居合拔刀气刃斩",//22
        "飞翔爪攻击",//23
        "飞身踢",//24
        "凝气于血",//25
        "气刃护盾",//26
        "居登",//27
        "伤口管理",//28
        "气刃踏步斩",//29
        "一字斩",//30
        "刚气刃斩",//31
        "强化纳刀",//32
        "樱花斩",//33
        "炼气解放无双斩",//34
        "集中突刺无尽",//35
        "圆月",//36
        "红刃狂热"//37
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
        "13",//13
        "14",//14
        "Direct slash",//15
        "Direct slash",//16
        "Direct slash",//17
        "18",//18
        "19",//19
        "The gas blade pierces",//20
        "Gas blade pocket cutting",//21
        "dajv",//22
        "Flying claw attack",//23
        "DSB",//24
        "DSB",//25
        "DSB",//26
        "DSB",//27
        "DSB",//28
        "29",//29
        "DSB",//30
        "DSB",//31
        "DSB",//32
        "DSG",//33
        "34",//34
        "35",//35
        "DSG",//36
        "DSG",//37
        }.AsReadOnly();
    public static IReadOnlyList<string> CardText = new List<string>
    { "", //卡牌描述
        "造成6点伤害，回复2格气刃槽" , //1
        "向前移动一格，造成4点伤害，回复1格气刃槽" ,//2
        "消耗1格气刃槽,造成7点伤害,转向,派生气刃斩2",//3
        "消耗2格气刃槽，造成两次4点伤害，转向，消耗，虚无，派生气刃大回旋",//4
        "消耗2格气刃槽,对前方三格范围内的敌人造成10点伤害，提升气刃等级，转向，消耗，虚无",//5
        "向任意方向移动一格,进入自由态",//6
        "限连携，消耗1格气刃槽,从后方三格中选择移动一格，获得一层免伤buff，若下回合开始前角色受到攻击，则获得【气刃大回旋】，回满气刃槽。",//7
        "获得3点格挡",//8
        "造成8点伤害，恢复3格气刃槽后退或水平（前方左右）移动一格，进入自由态。",//9
        "造成5点伤害,获得5点格挡,回复1气刃槽转向",//10
        "造成两次4点伤害，抽1张牌，回复2气刃槽，转向。",//11
        "造成2点伤害,造成1层【伤口】。",//12
        "向左边或右边位移一格（无前后），不改变状态",//13
        "造成6点伤害,回复2气刃槽, 造成2层【伤口】",//14  ！
        "回复8格气刃槽",//15
        "获得8点格挡，抽一张牌",//16
        "仅自由态，造成5*2点伤害，回复3格气刃槽",//17
        "造成8点伤害，下降气刃等级，获得1费抽一张牌。消耗",//18
        "消耗1气刃槽，对前方一格造成12点伤害，向后方三格移动一格，获得【见切】buff",//19
        "造成8伤害 恢复1气刃槽。如果有气刃等级，将【气刃兜割】加入手卡",//20 ！
        "限连携态，向任意方向移动一格，向任意方向攻击。根据气刃等级造成伤害。白刃：5*4 黄刃：5*5 红刃 5*6。下一回合开始获得1费抽一张牌。下降气刃等级，消耗，虚无",//21 ！
        "自由选择方向后结束你的回合。获得1层【大居合】buff，受到伤害消耗该buff会向人物所面对的方向位移1格，造成5*4点伤害，提升气刃等级。",//22
        "移动两格，造成12点伤害，造成4层【伤口】 ，回复3格气刃槽",//23
        "最多位移2格，如果有气刃等级，将【气刃兜割】加入手牌",//24 !
        "消耗1层气刃等级，消耗3点生命值，获得3点费用",//25
        "提升气刃等级后,获得4点格挡",//26 ! 能力
        "使用【居合】后，你的下一张【气刃兜割】费用减1",//27 ! 能力
        "你造成的【伤口】的效果提高一层",//28 ! 能力 
        "向前移动一格，造成8点伤害，消耗3格气刃槽，派生一字斩",//29
        "对前方造成4点伤害，消耗1格气刃槽，派生刚气刃斩,转向,消耗，虚无",//30
        "造成8*2点伤害，获得12点格挡，提升气刃等级,转向,消耗，虚无",//31
        "向任意方向最多移动两格，进入自由态。清空气刃槽，每清空一格下一次攻击伤害提高2。",//32 !
        "最多位移3格，对沿途的怪物造成6*3伤害，位移可以穿过怪物，提升气刃等级，进入自由态。",//33
        "限连携态，有两层气刃等级时使用。消耗两层气刃等级，造成7*6伤害，获得30点格挡，进入自由态。消耗",//34 
        "造成8点伤害，回复2气刃槽，如果有至少3层【伤口】，消耗3层，造成15点伤害并提升一级气刃等级 转向 。",//35 !
        "向角色的各个方向各两格展开一个圆形区域，持续三回合。在该区域内，角色的气刃槽锁定为8格，【见切】和【居合】所需费用降低1点。在离开圆月时气刃槽清空",//36 !
        "提升在红色气刃等级下伤害的倍率0.5，在红刃时不能获得任何格挡值"//37 ! 能力
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
        1,//15
        1,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
        2,//24
        1,//25
        3,//26
        3,//27
        3,//28
        0,//29
        0,//30
        0,//31
        2,//32
        0,//33
        0,//34
        0,//35
        1,//36
        3,//37
    }.AsReadOnly();//0 攻击 1技能 2移动 3能力
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
        0,//18
        2,//19
        1,//20
        2,//21
        2,//22
        2,//23
        1,//24
        0,//25
        1,//26
        2,//27
        1,//28
        1,//29
        0,//30
        2,//31
        1,//32
        2,//33
        3,//34
        1,//35
        2,//36
        1,//37
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
        new List<int>{2,3,4},//19
        null,//20
        new List<int>{0,1,2,4,5},//21
        null,//22
        new List<int>{0 },//23
        new List<int>{0 },//24
        null,//25
        null,//26
        null,//27
        null,//28
        new List<int>{0 },//29
        null,//30
        null,//31
        new List<int>{0,1,2,4,5},//32
        new List<int>{0,1,2,4,5},//33
        null,//34
        null,//35
        null,//36
        null,//37
    }.AsReadOnly();

    public static IReadOnlyList<List<int>> AttackRange = new List<List<int>>
    {   null,//0
        new List<int>{0},//1
        new List<int>{0},//2
        new List<int>{0},//3
        new List<int>{0},//4
        new List<int>{0,1,5},//5
        null,//6
        null,//7
        null,//8
        new List<int>{0},//9
        new List<int>{0},//10
        new List<int>{0},//11
        new List<int>{0},//12
        null,//13
        new List<int>{0},//14
        null,//15
        null,//16
        new List<int>{0},//17
        new List<int>{0},//18
        new List<int>{0},//19
        new List<int>{0},//20
        new List<int>{0},//21
        new List<int>{0},//22
        new List<int>{0},//23
        null,//24
        null,//25
        null,//26
        null,//27
        null,//28
        new List<int>{0},//29
        new List<int>{0},//30
        new List<int>{0},//31
        null,//32
        new List<int>{0},//33
        new List<int>{0},//34
        new List<int>{0},//35
        null,//36
        null,//37
    };


    public static IReadOnlyList<Vector2Int> MoveLength = new List<Vector2Int>
    {
        new Vector2Int(0,0),//移动长度 下上限
        new Vector2Int(0,0),//1
        new Vector2Int(0,1),//2
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
        new Vector2Int(1,1),//19
        new Vector2Int(0,0),//20
        new Vector2Int(1,1),//21
        new Vector2Int(0,0),//22
        new Vector2Int(1,2),//23
        new Vector2Int(1,2),//24
        new Vector2Int(0,0),//25
        new Vector2Int(0,0),//26
        new Vector2Int(0,0),//27
        new Vector2Int(0,0),//28
        new Vector2Int(0,1),//29
        new Vector2Int(0,0),//30
        new Vector2Int(0,0),//31
        new Vector2Int(0,2),//32
        new Vector2Int(0,3),//33
        new Vector2Int(0,0),//34
        new Vector2Int(0,0),//35
        new Vector2Int(0,0),//36
        new Vector2Int(0,0),//37
    }.AsReadOnly();

    public static IReadOnlyList<int> Derivation = new List<int>
    {0,//派生卡牌
        0,//1
        0,//2
        4,//3
        5,//4
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
        21,//20
        0,//21
        0,//22
        0,//23
        21,//24
        0,//25
        0,//26
        0,//27
        0,//28
        30,//29
        31,//30
        0,//31
        0,//32
        0,//33
        0,//34
        0,//35
        0,//36
        0,//37
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
        true,//18
        false,//19
        false,//20
        true,//21
        false,//22
        false,//23
        false,//24
        true,//25
        true,//26
        true,//27
        true,//28
        false,//29
        true,//30
        true,//31
        false,//32
        false,//33
        true,//34
        false,//35
        false,//36
        false,//37
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
        1,//16
        0,//17
        1,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
        0,//24
        0,//25
        0,//26
        0,//27
        0,//28
        0,//29
        0,//30
        0,//31
        0,//32
        0,//33
        0,//34
        0,//35
        0,//36
        0,//37
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
        true,//21
        false,//22
        false,//23
        false,//24
        false,//25
        false,//26
        false,//27
        false,//28
        false,//29
        true,//30
        true,//31
        false,//32
        false,//33
        false,//34
        false,//35
        false,//36
        false,//37
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
        2,//14
        2,//15
        2,//16
        0,//17
        2,//18
        2,//19
        2,//20
        1,//21
        2,//22
        2,//23
        2,//24
        2,//25
        2,//26
        2,//27
        2,//28
        2,//29
        2,//30
        2,//31
        2,//32
        2,//33
        1,//34
        2,//35
        2,//36
        2,//37
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
        0,//15
        0,//16
        2,//17
        2,//18
        2,//19
        2,//20
        2,//21
        2,//22
        2,//23
        2,//24
        0,//25
        0,//26
        0,//27
        0,//28
        2,//29
        2,//30
        2,//31
        1,//32
        1,//33
        1,//34
        2,//34
        0,//36
        0,//37
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
        new List<int>{2},//19
        null,//20
        new List<int>{8},//21
        new List<int>{4},//22
        null,//23
        null,//24
        null,//25
        new List<int>{10},//26 气刃护盾10
        new List<int>{11},//27 居登11
        new List<int>{12},//28 伤口管理12
        null,//29
        null,//30
        null,//31
        null,//32
        null,//33
        null,//34
        null,//35
        null,//36
        new List<int>{13},//37 红刃狂热13
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
        1,//12
        0,//13
        2,//14
        0,//15
        0,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        4,//23
        0,//24
        0,//25
        0,//26
        0,//27
        0,//28
        0,//29
        0,//30
        0,//31
        0,//32
        0,//33
        0,//34
        0,//35
        0,//36
        0,//37
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
        new List<int>{0},//17
        new List<int>{0},//18
        new List<int>{0},//19
        new List<int>{0},//20
        new List<int>{0,1,2,3,4,5},//21
        new List<int>{0,1,2,3,4,5},//22
        new List<int>{0},//23
        null,//24
        null,//25
        null,//26
        null,//27
        null,//28
        new List<int>{0},//29
        new List<int>{0,1,5},//30
        new List<int>{0,1,5},//31
        null,//32
        new List<int>{3},//33
        new List<int>{0},//34
        new List<int>{0},//35
        null,//36
        null,//37
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
        0,//15
        0,//16
        1,//17
        1,//18
        1,//19
        1,//20
        1,//21
        1,//22
        1,//23
        0,//24
        0,//25
        0,//26
        0,//27
        0,//28
        1,//29
        1,//30
        1,//31
        0,//32
        3,//33
        1,//34
        1,//35
        0,//36
        0,//37
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
        new Vector2Int(5 , 2),//17
        new Vector2Int(8 , 1),//18
        new Vector2Int(12 , 1),//19
        new Vector2Int(8 , 1),//20
        new Vector2Int(5 , 3),//21
        new Vector2Int(0 , 0),//22
        new Vector2Int(12,1),//23
        new Vector2Int(0 , 0),//24
        new Vector2Int(0 , 0),//25
        new Vector2Int(0 , 0),//26
        new Vector2Int(0 , 0),//27
        new Vector2Int(0 , 0),//28
        new Vector2Int(8 , 1),//29
        new Vector2Int(4 , 1),//30
        new Vector2Int(8 , 2),//31
        new Vector2Int(0 , 0),//32
        new Vector2Int(6 , 3),//33
        new Vector2Int(6 , 7),//34
        new Vector2Int(8 , 1),//35
        new Vector2Int(0,0),//36
        new Vector2Int(0,0),//37
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
        8,//16
        0,//17
        0,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
        0,//24
        0,//25
        0,//26
        0,//27
        0,//28
        0,//29
        0,//30
        12,//31
        0,//32
        0,//33
        30,//34
        0,//35
        0,//36
        0,//37
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
        1,//18
        0,//19
        0,//20
        0,//21
        0,//22
        0,//23
        0,//24
        0,//25
        0,//26
        0,//27
        0,//28
        0,//29
        0,//30
        0,//31
        0,//32
        0,//33
        0,//34
        0,//35
        0,//36
        0,//37
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
        8,//15
        0,//16
        3,//17
        0,//18
        -1,//19
        1,//20
        0,//21
        0,//22
        3,//23
        0,//24
        0,//25
        0,//26
        0,//27
        0,//28
        -3,//29
        -1,//30
        -4,//31
        0,//32
        0,//33
        0,//34
        2,//35
        0,//36
        0,//37
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
        -1,//18
        0,//19
        0,//20
        -1,//21
        0,//22
        0,//23
        0,//24
        -1,//25
        0,//26
        0,//27
        0,//28
        0,//29
        0,//30
        1,//31
        0,//32
        1,//33
        -2,//34
        0,//35
        0,//36
        0,//37
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
        0,//24
        -3,//25
        0,//26
        0,//27
        0,//28
        0,//29
        0,//30
        0,//31
        0,//32
        0,//33
        0,//34
        0,//35
        0,//36
        0,//37
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
        false,//19
        true,//20
        true,//21
        true,//22
        true,//23
        true,//24
        true,//25
        true,//26
        true,//27
        true,//28
        true,//29
        true,//30
        true,//31
        true,//32
        false,//33
        true,//34
        true,//35
        true,//36
        true,//37
    }.AsReadOnly();

    public List<int> EnemyID = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
    public enum MoveType { None, Forward, Backward,Fly}
    //怪物给自己加的buff:转向
    public enum EnemyBuff { None, TurntoPlayer }
    //怪物给玩家加的debuff:无法移动，麻痹，震慑，冰冻
    public enum EnemyDebuff { None, CantMove, Numbness, Deterrence,Freezed }
    public enum SkillType { Normal,Locked}//技能类型：一般，锁定
    //怪物生成环境要素:熔岩，电球，冰柱
    public enum AddEnvironment { None, Lava, ElectricBall, Icicle}
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
        public SkillType skilltype;
        public AddEnvironment addenvironment;
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
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 1,
            skillName = "爪击",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(0, 1), new Vector2Int(1, -1) },
            damage =6,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
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
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 3,
            skillName = "龙车",
            range = new List<Vector2Int>{new Vector2Int(1,0), new Vector2Int(2, 0), new Vector2Int(3, 0) },
            damage =9,
            hittimes = 1,
            moveType = MoveType.Forward,
            moveDistance = 3,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
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
            HPchange = -25,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 5,
            skillName = "浓痰喷射",
            range = new List<Vector2Int>{new Vector2Int(2,0), new Vector2Int(3, 0), new Vector2Int(2, 1), new Vector2Int(3, -1)},
            damage =6,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.CantMove,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 6,
            skillName = "下压",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(0,1),new Vector2Int(1,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1)},
            damage =10,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
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
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 8,
            skillName = "飞扑扫尾",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(1,1),
                new Vector2Int(2,1),new Vector2Int(2,-1),new Vector2Int(3,-1)},
            damage =9,
            hittimes = 1,
            moveType = MoveType.Forward,
            moveDistance = 2,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 9,
            skillName = "眩鸟的闪光",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(0,1),
                new Vector2Int(0,2),new Vector2Int(0,3),new Vector2Int(1,1),new Vector2Int(1,2),new Vector2Int(2,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1),new Vector2Int(3,-1),new Vector2Int(2,-2),new Vector2Int(3,-2),
                new Vector2Int(3,-3)},
            damage =0,
            hittimes = 0,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.Numbness,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 10,
            skillName = "龙吼",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(0,1),new Vector2Int(0,-1),new Vector2Int(1,-1),new Vector2Int(-1,1)},
            damage =6,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.Deterrence,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 11,
            skillName = "二连前咬",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(0,1),new Vector2Int(1,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1)},
            damage =5,
            hittimes = 2,
            moveType = MoveType.Forward,
            moveDistance = 1,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None, 
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 12,
            skillName = "昂头喷火",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(1,1),
                new Vector2Int(2,-1)},
            damage =8,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 13,
            skillName = "冲撞撕咬",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(4,0)},
            damage =5,
            hittimes = 2,
            moveType = MoveType.Forward,
            moveDistance = 4,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 14,
            skillName = "背身扫尾",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(1,1),new Vector2Int(1,2),
                new Vector2Int(2,1),new Vector2Int(2,-1),new Vector2Int(3,-1),new Vector2Int(3,-2)},
            damage =10,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 15,
            skillName = "扫火",
            range = new List<Vector2Int>{new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(1,2),new Vector2Int(2,1),
                new Vector2Int(3,-1),new Vector2Int(3,-2)},
            damage =10,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 18,
            skillName = "火球连射",
            range = new List<Vector2Int>{new Vector2Int(0,0),new Vector2Int(1,0),new Vector2Int(-1,1),new Vector2Int(0,-1)},
            damage =4,
            hittimes = 3,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Locked,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 19,
            skillName = "吞食岩石",
            range = new List<Vector2Int>(),
            damage =0,
            hittimes = 0,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=20,//第一次获得20点护甲，第二次获得40点护甲
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 20,
            skillName = "岩浆满溢",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(1,1),new Vector2Int(2,-1),
                new Vector2Int(2,2),new Vector2Int(3,1),new Vector2Int(4,-1),new Vector2Int(3,1),
                new Vector2Int(4,-1),new Vector2Int(4,-2)},
            damage =8,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.Lava
        },
        new EnemySkillConfig
        {
            skillID = 21,
            skillName = "巨型熔岩球",
            range = new List<Vector2Int>{new Vector2Int(0,0),new Vector2Int(1,0),new Vector2Int(-1,0),new Vector2Int(0,1),
                new Vector2Int(-1,1),new Vector2Int(0,-1),new Vector2Int(1,-1)},
            damage =8,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Locked,
            addenvironment=AddEnvironment.Lava
        },
        new EnemySkillConfig
        {
            skillID = 22,
            skillName = "多重拍击",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(0,1),new Vector2Int(1,1),
                new Vector2Int(1,-1),new Vector2Int(2,-1)},
            damage =5,
            hittimes = 2,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 23,
            skillName = "甩尾下砸",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(1,1),
                new Vector2Int(2,1),new Vector2Int(3,1),new Vector2Int(2,-1),new Vector2Int(3,-1),new Vector2Int(4,-1)},
            damage =12,
            hittimes = 1,
            moveType = MoveType.Backward,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 24,
            skillName = "夺命回旋",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(-1,0),new Vector2Int(-2,0),
                new Vector2Int(-2,1),new Vector2Int(-1,1),new Vector2Int(0,1),new Vector2Int(1,1),new Vector2Int(-2,2),
                new Vector2Int(-1,2),new Vector2Int(0,2),new Vector2Int(-1,-1),new Vector2Int(0,-1),new Vector2Int(1,-1),
                new Vector2Int(2,-1),new Vector2Int(0,-2),new Vector2Int(1,-2),new Vector2Int(2,-2)},
            damage =12,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 25,
            skillName = "肩撞",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(2,0),new Vector2Int(3,0),new Vector2Int(0,1),
                new Vector2Int(1,1),new Vector2Int(1,-1),new Vector2Int(2,-1)},
            damage =10,
            hittimes = 1,
            moveType = MoveType.Forward,
            moveDistance = 3,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 26,
            skillName = "积雷怒吼",
            range = new List<Vector2Int>{new Vector2Int(1,0),new Vector2Int(-1,0),new Vector2Int(-1,1),new Vector2Int(0,1),
                new Vector2Int(0,-1),new Vector2Int(1,-1)},
            damage =6,
            hittimes = 1,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.Deterrence,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 27,
            skillName = "雷霆一击",
            range = new List<Vector2Int>{new Vector2Int(0,0),new Vector2Int(1,0),new Vector2Int(-1,0),new Vector2Int(0,1),
                new Vector2Int(-1,1),new Vector2Int(0,-1),new Vector2Int(1,-1)},
            damage =18,
            hittimes = 1,
            moveType = MoveType.Fly,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Locked,
            addenvironment=AddEnvironment.None
        },
        new EnemySkillConfig
        {
            skillID = 28,
            skillName = "雷球之阵",
            range = new List<Vector2Int>{new Vector2Int(1,1),new Vector2Int(-1,2),new Vector2Int(-2,1),new Vector2Int(2,-1),
                new Vector2Int(1,-2),new Vector2Int(-1,-1)},
            damage =0,
            hittimes = 0,
            moveType = MoveType.None,
            moveDistance = 0,
            getbuff = EnemyBuff.None,
            pushdebuff = EnemyDebuff.None,
            HPchange = 0,
            armor=0,
            skilltype=SkillType.Normal,
            addenvironment=AddEnvironment.ElectricBall
        }
    }.AsReadOnly();

    //GameManager
    public const int CommissionAmount = 7;
    public static readonly Vector3 CameraNew = new Vector3(100, 100, -10);//摄像机移走位置
    public static readonly Vector3 CameraDefault = new Vector3(0, 0, -10);//摄像机原本位置

    //DeckWin 画图参数
    public const int XBound = 720, YBound = 270;//X,Y界限坐标
    public const int Xdelta = 440, Ydelta = 540;//两卡间隔
    public static readonly Vector3 hoverScale = new Vector3(2.122892f, 2.54747f, 1f);
    public static readonly Vector3 normalScale = new Vector3(1.73865f, 2.08638f, 1f);

    //CommissionBoard 画图参数
    public const int BoardX = -380,BoardY=0;

    //RouteRender参数
    public static IReadOnlyList<string> Destinies = new List<string> { "Camp","Forest", "Desert", "Valcano" };
    public const int RoutePointNum = 2;//路径上的节点
    public const int PointDistance = 1;//节点间距
    public const float MoveDuration = 1;//移动时长
    public static readonly Vector3 CampRestPos = new Vector3(-0.6819376f, -1.417444f, 0);
    //Choice(int id, string text, int modifydeck,bool random, int battle,int money, int health, int HPupper, List<int> cardsID, int equipment)
    //RogueMod
    public static readonly List<Commission> Commissions = new List<Commission> { new Commission(0, "大贼龙", 1, 1,100) };
    public static readonly List<Event> Events = new List<Event> {
        new Event(1,"剑术大师","年迈的剑术大师，自远方而来，背上的雌火龙太刀闪烁着他过去的功绩。他愿意指点你的狩猎技巧，如果你愿意付出一些代价的话，他甚至能教给你一些失传秘技。","3",
            new List<Choice>{
                new Choice(1,"[夯实基础]删一张牌",-1,false,0,0,0,0,new List<int>(),0),
                new Choice(2,"[学习技巧]获得蓝色稀有度的一张牌",1,false,0,0,0,0,rare,0),
                new Choice(3,"[祖传秘技]花费50金币，获得一张金色稀有度的卡牌",1,false,0,-50,0,0,gold,0)
            }),
                new Event(2,"黑龙","一路上安静的可怕，原本总能见到的小动物全都销声匿迹了，似乎发生了什么不得了的事。\r\n你正怀疑着发生了什么事，突然一阵风压袭来，你险些被吹飞，只能将手臂护在头前。\r\n当风压散去，立在你面前的是一只黑色的威严巨龙，他也看着你。。","3",
            new List<Choice>{
                new Choice(1,"[赶紧逃走]随机失去一张牌",-1,true,0,0,0,0,new List<int>(),0),
                new Choice(2,"[咬牙坚持]:失去12生命值",0,false,0,0,-12,0,new List<int>{},0),
                new Choice(3,"[不惧挑战]:扣15血量上限，获得黑龙素材。",0,false,0,0,0,-15,new List<int>{},7)
            }),
                new Event(3,"老猎人的遗愿","在返回的路上，你看到了升起的救难信号，便前去一探。可当你到达现场后，已经没有了怪物的踪迹，只有一个奄奄一息的老猎人躺在地上。你把他带回了营地医治，但是他显然命不久矣。临终之际，他将“火龙”的鳞片递给你，问你是否能帮他完成最后的委托。","3",
            new List<Choice>{
                new Choice(1,"[接受]触发关于火龙的委托",0,false,4,0,0,0,new List<int>(),0),
                new Choice(2,"[拒绝]:获得30金币",0,false,0,30,0,0,new List<int>{},0),
            }),
                new Event(4,"地盘争夺","蛮颚龙与大贼龙发生地盘争夺！只见蛮颚龙将大贼龙叼在嘴上到处乱甩，而大贼龙没有丝毫抵抗之力。不久后，蛮颚龙将伤痕累累的大贼龙放下了，大贼龙灰溜溜地离开此地。","3",
            new List<Choice>{
                new Choice(1,"[[挑战强者]获得一张卡牌，掉10血",1,false,4,0,-10,0,normal,0),
                new Choice(2,"[追逐弱者]获得大贼龙素材，掉5血",0,false,0,0,-5,0,new List<int>{},1),
            }),
                new Event(5,"来自陌生人的邀请","“嘿，那边的猎人！”远方几个友善的猎人正朝你招手，今晚他们的营地准备大摆宴席，邀请你加入。在宴会上，几个猎人又邀请你一起喝酒。","3",
            new List<Choice>{
                new Choice(1,"[小酌一杯]：回复20点血量",0,false,0,0,20,0,new List<int>(),0),
                new Choice(2,"[酩酊大醉]：删一张牌",-1,false,0,0,0,0,new List<int>{},1),
            }),
                new Event(6,"风言风语","“你知道吗，有人在森林发现了冰呪龙的踪迹！”“真的假的，那可是未知的古龙啊”早上村里人的闲聊吸引了你的注意。尽管你不太相信，但前去一探也可能会有所收获。","3",
            new List<Choice>{
                new Choice(1,"[前去一探]",7,false,0,0,0,0,new List<int>(),0),
                new Choice(2,"[不予理会]：无",0,false,0,0,0,0,new List<int>{},0),
            }),
                 new Event(7,"惊现共斗大神","“在路上，你听见与怪物打斗的声音，靠近一看，一个年轻的猎人正在讨伐眩鸟。你靠近时，他也注意到了你。\r\n“你，赶紧帮我打怪”他一边向你叫唤着，一边被怪物打得节节败退。你决定：","3",
            new List<Choice>{
                new Choice(1,"[与其共斗]：触发眩鸟战斗",0,false,2,0,0,0,new List<int>(),0),
                new Choice(2,"[不予理会]：无",0,false,0,0,0,0,new List<int>{},0),
            }),
                 new Event(8,"热情的铁匠","一个热情的龙人族铁匠来到了你的营地，如果你有足够的素材，他很乐意为你打造一件装备。","3",
            new List<Choice>{
                new Choice(1,"[让其打造]：免费打造一件装备（需要素材）",0,false,0,0,0,0,new List<int>(),0),//未完成
                new Choice(2,"[离开]：无",0,false,0,0,0,0,new List<int>{},0),
            }),
                new Event(9,"古代遗物","在探索时，你发现了一个奇怪的遗迹，遗迹内有一些神秘的机械材料散落在地，还有一扇神秘的大门。","3",
            new List<Choice>{
                new Choice(1,"[拾取材料]：获得特殊素材“古代遗物”",0,false,0,0,0,0,new List<int>(),8),
                new Choice(2,"[解读符文]：随机获得一张卡",1,true,0,0,0,0,normal,0),
            }),
                new Event(10,"蜂蜜采集","桃毛兽王的领地有很多蜂蜜，趁它不在，你可以试着采集一些。","3",
            new List<Choice>{
                new Choice(1,"[略微采集]：回复15血”",0,false,0,0,15,0,new List<int>(),0),
                new Choice(2,"[大肆采集]：回满血但扣除7点血量上限",0,false,0,0,0,-7,new List<int>(),0),
            }),
                new Event(11,"森狸人的捕猎技巧","","3",
            new List<Choice>{
                new Choice(1,"[学习]：选择并获得一张卡”",0,false,0,0,0,0,normal,0),
                new Choice(2,"[离开]：无",0,false,0,0,0,0,new List<int>(),0),
            }),
            new Event(12,"蘑菇蘑菇蘑菇","咕咕嘎嘎，咕咕嘎嘎","3",
            new List<Choice>{
                new Choice(1,"[吃掉蓝色的]：删一张牌”",-1,false,0,0,0,0,new List<int>(),0),
                new Choice(2,"[吃掉橙色的]：增加8点血量上限",0,false,0,0,0,8,new List<int>(),0),
            }),
            new Event(13,"雨夜与钢龙","","3",
            new List<Choice>{
                new Choice(1,"[一探究竟]：删一张牌,失去15点生命值”",-1,false,0,0,-5,0,new List<int>(),0),
                new Choice(2,"[快速离开]：失去5点生命值",0,false,0,0,-15,0,new List<int>(),0),
            }),
    };
    public static readonly Vector2Int EventAmountBounds = new Vector2Int(1, 1);//每层总事件数限制
    public const int EventPerTour = 2;
}