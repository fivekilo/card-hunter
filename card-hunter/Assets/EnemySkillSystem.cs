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
    private List<int> availableSkills =new List<int>();//�ù����еļ�����ID
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
    }

    //ִ�м���
    public IEnumerator ExecuteCurrentSkill()
    {
        currentSkillID = nextSkillID;//��ȡ�ϻغ�ѡ���ļ���
        var config = GameConfig.EnemySkills.FirstOrDefault(s => s.skillID == currentSkillID);
        if (config == null) yield break;

        // ִ���ƶ�
        //yield return HandleSkillMovement(config);
        // ִ���˺�
        //yield return ApplySkillDamage(config);
        // Ӧ��Debuff
        //ApplyDebuffs(config);
    }
}
