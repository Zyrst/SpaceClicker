using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GalaxyGeneretion : MonoBehaviour {

    public class StarBox : MonoBehaviour
    {
        public struct IDPos
        {
            public uint x;
            public uint y;
        }

        public IDPos _idPos;

        public uint _llevel = 0;
        public uint _ulevel = 0;

        public static float width = 480f;
        public static float height = 480f;

        public List<StarSystem> _starSystems;
    }

    // current box id
    private static uint _boxX = 0;
    private static uint _boxY = 0;

    // max boxes
    public static uint _boxMaxX = 9;
    public static uint _boxMaxY = 6;

    // max number of stars in a box 
    private static int _starMax = 8;

    private static int _minStarDist = 60;

    private static uint Level(uint startX_, uint startY_)
    {

        long tileX = uint.MaxValue / 2;
        tileX -= startX_ + _boxX;

        long tileY = uint.MaxValue / 2;
        tileY -= startY_ + _boxY;

        tileX = tileX > 0 ? tileX : -tileX;
        tileY = tileY > 0 ? tileY : -tileY;

        uint d = tileX > tileY ? (uint)tileX : (uint)tileY;

        return d;
    }

    public static List<StarBox> GenerateGalaxy(int seed_, uint llevel_, uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = 0;

        while (_boxY < _boxMaxY)
        {

            Global.Instance.StartCoroutine(GenerateStarBox(ret, llevel_ + Level(startX_, startY_), startX_, startY_));
        }

        return ret;
    }

    public static List<StarBox> GenerateLineBottom(int seed_, uint llevel_,  uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = 0;

        while (_boxY == 0)
        {
            Global.Instance.StartCoroutine(GenerateStarBox(ret, llevel_ + Level(startX_, startY_), startX_, startY_));
        }

        return ret;
    }

    public static List<StarBox> GenerateLineTop(int seed_, uint llevel_, uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = _boxMaxY-1;

        while (_boxY == _boxMaxY - 1)
        {
            Global.Instance.StartCoroutine(GenerateStarBox(ret, llevel_ + Level(startX_, startY_), startX_, startY_));
        }

        return ret;
    }

    public static List<StarBox> GenerateColumnRight(int seed_, uint llevel_, uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = 0;

        for (int i = 0; i < _boxMaxY; i++)
        {
            _boxX = _boxMaxX-1;
            Global.Instance.StartCoroutine(GenerateStarBox(ret, llevel_ + Level(startX_, startY_), startX_, startY_));
        }


        return ret;
    }

    public static List<StarBox> GenerateColumnLeft(int seed_, uint llevel_, uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = 0;

        for (int i = 0; i < _boxMaxY; i++)
        {
            _boxX = 0;
            Global.Instance.StartCoroutine(GenerateStarBox(ret, llevel_ + Level(startX_, startY_), startX_, startY_));
            _boxY++;
        }

        return ret;
    }

    private static IEnumerator GenerateStarBox(List<StarBox> starBoxList_, uint llevel_, uint startX_, uint startY_)
    {
        Vector2 screenScale = new Vector2(Screen.width/1920f, Screen.height/1080f);
        screenScale = new Vector2(1, 1);
        GameObject box = new GameObject();

        StarBox ret = box.AddComponent<StarBox>();
        box.transform.parent = GALAXY.Instance.boxes.transform;

        ret._idPos.x = _boxX + startX_;
        ret._idPos.y = _boxY + startY_;

        box.name = _boxX + " - " + _boxY;

        // get random seed
        int seed = (int)((uint)(_boxY + startY_) + ((uint)(_boxX + startX_) * (uint)(_boxY + startY_)) + (uint)(_boxX + startX_));
        Random.seed = seed;

        // generate stars
        ret._starSystems = GenerateStarSystems(llevel_);


        // set position of box
        box.transform.localPosition = new Vector3(
            (((StarBox.width / 2f) * screenScale.x) + ((StarBox.width * _boxX) * screenScale.x) - ((StarBox.width * 1.5f)) * screenScale.x),
            (((StarBox.height / 2f) * screenScale.y) + ((StarBox.height * _boxY) * screenScale.y) - ((StarBox.height * 1.5f)) * screenScale.y),
            0);

        // set scale of box 
        box.transform.localScale = new Vector3(1f, 1f, 1f);

        // make all stars children to box
        foreach (var item in ret._starSystems)
        {
            item.GetComponent<RectTransform>().SetParent(box.transform);
        }

        // position for stars 
        #region MyRegion
        foreach (var item in ret._starSystems)
        {
            bool next = false;
            while (!next)
            {
                next = true;

                // position
                item.transform.position = Vector3.zero;
                item.transform.localPosition = new Vector3(
                    Random.Range(-(float)StarBox.width / 2f, (float)StarBox.width / 2f),
                    Random.Range(-(float)StarBox.height / 2f, (float)StarBox.height / 2f),
                    0f);

                // fix scale
                item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                foreach (var other in ret._starSystems)
                {
                    if (item != other)
                    {
                        if (Vector3.Distance(item.transform.localPosition, other.transform.localPosition) < _minStarDist)
                        {
                            // redo star
                            next = false;
                        }
                    }
                }
            }
        } 
        #endregion

        // seed for each star
        for (int i = 0; i < ret._starSystems.Count; i++)
        {
            ret._starSystems[i]._seed = seed * i;
        }

        starBoxList_.Add(ret);

        _boxX += 1;

        if (_boxX == _boxMaxX)
        {
            _boxX = 0;
            _boxY += 1;
        }

        yield return null;

        yield break;
    }

    private static List<StarSystem> GenerateStarSystems(uint llevel_)
    {
        List<StarSystem> ret = new List<StarSystem>();

        int maxStar = Random.Range((int)0, (int)_starMax+1);
        for (int i = 0; i < maxStar; i++)
        {
            ret.Add(GenerateStarSystem(llevel_));
        }

        return ret;
    }

    private static StarSystem GenerateStarSystem(uint llevel_)
    {
        GameObject ss = GameObject.Instantiate(GALAXY.Instance.galaxyStarPrefab);
        StarSystem ret = ss.GetComponent<StarSystem>();
        Image img = ss.GetComponent<Image>();

        img.sprite = Sprites.Instance.galaxy.GalaxyStar.sprite;

        ss.name = "GalaxyStar";

        ret._llevel = llevel_ + (uint)Random.Range((int)0, (int)Global.Instance._galaxy._starLevelRangePerTile);
        if (llevel_ < 1)
        {
            ret._llevel = 1;
        }

        ret._starColor = Global.DetermineLevelColor(ret._llevel, Player.Instance._level);
        ret._starBackground = (StarSystem.StarBackgrounds)ret._starColor;
        img.color = Global.Instance._colors.levelColors[ret._starColor];

        /*ret._starColor = Random.Range(0, StarSystem.StarColor.Length);
        ret._starBackground = (StarSystem.StarBackgrounds)ret._starColor;
        img.color = StarSystem.StarColor[ret._starColor];*/
        ret.GetComponentInChildren<Text>().text = ret._llevel.ToString();

        ret.GenerateRotationAndStuff();

        return ret;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
