using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMonster : MonoBehaviour
{
    public SharedData shareddata;
    public string choosetype;
    public MonsterPrefabType monster;
    public enum MonsterPrefabType
    {
        Dazeilong,
        Xuanniao,
        Manelong,
        Yanzeilong,
        Leilanglong,
        Bingzhoulong
    }
    // �� Inspector ������Ԥ��������
    [System.Serializable]
    public class PrefabMapping
    {
        public MonsterPrefabType type;
        public GameObject prefab;
    }
    [SerializeField] private PrefabMapping[] prefabMappings; // Ԥ����ӳ������

    //���ع���Ԥ����
    public void InitializePrefab(
        MonsterPrefabType type,
        Vector3 position,
        Quaternion rotation,
        Transform parent )
    {
        // ���Ҷ�Ӧ���͵�Ԥ����
        foreach (var mapping in prefabMappings)
        {
            if (mapping.type == type)
            {
                // ʵ����Ԥ����
                GameObject instance = Instantiate(
                    mapping.prefab,
                    position,
                    rotation,
                    parent);

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        choosetype = shareddata.commission.monster;
        switch (choosetype)
        {
            case ("������"):
                monster=MonsterPrefabType.Dazeilong; break;
            case ("ѣ��"):
                monster = MonsterPrefabType.Xuanniao; break;
            case ("�����"):
                monster = MonsterPrefabType.Manelong; break;
            case ("������"):
                monster = MonsterPrefabType.Yanzeilong; break;
            case ("������"):
                monster = MonsterPrefabType.Leilanglong; break;
            case ("������"):
                monster = MonsterPrefabType.Bingzhoulong; break;

        }

        InitializePrefab(monster,
        Vector3.zero,
        Quaternion.identity,
        transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
