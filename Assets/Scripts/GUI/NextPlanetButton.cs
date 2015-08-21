using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextPlanetButton : Button {

    public Planet _nextPlanet = null;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        FarmMode.Instance.backToShip();
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.eterStarmap);
        Global.Instance.SwitchScene(Global.GameType.Star);
        Starmap.Instance.SelectPlanet(_nextPlanet);
        Ship.Instance.Farm();
        FarmMode.Instance._nextPlanetButton.SetActive(false);
    }
}
