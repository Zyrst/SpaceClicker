﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Global : MonoBehaviour {
    [System.Serializable]
    public class Prefabs
    {
        public GameObject Number;
    }

    [System.Serializable]
    public class Colors
    {
        public Color normalAttackColor;
        public Color techAttackColor;
        public Color kineticAttackColor;
        public Color psychicAttackColor;
        public Color healColor;
    }

    public enum GameType : int { Farm = 0, Quest = 1 };
    public GameType _gameType = GameType.Farm;

    public Player _player;
    public Prefabs _prefabs = new Prefabs();
    public Colors _colors = new Colors();
    public float _expVariable = 10f;
    public GameObject _playerGUI;
    public Planet _planet;
  

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
        UpdateLevel();
        UpdateExpBar();
    }
    void Update()
    {
        MouseController.Instance.Update();

        if (Input.GetKeyDown(KeyCode.G))
        {
            _player.SetExperience(10);
        }
    }

    void LateUpdate()
    {
        MouseController.Instance.LateUpdate();
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
        Debug.Log("Enter AllEnemiesDied in Global");
        switch (_gameType)
        {
            case GameType.Farm:
                FarmMode.Instance.allEnemiesDied();
                Debug.Log("Calling allenemiesdied in global");
                break;
            case GameType.Quest:
                break;
            default:
                break;
        }
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
            int level = Random.Range((int)_player._level - 1, (int)_player._level+1);
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
        Debug.Log("Value for exp bar : " + value);
        _playerGUI.transform.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Exp").transform.localScale = new Vector3(value, 1f, 1f);
    }

    public void UpdateLevel()
    {
        _playerGUI.transform.GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Level").text = _player._level.ToString();
    }
}
