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

        public FMODAsset damage_heavy_mech;
        public FMODAsset damage_heavy_postAp;
        public FMODAsset damage_medium_mech;
        public FMODAsset damage_medium_postAp;
        public FMODAsset damage_light_mech;
        public FMODAsset damage_light_postAp;

        public FMODAsset damage_heavy
        {
            get
            {
                switch (Global.Instance._planet._type)
                {
                    case Planet.PlanetType.PostApc:
                        return damage_heavy_postAp;
                    case Planet.PlanetType.Mech:
                        return damage_heavy_mech;
                    default:
                        break;
                }
                return null;
            }
        }
        public FMODAsset damage_medium
        {
            get
            {
                switch (Global.Instance._planet._type)
                {
                    case Planet.PlanetType.PostApc:
                        return damage_medium_postAp;
                    case Planet.PlanetType.Mech:
                        return damage_medium_mech;
                    default:
                        break;
                }
                return null;
            }
        }
        public FMODAsset damage_light
        {
            get
            {
                switch (Global.Instance._planet._type)
                {
                    case Planet.PlanetType.PostApc:
                        return damage_light_postAp;
                    case Planet.PlanetType.Mech:
                        return damage_light_mech;
                    default:
                        break;
                }
                return null;
            }
        }

        public ShieldSounds shieldSounds;
    }

    [System.Serializable]
    public struct SpellSounds
    {
        public FMODAsset hold;
        public FMODAsset ready;
        public FMODAsset take;
        public FMODAsset use;
    }

    [System.Serializable]
    public struct AbilityBaseSounds
    {
        public SpellSounds damage;
        public SpellSounds heal;
        public SpellSounds stun;
    }

    [System.Serializable]
    public struct TechAbilitySounds
    {
        public FMODAsset granadeReady;
        public FMODAsset granadeTake;
        public FMODAsset granadeUse;
        public FMODAsset lightningUse;
        public FMODAsset lightningReady;
        public FMODAsset lightningTake;
        public FMODAsset shieldReady;
        public FMODAsset shieldUse;
    }

    [System.Serializable]
    public struct KineticAbilitySounds
    {
        public FMODAsset siesmicReady;
        public FMODAsset siesmicTake;
        public FMODAsset siesmicUse;
        public FMODAsset supersonicReady;
        public FMODAsset supersonicTake;
        public FMODAsset supersonicUse;
        public FMODAsset tremorReady;
        public FMODAsset tremorTake;
        public FMODAsset tremorUse;

    }

    [System.Serializable]
    public struct PsychicAbilitySounds
    {
        public FMODAsset drainReady;
        public FMODAsset drainTake;
        public FMODAsset drainUse;
        public FMODAsset mindfrayReady;
        public FMODAsset mindfrayTake;
        public FMODAsset mindfrayUse;
    }

    [System.Serializable]
    public struct ExtensionsAbilitySounds
    {
        public FMODAsset adrenalineReady;
        public FMODAsset adrenalineTake;
        public FMODAsset adrenalineUse;
        public FMODAsset protShieldReady;
        public FMODAsset protShieldTake;
        public FMODAsset protShieldUse;
        public FMODAsset overpowerReady;
        public FMODAsset overpowerTake;
        public FMODAsset overpowerUse;
        public FMODAsset overloadReady;
        public FMODAsset overloadTake;
        public FMODAsset overloadUse;
        public FMODAsset shockwaveReady;
        public FMODAsset shockwaveTake;
        public FMODAsset shockwaveUse;

    }
    
    [System.Serializable]
    public struct AbilitySounds
    {
        public AbilityBaseSounds Base;
        public TechAbilitySounds tech;
        public KineticAbilitySounds kinetic;
        public PsychicAbilitySounds psychic;
        public ExtensionsAbilitySounds extensions;
    }

    [System.Serializable]
    public struct PlayerSounds
    {
        public FMODAsset swipe;
        public FMODAsset takeDamage_postAp;
        public FMODAsset takeDamage_mech;
        public FMODAsset takeDamage {
            get
            {
                switch (Global.Instance._planet._type)
                {
                    case Planet.PlanetType.PostApc:
                        return takeDamage_postAp;
                    case Planet.PlanetType.Mech:
                        return takeDamage_mech;
                    default:
                        break;
                }
                return null;
            } 
        }
        public FMODAsset dies;
        public AbilitySounds abilities;
    }

    [System.Serializable]
    public struct MerchantSounds
    {
        public FMODAsset buy;
    }

    [System.Serializable]
    public struct NavigationSounds
    {
        public FMODAsset closeInventory;
        public FMODAsset openInventory;
        public FMODAsset eterStarmap;
        public FMODAsset exitStarmap;
        public FMODAsset equipGear;
        public FMODAsset unequipGear;
        public FMODAsset selectWorld;
        public FMODAsset enterBattle;
        public FMODAsset exitBattle;
        public FMODAsset enterGalaxy;
        public FMODAsset selectGalaxy;
    }

    [System.Serializable]
    public struct UISounds
    {
        public MerchantSounds merchant;
        public NavigationSounds navigation;
        public FMODAsset coinCollect;
        public FMODAsset Button;
        public FMODAsset medpackPickup;
        public FMODAsset lootcrate;
        public FMODAsset changeAbility;
        public FMODAsset pickUpAbility;
        public FMODAsset pauseGame;
        public FMODAsset unPauseGame;
    }
    [System.Serializable]
    public struct Music
    {
        public FMODAsset gameOver;
        public FMODAsset clearLevel;
        public FMODAsset levelUp;
    }

    public PlayerSounds playerSounds;
    public EnemySounds enemySounds;
    public UISounds uiSounds;
    public Music music;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public static void OneShot(FMODAsset asset_)
    {
        OneShot(asset_, new Vector3(0, 0, 0));
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
    public static int GetLength(FMODAsset asset_)
    {
        FMOD.Studio.EventInstance ev = FMOD_StudioSystem.instance.GetEvent(asset_);
        return GetLength(ev);
    }

    public static int GetLength(FMOD.Studio.EventInstance event_)
    {
        int lengthms = 0;

        FMOD.Studio.EventDescription ed = null;
        event_.getDescription(out ed);

        ed.getLength(out lengthms);

        return lengthms;
    }
}
