using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class EventData : MonoBehaviour
{
    public Image EventImage;
    public TextMeshProUGUI discription;
    public TextMeshProUGUI Name;
    public float buttonSpace = 20f;
    public GameObject button;
    public bool Clicked=false;

    // Start is called before the first frame update
    public IEnumerator EventInit(Event myevent, Action<Choice> Chosed)
    {
        Name.text = myevent.Name;
        discription.text = myevent.text;
        Sprite newSprite;
        newSprite = Resources.Load<Sprite>(myevent.Image);
        EventImage.sprite = newSprite;
        float totalHeigh = (myevent.choices.Count - 1) * buttonSpace;
        float startX = -totalHeigh / 2f;

        for (int i = 0; i < myevent.choices.Count; i++)
        {
           GameObject choice = Instantiate(button,transform);
           float yPos = startX + (myevent.choices.Count-i) * buttonSpace;
           choice.transform.position = new Vector3(0, yPos, 0) + transform.position;
            choiceInit(myevent.choices[i], choice,Chosed);
        }
        yield return new WaitUntil(() => Clicked);
        Clicked = false;
    }
    public void choiceInit(Choice choicedata,GameObject choice,Action<Choice>Chosed)
    {
        choice.GetComponentInChildren<TextMeshProUGUI>().text = choicedata.text;
        choice.GetComponentInChildren<Button>().onClick.AddListener(() => {
            Clicked = true;
            Chosed?.Invoke(choicedata);
        });
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
