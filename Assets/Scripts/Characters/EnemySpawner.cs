using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public class Triggers
    {
        public void newWave()
        {
            Global.Instance.AllEnemiesDied();
            int rnd = Random.Range(1, spawns.Count + 1);
            EnemySpawner._enemiesSpawn = rnd;
           // Debug.Log(EnemySpawner._enemiesSpawn);
            Spawn(rnd);
        }
        public ArrayList spawns = new ArrayList();

        /// <summary>
        /// Spawns new enemies
        /// </summary>
        /// <param name="number_">Number of enemies</param>
        public void Spawn(int number_)
        {
            int count = -1;
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

    public static Triggers triggers = new Triggers();
    
    public Enemy _enemy = null;
    public static int _enemiesSpawn = 3;
    
    public bool EnemyIsActive()
    {
        return _enemy != null && _enemy._isAlive;
    }

	// Use this for initialization
	void Start () {
        triggers.spawns.Add(this);
        triggers.newWave();
	}
	
	// Update is called once per frame
	void Update () {
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
                _enemy.gameObject.SetActive(true);
                _enemy._stats._health = new vap(_enemy._stats._maxHealth);
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
