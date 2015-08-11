using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GALAXY : MonoBehaviour {

    public GameObject galaxyStarPrefab;

    public GameObject boxes;
    public List<GalaxyGeneretion.StarBox> boxList;

    public GameObject popup;
    public GameObject selector;

    private bool _mouseDown = false;
    private Vector3 _mouseOld = Vector3.zero;

    public StarSystem _lastStar = null;

    public Vector2 screenScale
    {
        get
        {
            return new Vector2(Screen.width / 1920f , Screen.height / 1080f);
        }
    }

    public static uint galacticCenterTileX
    {
        get
        {
            return (uint.MaxValue / 2) + (GalaxyGeneretion._boxMaxX / 2);
        }
    }

    public static uint galacticCenterTileY
    {
        get
        {
            return (uint.MaxValue / 2) + (GalaxyGeneretion._boxMaxY / 2);
        }
    }

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
        // middle of the fucking universe
        Global.Instance._galaxy._galaxyoffsetX = (uint.MaxValue / 2) - 2;
        Global.Instance._galaxy._galaxyoffsetY = (uint.MaxValue / 2) - 2;

        GenerateGalaxy();

        popup.gameObject.SetActive(false);

	}

    public void DoAnew()
    {
        foreach (var item in boxList)
        {
            Destroy(item.gameObject);
        }
        boxList.Clear();
        GenerateGalaxy();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.B))
        {
            DoAnew();
        }

        if (MouseController.Instance.buttonDown)
        {
            if (_mouseDown)
            {
                Vector3 delta = _mouseOld - MouseController.Instance.position;
                boxes.GetComponent<RectTransform>().position -= delta;
            }
            
            _mouseDown = true;

        }
        else if (_mouseDown)
        {
            _mouseDown = false;
        }

        _mouseOld = MouseController.Instance.position;

        Vector3 pos = boxes.GetComponent<RectTransform>().position;
        if (pos.x < -GalaxyGeneretion.StarBox.width / 2f)
        {
            // new collum on the right
            if (Global.Instance._galaxy._galaxyoffsetX < uint.MaxValue - GalaxyGeneretion._boxMaxX)
            {
                //pos.x += GalaxyGeneretion.StarBox.width / 2f;
                pos.x = 50f;
                Global.Instance._galaxy._galaxyoffsetX++;
                OffsetBoxes(-1, 0);
                GenerateNewColumn(true);
            }
        }
        if (pos.x > GalaxyGeneretion.StarBox.width / 2f)
        {
            // new collum on the left
            if (Global.Instance._galaxy._galaxyoffsetX > 0)
            {
                //pos.x -= GalaxyGeneretion.StarBox.width / 2f;
                pos.x = -50f;
                Global.Instance._galaxy._galaxyoffsetX--;
                OffsetBoxes(1, 0);
                GenerateNewColumn(false);
            }
        }
        if (pos.y < -GalaxyGeneretion.StarBox.height / 2f)
        {
            // new line bottom (first)
            if (Global.Instance._galaxy._galaxyoffsetY < uint.MaxValue - GalaxyGeneretion._boxMaxY)
            {
                //pos.y += GalaxyGeneretion.StarBox.height / 2f;
                pos.y = 50f;
                Global.Instance._galaxy._galaxyoffsetY++;
                OffsetBoxes(0, -1);
                GenerateNewLine(false);
            }
        }
        if (pos.y > GalaxyGeneretion.StarBox.height / 2f)
        {
            // new line top (last)
            if (Global.Instance._galaxy._galaxyoffsetY > 0)
            {
                //pos.y -= GalaxyGeneretion.StarBox.height / 2f;
                pos.y = -50f;
                Global.Instance._galaxy._galaxyoffsetY--;
                OffsetBoxes(0, 1);
                GenerateNewLine(true);
            }
        }

        boxes.GetComponent<RectTransform>().position = pos;

        if (_lastStar == null && selector.gameObject.activeInHierarchy)
        {
            selector.gameObject.SetActive(false);
            popup.SetActive(false);
        }
        if (selector.gameObject.activeInHierarchy)
        {
            try
            {
                selector.transform.position = _lastStar.transform.position;
            } catch(System.Exception) {}
        }
	}

    public void OffsetBoxes(int dirX_, int dirY_)
    {
        foreach (var item in boxes.GetComponentsInChildren<Transform>())
        {
            if (item.name != "Boxes")
            {
                Vector3 pos = item.transform.localPosition;
                pos.x += GalaxyGeneretion.StarBox.width * dirX_ / 2f;
                pos.y += GalaxyGeneretion.StarBox.height * dirY_ / 2f;
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
            newLine = GalaxyGeneretion.GenerateLineBottom(Random.Range((int)0, int.MaxValue), 0, Global.Instance._galaxy._galaxyoffsetX, Global.Instance._galaxy._galaxyoffsetY);

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
            newLine = GalaxyGeneretion.GenerateLineTop(Random.Range((int)0, int.MaxValue), 0, Global.Instance._galaxy._galaxyoffsetX, Global.Instance._galaxy._galaxyoffsetY);
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
            newColumn = GalaxyGeneretion.GenerateColumnRight(Random.Range((int)0, int.MaxValue), 0, Global.Instance._galaxy._galaxyoffsetX, Global.Instance._galaxy._galaxyoffsetY);

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
            newColumn = GalaxyGeneretion.GenerateColumnLeft(Random.Range((int)0, int.MaxValue), 0, Global.Instance._galaxy._galaxyoffsetX, Global.Instance._galaxy._galaxyoffsetY);

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
        Starmap.Instance.Generate(_lastStar._llevel, _lastStar._ulevel, _lastStar._seed);
        Global.DebugOnScreen("genererar i galaxy");

        Global.Instance.SwitchScene(Global.GameType.Star);
    }

    public void GenerateGalaxy()
    {
        int seed = (int)Mathf.Pow((float)Global.Instance._galaxy._galaxyoffsetX, (float)Global.Instance._galaxy._galaxyoffsetY);
        Random.seed = seed;

        boxList = GalaxyGeneretion.GenerateGalaxy(Random.Range((int)0, int.MaxValue), 0, Global.Instance._galaxy._galaxyoffsetX, Global.Instance._galaxy._galaxyoffsetY);
    }

    void Awake()
    {
        //GenerateGalaxy();
        //GenerateNewColumn(true);
    }
}
