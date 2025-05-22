using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using TMPro; // ���LINQ�����ռ��Բ���ֻ����ļ���

public class EnemySkillSystem : MonoBehaviour
{
    public EnemyAIController aiController;
    private MapManager mapManager;
    private BattleManager battleManager;
    public TextMeshProUGUI monstersituasion;

    //���������Ϣ
    public int currentSkillID;
    public int nextSkillID;
    public List<int> availableSkills =new List<int>();//�ù����еļ�����ID
    public List<Vector2Int> nextskillpos = new List<Vector2Int>();
    //����aicontroll�ڿ�ʼʱ/ת��̬ʱ�������룩

    //����������
    public int skillselected = 0;//�Ƿ��Ѿ���ѡ��
    public int skill4selected = 0;//ĳ�������Ƿ��Ѿ���ѡ��
    public int turncount = 0;//�غϼ�����������д��
    public bool oldplayerinrange = false;
    public List<Vector2Int> iciclerange = new List<Vector2Int>();//����������λ��
    public List<Vector2Int> icicleblowuprange = new List<Vector2Int>();//����������ը��Χ

    void Awake()
    {
        aiController = GetComponent<EnemyAIController>();
        battleManager = GetComponentInParent<BattleManager>();
        mapManager = battleManager.mapmanager;
        monstersituasion = FindObjectOfTypeWithName<TextMeshProUGUI>("Monster indicator");
    }
    // ��ȷ���ң��������Ż���
    public static T FindObjectOfTypeWithName<T>(string name, bool includeInactive = false) where T : Component
    {
        // ���ȳ��Կ��ٲ���
        T[] candidates = Object.FindObjectsOfType<T>(includeInactive);
        return candidates.FirstOrDefault(obj => obj.name == name);
    }

    public void SelectNextSkill(int certainskill)
    {
        skillselected = 0;//����0��ѡ����
        if(certainskill!=-1)//����ֱ�Ӵ���̶�ѡ�ü���
        {
            skillselected = 1;
            nextSkillID = certainskill;
        }

        // ѡ��ǰ����1����������4���ܣ���ʳ��Ѫת�׶Σ�
        if (aiController.ID==1 && (float)aiController._currentHealth * 2.5 < (float)aiController._maxHealth && skill4selected == 0)
        {
            nextSkillID = 4;
            skillselected = 1;
            skill4selected = 1;
            aiController.selfSkills.Add(5);
            aiController.selfSkills.Add(6);
            //�����ﲻ��remove����Ϊ����ֻ��ѡ�˻�û�ų����ء�ֻ�ܼӲ���ɾ
            monstersituasion.text = $"�����������ˡ���ʳ�����ܣ�" ;
        }
        // ѡ��ǰ����2��������������19����ת�׶�
        if (aiController.ID == 5 && (aiController.TurnCount==5-1 || aiController.TurnCount==10-1))
        {
            nextSkillID = 19;
            skillselected = 1;
            if(aiController.TurnCount == 5-1)
            {
                aiController.selfSkills.Add(20);
                aiController.selfSkills.Add(21);
            }
            monstersituasion.text =$"�����������ˡ���ʳ��ʯ�����ܣ�" ;
        }
        // ѡ��ǰ����3����������33����
        if (aiController.ID == 7 && aiController.FrozenTurnCount ==5-1)
        {
            nextSkillID = 33;
            skillselected = 1;
            aiController.enemystate = 1;//����֮����̬
            aiController.selfSkills.Add(34);
            //aiController.selfSkills.Add(35);
            //aiController.selfSkills.Add(36);
            monstersituasion.text = $"�����������ˡ���֮���ס����ܣ�";
        }
        // ѡ��ǰ����4����������34�����ڽ���֮�������ͷ�
        if (aiController.ID == 7 && currentSkillID==33)
        {
            nextSkillID = 34;
            skillselected = 1;
        }
        // ѡ��ǰ����5����������36���������35����֮�������ͷ�
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
            // ���У���������4����,0�������ߣ���������19����,��������33��36���ܲ���������ѡ
        }


