using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HP : MonoBehaviour
{
    public SharedData sharedData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "HP:" + sharedData.playerinfo.MaxHealth.ToString()+" /"+sharedData.playerinfo.curHealth;
    }
}
