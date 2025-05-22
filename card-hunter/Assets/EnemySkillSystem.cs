using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using TMPro; // 添加LINQ命名空间以查找只读表的技能

public class EnemySkillSystem : MonoBehaviour
{
    public EnemyAIController aiController;
    private MapManager mapManager;
    private BattleManager battleManager;
    public TextMeshProUGUI monstersituasion;

    //技能相关信息
    public int currentSkillID;
    public int nextSkillID;
    public List<int> availableSkills =new List<int>();//该怪物有的技能组ID
    public List<Vector2Int> nextskillpos = new List<Vector2Int>();
    //（靠aicontroll在开始时/转形态时主动传入）

    //特判相关组件
    public int skillselected = 0;//是否已经被选过
    public int skill4selected = 0;//某个技能是否已经被选过
    public int turncount = 0;//回合计数器（待编写）
    public bool oldplayerinrange = false;
    public List<Vector2Int> iciclerange = new List<Vector2Int>();//冰咒龙冰柱位置
    public List<Vector2Int> icicleblowuprange = new List<Vector2Int>();//冰咒龙秉烛爆炸范围

    void Awake()
    {
        aiController = GetComponent<EnemyAIController>();
        battleManager = GetComponentInParent<BattleManager>();
        mapManager = battleManager.mapmanager;
        monstersituasion = FindObjectOfTypeWithName<TextMeshProUGUI>("Monster indicator");
    }
    // 精确查找（带缓存优化）
    public static T FindObjectOfTypeWithName<T>(string name, bool includeInactive = false) where T : Component
    {
        // 优先尝试快速查找
        T[] candidates = Object.FindObjectsOfType<T>(includeInactive);
        return candidates.FirstOrDefault(obj => obj.name == name);
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
        if (aiController.ID==1 && (float)aiController._currentHealth * 2.5 < (float)aiController._maxHealth && skill4selected == 0)
        {
            nextSkillID = 4;
            skillselected = 1;
            skill4selected = 1;
            aiController.selfSkills.Add(5);
            aiController.selfSkills.Add(6);
            //在这里不能remove，因为技能只是选了还没放出来呢。只能加不能删
            monstersituasion.text = $"大贼龙触发了“进食”技能！" ;
        }
        // 选择前特判2：岩贼龙的两次19技能转阶段
        if (aiController.ID == 5 && (aiController.TurnCount==5-1 || aiController.TurnCount==10-1))
        {
            nextSkillID = 19;
            skillselected = 1;
            if(aiController.TurnCount == 5-1)
            {
                aiController.selfSkills.Add(20);
                aiController.selfSkills.Add(21);
            }
            monstersituasion.text =$"岩贼龙触发了“吞食岩石”技能！" ;
        }
        // 选择前特判3：冰咒龙的33技能
        if (aiController.ID == 7 && aiController.FrozenTurnCount ==5-1)
        {
            nextSkillID = 33;
            skillselected = 1;
            aiController.enemystate = 1;//极寒之冰形态
            aiController.selfSkills.Add(34);
            //aiController.selfSkills.Add(35);
            //aiController.selfSkills.Add(36);
            monstersituasion.text = $"冰咒龙触发了“冰之铠甲”技能！";
        }
        // 选择前特判4：冰咒龙的34技能在进冰之后立刻释放
        if (aiController.ID == 7 && currentSkillID==33)
        {
            nextSkillID = 34;
            skillselected = 1;
        }
        // 选择前特判5：冰咒龙的36技能碎冰在35技能之后立刻释放
        if (aiController.ID == 7 && currentSkillID == 35)
        {
            nextSkillID = 36;
            skillselected = 1;
        }

        if (skillselected ==0)
        {
            do
            {
                nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
                skillselected = 1;
            } while (nextSkillID == 4 || nextSkillID == 0 || nextSkillID == 19 || nextSkillID == 33 || nextSkillID == 36);
            // 特判：大贼龙的4技能,0技能力竭，岩贼龙的19技能,冰咒龙的33，36技能不能在这里选
        }


        //获取技能范围并展示
        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos =GetSkillRange(nextskillconfig,enemypos,enemydirection);
        mapManager.ChangeColorByPos(actualrangepos, Color.magenta);//记得改回来
        //特判：冰咒龙的碎冰技能展示
        if(aiController.ID==7&&nextSkillID==36)
        {
            foreach(Vector2Int pos in iciclerange)
            {

                Vector2Int sidepos = pos + new Vector2Int(1, 0);
                if (battleManager.CheckPosIsValid(sidepos) && battleManager.mapmanager.isObstacle(sidepos)&&(icicleblowuprange.Find(a=>a==sidepos)!=null)) icicleblowuprange.Add(sidepos);
                sidepos = pos + new Vector2Int(0, 1);
                if (battleManager.CheckPosIsValid(sidepos) && battleManager.mapmanager.isObstacle(sidepos) && (icicleblowuprange.Find(a => a == sidepos) != null)) icicleblowuprange.Add(sidepos);
                sidepos = pos + new Vector2Int(-1, 0);
                if (battleManager.CheckPosIsValid(sidepos) && battleManager.mapmanager.isObstacle(sidepos) && (icicleblowuprange.Find(a => a == sidepos) != null)) icicleblowuprange.Add(sidepos);
                sidepos = pos + new Vector2Int(0, -1);
                if (battleManager.CheckPosIsValid(sidepos) && battleManager.mapmanager.isObstacle(sidepos) && (icicleblowuprange.Find(a => a == sidepos) != null)) icicleblowuprange.Add(sidepos);
                sidepos = pos + new Vector2Int(1, 1);
                if (battleManager.CheckPosIsValid(sidepos) && battleManager.mapmanager.isObstacle(sidepos) && (icicleblowuprange.Find(a => a == sidepos) != null)) icicleblowuprange.Add(sidepos);
                sidepos = pos + new Vector2Int(-1, -1);
                if (battleManager.CheckPosIsValid(sidepos) && battleManager.mapmanager.isObstacle(sidepos) && (icicleblowuprange.Find(a => a == sidepos) != null)) icicleblowuprange.Add(sidepos);
            }
            mapManager.ChangeColorByPos(icicleblowuprange, Color.magenta);//记得改回来
        }
        monstersituasion.text = $"{aiController.name}使用了“{nextskillconfig.skillName}”技能,将要造成{nextskillconfig.damage}点伤害！";
        //检测玩家是否在范围内
        oldplayerinrange = battleManager.PlayerInRange(actualrangepos);
        nextskillpos = actualrangepos;
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

        //锁定招式直接加真实坐标
        if (config.skilltype == GameConfig.SkillType.Locked)
        {
            foreach (Vector2Int singlepos in VectorRange)
            {
                Vector2Int playerpos = battleManager.Player.PlayerGridPos;
                Vector2Int RangeRealPos = playerpos + singlepos;
                //防越界
                if (RangeRealPos.x < GameConfig.size && RangeRealPos.y < GameConfig.size && RangeRealPos.x >= 0 && RangeRealPos.y >= 0)
                    ActualRangePos.Add(RangeRealPos);
            }
        }
        else
        {
            //非锁定招式要推导相对向量再加上方向
            foreach (Vector2Int singlepos in VectorRange)
            {
                Vector2Int temp = new Vector2Int();
                //2.重要推导公式
                temp.x = singlepos.x * StdVector[0 + enemydirection].x + singlepos.y * StdVector[(5 + enemydirection) % 6].x;
                temp.y = singlepos.x * StdVector[0 + enemydirection].y + singlepos.y * StdVector[(5 + enemydirection) % 6].y;
                Vector2Int RangeRealPos = enemypos + temp;
                //3.防越界
                if (RangeRealPos.x < GameConfig.size && RangeRealPos.y < GameConfig.size && RangeRealPos.x >= 0 && RangeRealPos.y >= 0)
                    ActualRangePos.Add(RangeRealPos);
            }
        }
        return ActualRangePos;
    }


