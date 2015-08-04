using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GALAXY : MonoBehaviour {

    public GameObject galaxyStarPrefab;

    public GameObject boxes;
    public List<GalaxyGeneretion.StarBox> boxList;

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

        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (var item in boxList)
            {
                Destroy(item.gameObject);
            }
            boxList.Clear();
            GenerateGalaxy();
        }

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

        Vector2 screenScale = new Vector2(Screen.width / 1920f, Screen.height / 1080f);

        Vector3 pos = boxes.transform.position;
        if (pos.x < -GalaxyGeneretion.StarBox.width * screenScale.x)
        {
            pos.x += GalaxyGeneretion.StarBox.width * screenScale.x;
            _galaxyoffsetX++;
            OffsetBoxes(-1, 0);
            //update = true;
            // new collum on the right
        }
        if (pos.x > GalaxyGeneretion.StarBox.width * screenScale.x)
        {
            pos.x -= GalaxyGeneretion.StarBox.width * screenScale.x;
            _galaxyoffsetX--;
            OffsetBoxes(1, 0);
            //update = true;
            // new collum on the left
        }
        if (pos.y < -GalaxyGeneretion.StarBox.height * screenScale.y)
        {
            pos.y += GalaxyGeneretion.StarBox.height * screenScale.y;
            _galaxyoffsetY++;
            OffsetBoxes(0, -1);
            GenerateNewLine(false);
            // new line bottom (first)
        }
        if (pos.y > GalaxyGeneretion.StarBox.height * screenScale.y)
        {
            pos.y -= GalaxyGeneretion.StarBox.height * screenScale.y;
            _galaxyoffsetY--;
            OffsetBoxes(0, 1);
            GenerateNewLine(true);
            // new line top (last)
        }

        boxes.transform.position = pos;
	}

    public void OffsetBoxes(int dirX_, int dirY_)
    {
        Vector2 screenScale = new Vector2(Screen.width / 1920f, Screen.height / 1080f);
        foreach (var item in boxes.GetComponentsInChildren<Transform>())
        {
            if (item.name != "Boxes")
            {
                Vector3 pos = item.transform.localPosition;
                pos.x += GalaxyGeneretion.StarBox.width * screenScale.x * dirX_;
                pos.y += GalaxyGeneretion.StarBox.height * screenScale.y * dirY_;
                item.transform.localPosition = pos;
            }
        }
    }

    private void GenerateNewLine(bool top_)
    {
        List<GalaxyGeneretion.StarBox> newLine = null;
        if (top_)
        {
            Global.DebugOnScreen("lägger till en ny rad under");

            // skapar nya raden (lägger skiten på rätt plats i hierarkin av sg sjävt)
            newLine = GalaxyGeneretion.GenerateLineBottom(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);

            // för en hel rad
            for (int i = 0; i < (int)GalaxyGeneretion._boxMaxX; i++)
            {
                // ta bort objektet i listan och hierarkin
                GameObject.Destroy(boxList[boxList.Count - (int)GalaxyGeneretion._boxMaxX + i].gameObject);
                boxList.Remove(boxList[boxList.Count - (int)GalaxyGeneretion._boxMaxX + i]);
            }
            boxList.InsertRange(0, newLine);
        }
        else
        {
            Global.DebugOnScreen("lägger till en ny rad ovan");
            newLine = GalaxyGeneretion.GenerateLineTop(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);
            for (int i = 0; i < (int)GalaxyGeneretion._boxMaxX; i++)
            {
                GameObject.Destroy(boxList[i].gameObject);
            }
            boxList.RemoveRange(0, (int)GalaxyGeneretion._boxMaxX);
            boxList.InsertRange(boxList.Count, newLine);
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

        boxList = GalaxyGeneretion.GenerateGalaxy(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);
    }

    void Awake()
    {
        GenerateGalaxy();
    }
}
