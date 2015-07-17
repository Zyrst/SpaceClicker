using UnityEngine;
using System.Collections;
using System.Linq;

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
        FixPlayerStats();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Farm()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        if (Global.Instance._planet == null)
        {
            Global.DebugOnScreen("Please select a planet");
        }
        else
            Global.Instance.SwitchScene(Global.GameType.Farm);

        Global.Instance.player.UpdateCombinedStats();
    }

    public void Character()
    {
        CharacterScreen.Instance.gameObject.SetActive(true);
        //Global.Instance._player.gameObject.SetActive(true);
        Global.Instance.player.SortInventory();
        Global.Instance.player.UpdateCombinedStats();
        CharacterScreen.Instance.Activate();
        gameObject.SetActive(false);

        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.openInventory);
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        
    }

    public void Star()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.eterStarmap);
        Global.Instance.SwitchScene(Global.GameType.Star);
    }

    public void ExitCharacter()
    {
        CharacterScreen.Instance.gameObject.SetActive(false);
        CharacterScreen.Instance.RemoveInventorySlots();
        CharacterScreen.Instance.ResetModel();
        Global.Instance.player.gameObject.SetActive(false);
        gameObject.SetActive(true);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.closeInventory);
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
    }

    public void ExitGame()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Application.Quit();
    }

    public void FixPlayerStats()
    {
        Global.Instance.player.gameObject.SetActive(true);
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = false;

        Invoke("DisablePlayer", 0.1f);
    }

    public void DisablePlayer()
    {
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = true;
        Global.Instance.player.gameObject.SetActive(false);
    }
}