    //是否变招（仅特殊怪拥有）
    public void ChangeSkillinRealtime(Vector2Int playergridpos_now)
    {
        if (aiController.havechangedskill == true) return;//一回合只能变一次
        if (oldplayerinrange == false) return;//如果原来就不在范围里，不进行变招
        //特判：岩贼龙只能在1，3，6技能中进行变招
        if (aiController.ID == 5 && nextSkillID != 1 && nextSkillID != 3 && nextSkillID != 6) return;
        //特判：冰咒龙只能在29，30，31技能中进行变招
        if (aiController.ID == 7 && nextSkillID != 29 && nextSkillID != 30 && nextSkillID != 31) return;

        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos = nextskillpos;//用上一次技能已经确定的位置看
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
        //1.回退之前的范围颜色
        Debug.Log("触发变招了！");
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);

        //2.先进行转向(待实现)

        //3.再选新技能
        int someskillID;
        bool OK = false;
        bool specialchoose = true;
        for (int i = 1; i <= 3; i++)//最多随机选3次
        {
            do
            {
                //特判：岩贼龙只能变成6技能
                if (aiController.ID == 5)
                    someskillID = 6;
                else if (aiController.ID == 7 && nextSkillID == 31)//特判：冰咒龙的尾刺只能重新锁定
                    someskillID = 31;
                else if (aiController.ID == 7 && nextSkillID == 29)//特判：冰咒龙的两下喷冰只会相互变招
                    someskillID = 30;
                else if (aiController.ID == 7 && nextSkillID == 30)
                    someskillID = 29;
                else
                {
                    someskillID = availableSkills[Random.Range(0, availableSkills.Count)];
                    specialchoose = false;
                }
            } while (someskillID == 4 || someskillID == 0 || someskillID == 19 || someskillID == 33 || someskillID == 36);
            //获取技能范围并展示
            GameConfig.EnemySkillConfig someskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == someskillID);
            Vector2Int newenemypos = aiController._currentGridPos;
            int newenemydirection = aiController.direction;
            List<Vector2Int> newactualrangepos = GetSkillRange(someskillconfig, newenemypos, newenemydirection);
            foreach (Vector2Int pos in newactualrangepos)
            {
                if (pos == playergridpos_now||specialchoose==true) { OK = true; break; }//特判的变招自动通过
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
        List<Vector2Int> actualrangepos = nextskillpos;
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);

