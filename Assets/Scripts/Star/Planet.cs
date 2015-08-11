using UnityEngine;
using System.Collections;

public class Planet {

    public uint _minLevel = 1;
    public uint _maxLevel = 10;

    public string _name = "Plenteru";
    public Sprite _sprite;

    public PlanetType _type = PlanetType.PostApc;

    public enum PlanetType : int
    {
        PostApc = 0,
        Mech =  1
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
