using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public class Triggers
    {
        private uint _enemyCounter = 0;
        public uint enemyCounter
        {
            get { return _enemyCounter; }
            set
            {
                _enemyCounter = value;

                if (_enemyCounter == 0)
                {
                    Global.Instance.AllEnemiesDied();
                    Spawn();
                }
            }
        }
        public ArrayList spawns = new ArrayList();

        public void Spawn()
        {
            foreach (var item in spawns)
	        {
                ((EnemySpawner)item).Spawn();
	        }
        }
    }

    public static Triggers triggers = new Triggers();

    public GameObject _enemyPrefab;
    public Enemy _enemy = null;

	// Use this for initialization
	void Start () {
        triggers.spawns.Add(this);

        _enemy = (GameObject.Instantiate(_enemyPrefab) as GameObject).GetComponent<Enemy>();
        _enemy.transform.position = transform.position;
        _enemy.transform.parent = transform;
        _enemy.afterSpawn();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Spawn()
    {
        Invoke("spawner", 1f);
    }

    private void spawner()
    {
        _enemy = null;
        _enemy = (GameObject.Instantiate(_enemyPrefab) as GameObject).GetComponent<Enemy>();
        _enemy.transform.position = transform.position;
        _enemy.transform.parent = transform;
        _enemy.afterSpawn();
    }

    public void ResetWave()
    {
        if (!_enemy.gameObject.activeInHierarchy)
        {
            triggers.enemyCounter++;
        }
        _enemy.gameObject.SetActive(true);
        _enemy._stats._health = _enemy._stats._maxHealth;
    }
}
