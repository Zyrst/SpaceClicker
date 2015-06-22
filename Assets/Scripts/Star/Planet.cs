using UnityEngine;
using System.Collections;

[System.Serializable]
public class Planet {

    public int _minLevel = 1;
    public int _maxLevel = 10;

    public string _name = "Plenteru";
    public Sprite _sprite;

    public enum PlanetType : int
    {
        PostApc = 0,
        Mech =  1
    }

    public PlanetType _type = PlanetType.PostApc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
