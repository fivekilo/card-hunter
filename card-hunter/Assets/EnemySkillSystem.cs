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

    //特判相关组件
    public int skillselected = 0;//是否已经被选过
    public int skill4selected = 0;//某个技能是否已经被选过
    public int turncount = 0;//回合计数器（待编写）
    public bool oldplayerinrange = false;

    void Awake()
    {
        aiController = GetComponent<EnemyAIController>();
        battleManager = GetComponentInParent<BattleManager>();
        mapManager = battleManager.mapmanager;
    }

    public void SelectNextSkill(int certainskill)
    {
        skillselected = 0;//先置0再选技能
        if(certainskill!=-1)//可以直接传入固定选用技能
        {
            skillselected = 1;
            nextSkillID = certainskill;
        }
        // 选择前特判1：大贼龙的4技能（吞食回血转阶段）
        while (aiController.name=="大贼龙" && (float)aiController._currentHealth * 2.5 < (float)aiController._maxHealth && skill4selected == 0)
        {
            nextSkillID = 4;
            skillselected = 1;
            skill4selected = 1;
            aiController.selfSkills.Add(5);
            aiController.selfSkills.Add(6);
            //在这里不能remove，因为技能只是选了还没放出来呢。只能加不能删
            Debug.Log("大贼龙触发了“进食”技能！");
        }
        // 简单AI：随机选择下个技能
        if(skillselected ==0)
        {
            do
            {
                nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
                skillselected = 1;
            } while (nextSkillID == 4 || nextSkillID == 0 );
            // 特判：大贼龙的4技能,0技能力竭不能在这里选
        }


        //获取技能范围并展示
        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos =GetSkillRange(nextskillconfig,enemypos,enemydirection);
        mapManager.ChangeColorByPos(actualrangepos, Color.magenta);//记得改回来

        //检测玩家是否在范围内
        oldplayerinrange = battleManager.PlayerInRange(actualrangepos);
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


    //是否变招（仅特殊怪拥有）
    public void ChangeSkillinRealtime(Vector2Int playergridpos_now)
    {
        if (aiController.havechangedskill == true) return;//一回合只能变一次
        if (oldplayerinrange == false) return;//如果原来就不在范围里，不进行变招

        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos = GetSkillRange(nextskillconfig, enemypos, enemydirection);
        bool inrange = false;
        foreach (Vector2Int pos in actualrangepos)
        {
            if (pos == playergridpos_now) 
            { 
                inrange = true;
                Debug.Log($"范围的中的坐标{pos}和玩家坐标{playergridpos_now}相同，不变招");
                break; 
            }
        }
        if (inrange == true) return;

        //如果发现不在范围里了，进行变招(尽量选用在范围内的招式)
        //回退之前的范围颜色
        Debug.Log("触发变招了！");
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);
        int someskillID;
        bool OK = false;
        for (int i = 1; i <= 3; i++)//最多随机选3次
        {
            do
            {
                someskillID = availableSkills[Random.Range(0, availableSkills.Count)];
            } while (someskillID == 4 || someskillID == 0);
            //获取技能范围并展示
            GameConfig.EnemySkillConfig someskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == someskillID);
            Vector2Int newenemypos = aiController._currentGridPos;
            int newenemydirection = aiController.direction;
            List<Vector2Int> newactualrangepos = GetSkillRange(someskillconfig, newenemypos, newenemydirection);
            foreach (Vector2Int pos in newactualrangepos)
            {
                if (pos == playergridpos_now) { OK = true; break; }
            }
            if (OK == true)
            {
                SelectNextSkill(someskillID);
                aiController.havechangedskill = true;
                break;
            }
        }
        if (OK == false)
            SelectNextSkill(-1);
    }

    //执行技能
    public IEnumerator ExecuteCurrentSkill(int cut)
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

        //当cut=1时，打断剩余部分
        if (cut ==1) yield break;

        // 执行移动
        if (config.moveType!=GameConfig.MoveType.None)
            yield return HandleSkillMovement(config);
        //执行伤害并应用debuff，回血,叠加护甲
        yield return Heal(config);
        yield return ApplySkill(config, actualrangepos);
        yield return AddArmor(config);
    }

    private IEnumerator HandleSkillMovement(GameConfig.EnemySkillConfig config)
    {
        List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1),
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };
        for(int i=1;i<=config.moveDistance;i++)
        {
            Vector2Int newpos = aiController._currentGridPos + StdVector[aiController.direction];
            //如果前面有墙或者人，直接停止
            if (!checkPosValid(newpos) || mapManager.IsPositionOccupied(newpos))  break;
            aiController._currentGridPos += StdVector[aiController.direction];
            aiController.UpdatePosition(aiController._currentGridPos);
            yield return new WaitForSeconds(0.3f);
        }
    }
    private bool checkPosValid(Vector2Int Pos)
    {
        if (Pos.x < 0 || Pos.x >= GameConfig.size || Pos.y < 0 || Pos.y >= GameConfig.size) return false;
        return true;
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

    private IEnumerator ApplySkill(GameConfig.EnemySkillConfig config, List<Vector2Int> actualrangepos)
    {
        List<PlayerInfo> players = battleManager.GetTargetsInRange(actualrangepos);
        foreach (PlayerInfo player in players)
        {
            if(player!=null)
            {
                if (config.pushdebuff != GameConfig.EnemyDebuff.None)
                    battleManager.ApplyDebuff(player, config.pushdebuff, aiController);
                for (int i = 1; i <= config.hittimes; i++)
                {
                    battleManager.ApplyDamage(player, config.damage, aiController);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }

}
