using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SpellSlotButton : Button
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        int index = 0;
        switch (transform.parent.name)
        {
            case "Spellslot 0":
                index = 0;
                break;
            case "Spellslot 1":
                index = 1;
                break;
            case "Spellslot 2":
                index = 2;
                break;
            case "Spellslot 3":
                index = 3;
                break;
            default:
                break;
        }

        Global.Instance._player._spellsArray[index].Clicked();
    }
}
