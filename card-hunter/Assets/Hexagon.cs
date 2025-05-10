using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2Int pos { get; set; }
    public GameConfig.Content content { get; set; }
    public void AddImage(string image)//添加图像
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(image);
    }
    public void ChangeColor(Color color)//改变颜色
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = color;
    }
    public void ContentChange(GameConfig.Content content)
    {
        this.content = content;
        AddImage(content.ToString());
        this.tag = "Content";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
