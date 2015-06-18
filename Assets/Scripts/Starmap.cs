using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Starmap : MonoBehaviour {

    public Planet _selectedPlanet = null;
    public GameObject _planetInfoBox;
    public int _numberOfPlanets = 0;

    public Sprite[] _planetSprites;
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

    public void Generate(int min_, int max_, int seed_)
    {
        Clear();
        Random.seed = 0;
        Random.seed = seed_;

        _numberOfPlanets = Random.Range(3, 9);
        int level = (max_ - min_) / _numberOfPlanets + min_;
        int levelForPlanet = level;
        for (int i = 0; i < _numberOfPlanets; i++)
        {
            Planet _plan = new Planet();
            _plan._sprite = _planetSprites[Random.Range(0, _planetSprites.Length)];
            _plan._name = (Random.value.ToString());
            _plan._minLevel = levelForPlanet - level;
            _plan._maxLevel = levelForPlanet;
            levelForPlanet += level;

            PlanetButton planBut = GameObject.Instantiate(_planetButtonPrefab as GameObject).GetComponent<PlanetButton>();
            planBut.gameObject.GetComponent<RectTransform>().SetParent(_planets.GetComponent<RectTransform>());
            planBut.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            planBut.gameObject.GetComponent<Image>().sprite = _plan._sprite;
            planBut._planet = _plan;
            bool looping = false;
            do
            {
                looping = false;
                float x = Random.Range(0f, 1340f);
                Debug.Log(x);
                planBut.gameObject.GetComponent<RectTransform>().position = new Vector3(x, Random.Range(0f, 650f), 0f);
                Vector4 rect = new Vector4();
                rect.x = planBut.gameObject.GetComponent<RectTransform>().position.x - (planBut.gameObject.GetComponent<RectTransform>().sizeDelta.x/2);
                rect.y = planBut.gameObject.GetComponent<RectTransform>().position.y - (planBut.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2);
                rect.z = planBut.gameObject.GetComponent<RectTransform>().sizeDelta.x;
                rect.w = planBut.gameObject.GetComponent<RectTransform>().sizeDelta.y;
                Debug.Log(rect);
                looping = CheckOverlap(rect);

            } while (looping);
            Vector4 _rect = new Vector4();
            _rect.x = planBut.gameObject.GetComponent<RectTransform>().position.x - (planBut.gameObject.GetComponent<RectTransform>().sizeDelta.x / 2);
            _rect.y = planBut.gameObject.GetComponent<RectTransform>().position.y - (planBut.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2);
            _rect.z = planBut.gameObject.GetComponent<RectTransform>().sizeDelta.x;
            _rect.w = planBut.gameObject.GetComponent<RectTransform>().sizeDelta.y;
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
        Debug.Log(c1 + " " +  c2 + " " + c3 + " " + c4);
        return c1 && c2 && c3 && c4;
    }
}
