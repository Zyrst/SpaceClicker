using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
    public GameObject _EquipPopup;

    private static Ship _instance = null;
    public static Ship Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("SHIP").GetComponent<Ship>();
            }
            return _instance;
        }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PopItUp(Equipment equi_)
    {
        _EquipPopup.gameObject.SetActive(true);
        _EquipPopup.transform.position = equi_.transform.position;
    }

    public void Farm()
    {
        Global.Instance.SwitchScene(Global.GameType.Farm);
    }

    public void Character()
    {
        CharacterScreen.Instance.gameObject.SetActive(true);
        CharacterScreen.Instance.GenerateInventorySlots();
    }

    public void ExitCharacter()
    {
        CharacterScreen.Instance.gameObject.SetActive(false);
        CharacterScreen.Instance.RemoveInventorySlots();
    }
}
