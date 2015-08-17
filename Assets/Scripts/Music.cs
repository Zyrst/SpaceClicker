using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

    [System.Serializable]
    public struct MenuTheme
    {
        public FMOD.Studio.EventInstance _instance;
        public FMOD.Studio.ParameterInstance _merchantParameter;
        public FMOD.Studio.ParameterInstance _starmapParameter;
        public FMOD.Studio.ParameterInstance _combatParameter;
        public FMODAsset _theme;
        public bool _isRunning;

        private int _dir;
        private float _fade;

        public enum Events { NoEvent, Merchant, Starmap, Combat };
        private Events _event;

        public void Update()
        {
            switch (_event)
            {
                case Events.Merchant:
                    {
                        float currentParameter;
                        _merchantParameter.getValue(out currentParameter);
                        if (currentParameter < 1f && _dir == 1)
                        {
                            currentParameter += _dir * Time.deltaTime;
                            _merchantParameter.setValue(currentParameter);
                        }
                        else if (currentParameter > 0f && _dir == -1)
                        {
                            currentParameter += _dir * Time.deltaTime;
                            _merchantParameter.setValue(currentParameter);
                        }
                        else
                            _event = Events.NoEvent;
                    }
                    break;
                case Events.Starmap:
                    {
                        float currentParameter;
                        _starmapParameter.getValue(out currentParameter);
                        if (currentParameter < 1f && _dir == 1)
                        {
                            currentParameter += _dir * Time.deltaTime;
                            _starmapParameter.setValue(currentParameter);
                        }
                        else if (currentParameter > 0f && _dir == -1)
                        {
                            currentParameter += _dir * Time.deltaTime;
                            _starmapParameter.setValue(currentParameter);
                        }
                        else
                            _event = Events.NoEvent;
                    }
                    break;
                case Events.Combat:
                    {
                        float currentParameter;
                        _combatParameter.getValue(out currentParameter);
                        if (currentParameter < 1f && _dir == 1)
                        {
                            currentParameter += _dir * Time.deltaTime;
                            _combatParameter.setValue(currentParameter);
                        }
                        else
                        {
                            Global.DebugOnScreen("Done with combat event");
                            _event = Events.NoEvent;
                            _instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                            _isRunning = false;
                        }
                            
                    }
                    break;
                default:
                    break;
            }
        }

        public void StartMerchant()
        {
            _event = Events.Merchant;
            _dir = 1;
        }

        public void ExitMerchant()
        {
            _event = Events.Merchant;
            _dir = -1;
        }

        public void StartStarmap()
        {
            _event = Events.Starmap;
            _dir = 1;
        }

        public void ExitStarmap()
        {
            _event = Events.Starmap;
            _dir = -1;
        }

        public void EnterCombat()
        {
            _event = Events.Combat;
            _dir = 1;
        }
    }

    static Music _instance = null;
    bool _init = false;


    public static Music Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("MUSIC").GetComponent<Music>();
                _instance.Init();
            }
            return _instance;
        }
    }

    public Music()
    {
    }

    public MenuTheme _menuTheme;

    private void Init()
    {
        if (!_init)
        {
            _menuTheme._instance = FMOD_StudioSystem.instance.GetEvent(_menuTheme._theme);
            _menuTheme._instance.getParameter("Merchant", out _menuTheme._merchantParameter);
            _menuTheme._instance.getParameter("Starmap", out _menuTheme._starmapParameter);
            _menuTheme._instance.getParameter("Enter Combat", out _menuTheme._combatParameter);
            _menuTheme._isRunning = false;
            _init = true;

        }
        
    }

	// Use this for initialization
    void Start()
    {
        _instance = this;

        Init();

        //StartMenuTheme();
	}

    void Awake()
    {
    }
	
	// Update is called once per frame
	void Update () {
        _menuTheme.Update();
	}

    static int iiu = 0;
    public void StartMenuTheme()
    {
        if (_menuTheme._isRunning)
            _menuTheme._instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        _menuTheme._merchantParameter.setValue(0f);
        _menuTheme._starmapParameter.setValue(0f);
        _menuTheme._combatParameter.setValue(0f);
        _menuTheme._instance.start();
        _menuTheme._isRunning = true;
    }

    public void EnterCombatFromMenu()
    {
        _menuTheme.EnterCombat();
        
    }
    

    
}
