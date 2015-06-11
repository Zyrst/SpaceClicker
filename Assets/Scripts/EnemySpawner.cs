using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject _enemyPrefab;
    public Enemy _enemy;

	// Use this for initialization
	void Start () {
        _enemy = (GameObject.Instantiate(_enemyPrefab) as GameObject).GetComponent<Enemy>();
        _enemy.transform.position = transform.position;
        _enemy.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
