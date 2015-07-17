using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetButton : Button {

    public Planet _planet = new Planet();
    public float _gegegeg = 9;
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.selectWorld);

        Starmap.Instance.SelectPlanet(_planet);
    }
}
