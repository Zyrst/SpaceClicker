﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class FarmMode : MonoBehaviour {
    public GameObject arenaPrefab;
    public GameObject _arena;

    private static FarmMode _instance = null;
    public static FarmMode Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("FARMMODE").GetComponent<FarmMode>();
            }
            return _instance;
        }
    }

    public void startFarmMode()
    {
        _arena = (GameObject.Instantiate(arenaPrefab) as GameObject);
        Global.Instance._player.transform.position = _arena.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "PlayerSpawnPoint").position;
    }

    public void allEnemiesDied()
    {
        Debug.Log("All enemies died, plz giff health");
        Global.Instance._player._stats._health = Global.Instance._player._stats._maxHealth;

    }

    public void playerDied()
    {
        // reset enemies
        foreach (var item in EnemySpawner.triggers.spawns)
        {
            ((EnemySpawner)item).ResetWave();
        }

        // reset player
        Global.Instance._player.Reset(2f);
    }

    public void backToShip()
    {
        Global.Instance.SwitchScene(Global.GameType.Ship);
        Destroy(_arena);
    }
}