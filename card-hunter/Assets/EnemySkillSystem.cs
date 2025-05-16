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

    void Awake()
    {
        aiController = GetComponent<EnemyAIController>();
        battleManager = GetComponentInParent<BattleManager>();
        mapManager = battleManager.mapmanager;
    }

    public void SelectNextSkill()
    {
        // ��AI�����ѡ���¸�����
        nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
        //�����ж����������ϵ�����ѡ����
        // �����ж�1����������4���ܣ���ʳ��Ѫ��
        while(nextSkillID==4 && (float)aiController._currentHealth*2.5>(float)aiController._maxHealth)
        {
            nextSkillID = availableSkills[Random.Range(0, availableSkills.Count)];
        }

        //��ȡ���ܷ�Χ��չʾ
        GameConfig.EnemySkillConfig nextskillconfig = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == nextSkillID);
        Vector2Int enemypos = aiController._currentGridPos;
        int enemydirection = aiController.direction;
        List<Vector2Int> actualrangepos =GetSkillRange(nextskillconfig,enemypos,enemydirection);

    }

    public List<Vector2Int> GetSkillRange(GameConfig.EnemySkillConfig config, Vector2Int enemypos, int enemydirection)
    {
        List <Vector2Int> ActualRangePos= new List<Vector2Int>();
        List<Vector2Int> VectorRange = config.range;
        //���㷽���µ�����任ƫ����
        //��׼��������  0��1��2��3��4��5
        List<Vector2Int> StdVector = new List<Vector2Int> {new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(-1, 1), 
            new Vector2Int(-1, 0), new Vector2Int(0, -1),new Vector2Int(1,-1) };
        foreach (Vector2Int singlepos in VectorRange)
        {
            Vector2Int temp = new Vector2Int();
            //��Ҫ�Ƶ���ʽ
            temp.x = singlepos.x * StdVector[0 + enemydirection].x + singlepos.y * StdVector[(5 + enemydirection) % 6].x;
            temp.y = singlepos.x * StdVector[0 + enemydirection].y + singlepos.y * StdVector[(5 + enemydirection) % 6].y;
            Vector2Int RangeRealPos = enemypos + temp;
            ActualRangePos.Add(RangeRealPos);
        }
        return ActualRangePos;
    }

    //ִ�м���
    public IEnumerator ExecuteCurrentSkill()
    {
        currentSkillID = nextSkillID;//��ȡ�ϻغ�ѡ���ļ���
        GameConfig.EnemySkillConfig config = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == currentSkillID);
        if (config == null) yield break;

        // ִ���ƶ�
        //yield return HandleSkillMovement(config);
        // ִ���˺�
        //yield return ApplySkillDamage(config);
        // Ӧ��Debuff
        //ApplyDebuffs(config);
    }
}
