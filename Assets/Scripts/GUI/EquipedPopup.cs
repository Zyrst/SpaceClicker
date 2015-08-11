using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipedPopup : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


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
