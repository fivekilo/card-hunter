using System.Collections.Generic;
using UnityEngine;

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
    public const int MaxBladeLevel = 2; //最大气刃等级
    public const int MaxBladeNum = 50;//最大气刃值
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
        "格挡"
    }.AsReadOnly();
    public static IReadOnlyList<string> CardImageName=new List<string> 
    {"",//卡图名称
        "Direct slash" ,
        "cardbottom"}.AsReadOnly();
    public static IReadOnlyList<string> CardText = new List<string>
    { "", //卡牌描述
        "造成6点伤害" , 
        "向前移动一格，造成4点伤害，回复1格气刃槽" 
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
    public static IReadOnlyList<List<int>> Attack = new List<List<int>>
    {null,//造成伤害
        new List<int>{6},
        new List<int>{4},
        new List<int>{7},
        new List<int>{4,4},
        new List<int>{10},
        null,
        null

    }.AsReadOnly();

}