        //当cut=1时，打断剩余部分
        if (cut ==1) yield break;

        //执行地形生成
        if (config.addenvironment != GameConfig.AddEnvironment.None)
            AddContent(config, actualrangepos);
        // 执行移动
        if (config.moveType!=GameConfig.MoveType.None)
            yield return HandleSkillMovement(config);
        //执行伤害并应用debuff，回血,叠加护甲
        if(config.HPchange!=0)      yield return Heal(config);
        yield return ApplySkill(config, actualrangepos);
        if (config.armor != 0)   yield return AddArmor(config);
    }

    //添加地图要素
    private void AddContent(GameConfig.EnemySkillConfig config, List<Vector2Int> actualrangepos)
    {
        GameConfig.Content newcontent = new GameConfig.Content();
        if (config.addenvironment == GameConfig.AddEnvironment.Lava) newcontent = GameConfig.Content.Lava;
        if (config.addenvironment == GameConfig.AddEnvironment.ElectricBall) newcontent = GameConfig.Content.ElectricBall;
        //冰柱现已改为换色障碍物
        if (config.addenvironment == GameConfig.AddEnvironment.Icicle)
        {
            foreach (Vector2Int pos in actualrangepos)
            {
                if (!checkPosValid(pos)) continue;
                mapManager.AddMonsterObstacles(pos);
                iciclerange.Add(pos);
            }
        }
        else
        {
            foreach (Vector2Int pos in actualrangepos)
            {
                if (!checkPosValid(pos)) continue;
                mapManager.AddMonsterContent(pos, newcontent);
            }
        }

        //特判：36技能寒冬嘶吼碎冰
        if (currentSkillID == 36)
        {
            foreach (Vector2Int pos in iciclerange)
            {
                mapManager.RemoveMonsterObstacles(pos);
            }
            iciclerange = new List<Vector2Int>();
        }
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
        if (aiController.ID == 5 && aiController.TurnCount == 10)//第二次使用吞食岩石额外获得20甲
            yield return aiController.ChangeArmor(config.armor + 20);
        else
            yield return aiController.ChangeArmor(config.armor);
    }

    private IEnumerator Heal(GameConfig.EnemySkillConfig config)
    {
        aiController.ReduceHealth(config.HPchange);
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator ApplySkill(GameConfig.EnemySkillConfig config, List<Vector2Int> actualrangepos)
    {
        //特判：36技能用选定的爆炸范围
        if(currentSkillID==36)actualrangepos = icicleblowuprange;

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
