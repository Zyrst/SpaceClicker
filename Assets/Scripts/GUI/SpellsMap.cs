using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpellsMap : MonoBehaviour {

    public GameObject spellSlots;
    public GameObject spellList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open()
    {
        gameObject.SetActive(true);

        MenuSpellSlotButton[] arr = GetComponentsInChildren<MenuSpellSlotButton>() as MenuSpellSlotButton[];
        Global.DebugOnScreen("längden: " + Global.Instance._player._spellsArray.Length);
        for (int i = 0; i < Global.Instance._player._spellsArray.Length; i++)
        {
            if (Global.Instance._player._spellsArray[i] == null)
            {
                break;
            }
            arr[i].GetComponent<Image>().sprite = Global.Instance._player._spellsArray[i]._spellImage;
            Global.DebugOnScreen("lägger till");
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
