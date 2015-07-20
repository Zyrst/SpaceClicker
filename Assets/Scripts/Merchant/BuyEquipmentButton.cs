using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyEquipmentButton : MonoBehaviour {
    public Image _equipmentImage;
    public Text _nameText;
    public Text _levelText;
    public Text _typeText;
    public Text _infoText;
    public Text _goldText;
    public Text _equipText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// this one is for when you buy from merchant
    /// </summary>
    public void Click()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);

        Debug.Log("Clicked on the button");
        if (Global.Instance._gold >= GetComponentInChildren<Equipment>()._cost)
        {

            Sounds.OneShot(Sounds.Instance.uiSounds.merchant.buy);
            Global.Instance._gold -= GetComponentInChildren<Equipment>()._cost;
            Global.Instance.UpdateGoldText();
            bool set = false;
            for (int i = 0; i < Global.Instance.player._inventoryArray.Length; i++)
            {
                if (Global.Instance.player._inventoryArray[i] == null)
                {
                    Global.Instance.player._inventoryArray[i] = GetComponentInChildren<Equipment>().gameObject;
                    GetComponentInChildren<Equipment>().gameObject.transform.parent = Global.Instance.player._inventoryObject.transform;

                    GetComponent<Button>().interactable = false;
                    
                    set = true;
                    break;
                }
            }

            if (!set)
            {
                Global.DebugOnScreen("INVENTORY IS FULL!!!!");
            }
        }
        else
        {
            Global.DebugOnScreen("ITEM WAS TOO EXPENSIVE");
        }
    }

    /// <summary>
    /// this one is for when you are on preview tab
    /// </summary>
    public void ClickPreview()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Debug.Log("Clicked on the button");
    }
}
