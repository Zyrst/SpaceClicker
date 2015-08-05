using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarSystemButton : Button {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        GALAXY.Instance._lastStar = gameObject.GetComponent<StarSystem>();

        GameObject popup = GALAXY.Instance.popup.gameObject;
        popup.SetActive(true);

        GameObject selector = GALAXY.Instance.selector.gameObject;
        selector.gameObject.SetActive(true);
        selector.transform.position = gameObject.transform.position;

        GetComponent<StarSystem>().GenerateNumberOfPlanets();

        GALAXY.Instance.popup.GetComponent<GalaxyPopup>().planetInfo.text = GetComponent<StarSystem>()._numberOfPlanets.ToString();
        GALAXY.Instance.popup.GetComponent<GalaxyPopup>().levelRange.text = GetComponent<StarSystem>()._llevel + " - " + GetComponent<StarSystem>()._ulevel;
    }
}
