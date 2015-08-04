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
    public static uint _boxMaxX = 7;
    public static uint _boxMaxY = 5;

    // max number of stars in a box 
    private static int _starMax = 4;

    private static int _minStarDist = 120;

    public static List<StarBox> GenerateGalaxy(int seed_, uint llevel_, uint ulevel_, uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = 0;

        while (_boxY < _boxMaxY)
        {
            Global.Instance.StartCoroutine(GenerateStarBoxLine(ret, llevel_, ulevel_, startX_, startY_));
        }


        return ret;
    }

    public static List<StarBox> GenerateLineBottom(int seed_, uint llevel_, uint ulevel_,  uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = 0;

        while(_boxY == 0)
            Global.Instance.StartCoroutine(GenerateStarBoxLine(ret, llevel_, ulevel_, startX_, startY_));

        return ret;
    }

    public static List<StarBox> GenerateLineTop(int seed_, uint llevel_, uint ulevel_, uint startX_, uint startY_)
    {
        Random.seed = seed_;

        List<StarBox> ret = new List<StarBox>();

        _boxX = 0;
        _boxY = _boxMaxY-1;

        while (_boxY == _boxMaxY-1)
            Global.Instance.StartCoroutine(GenerateStarBoxLine(ret, llevel_, ulevel_, startX_, startY_));

        return ret;
    }

    private static IEnumerator GenerateStarBoxLine(List<StarBox> starBoxList_, uint llevel_, uint ulevel_, uint startX_, uint startY_)
    {
        GameObject box = new GameObject();
        StarBox ret = box.AddComponent<StarBox>();
        box.transform.parent = GALAXY.Instance.boxes.transform;

        ret._idPos.x = _boxX + startX_;
        ret._idPos.y = _boxY + startY_;

        box.name = _boxX + " - " + _boxY;

        _boxX += 1;

        if (_boxX == _boxMaxX)
        {
            _boxX = 0;
            _boxY += 1;
        }

        // get random seed
        int seed = (int)(_boxY + startY_) + ((int)(_boxX + startX_) * (int)(_boxY + startY_)) + (int)(_boxX + startX_);
        Random.seed = seed;

        // generate stars
        ret._starSystems = GenerateStarSystems(llevel_, ulevel_);

        // set position of box
        box.transform.localPosition = new Vector3(
            StarBox.width/2f + (StarBox.width * _boxX) - StarBox.width * 1.5f, 
            StarBox.height/2f + (StarBox.height * _boxY) - StarBox.height * 1.5f,
            0);

        // set scale of box 
        box.transform.localScale = new Vector3(1f, 1f, 1f);

        // make all stars children to box
        foreach (var item in ret._starSystems)
        {
            item.GetComponent<RectTransform>().SetParent(box.transform);
        }

        // position for stars 
        foreach (var item in ret._starSystems)
        {
            bool next = false;
            while (!next)
            {
                next = true;

                // position
                item.transform.position = Vector3.zero;
                item.transform.localPosition = new Vector3(
                    Random.Range(0f, (float)StarBox.width/2f),
                    Random.Range(0f, (float)StarBox.height/2f),
                    0f);

                // fix scale
                item.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                foreach (var other in ret._starSystems)
                {
                    if (item != other)
                    {
                        if (Vector3.Distance(item.transform.localPosition, other.transform.localPosition) < _minStarDist)
                        {
                            next = false;
                        }
                    }
                }
            }
        }

        starBoxList_.Add(ret);
        yield return null;

        yield break;
    }

    private static List<StarSystem> GenerateStarSystems(uint llevel_, uint ulevel_)
    {
        List<StarSystem> ret = new List<StarSystem>();

        int maxStar = Random.Range((int)0, (int)_starMax+1);
        for (int i = 0; i < maxStar; i++)
        {
            ret.Add(GenerateStarSystem(llevel_, ulevel_));
        }

        return ret;
    }


    private static StarSystem GenerateStarSystem(uint llevel_, uint ulevel_)
    {
        GameObject ss = GameObject.Instantiate(GALAXY.Instance.galaxyStarPrefab);
        StarSystem ret = ss.GetComponent<StarSystem>();
        Image img = ss.GetComponent<Image>();

        img.sprite = Sprites.Instance.galaxy.GalaxyStar.sprite;
        img.color = Color.yellow;

        ss.name = "GalaxyStar";

        ret._llevel = llevel_;
        ret._ulevel = ulevel_;

        return ret;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
