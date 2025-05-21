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
    // 在 Inspector 中配置预制体引用
    [System.Serializable]
    public class PrefabMapping
    {
        public MonsterPrefabType type;
        public GameObject prefab;
    }
    [SerializeField] private PrefabMapping[] prefabMappings; // 预制体映射配置

    //加载怪物预制体
    public void InitializePrefab(
        MonsterPrefabType type,
        Vector3 position,
        Quaternion rotation,
        Transform parent )
    {
        // 查找对应类型的预制体
        foreach (var mapping in prefabMappings)
        {
            if (mapping.type == type)
            {
                // 实例化预制体
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
            case ("大贼龙"):
                monster=MonsterPrefabType.Dazeilong; break;
            case ("眩鸟"):
                monster = MonsterPrefabType.Xuanniao; break;
            case ("蛮颚龙"):
                monster = MonsterPrefabType.Manelong; break;
            case ("岩贼龙"):
                monster = MonsterPrefabType.Yanzeilong; break;
            case ("雷狼龙"):
                monster = MonsterPrefabType.Leilanglong; break;
            case ("冰咒龙"):
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
