using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PlanetButton : Button {

    public Planet _planet = new Planet();
    public float _gegegeg = 9;

    public float _glowSpeed = 0.5f;

    private Image _glow = null;

    private float _noise = 0f;

	// Update is called once per frame
	void Update () {

        if (_glow == null)
        {
            _glow = transform.parent.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "BackGlow");
        }

        _glow.materialForRendering.SetFloat("_Noise2", _noise);

        _noise += _glowSpeed * Time.deltaTime;
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.selectWorld);

        Starmap.Instance.SelectPlanet(_planet);
    }
}
