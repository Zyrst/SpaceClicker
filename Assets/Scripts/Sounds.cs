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

    [System.Serializable]
    public struct PlayerSounds
    {
        public FMODAsset swipe;
    }

    public PlayerSounds playerSounds;
    public EnemySounds enemySounds;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public static void OneShot(FMODAsset asset_)
    {
        OneShot(asset_, new Vector3(-74, 61, -74));
    }

    public static void OneShot(FMODAsset asset_, Vector3 pos_)
    {
        FMOD_StudioSystem.instance.PlayOneShot(asset_.path, pos_);
    }

    /// <summary>
    /// returns lenght of sound in milliseconds
    /// </summary>
    /// <param name="asset_"></param>
    /// <returns></returns>
    public static int GetLenght(FMODAsset asset_)
    {
        int lenghtms = 0;

        FMOD.Studio.EventInstance ev = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.enemySounds.shieldSounds.start);

        FMOD.Studio.EventDescription ed = null;
        ev.getDescription(out ed);

        ed.getLength(out lenghtms);

        return lenghtms;
    }
}
