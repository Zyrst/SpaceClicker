using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Enemy : Character
{
	// Use this for initialization
    void Start()
    {
        _stats._health = _stats._maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = Vector3.up;

        transform.Rotate(rot * 10f * Time.deltaTime);
	}

    public void afterSpawn()
    {
        EnemySpawner.triggers.enemyCounter++;
    }

    public override void TakeDamage(DamageStats ds_, Vector3 hitPoint_)
    {
        base.TakeDamage(ds_, hitPoint_);
    }

    public override void Die()
    {
        // effekter och skit
        base.Die();
        gameObject.SetActive(false);
        EnemySpawner.triggers.enemyCounter--;
    }
}
