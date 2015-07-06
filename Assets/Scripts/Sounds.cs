using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

    private static Sounds _instance = null;
    public static Sounds Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("SOUNDS").GetComponent<Sounds>();
            }
            return _instance;
        }
    }

    [System.Serializable]
    public struct ShieldSounds
    {
        public FMODAsset start;
        public FMODAsset damage;
        public FMODAsset loop;
        public FMODAsset stop;
    }
    
    [System.Serializable]
    public struct EnemySounds
    {
        public FMODAsset damage_heavy;
        public FMODAsset damage_medium;
        public FMODAsset damage_light;

        public ShieldSounds shieldSounds;
    }

    public EnemySounds enemySounds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public static void OneShot(FMODAsset asset_, Vector3 pos_)
    {
        FMOD_StudioSystem.instance.PlayOneShot(asset_.path, pos_);
    }
}
