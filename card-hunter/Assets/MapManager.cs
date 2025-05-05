using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject Hex;
    private GameObject [,]Hexs;
    void spawn(int size)
    {
        Hexs=new GameObject[size,size];
        for(int x=0;x<size;x++){
            for(int y=0;y<size;y++){
                Vector3 pos=new Vector3(3*(x+y)/2.0f,math.sqrt(3)*(y-x)/2,0);
                GameObject GO=Instantiate(Hex,pos,Quaternion.identity);
                GO.transform.SetParent(transform);
                Hexs[x,y]=GO;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawn(7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
