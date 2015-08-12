using UnityEngine;
using System.Collections;
using System.Linq;

public class FarmMode : MonoBehaviour
{
    public GameObject arenaPrefab;
    public GameObject _arena;
    public GameObject _bossSpawn;
    public UIDeath _uiDeath;

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
        _arena.GetComponent<MeshRenderer>().material.mainTexture = TextureTracker.Instance._arena._postAp[Random.Range(0, TextureTracker.Instance._arena._postAp.Length)];
        Global.Instance.player.transform.position = _arena.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "PlayerSpawnPoint").position;

        _uiDeath.gameObject.SetActive(false);
    }

    public void allEnemiesDied()
    {
        Global.Instance.player._stats._health = new vap(Global.Instance.player._combinedStats._maxHealth);
    }

    public void playerDied()
    {
        // reset enemies
        Sounds.OneShot(Sounds.Instance.music.gameOver);
        if (Global.Instance.player._miniBoss)
        {
            _bossSpawn.GetComponent<EnemySpawner>().ResetWave();
        }
        else
        {
            foreach (var item in EnemySpawner.triggers.spawns)
            {
                ((EnemySpawner)item).ResetWave();
            }
        }

        // reset player
        Global.Instance.player.Reset(2f);
        _uiDeath.gameObject.SetActive(true);
        _uiDeath.Died(2f);
    }

    public void backToShip()
    {

        if (!Global.Instance.PlayerAlive())
        {
            Global.Instance.player.Reset(0f);
            //Let it reset , animation will not glitch the fuck out
            StartCoroutine(WaitForPlayerReset());

        }
        else
        {
            Sounds.OneShot(Sounds.Instance.uiSounds.Button);
            Global.Instance.SwitchScene(Global.GameType.Ship);
            foreach (var item in EnemySpawner.triggers.spawns)
            {
                ((EnemySpawner)item)._enemy.KillIt();
            }
            Destroy(_arena);
        }

        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.exitBattle);
    }

    IEnumerator WaitForPlayerReset()
    {
        yield return new WaitForSeconds(0.1f);
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Global.Instance.SwitchScene(Global.GameType.Ship);
        foreach (var item in EnemySpawner.triggers.spawns)
        {
            ((EnemySpawner)item)._enemy.KillIt();
        }
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
