﻿using UnityEngine;
using System.Collections;

public class Sprites : MonoBehaviour {

    private static Sprites _instance;
    public static Sprites Instance
    {
        get
        {
            return _instance;
        }
    }

    public Sprites()
    {
        _instance = this;
    }

    [System.Serializable]
    public class Equipment {
        public SpriteRef Head;
        public SpriteRef Chest;
        public SpriteRef Weapon;
        public SpriteRef Legs;
    }
    [System.Serializable]
    public class Spells
    {
        public SpriteRef Damage;
        public SpriteRef Heal;
        public SpriteRef Stun;
    }

    public Spells spells;
    public Equipment equipment;

    //public Spells spells;
    //public Equipment equipment;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}