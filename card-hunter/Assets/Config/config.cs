using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public const double ObstacleRate = 0.15;//障碍物生成概率
    public const int ObstacleSup = 10;//障碍物生成上限
    public const int size = 7;
    public const int InitialHealth = 80; //血量
    public const int InitialCost = 3; //初始费用
    public const int InitialHandCardNum = 4; //初始手牌数
    public const int MaxHandCardNum = 8;
    public const int MaxBladeLevel = 2; //最大气刃等级
    public const int MaxBladeNum = 50;//最大气刃值

    public readonly List<Card> Deck = new()
    {
        new Card("呃呃?","?","攻击?",0,new Vector2Int(0 , 0) , 1 , 5 , 0 , 0 , 10 , 0 , 0)
    };
}