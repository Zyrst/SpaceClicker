using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemySpawner : MonoBehaviour {
    public class Triggers
    {
        private float dontCallNewWaveToOftenTrigger = 0f;
        public void newWave()
        {
            if (dontCallNewWaveToOftenTrigger > Time.time)
            {
                return;
            }
            dontCallNewWaveToOftenTrigger = Time.time + 0.1f;

            if (Global.Instance._player._miniBoss)
            {                
                foreach (var item in spawns)
                {
                    try
                    {
                        Destroy(((EnemySpawner)item)._enemy.gameObject);
                    }
                    catch (System.NullReferenceException) { }

                    ((EnemySpawner)item)._enemy = null;
                    Global.Instance.AllEnemiesDied();
                    LootCrate[] lootCrates = Object.FindObjectsOfType<LootCrate>();
                    foreach (var loot in lootCrates)
                    {
                        loot.UltimateDestroy();
                    }

                    _bossSpawn.Spawn();

                }
            }
            else
            {
                Global.Instance.AllEnemiesDied();
                LootCrate[] lootCrates = Object.FindObjectsOfType<LootCrate>();
                foreach (var item in lootCrates)
                {
                    item.UltimateDestroy();
                }
                int rnd = Random.Range(1, spawns.Count + 1);
                EnemySpawner._enemiesSpawn = rnd;
                //Debug.Log(EnemySpawner._enemiesSpawn);
                Spawn(rnd);
            }
            
        }
        public ArrayList spawns = new ArrayList();
        public EnemySpawner _bossSpawn;

        /// <summary>
        /// Spawns new enemies
        /// </summary>
        /// <param name="number_">Number of enemies</param>
        public void Spawn(int number_)
        {
            int count = -1;
            if (!Global.Instance._player._miniBoss)
            {
                foreach (var item in spawns)
                {
               
                    count++;
                    if (count >= number_)
                    {
                        try
                        {
                            Destroy(((EnemySpawner)item)._enemy.gameObject);
                        }
                        catch (System.NullReferenceException) { }

                        ((EnemySpawner)item)._enemy = null;
                    }
                    else
                        ((EnemySpawner)item).Spawn();
                }
	        }
        }
    }

    public static Triggers triggers = new Triggers();
    
    public Enemy _enemy = null;
    public static int _enemiesSpawn = 3;
    public bool _isBossSpawn = false;
    
    public bool EnemyIsActive()
    {
        return _enemy != null && _enemy._isAlive;
    }

	// Use this for initialization
	void Start () {
        if(gameObject.name == "BossSpawn")
        {
            _isBossSpawn = true;
            triggers._bossSpawn = this;
            triggers.newWave();
        }
        else
        {
            triggers.spawns.Add(this);

            if (transform.parent.GetComponentsInChildren<EnemySpawner>().Count()-1 == triggers.spawns.Count)
            {
            if (!Global.Instance._player._miniBoss)
                triggers.newWave();
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            triggers.newWave();
        }
	}

    public void Spawn()
    {
        if(!IsInvoking("spawner"))
            Invoke("spawner", 0.5f);
    }

    private void spawner()
    {
        if (_enemy != null)
        {
            GameObject.Destroy(_enemy.gameObject);
        }
        int rnd = Random.Range(0, Global.Instance._enemies._currentEnemies.Length);
        _enemy = (GameObject.Instantiate(Global.Instance._enemies._currentEnemies[rnd]) as GameObject).GetComponent<Enemy>();
        _enemy.transform.position = transform.position;
        _enemy.transform.parent = transform;
    }

    public void ResetWave()
    {
        try
        {
            if (_enemy != null)
            {
                if (_enemy.IsInvoking("Kill"))
                    _enemy.CancelInvoke("Kill");
                _enemy.gameObject.SetActive(true);
                _enemy._stats._health = new vap(_enemy._stats._maxHealth);
                _enemy._isAlive = true;
                try
                {
                    _enemy.gameObject.GetComponentsInChildren<CharacterGUI>(true)[0].gameObject.SetActive(true);
                    _enemy.gameObject.GetComponentsInChildren<Animator>(true)[0].SetTrigger("IdleTrigger");
                } catch (System.IndexOutOfRangeException) {
                }
            }
        }
        catch (System.NullReferenceException) { }
    }

    public static void Reset()
    {
        triggers.spawns.Clear();
        triggers.spawns = new ArrayList();
    }
}
