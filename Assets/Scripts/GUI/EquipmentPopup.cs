using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentPopup : Button {

    public static int _count = -1;
    public int _myCount;
	// Use this for initialization

	protected override void Start () {
        _count++;
        _myCount = _count;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void reset()
    {
        _count = -1;
    }

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        try
        {
            Global.Instance.player._inventoryArray[_myCount].GetComponent<Equipment>().Popup();
        }
        catch (System.NullReferenceException) { }
    }
}
