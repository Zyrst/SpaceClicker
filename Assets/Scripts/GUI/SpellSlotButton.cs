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
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
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

        try
        {
            Global.Instance.player._spellsArray[index].Clicked();
        }
        catch (System.NullReferenceException) { }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }
}
