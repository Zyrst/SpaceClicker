﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
        public Sprite equipment_legs;
        public Sprite equipment_chest;
        public Sprite equipment_head;
        public Sprite equipment_weapon;

        public Sprite spell_attack;
        public Sprite spell_heal;
        public Sprite spell_stun;
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

        UpdateBebugMessages();
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

    /// <summary>
    /// displays message on screen for 5 seconds
    /// </summary>
    /// <param name="message_"></param>
    public static void DebugOnScreen(string message_)
    {
        DebugMessage newMess = new DebugMessage();
        newMess.message = message_;
        newMess.time = 5f;

        _debugMessages.Add(newMess);
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
}
