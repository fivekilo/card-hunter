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
    private List<int> availableSkills =new List<int>();//该怪物有的技能组ID
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
    }

    //执行技能
    public IEnumerator ExecuteCurrentSkill()
    {
        currentSkillID = nextSkillID;//获取上回合选定的技能
        var config = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == currentSkillID);
        if (config == null) yield break;

        // 执行移动
        //yield return HandleSkillMovement(config);
        // 执行伤害
        //yield return ApplySkillDamage(config);
        // 应用Debuff
        //ApplyDebuffs(config);
    }
}
