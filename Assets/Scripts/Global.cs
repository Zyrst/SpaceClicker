using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

[System.Serializable]
public class Global : MonoBehaviour {
    [System.Serializable]
    public class Prefabs
    {
        public GameObject Number;
        public GameObject InventorySlot;
        public GameObject[] _enemyPrefab;
        public GameObject GoldCoin;
        public GameObject HealthPotion;
        public GameObject LootCrate;
        public GameObject ExpBall;
        public GameObject[] _effects;
    }

    [System.Serializable]
    public class Colors
    {
        public Color normalAttackColor;
        public Color techAttackColor;
        public Color kineticAttackColor;
        public Color psychicAttackColor;
        public Color healColor;

        public Color pausColor;

        public Color green;
        public Color blue;
        public Color purple;
        public Color orange;

        public Color[] levelColors;
    }
    [System.Serializable]
    public class Enemies
    {
        public GameObject[] _postApc;
        public GameObject[] _mech;
        public GameObject[] _currentEnemies;
    }
    [System.Serializable]
    public class Spells
    {
        public List<SpellAttack> damageSpells = new List<SpellAttack>();
        public List<SpellAttack> healSpells = new List<SpellAttack>();
        public List<SpellAttack> stunSpells = new List<SpellAttack>();
    }

    [System.Serializable]
    public class Textures
    {
        public Texture _hitEffect;
        public Material _shield;
        public Material _player;
    }

    [System.Serializable]
    public class GalaxyShizz
    {
        public uint _galaxyoffsetX = 0;
        public uint _galaxyoffsetY = 0;

        public uint _starLevelRangePerTile = 3;
        public uint _increasePerPlanet = 3;
    }

    public class Effects
    {
        public class LevelUp
        {
            public void Start()
            {
				GameObject go = GameObject.Instantiate (Instance._prefabs._effects [2]);
				go.transform.position = Instance._player.transform.position;
            }
        }

        public LevelUp levelUp = new LevelUp();
    }

    public enum GameType : int { Farm = 0, Quest = 1 , Ship = 3, Star = 4, Galaxy = 5, CharCreation = 6 }
    public GameType _gameType = GameType.Farm;