        //��ȡ���ܷ�Χ��չʾ
        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos =GetSkillRange(nextskillconfig,enemypos,enemydirection);
        mapManager.ChangeColorByPos(actualrangepos, Color.magenta);//�ǵøĻ���
        //���У����������������չʾ
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
            mapManager.ChangeColorByPos(icicleblowuprange, Color.magenta);//�ǵøĻ���
        }
        monstersituasion.text = $"{aiController.name}ʹ���ˡ�{nextskillconfig.skillName}������,��Ҫ���{nextskillconfig.damage}���˺���";
        //�������Ƿ��ڷ�Χ��
        oldplayerinrange = battleManager.PlayerInRange(actualrangepos);
        nextskillpos = actualrangepos;
    }

    //��ȡ������Χ��Ӧ�ĵ�ͼ����
    public List<Vector2Int> GetSkillRange(GameConfig.EnemySkillConfig config, Vector2Int enemypos, int enemydirection)
    {
        List <Vector2Int> ActualRangePos= new List<Vector2Int>();
        List<Vector2Int> VectorRange = config.range;
        //1.���㷽���µ�����任ƫ����
        //��׼��������  0��1��2��3��4��5
        List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1), 
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };

        //������ʽֱ�Ӽ���ʵ����
        if (config.skilltype == GameConfig.SkillType.Locked)
        {
            foreach (Vector2Int singlepos in VectorRange)
            {
                Vector2Int playerpos = battleManager.Player.PlayerGridPos;
                Vector2Int RangeRealPos = playerpos + singlepos;
                //��Խ��
                if (RangeRealPos.x < GameConfig.size && RangeRealPos.y < GameConfig.size && RangeRealPos.x >= 0 && RangeRealPos.y >= 0)
                    ActualRangePos.Add(RangeRealPos);
            }
        }
        else
        {
            //��������ʽҪ�Ƶ���������ټ��Ϸ���
            foreach (Vector2Int singlepos in VectorRange)
            {
                Vector2Int temp = new Vector2Int();
                //2.��Ҫ�Ƶ���ʽ
                temp.x = singlepos.x * StdVector[0 + enemydirection].x + singlepos.y * StdVector[(5 + enemydirection) % 6].x;
                temp.y = singlepos.x * StdVector[0 + enemydirection].y + singlepos.y * StdVector[(5 + enemydirection) % 6].y;
                Vector2Int RangeRealPos = enemypos + temp;
                //3.��Խ��
                if (RangeRealPos.x < GameConfig.size && RangeRealPos.y < GameConfig.size && RangeRealPos.x >= 0 && RangeRealPos.y >= 0)
                    ActualRangePos.Add(RangeRealPos);
            }
        }
        return ActualRangePos;
    }


    //�Ƿ���У��������ӵ�У�
    public void ChangeSkillinRealtime(Vector2Int playergridpos_now)
    {
        if (aiController.havechangedskill == true) return;//һ�غ�ֻ�ܱ�һ��
        if (oldplayerinrange == false) return;//���ԭ���Ͳ��ڷ�Χ������б���
        //���У�������ֻ����1��3��6�����н��б���
        if (aiController.ID == 5 && nextSkillID != 1 && nextSkillID != 3 && nextSkillID != 6) return;
        //���У�������ֻ����29��30��31�����н��б���
        if (aiController.ID == 7 && nextSkillID != 29 && nextSkillID != 30 && nextSkillID != 31) return;

        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos = nextskillpos;//����һ�μ����Ѿ�ȷ����λ�ÿ�
        bool inrange = false;
        foreach (Vector2Int pos in actualrangepos)
        {
            if (pos == playergridpos_now) 
            { 
                inrange = true;
                Debug.Log($"��Χ���е�����{pos}���������{playergridpos_now}��ͬ��������");
                break; 
            }
        }
        if (inrange == true) return;

        //������ֲ��ڷ�Χ���ˣ����б���(����ѡ���ڷ�Χ�ڵ���ʽ)
        //1.����֮ǰ�ķ�Χ��ɫ
        Debug.Log("���������ˣ�");
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);

        //2.�Ƚ���ת��(��ʵ��)

        //3.��ѡ�¼���
        int someskillID;
        bool OK = false;
        bool specialchoose = true;
        for (int i = 1; i <= 3; i++)//������ѡ3��
        {
            do
            {
                //���У�������ֻ�ܱ��6����
                if (aiController.ID == 5)
                    someskillID = 6;
                else if (aiController.ID == 7 && nextSkillID == 31)//���У���������β��ֻ����������
                    someskillID = 31;
                else if (aiController.ID == 7 && nextSkillID == 29)//���У����������������ֻ���໥����
                    someskillID = 30;
                else if (aiController.ID == 7 && nextSkillID == 30)
                    someskillID = 29;
                else
                {
                    someskillID = availableSkills[Random.Range(0, availableSkills.Count)];
                    specialchoose = false;
                }
            } while (someskillID == 4 || someskillID == 0 || someskillID == 19 || someskillID == 33 || someskillID == 36);
            //��ȡ���ܷ�Χ��չʾ
            GameConfig.EnemySkillConfig someskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == someskillID);
            Vector2Int newenemypos = aiController._currentGridPos;
            int newenemydirection = aiController.direction;
            List<Vector2Int> newactualrangepos = GetSkillRange(someskillconfig, newenemypos, newenemydirection);
            foreach (Vector2Int pos in newactualrangepos)
            {
                if (pos == playergridpos_now||specialchoose==true) { OK = true; break; }//���еı����Զ�ͨ��
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

    //ִ�м���
    public IEnumerator ExecuteCurrentSkill(int cut)
    {
        currentSkillID = nextSkillID;//��ȡ�ϻغ�ѡ���ļ���
        GameConfig.EnemySkillConfig config = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == currentSkillID);
        if (config == null) yield break;
        //�Ļع�����Χ���ɫ
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos = nextskillpos;
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);

        //��cut=1ʱ�����ʣ�ಿ��
        if (cut ==1) yield break;

        //ִ�е�������
        if (config.addenvironment != GameConfig.AddEnvironment.None)
            AddContent(config, actualrangepos);
        // ִ���ƶ�
        if (config.moveType!=GameConfig.MoveType.None)
            yield return HandleSkillMovement(config);
        //ִ���˺���Ӧ��debuff����Ѫ,���ӻ���
        if(config.HPchange!=0)      yield return Heal(config);
        yield return ApplySkill(config, actualrangepos);
        if (config.armor != 0)   yield return AddArmor(config);
    }

    //��ӵ�ͼҪ��
    private void AddContent(GameConfig.EnemySkillConfig config, List<Vector2Int> actualrangepos)
    {
        GameConfig.Content newcontent = new GameConfig.Content();
        if (config.addenvironment == GameConfig.AddEnvironment.Lava) newcontent = GameConfig.Content.Lava;
        if (config.addenvironment == GameConfig.AddEnvironment.ElectricBall) newcontent = GameConfig.Content.ElectricBall;
        //�������Ѹ�Ϊ��ɫ�ϰ���
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

        //���У�36���ܺ���˻�����
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
            //���ǰ����ǽ�����ˣ�ֱ��ֹͣ
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
        if (aiController.ID == 5 && aiController.TurnCount == 10)//�ڶ���ʹ����ʳ��ʯ������20��
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
        //���У�36������ѡ���ı�ը��Χ
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
