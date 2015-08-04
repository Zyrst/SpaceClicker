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
        //screenScale = new Vector2(1f, 1f);

        Vector3 pos = boxes.transform.position;
        if (pos.x < -GalaxyGeneretion.StarBox.width * screenScale.x)
        {
            pos.x += GalaxyGeneretion.StarBox.width *screenScale.x;
            _galaxyoffsetX++;
            OffsetBoxes(-1,0);
            GenerateNewColumn(true);
            // new collum on the right
        }
        if (pos.x > GalaxyGeneretion.StarBox.width * screenScale.x)
        {
            pos.x -= GalaxyGeneretion.StarBox.width * screenScale.x;
            _galaxyoffsetX--;
            OffsetBoxes(1,0);
            GenerateNewColumn(false);
            // new collum on the left
        }
        if (pos.y < -GalaxyGeneretion.StarBox.height * screenScale.y)
        {
            pos.y += GalaxyGeneretion.StarBox.height * screenScale.y;
            _galaxyoffsetY++;
            OffsetBoxes(0,-1);
            GenerateNewLine(false);
            // new line bottom (first)
        }
        if (pos.y > GalaxyGeneretion.StarBox.height * screenScale.y)
        {
            pos.y -= GalaxyGeneretion.StarBox.height * screenScale.y;
            _galaxyoffsetY--;
            OffsetBoxes(0,1);
            GenerateNewLine(true);
            // new line top (last)
        }

        boxes.transform.position = pos;
	}

    public void OffsetBoxes(int dirX_, int dirY_)
    {
        Vector2 screenScale = new Vector2(Screen.width / 1920f, Screen.height / 1080f);
        //screenScale = new Vector2(1f,1f);
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
            newLine = GalaxyGeneretion.GenerateLineTop(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);
            for (int i = 0; i < (int)GalaxyGeneretion._boxMaxX; i++)
            {
                GameObject.Destroy(boxList[i].gameObject);
            }
            boxList.RemoveRange(0, (int)GalaxyGeneretion._boxMaxX);
            boxList.InsertRange(boxList.Count, newLine);
        }
    }

    private void GenerateNewColumn(bool right_)
    {
        List<GalaxyGeneretion.StarBox> newColumn = null;
        if (right_)
        {
            // skapar nya kolonnen
            newColumn = GalaxyGeneretion.GenerateColumnRight(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);

            // för en hel kolonn
            for (int i = 0; i < (int)GalaxyGeneretion._boxMaxY; i++)
            {
                // skapa index
                int delIn = (int)((GalaxyGeneretion._boxMaxX) * i);
                int addIn = (int)((GalaxyGeneretion._boxMaxX - 1) + ((GalaxyGeneretion._boxMaxX) * i));

                // ta bort gammal
                GameObject.Destroy(boxList[delIn].gameObject);
                boxList.Remove(boxList[delIn]);

                // lägg in ny
                boxList.Insert(addIn, newColumn[i]);
            }
        }
        else
        {
            // skapar nya kolonnen
            newColumn = GalaxyGeneretion.GenerateColumnLeft(Random.Range((int)0, int.MaxValue), 1, 400, _galaxyoffsetX, _galaxyoffsetY);

            // för en hel kolonn
            for (int i = 0; i < (int)GalaxyGeneretion._boxMaxY; i++)
            {
                int delIn = (int)((GalaxyGeneretion._boxMaxX - 1) + ((GalaxyGeneretion._boxMaxX) * i));
                int addIn = (int)((GalaxyGeneretion._boxMaxX) * i);

                GameObject.Destroy(boxList[delIn].gameObject);
                boxList.Remove(boxList[delIn]);

                boxList.Insert(addIn, newColumn[i]);
            }
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
        //GenerateNewColumn(true);
    }
}
