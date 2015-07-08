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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        Debug.Log("Clicked on the button");
        bool set = false;
        for (int i = 0; i < Global.Instance._player._inventoryArray.Length; i++)
        {
            if (Global.Instance._player._inventoryArray[i] == null)
            {
                Global.Instance._player._inventoryArray[i] = GetComponentInChildren<Equipment>().gameObject;
                GetComponentInChildren<Equipment>().gameObject.transform.parent = Global.Instance._player._inventoryObject.transform;

                set = true;
                break;
            }
        }

        if (!set)
        {
            Debug.Log("<color=red>Inentory is full</color>");
        }

        GetComponent<Button>().interactable = false;
    }
}
