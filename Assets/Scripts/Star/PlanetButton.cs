using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetButton : Button {

    public Planet _planet = new Planet();
    public float _gegegeg = 9;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Starmap.Instance.SelectPlanet(_planet);
    }
}
