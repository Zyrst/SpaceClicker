using UnityEngine;
using System.Collections;

public class GALAXY : MonoBehaviour {

    public float _float = 54f;

    public GameObject boxes;

    private bool _mouseDown = false;
    private Vector3 _mouseOld = Vector3.zero;

    private uint _galaxyoffsetX = 100;
    private uint _galaxyoffsetY = 100;

    private static GALAXY _instance = null;
    public static GALAXY Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GALAXY").GetComponent<GALAXY>();
            }
            return _instance;
        }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (MouseController.Instance.buttonDown)
        {
            if (_mouseDown)
            {
                Vector3 delta = _mouseOld - MouseController.Instance.position;
                boxes.transform.position -= delta;
            }
            
            _mouseDown = true;

        }
        else if (_mouseDown)
        {
            _mouseDown = false;
        }

        _mouseOld = MouseController.Instance.position;

        bool update = false;

        Vector2 screenScale = new Vector2(Screen.width / 1920f, Screen.height / 1080f);

        Vector3 pos = boxes.transform.position;
        if (pos.x < -GalaxyGeneretion.StarBox.width * screenScale.x)
        {
            pos.x += GalaxyGeneretion.StarBox.width * screenScale.x;
            _galaxyoffsetX++;
            update = true;
        }
        if (pos.x > GalaxyGeneretion.StarBox.width * screenScale.x)
        {
            pos.x -= GalaxyGeneretion.StarBox.width * screenScale.x;
            _galaxyoffsetX--;
            update = true;
        }
        if (pos.y < -GalaxyGeneretion.StarBox.height * screenScale.y)
        {
            pos.y += GalaxyGeneretion.StarBox.height * screenScale.y;
            _galaxyoffsetY++;
            update = true;
        }
        if (pos.y > GalaxyGeneretion.StarBox.height * screenScale.y)
        {
            pos.y -= GalaxyGeneretion.StarBox.height * screenScale.y;
            _galaxyoffsetY--;
            update = true;
        }

        boxes.transform.position = pos;

        if (update)
        {
            foreach (var item in boxes.GetComponentsInChildren<Transform>())
            {
                if (item.name != "Boxes")
                    GameObject.Destroy(item.gameObject);
            }
            GenerateGalaxy();
        }
	}

    public void Generate()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Starmap.Instance.Generate(1, 100, 9001);
        Global.DebugOnScreen("genererar i galaxy");
    }

    public void GenerateGalaxy()
    {
        int seed = (int)Mathf.Pow((float)_galaxyoffsetX, (float)_galaxyoffsetY);
        Random.seed = seed;

        GalaxyGeneretion.GenerateGalaxy(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);
    }

    void Awake()
    {
        GenerateGalaxy();
    }
}
