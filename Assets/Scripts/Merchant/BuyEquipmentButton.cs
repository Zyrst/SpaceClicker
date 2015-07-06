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
    }
}
