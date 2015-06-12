using UnityEngine;
using System.Collections;

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
    }

    public enum GameType : int { Farm = 0, Quest = 1 };
    public GameType _gameType = GameType.Farm;

    public Player _player;
    public Prefabs _prefabs = new Prefabs();
    public Colors _colors = new Colors();

  

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

    void Update()
    {
        MouseController.Instance.Update();
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
}