    public uint _gold = 0;
    public uint Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            _playerGUI.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "GoldText").text = _gold.ToString();
        }
    }

    
    public Player _player;
    public Player player
    {
        get
        {
            if (_player == null)
            {
                _player = Player.Instance;
            }

            return _player;
        }
    }
    public Prefabs _prefabs = new Prefabs();
    public Enemies _enemies = new Enemies();
    public Colors _colors = new Colors();
    public Spells _lockedSpells = new Spells();
    public Spells _unlockedSpells = new Spells();
    public Textures _textures = new Textures();
    public GalaxyShizz _galaxy = new GalaxyShizz();
    public float _expVariable = 10f;
    public float _expScale = 1.5f;
    public GameObject _playerGUI;
    public Planet _planet = null;
    public GameObject _spellsObject;
    public Effects effects = new Effects();
    public Text _FPS;
    public bool _showFPS = true;

    public float _damageScale = 1.2f;
    public float _healthScale = 1.5f;


    public TalentStats.Percent _potionDropChans = new TalentStats.Percent(1.05f);
    public TalentStats.Percent _potionHealthPercent = new TalentStats.Percent(1.2f);

    public TalentStats.Percent _potionChansIncrease = new TalentStats.Percent(0.01f);
    public TalentStats.Percent _PotionHealingIncrease = new TalentStats.Percent(0.01f);

    public vap[] _prevLevels = new vap[50];
    public List<vap> _refLevels = new List<vap>();
    [HideInInspector]
    vap _refVap = new vap();
    public float _levelModifier;
    public float _playerLevelModifier;

    public Camera _gameCamera;
    public Camera _uiCamera;

    private bool _paused = false;

    private class DebugMessage
    {
        public string message;
        public float time;
        public void Update() { time -= Time.unscaledDeltaTime; }
    }
    private static List<DebugMessage> _debugMessages = new List<DebugMessage>();

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
        set
        {
            _instance = value;
        }
    }

    void Start()
    {
        Profiler.maxNumberOfSamplesPerFrame = 50000;
        UpdateLevel();
        UpdateExpBar();
        UpdateGoldText();
        //See if a Save.txt files exists
        string fileName = "Save.txt";
        if (File.Exists(fileName))
        {
            //Read first line
            StreamReader sr = File.OpenText(fileName);
            string result = sr.ReadLine();
            //If CharCreate is 0 , go to CharCreate
            if (result == "CharCreation: 0")
            {
                SwitchScene(GameType.CharCreation);
				sr.Close();
            }
            else
            {
                SwitchScene(GameType.Ship);
                sr.Close();
            }
        }
        else
        {
            SwitchScene(GameType.CharCreation);
            //File doesn't exist, create it and add line
            StreamWriter sw = File.CreateText(fileName);
            sw.WriteLine("CharCreation: 0");
            sw.Close();
           
        }

        //Starmap.Instance.Generate(1, 100, 9001);

        _planet = null;
        StartCoroutine(LevelFiller());
    }
    void Update()
    {
        MouseController.Instance.Update();

        if (Input.GetKey(KeyCode.G))
        {
            player.SetExperience((uint)(player._experianceToNext * 0.1f), Global.Instance.player);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(Sounds.Instance.enemySounds.damage_medium.path);
            Sounds.OneShot(Sounds.Instance.enemySounds.damage_medium);
        }
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f;
            }
            else
            {
                if (Time.timeScale + 2f <= 100f)
                    Time.timeScale += 2f;
                else
                    Time.timeScale = 100;
            }
            DebugOnScreen("new timescale is: " + Time.timeScale.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (Time.timeScale - 2f >= 0f)
                Time.timeScale -= 2f;
            else
                Time.timeScale = 0f;
            DebugOnScreen("new timescale is: " + Time.timeScale.ToString());
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Gold += 1000;
        }

        UpdateBebugMessages();
    }

    void LateUpdate()
    {
        MouseController.Instance.LateUpdate();

        if (_showFPS)
        {
            _FPS.text = (1f / Time.deltaTime).ToString();
        }
    }
    public void SwitchScene(GameType gt_)
    {
        EnemySpawner.Reset();
        switch (gt_)
        {
            case GameType.Farm:
                player.gameObject.SetActive(true);
                FarmMode.Instance.gameObject.SetActive(true);
               // Ship.Instance.gameObject.SetActive(false);
                Starmap.Instance.gameObject.SetActive(false);
                FarmMode.Instance.startFarmMode();
              //  GALAXY.Instance.gameObject.SetActive(false);
                _gameCamera.gameObject.SetActive(true);
                _uiCamera.gameObject.SetActive(false);
                _gameCamera.tag = "MainCamera";
                break;
            case GameType.Quest:
                break;
            case GameType.Ship:
                player.gameObject.SetActive(false);
                FarmMode.Instance.gameObject.SetActive(false);
                Ship.Instance.gameObject.SetActive(true);
                Starmap.Instance.gameObject.SetActive(false);
                GALAXY.Instance.gameObject.SetActive(false);
                CharacterCreation.Instance.gameObject.SetActive(false);
                Music.Instance.StartMenuTheme();
                try
                {
                    _gameCamera.gameObject.SetActive(false);
                    _uiCamera.gameObject.SetActive(true);
                    _uiCamera.tag = "MainCamera";
                }
                catch (System.NullReferenceException) { Debug.Log("kamera i switch scene"); }

                foreach (var item in player.gameObject.GetComponentsInChildren<SpellAttack>(true))
                {
                    if (item.IsInvoking())
                        item.CancelInvoke();

                    item.ResetCooldown();
                }
                break;
            case GameType.Star:
              //  player.gameObject.SetActive(false);
               // FarmMode.Instance.gameObject.SetActive(false);
              //  Ship.Instance.gameObject.SetActive(false);
                Starmap.Instance.gameObject.SetActive(true);
                GALAXY.Instance.gameObject.SetActive(false);
                break;

            case GameType.Galaxy:
              //  player.gameObject.SetActive(false);
              //  FarmMode.Instance.gameObject.SetActive(false);
                Ship.Instance.gameObject.SetActive(false);
                Starmap.Instance.gameObject.SetActive(false);
                GALAXY.Instance.gameObject.SetActive(true);
                break;
            case GameType.CharCreation:
                FarmMode.Instance.gameObject.SetActive(false);
                Ship.Instance.gameObject.SetActive(false);
                Starmap.Instance.gameObject.SetActive(false);
                GALAXY.Instance.gameObject.SetActive(false);
                CharacterCreation.Instance.gameObject.SetActive(true);
                CharacterCreation.Instance.Init();
                Music.Instance.StartMenuTheme();
                try
                {
                    _gameCamera.gameObject.SetActive(false);
                    _uiCamera.gameObject.SetActive(true);
                    _uiCamera.tag = "MainCamera";
                }
                catch (System.NullReferenceException) { }
                break;
            default:
                break;
        }
    }

    public ArrayList GetAllEnemies()
    {
        switch (_gameType)
        {
            case GameType.Farm:
                return FarmMode.Instance.GetAllEnemies();
            default:
                break;
        }

        return new ArrayList();
    }

    /// <summary>
    /// returns if the player is alive
    /// </summary>
    /// <returns></returns>
    public bool PlayerAlive()
    {
        return player._isAlive;
    }

    /// <summary>
    /// call this when all enemis are dead
    /// </summary>
    public void AllEnemiesDied()
    {
        switch (_gameType)
        {
            case GameType.Farm:
                FarmMode.Instance.allEnemiesDied();
                break;

            case GameType.Quest:
                break;
            default:
                break;
        }
    }

    public int EnemiesAlive()
    {
        int count = 0;
        foreach (var item in EnemySpawner.triggers.spawns)
        {
            if (((EnemySpawner)item).EnemyIsActive())
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// call this when player dies
    /// </summary>
    public void PlayerDied()
    {
        switch (_gameType)
        {
            case GameType.Farm:
                FarmMode.Instance.playerDied();
                break;
            case GameType.Quest:
                break;
            default:
                break;
        }
    }

    public uint GetEnemyLevel()
    {
        if (_gameType == GameType.Farm)
        {
            int level = Random.Range((int)player._level - 1, (int)player._level+2);
            if (level <= _planet._minLevel)
            {
                return (uint)_planet._minLevel;
            }
            if (level >= _planet._maxLevel)
            {
                return (uint)_planet._maxLevel;
            }
            return (uint)level;
        }
        return 1;
    }

    public void UpdateExpBar()
    {
        try
        {
            float value = (float)(player._experience) / (float)(player._experianceToNext);
            _playerGUI.transform.GetComponentsInChildren<Image>(true).FirstOrDefault(x => x.name == "Exp").transform.localScale = new Vector3(value, 1f, 1f);
            _playerGUI.transform.GetComponentsInChildren<Text>(true).FirstOrDefault(x => x.name == "ExpText").text = "Exp: " + player._experience.ToString() + "/" + player._experianceToNext.ToString();
        }
        catch (System.NullReferenceException) { }
    }

    public void UpdateLevel()
    {
        try
        {
            _playerGUI.transform.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Level").text = player._level.ToString();
        }
        catch (System.NullReferenceException) { }
    }

    public void UpdateGoldText()
    {
        _playerGUI.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "GoldText").text = _gold.ToString();
    }

    public void ShakeCamera()
    {
        try
        {
            Camera.main.GetComponent<GameCamera>().Shake();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Cannot shake current camera");
        }
    }

    /// <summary>
    /// displays message on screen for 'time_' amount of seconds
    /// </summary>
    /// <param name="message_"></param>
    /// <param name="time_"></param>
    public static void DebugOnScreen(string message_, float time_)
    {
        DebugMessage newMess = new DebugMessage();
        newMess.message = message_;
        newMess.time = time_;

        _debugMessages.Add(newMess);
    }

    /// <summary>
    /// displays message on screen for 5 seconds
    /// </summary>
    /// <param name="message_"></param>
    public static void DebugOnScreen(string message_)
    {
        DebugOnScreen(message_, 5f);
    }

    public void UpdateBebugMessages()
    {
        string allMessages = "";
        for (int i = 0; i < _debugMessages.Count; i++)
        {
            _debugMessages.ToArray()[i].Update();

            if (_debugMessages.ToArray()[i].time <= 0f)
            {
                _debugMessages.Remove(_debugMessages.ToArray()[i]);
                i--;
            }
            else
            {
                allMessages = allMessages + "\n" + _debugMessages.ToArray()[i].message;
            }
        }

        GetComponentInChildren<Text>().text = allMessages;
    }

    public void PostIO()
    {
        GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Spells").gameObject.SetActive(false);
    }

    public static int DetermineLevelColor(uint otherLevel_, uint playerLevel_)
    {
        int ret = 0;

        int p = (int)playerLevel_;
        int s = (int)otherLevel_;

        if (playerLevel_ > uint.MaxValue / 2)
        {
            p = (int)(playerLevel_ - uint.MaxValue / 2);
            s = (int)(otherLevel_ - uint.MaxValue / 2);
        }

        if (p - 9 >= s)
        {
            ret = 0;
            return 0;
        }
        if (p - 6 >= s)
        {
            ret = 1;
            return 1;
        }
        if (p - 3 >= s)
        {
            ret = 2;
            return 2;
        }
        if (p - 3 <= s && p + 3 >= s)
        {
            return 2;
        }
        if (p + 3 <= s)
        {
            ret = 3;
        }
        if (p + 6 <= s)
        {
            ret = 4;
        }
        if (p + 9 <= s)
        {
            ret = 5;
        }
        if (p + 12 <= s)
        {
            ret = 6;
        }
        if (p + 15 <= s)
        {
            ret = 7;
        }

        return ret;
    }

    public void PausGame()
    {
        if (!_paused)
        {
            _paused = true;
            Time.timeScale = 0f;
            Sounds.OneShot(Sounds.Instance.uiSounds.pauseGame);

            FarmMode.Instance.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "OverLayImage").color = _colors.pausColor;
        }
        else
        {
            _paused = false;
            Time.timeScale = 1f;
            Sounds.OneShot(Sounds.Instance.uiSounds.unPauseGame);
            FarmMode.Instance.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "OverLayImage").color = new Color(0f,0f,0f,0f);
        }
    }

    IEnumerator LevelFiller()
    {
        /*if (_prevLevels.Length == 0)
        {
            _prevLevels = new vap[50];
            vap first = new vap();
            first._values[0] = _levelModifier;
            first.Checker();
            _prevLevels[0] = new vap(first);
        }

       for (int i = 1; i < 50; i++)
        {
            
            vap tmpVap = new vap();
            tmpVap =  (Global.Instance._prevLevels[i - 1] * _levelModifier);
            tmpVap.Checker();
            _prevLevels[i] = new vap(tmpVap);
           yield return null; 
        }*/
        _refVap._values[0] = _levelModifier;
        _refVap.Checker();
        _refLevels.Add(new vap(_refVap));
        for (int i = 1; i <= 600; i++)
        {
            vap tmpVap = new vap();
            tmpVap = _refVap * _levelModifier;
            tmpVap.Checker();
            if (i % 50 == 0)
            {
                _refLevels.Add(new vap(tmpVap));
            }
            _refVap = new vap(tmpVap);
            yield return null;
        }

            yield break;
    }

    IEnumerator SecondLevelFiller(uint lowLvl_, uint maxLvl_)
    {
        vap[] tmpArray = new vap[maxLvl_ - lowLvl_ + 1];
        vap[] evenTmp = new vap[50];

        evenTmp[0] = new vap(_refLevels[Mathf.FloorToInt(lowLvl_ / 50f)]);
        for (int x = 1; x < lowLvl_ % 50; x++)
        {
            vap tmp = new vap();
            tmp = evenTmp[x - 1] * _levelModifier;
            evenTmp[x] = new vap(tmp);

        }
        
        tmpArray[0] = new vap(evenTmp[(lowLvl_ % 50) - 1]);
        for (int i = 1; i < tmpArray.Length; i++)
        {
            vap tmpVap = new vap();
            tmpVap = (tmpArray[i - 1] * _levelModifier);
            tmpVap.Checker();
            tmpArray[i] = new vap(tmpVap);
            yield return null;
        }
        _prevLevels = tmpArray;
        yield break;
    }

    public void StartLevelFill(uint lowLvl_ , uint maxLvl_)
    {
        StartCoroutine(SecondLevelFiller(lowLvl_, maxLvl_));
    }

}