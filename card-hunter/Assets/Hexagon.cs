using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2Int pos { get; set; }
    List<Color>ContentColor= new List<Color> {Color.blue,Color.red,Color.green,Color.yellow,Color.cyan};
    public GameConfig.Content content { get; set; }
    public void ContentChange(GameConfig.Content content)
    {
        this.content = content;
        SpriteRenderer render= this.GetComponent<SpriteRenderer>();
        render.material.color = ContentColor[(int)content];
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
