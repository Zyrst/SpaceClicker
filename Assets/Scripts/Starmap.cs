using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Starmap : MonoBehaviour {

    public Planet _selectedPlanet = null;
    public GameObject _planetInfoBox;
    public int _numberOfPlanets = 0;
    public uint _minLevel = 0;
    public uint _maxLevel = 0;

    public Sprite[] _planetSprites;
    public Sprite[] _backgrounds;
    public List<Vector4> _planetBounds = new List<Vector4>();

    public GameObject _planets;

    public GameObject _planetButtonPrefab;

    private static Starmap _instance = null;
    public static Starmap Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("STARMAP").GetComponent<Starmap>();
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

    public void Clear()
    {
        foreach (var item in _planets.GetComponentsInChildren<Transform>(true))
        {
            if (item.gameObject.name != "Planets")
                Destroy(item.gameObject);
        }
        _planetBounds.Clear();
        _planetInfoBox.gameObject.SetActive(false);
    }

    public void BackToShip()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.exitStarmap);
        Global.Instance.SwitchScene(Global.GameType.Ship);
        Music.Instance._menuTheme.ExitStarmap();
    }

    public void BackToGalaxy()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.exitStarmap);
        Global.Instance.SwitchScene(Global.GameType.Galaxy);
        GALAXY.Instance.DoAnew();
    }

    public void SelectPlanet(Planet planet_)
    {
        _selectedPlanet = planet_;
        _planetInfoBox.gameObject.SetActive(true);
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "MinLevel").text = "Min " + planet_._minLevel.ToString();
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "MaxLevel").text = "Max " + planet_._maxLevel.ToString();
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Name").text = planet_._name;
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Theme").text = "Theme: " + System.Enum.GetName(typeof(Planet.PlanetType), planet_._type);

        Global.Instance._planet = _selectedPlanet;
        switch (_selectedPlanet._type)
	    {
            case Planet.PlanetType.PostApc :
                Global.Instance._enemies._currentEnemies = Global.Instance._enemies._postApc;
                break;
            case Planet.PlanetType.Mech :
                Global.Instance._enemies._currentEnemies = Global.Instance._enemies._mech;
                break;
		    default:
                break;
	    }
    }

    public void Generate(uint min_, uint max_, int seed_)
    {
        Clear();
        Random.seed = 0;
        Random.seed = seed_;

        _numberOfPlanets = GALAXY.Instance._lastStar._numberOfPlanets;

        if (!Global.Instance.IsInvoking("SecondLevelFiller"))
            Global.Instance.StartLevelFill(min_, max_);
        _minLevel = min_;
        _maxLevel = max_;

        uint llevel = min_;
        uint ulevel = llevel + Global.Instance._galaxy._increasePerPlanet-1;

        gameObject.SetActive(true);
        gameObject.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "BackgroundPanel").sprite = _backgrounds[(int)GALAXY.Instance._lastStar._starBackground];
        gameObject.SetActive(false);
        float planetPosX = 1400 / _numberOfPlanets;
        for (int i = 0; i < _numberOfPlanets; i++)
        {
            Planet _plan = new Planet();

            int planetTypes = System.Enum.GetNames(typeof(Planet.PlanetType)).Length;
            _plan._type = (Planet.PlanetType)Random.Range(0, planetTypes);

            _plan._sprite = _planetSprites[Random.Range(0, _planetSprites.Length)];

            _plan._name = (Random.value.ToString());

            _plan._minLevel = llevel;
            _plan._maxLevel = ulevel;
            //levelForPlanet += level; // <-----------------
            llevel += Global.Instance._galaxy._increasePerPlanet;
            ulevel = llevel + Global.Instance._galaxy._increasePerPlanet - 1;

            GameObject plent = GameObject.Instantiate(_planetButtonPrefab as GameObject);
            plent.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "BackGlow").GetComponent<Image>().color =
                Global.Instance._colors.levelColors[Global.DetermineLevelColor(_plan._minLevel, Player.Instance._level)];
            PlanetButton planBut = plent.GetComponentInChildren<PlanetButton>();
            RectTransform planRect = planBut.gameObject.GetComponent<RectTransform>();
            plent.transform.parent = (_planets.GetComponent<RectTransform>());
            plent.transform.localScale = new Vector3(1f, 1f, 1f);
            planBut.gameObject.GetComponent<Image>().sprite = _plan._sprite;
            planBut._planet = _plan;
            bool looping = false;
            do
            {
                looping = false;
                float x = Random.Range(planetPosX * i, planetPosX * (i + 1));
                plent.transform.localPosition = new Vector3(x, Random.Range(0f, 750f), 0f);
                Vector4 rect = new Vector4();
                rect.x = planRect.position.x - (planBut.gameObject.GetComponent<RectTransform>().sizeDelta.x / 2);
                rect.y = planRect.position.y - (planRect.sizeDelta.y / 2);
                rect.z = planRect.sizeDelta.x;
                rect.w = planRect.sizeDelta.y;
                looping = CheckOverlap(rect);

            } while (looping);
            Vector4 _rect = new Vector4();
            _rect.x = planRect.position.x - (planRect.sizeDelta.x / 2);
            _rect.y = planRect.position.y - (planRect.sizeDelta.y / 2);
            _rect.z = planRect.sizeDelta.x;
            _rect.w = planRect.sizeDelta.y;
            _planetBounds.Add(_rect);
        }
    }

    public bool CheckOverlap(Vector4 rect_)
    {
        bool outcome = false;
        for (int i = 0; i < _planetBounds.Count; i++)
        {
            if (Intersect(_planetBounds[i], rect_))
            {
                outcome = true;
                break;
            }
        }
        return outcome;
    }

    public bool Intersect(Vector4 a, Vector4 b)
    {
        bool c1 = a.x < (b.x + b.z);
        bool c2 = (a.x + a.z) > b.x;
        bool c3 = a.y < (b.y + b.w);
        bool c4 = (a.y + a.w) > b.y;
        return c1 && c2 && c3 && c4;
    }
}
