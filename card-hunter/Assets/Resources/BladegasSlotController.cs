using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladegasSlotController : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider BladegasSlot;
    public Image Blade;
    public Sprite newSprite;
    public void ShowBladeLevel(int n)
    {
        switch (n)
        {
            case 0:
                newSprite= Resources.Load<Sprite>("None");
                break;
            case 1:
                newSprite = Resources.Load<Sprite>("white");
                break;
            case 2:
                newSprite = Resources.Load<Sprite>("yellow");
                break;
            case 3:
                newSprite = Resources.Load<Sprite>("Red");
                break;
        }
        Blade.sprite = newSprite;
    }
    public void ShowBladeGas(int n)
    {
        BladegasSlot.value = n;
    }
    void Start()
    {

        ShowBladeGas(0);
        ShowBladeLevel(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
