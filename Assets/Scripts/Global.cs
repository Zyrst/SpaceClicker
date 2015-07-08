using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Global : MonoBehaviour {
    [System.Serializable]
    public class Prefabs
    {
        public GameObject Number;
        public GameObject InventorySlot;
        public GameObject[] _enemyPrefab;
        public GameObject GoldCoin;
        public GameObject HealthPotion;
    }

    [System.Serializable]
    public class Colors
    {
        public Color normalAttackColor;
        public Color techAttackColor;
        public Color kineticAttackColor;
        public Color psychicAttackColor;
        public Color healColor;

        public Color green;
        public Color blue;
        public Color purple;
        public Color orange;
    }
    [System.Serializable]
    public class Enemies
    {
        public GameObject[] _postApc;
        public GameObject[] _mech;
        public GameObject[] _currentEnemies;
    }
    [System.Serializable]
    public class Sprites
    {
        public Sprite equipment;
    }

    public enum GameType : int { Farm = 0, Quest = 1 , Ship = 3, Star = 4 }
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
    public Prefabs _prefabs = new Prefabs();
    public Enemies _enemies = new Enemies();
    public Colors _colors = new Colors();
    public Sprites _sprites = new Sprites();
    public float _expVariable = 10f;
    public float _expScale = 1.5f;
    public GameObject _playerGUI;
    public Planet _planet;

    public float _damageScale = 1.2f;
    public float _healthScale = 1.5f;


    public TalentStats.Percent _potionDropChans = new TalentStats.Percent(1.05f);
    public TalentStats.Percent _potionHealthPercent = new TalentStats.Percent(1.2f);

    public Camera _gameCamera;
    public Camera _uiCamera;

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
    }

    void Start()
    {
        Profiler.maxNumberOfSamplesPerFrame = 50000;
        UpdateLevel();
        UpdateExpBar();
        SwitchScene(GameType.Ship);
        Starmap.Instance.Generate(1, 100, 9001);
    }
    void Update()
    {
        MouseController.Instance.Update();

        if (Input.GetKey(KeyCode.G))
        {
            _player.SetExperience((uint)(_player._experianceToNext * 0.1f));
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
    }

    void LateUpdate()
    {
        MouseController.Instance.LateUpdate();
    }
    public void SwitchScene(GameType gt_)
    {
        EnemySpawner.Reset();
        switch (gt_)
        {
            case GameType.Farm:
                _player.gameObject.SetActive(true);
                FarmMode.Instance.gameObject.SetActive(true);
                Ship.Instance.gameObject.SetActive(false);
                Starmap.Instance.gameObject.SetActive(false);
                FarmMode.Instance.startFarmMode();
                _gameCamera.gameObject.SetActive(true);
                _uiCamera.gameObject.SetActive(false);
                break;
            case GameType.Quest:
                break;
            case GameType.Ship:
                _player.gameObject.SetActive(false);
                FarmMode.Instance.gameObject.SetActive(false);
                Ship.Instance.gameObject.SetActive(true);
                Starmap.Instance.gameObject.SetActive(false);
                _gameCamera.gameObject.SetActive(false);
                _uiCamera.gameObject.SetActive(true);

                foreach (var item in _player.gameObject.GetComponentsInChildren<SpellAttack>(true))
                {
                    if (item.IsInvoking())
                        item.CancelInvoke();

                    item.ResetCooldown();
                }
                break;
            case GameType.Star:
                _player.gameObject.SetActive(false);
                FarmMode.Instance.gameObject.SetActive(false);
                Ship.Instance.gameObject.SetActive(false);
                Starmap.Instance.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// returns if the player is alive
    /// </summary>
    /// <returns></returns>
    public bool PlayerAlive()
    {
        return _player._isAlive;
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
            int level = Random.Range((int)_player._level - 1, (int)_player._level+2);
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
        float value = (float)(_player._experience) /(float)( _player._experianceToNext);
        _playerGUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Exp").transform.localScale = new Vector3(value, 1f, 1f);
        _playerGUI.transform.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "ExpText").text = _player._experience.ToString() + "/" + _player._experianceToNext.ToString();
    }

    public void UpdateLevel()
    {
        _playerGUI.transform.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Level").text = _player._level.ToString();
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
}
