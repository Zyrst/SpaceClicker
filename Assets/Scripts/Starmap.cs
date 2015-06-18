using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Starmap : MonoBehaviour {

    public Planet _selectedPlanet = null;
    public GameObject _planetInfoBox;
    public int _numberOfPlanets = 0;

    public Sprite[] _planetSprites;

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
        Random.seed = 1339;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BackToShip()
    {
        Global.Instance.SwitchScene(Global.GameType.Ship);
    }

    public void SelectPlanet(Planet planet_)
    {
        _selectedPlanet = planet_;
        _planetInfoBox.gameObject.SetActive(true);
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "MinLevel").text = "Min " + planet_._minLevel.ToString();
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "MaxLevel").text = "Max " + planet_._maxLevel.ToString();
        _planetInfoBox.gameObject.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Name").text = planet_._name;

        Global.Instance._planet = _selectedPlanet;
    }

    public void Generate(int min_, int max_)
    {
        _numberOfPlanets = Random.Range(1, 5);
        int level = (max_ - min_) / _numberOfPlanets + min_;
        int levelForPlanet = level;
        for (int i = 0; i < _numberOfPlanets; i++)
        {
            Planet _plan = new Planet();
            _plan._sprite = _planetSprites[Random.Range(0, _planetSprites.Length)];
            _plan._name = (Random.value.ToString() + Random.value.ToString() + Random.value.ToString() + Random.value.ToString() + Random.value.ToString());
            _plan._minLevel = levelForPlanet - level;
            _plan._maxLevel = levelForPlanet;
            levelForPlanet += level;

            PlanetButton planBut = GameObject.Instantiate(_planetButtonPrefab as GameObject).GetComponent<PlanetButton>();
            planBut.gameObject.GetComponent<RectTransform>().SetParent(_planets.GetComponent<RectTransform>());
            planBut.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            float x = Random.Range(0f, 1920f);
            Debug.Log(x);
            planBut.gameObject.GetComponent<RectTransform>().position = new Vector3(x, Random.Range(0f, 1080f), 0f);
            planBut.gameObject.GetComponent<Image>().sprite = _plan._sprite;
            planBut._planet = _plan;
        }
    }
}
