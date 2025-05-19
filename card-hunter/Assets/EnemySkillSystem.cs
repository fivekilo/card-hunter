using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 添加LINQ命名空间以查找只读表的技能

public class EnemySkillSystem : MonoBehaviour
{
    public EnemyAIController aiController;
    private MapManager mapManager;
    private BattleManager battleManager;

    //技能相关信息
    public int currentSkillID;
    public int nextSkillID;
    public List<int> availableSkills =new List<int>();//该怪物有的技能组ID
    //（靠aicontroll在开始时/转形态时主动传入）

    void Awake()
    {
        aiController = GetComponent<EnemyAIController>();
        battleManager = GetComponentInParent<BattleManager>();
        mapManager = battleManager.mapmanager;
    }

    public void SelectNextSkill()
    {
        // 简单AI：随机选择下个技能
        nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
        //特殊判定集：不符合的重新选技能
        // 特殊判定1：大贼龙的4技能（吞食回血）
        while(nextSkillID==4 && (float)aiController._currentHealth*2.5>(float)aiController._maxHealth)
        {
            nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
        }

        //获取技能范围并展示
        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos =GetSkillRange(nextskillconfig,enemypos,enemydirection);
        mapManager.ChangeColorByPos(actualrangepos, Color.magenta);//记得改回来
    }

    //获取攻击范围对应的地图坐标
    public List<Vector2Int> GetSkillRange(GameConfig.EnemySkillConfig config, Vector2Int enemypos, int enemydirection)
    {
        List <Vector2Int> ActualRangePos= new List<Vector2Int>();
        List<Vector2Int> VectorRange = config.range;
        //1.计算方向导致的坐标变换偏移量
        //标准方向向量  0，1，2，3，4，5
        List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1), 
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };
        foreach (Vector2Int singlepos in VectorRange)
        {
            Vector2Int temp = new Vector2Int();
            //2.重要推导公式
            temp.x = singlepos.x * StdVector[0 + enemydirection].x + singlepos.y * StdVector[(5 + enemydirection) % 6].x;
            temp.y = singlepos.x * StdVector[0 + enemydirection].y + singlepos.y * StdVector[(5 + enemydirection) % 6].y;
            Vector2Int RangeRealPos = enemypos + temp;
            //3.防越界
            if(RangeRealPos.x < GameConfig.size && RangeRealPos.y < GameConfig.size && RangeRealPos.x >= 0 && RangeRealPos.y >= 0)
                ActualRangePos.Add(RangeRealPos);
        }
        return ActualRangePos;
    }

    //执行技能
    public IEnumerator ExecuteCurrentSkill()
    {
        currentSkillID = nextSkillID;//获取上回合选定的技能
        GameConfig.EnemySkillConfig config = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == currentSkillID);
        if (config == null) yield break;
        //改回攻击范围变的色
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos = GetSkillRange(config, enemypos, enemydirection);
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);

        // 执行移动
        //yield return HandleSkillMovement(config);
        //执行伤害，回血,叠加护甲
        yield return Heal(config);
        yield return ApplySkillDamage(config, actualrangepos);
        yield return AddArmor(config);
        // 应用Debuff
        //ApplyDebuffs(config);
    }

    private IEnumerator AddArmor(GameConfig.EnemySkillConfig config)
    {
        aiController.armor += config.armor;
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator Heal(GameConfig.EnemySkillConfig config)
    {
        aiController.ReduceHealth(config.HPchange);
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator ApplySkillDamage(GameConfig.EnemySkillConfig config, List<Vector2Int> actualrangepos)
    {
        List<PlayerInfo> players = battleManager.GetTargetsInRange(actualrangepos);
        foreach (PlayerInfo player in players)
        {
            for (int i = 1; i <= config.hittimes; i++)
            {
                battleManager.ApplyDamage(player, config.damage,aiController);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

}
