using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Enemy : Character
{
    public bool _shieldUp = false;
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
	}

    public override void Die()
    {
        try { 
            GetComponentInChildren<Animator>().SetTrigger("DeadTrigger"); 
        }
        catch (System.NullReferenceException) { }
        // effekter och skits
        base.Die();

        // spawn goldcoin
        Vector3 dir = (Vector3.up * 10f) + -(transform.position - Global.Instance._player.transform.position);
        GoldCoin.Create(transform.position, dir * 20f).GetComponent<GoldCoin>()._value = _level;
        float rnd = Random.Range(0f, 1f);
        if(rnd >= 0.9f)
            HealthPotion.Create(transform.position + (Vector3.up * 2f), dir * 20f);

        Debug.Log("EnemisAlive: " + Global.Instance.EnemiesAlive());

        Invoke("Kill", 3f);
        GetComponentInChildren<CharacterGUI>().gameObject.SetActive(false);
    }

    public void Kill()
    {
        gameObject.SetActive(false);

        if (Global.Instance.EnemiesAlive() == 0)
        {
            EnemySpawner.triggers.newWave();
        }
    }

    public override void TakeDamage(DamageStats ds_, Vector3 hitPoint_, Character hitter_)
    {
        if (_isAlive)
        {
            if (!_shieldUp)
            {
                try
                {
                    GetComponentInChildren<Animator>().SetTrigger("HitTrigger");
                }

                catch (System.NullReferenceException) { }
                base.TakeDamage(ds_, hitPoint_, hitter_);
                if (ds_._stunTime > 0f)
                {
                    GetComponent<EnemyAttack>().Stunned(ds_._stunTime);
                }
            }
            else
            {
                CharacterStats _shieldStats = new CharacterStats(_stats);
                vap _shieldVap = _shieldStats._normal.damage + _shieldStats._tech.damage + _shieldStats._kinetic.damage + _shieldStats._psychic.damage;
                _shieldVap *= GetComponent<EnemyAttack>()._shieldDamageMulti;

                // hitta vilken typ av sköld det är

                _shieldStats = new CharacterStats();
                _shieldStats._tech.damage = _shieldVap;


                Global.Instance._player.TakeDamage(DamageStats.GenerateFromCharacterStats(_shieldStats, false), gameObject.GetComponent<Enemy>());
                GetComponent<EnemyAttack>().ResetShield();
                if (GetComponent<EnemyAttack>().IsInvoking())
                {
                    GetComponent<EnemyAttack>().CancelInvoke();
                }
            }
        }
    }
}
