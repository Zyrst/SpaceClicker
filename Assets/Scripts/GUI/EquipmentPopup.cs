using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentPopup : Button {

    public static int _count = -1;
    public int _myCount;
    public static bool _poped;
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
        _poped = false;
    }

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!_poped)
        {
            try
            {
                _poped = true;
                Global.Instance.player._inventoryArray[_myCount].GetComponent<Equipment>().Popup();

            }
            catch (System.NullReferenceException) { }
        }
    }

    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        _poped = false;
        CharacterScreen.Instance._lastFrameClick = false;
    }
}
