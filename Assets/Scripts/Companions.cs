using UnityEngine;
using System.Collections;

public class Companions : MonoBehaviour {

    [HideInInspector]
    System.DateTime _timeStamp;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private static Companions _instance = null;

    public static Companions Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GLOBALS").GetComponent<Companions>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    /// <summary>
    /// Call on to save current time, use for comparing next time you start the game
    /// </summary>
    public void SaveTime()
    {
        _timeStamp = System.DateTime.Now;
    }
}
