using UnityEngine;
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

    void Update()
    {
    }

    public void startFarmMode()
    {
        _arena = (GameObject.Instantiate(arenaPrefab) as GameObject);
        Global.Instance.player.transform.position = _arena.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "PlayerSpawnPoint").position;
    }

    public void allEnemiesDied()
    {
        Global.Instance.player._stats._health = new vap(Global.Instance.player._combinedStats._maxHealth);

    }

    public void playerDied()
    {
        // reset enemies
        foreach (var item in EnemySpawner.triggers.spawns)
        {
            ((EnemySpawner)item).ResetWave();
        }

        // reset player
        Global.Instance.player.Reset(2f);
    }

    public void backToShip()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Global.Instance.SwitchScene(Global.GameType.Ship);
        Destroy(_arena);
    }

    public ArrayList GetAllEnemies()
    {
        ArrayList ret = new ArrayList();
        foreach (var item in _arena.GetComponentsInChildren<EnemySpawner>())
        {
            if (item.EnemyIsActive())
            {
                ret.Add(item._enemy);
            }
        }

        return ret;
    }
}
