using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Sprites : MonoBehaviour {

    public static Sprites _instance;
    public static Sprites Instance
    {
        get
        {
            return _instance;
        }
    }

    [System.Serializable]
    public class Spells
    {
        public Sprite[] sprites = new Sprite[24];
    }

    public Spells spells = new Spells();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
