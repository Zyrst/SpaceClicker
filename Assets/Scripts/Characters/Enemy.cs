using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Enemy : Character
{
	// Use this for initialization
    void Start()
    {
        _level = Global.Instance.GetEnemyLevel();
        //_stats.LevelUp(_level);
        
        _stats._baseStat = (_stats._constMultiplier*_level + ( Mathf.Pow(_stats._basePower,(_level/_stats._powerDiv)))) * _stats._valueMultiplier;
        Debug.Log("BaseStat Enemy Before: " + _stats._baseStat + " enemies: " + EnemySpawner._enemiesSpawn);

        _stats._baseStat = _stats._baseStat * (1f / ((EnemySpawner._enemiesSpawn / 2f) + 0.5f));

        Debug.Log("BaseStat Enemy After: " + _stats._baseStat);
        _stats._maxHealth = (_stats._baseStat * _stats._multiplierHealth) * _stats._healthStatDist;
        _stats._health = _stats._maxHealth;
        _stats._normal.damage = (_stats._baseStat * _stats._multiplierDamage) * _stats._damageStatDist;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = Vector3.up;

        transform.Rotate(rot * 0f * Time.deltaTime);
	}

    public override void TakeDamage(DamageStats ds_, Vector3 hitPoint_)
    {
        base.TakeDamage(ds_, hitPoint_);
    }

    public override void Die()
    {
        // effekter och skits
        base.Die();

        // spawn goldcoin
        Vector3 dir = Vector3.up + -Camera.main.transform.forward;
        GoldCoin.Create(transform.position, dir * 200f).GetComponent<GoldCoin>()._value = _level;

        Debug.Log("EnemisAlive: " + Global.Instance.EnemiesAlive());
        if (Global.Instance.EnemiesAlive() == 0)
        {
            EnemySpawner.triggers.newWave();
        }

        gameObject.SetActive(false);
    }
}
