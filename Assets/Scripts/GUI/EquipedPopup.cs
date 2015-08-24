using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipedPopup : Button {

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        switch (this.targetGraphic.name)
        {
            case "Head":
                Global.Instance.player._equipped._head.EqupipedPop();
                break;
            case "Chest":
                Global.Instance.player._equipped._chest.EqupipedPop();
                break;
            case "Pants":
                Global.Instance.player._equipped._legs.EqupipedPop();
                break;
            case "Weapon":
                Global.Instance.player._equipped._weapon.EqupipedPop();
                break;
        }
    }

}
