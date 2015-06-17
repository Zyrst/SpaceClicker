using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
   
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
