using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2Int pos;
    private Stack<Color> history = new Stack<Color>();
    public GameConfig.Content content { get; set; }
    private void OnMouseDown()
    {
        GameObject parent = transform.parent.gameObject;
        MapManager manager=parent.GetComponent<MapManager>();
        manager.ClickedPos = pos;
    }
    public void AddImage(string image)//���ͼ��
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(image);
    }
    public void ChangeColor(Color color)//�ı���ɫ
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = color;
        history.Push(color);
    }
    public void RollbackColor()
    {
        if (history.Count > 1)
        {
            history.Pop();
            GetComponent<SpriteRenderer>().color = history.Peek();
        }
        else
        {
            throw new System.Exception("��ɫջ��Ԫ�ز���");
        }
    }
    public void ContentChange(GameConfig.Content content)
    {
        this.content = content;
        AddImage(content.ToString());
        this.tag = "Content";
    }
    public void ContentRemove()
    {
        AddImage("background");
        ColorUtility.TryParseHtmlString(GameConfig.BackgroundColor, out Color color);
        ChangeColor(color);
        this.tag = "Untagged";
    }
    public void ObstacleAdd()
    {
        this.tag = "Obstacle";
        ChangeColor(Color.grey);
    }
    public void ObstacleAddIcicle()
    {
        this.tag = "Obstacle";
        ChangeColor(new Color(0f, 0.5f, 1f, 0.5f));//ǳ��ɫ
    }
    public void ObstacleRemove()
    {
        this.tag = "Untagged";
        ChangeColor(Color.white);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
