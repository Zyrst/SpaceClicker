using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
    [System.Serializable]
    public class Prefabs
    {
        public GameObject Number;
    }

    [System.Serializable]
    public class Colors
    {
        public Color normalAttackColor;
        public Color techAttackColor;
        public Color kineticAttackColor;
        public Color psychicAttackColor;
    }

    public Player _player;

    public Prefabs _prefabs = new Prefabs();
    public Colors _colors = new Colors();

    private static Global _instance = null;
    public static Global Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GLOBALS").GetComponent<Global>();
            }
            return _instance;
        }
    }

}
