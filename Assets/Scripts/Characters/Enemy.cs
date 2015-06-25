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
        
        _stats._baseStat._values[0] = (_stats._constMultiplier*_level + ( Mathf.Pow(_stats._basePower,(_level/_stats._powerDiv)))) * _stats._valueMultiplier;
        _stats._baseStat.Checker();
        _stats._baseStat = _stats._baseStat * (1f / ((EnemySpawner._enemiesSpawn / 2f) + 0.5f));
        _stats._baseStat.Checker();

        _stats._maxHealth = (_stats._baseStat * _stats._multiplierHealth) * _stats._healthStatDist;
        _stats._health = new vap(_stats._maxHealth);
        _stats._normal.damage = (_stats._baseStat * _stats._multiplierDamage) * _stats._damageStatDist;

        Transform tr = transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Model");
        tr.LookAt(Global.Instance._player.transform);
        Quaternion rot = transform.rotation;
        //rot.z = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = Vector3.up;

        transform.Rotate(rot * 0f * Time.deltaTime);

        GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "HitCooldown").transform.localScale = new Vector3(GetComponent<EnemyAttack>()._attackTimer / GetComponent<EnemyAttack>()._cooldownTimer, 1, 1);
	}

    public override void Die()
    {
        // effekter och skits
        base.Die();

        // spawn goldcoin
        Vector3 dir = Vector3.up + -Camera.main.transform.forward;
        GoldCoin.Create(transform.position, dir * 200f).GetComponent<GoldCoin>()._value = _level;
        float rnd = Random.Range(0f, 1f);
        if(rnd >= 0.9f)
            HealthPotion.Create(transform.position, dir * 200f);

        Debug.Log("EnemisAlive: " + Global.Instance.EnemiesAlive());
        if (Global.Instance.EnemiesAlive() == 0)
        {
            EnemySpawner.triggers.newWave();
        }

        gameObject.SetActive(false);
    }
}
