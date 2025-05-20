using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // ���LINQ�����ռ��Բ���ֻ����ļ���

public class EnemySkillSystem : MonoBehaviour
{
    public EnemyAIController aiController;
    private MapManager mapManager;
    private BattleManager battleManager;

    //���������Ϣ
    public int currentSkillID;
    public int nextSkillID;
    public List<int> availableSkills =new List<int>();//�ù����еļ�����ID
    //����aicontroll�ڿ�ʼʱ/ת��̬ʱ�������룩

    //����������
    public int skillselected = 0;//�Ƿ��Ѿ���ѡ��
    public int skill4selected = 0;//ĳ�������Ƿ��Ѿ���ѡ��
    public int turncount = 0;//�غϼ�����������д��
    public bool oldplayerinrange = false;

    void Awake()
    {
        aiController = GetComponent<EnemyAIController>();
        battleManager = GetComponentInParent<BattleManager>();
        mapManager = battleManager.mapmanager;
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
        while (aiController.name=="������" && (float)aiController._currentHealth * 2.5 < (float)aiController._maxHealth && skill4selected == 0)
        {
            nextSkillID = 4;
            skillselected = 1;
            skill4selected = 1;
            aiController.selfSkills.Add(5);
            aiController.selfSkills.Add(6);
            //�����ﲻ��remove����Ϊ����ֻ��ѡ�˻�û�ų����ء�ֻ�ܼӲ���ɾ
            Debug.Log("�����������ˡ���ʳ�����ܣ�");
        }
        // ��AI�����ѡ���¸�����
        if(skillselected ==0)
        {
            do
            {
                nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
                skillselected = 1;
            } while (nextSkillID == 4 || nextSkillID == 0 );
            // ���У���������4����,0�������߲���������ѡ
        }


        //��ȡ���ܷ�Χ��չʾ
        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos =GetSkillRange(nextskillconfig,enemypos,enemydirection);
        mapManager.ChangeColorByPos(actualrangepos, Color.magenta);//�ǵøĻ���

        //�������Ƿ��ڷ�Χ��
        oldplayerinrange = battleManager.PlayerInRange(actualrangepos);
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
        foreach (Vector2Int singlepos in VectorRange)
        {
            Vector2Int temp = new Vector2Int();
            //2.��Ҫ�Ƶ���ʽ
            temp.x = singlepos.x * StdVector[0 + enemydirection].x + singlepos.y * StdVector[(5 + enemydirection) % 6].x;
            temp.y = singlepos.x * StdVector[0 + enemydirection].y + singlepos.y * StdVector[(5 + enemydirection) % 6].y;
            Vector2Int RangeRealPos = enemypos + temp;
            //3.��Խ��
            if(RangeRealPos.x < GameConfig.size && RangeRealPos.y < GameConfig.size && RangeRealPos.x >= 0 && RangeRealPos.y >= 0)
                ActualRangePos.Add(RangeRealPos);
        }
        return ActualRangePos;
    }


    //�Ƿ���У��������ӵ�У�
    public void ChangeSkillinRealtime(Vector2Int playergridpos_now)
    {
        if (aiController.havechangedskill == true) return;//һ�غ�ֻ�ܱ�һ��
        if (oldplayerinrange == false) return;//���ԭ���Ͳ��ڷ�Χ������б���

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
                Debug.Log($"��Χ���е�����{pos}���������{playergridpos_now}��ͬ��������");
                break; 
            }
        }
        if (inrange == true) return;

        //������ֲ��ڷ�Χ���ˣ����б���(����ѡ���ڷ�Χ�ڵ���ʽ)
        //����֮ǰ�ķ�Χ��ɫ
        Debug.Log("���������ˣ�");
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);
        int someskillID;
        bool OK = false;
        for (int i = 1; i <= 3; i++)//������ѡ3��
        {
            do
            {
                someskillID = availableSkills[Random.Range(0, availableSkills.Count)];
            } while (someskillID == 4 || someskillID == 0);
            //��ȡ���ܷ�Χ��չʾ
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

    //ִ�м���
    public IEnumerator ExecuteCurrentSkill(int cut)
    {
        currentSkillID = nextSkillID;//��ȡ�ϻغ�ѡ���ļ���
        GameConfig.EnemySkillConfig config = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == currentSkillID);
        if (config == null) yield break;
        //�Ļع�����Χ���ɫ
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos = GetSkillRange(config, enemypos, enemydirection);
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        mapManager.ChangeColorByPos(actualrangepos, color);

        //��cut=1ʱ�����ʣ�ಿ��
        if (cut ==1) yield break;

        // ִ���ƶ�
        if (config.moveType!=GameConfig.MoveType.None)
            yield return HandleSkillMovement(config);
        //ִ���˺���Ӧ��debuff����Ѫ,���ӻ���
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
