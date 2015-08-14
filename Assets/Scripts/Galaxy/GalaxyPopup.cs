using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GalaxyPopup : MonoBehaviour {

    public Text planetInfo;
    public Text levelRange;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Visit()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.enterGalaxy);
        GALAXY.Instance.Generate();

    }
}
