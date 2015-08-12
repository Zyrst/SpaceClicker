using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SpellSlotButton : Button
{
    // Update is called once per frame
    void Update()
    {

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        daThing();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        daThing();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }

    private void daThing()
    {

        if ((Player.Instance._isHoldingSpell || !Player.Instance._isAlive) || !MouseController.Instance.buttonDown)
        {
            goto sist;
        }
        Player.Instance._isHoldingSpell = true;
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
    sist: ;
    }

}
