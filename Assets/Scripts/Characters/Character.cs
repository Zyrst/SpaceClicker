using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Threading;

[System.Serializable]
public class Character : MonoBehaviour {
    
    public CharacterStats _stats = new CharacterStats();
    public bool _isAlive = true;
    public uint _level = 0;
    public uint _experience = 1;
    public uint _experianceToNext = 100;

    public virtual vap maxHealth
    {
        get
        {
            return _stats._maxHealth;
        }
    }
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void TakeDamage(DamageStats ds_)                     {   TakeDamage(ds_, transform.position, null);  }
    public virtual void TakeDamage(DamageStats ds_, Vector3 hitPoint_)  {   TakeDamage(ds_, hitPoint_, null);   }

    public virtual void TakeDamage(DamageStats ds_, Character hitter_) { TakeDamage(ds_, transform.position, hitter_); }
    public virtual void TakeDamage(DamageStats ds_, Vector3 hitPoint_, Character hitter_)
    {
        //Calculate damage with resistance from the characters stats
        vap normal = ds_._normal * (1f - _stats._normal.resistance);
        vap tech = ds_._tech * (1f - _stats._tech.resistance);
        vap psychic = ds_._psychic * (1f - _stats._psychic.resistance);
        vap kinetic = ds_._kinetic * (1f - _stats._kinetic.resistance);

        vap totalDamage = normal + tech + psychic + kinetic;

        if (ds_._healPercent > 0f)
            ds_._heal += _stats._maxHealth * (ds_._healPercent * 0.01f);

        if (ds_._healthDamagePercent > 0f)
        {
            totalDamage += hitter_.maxHealth * ds_._healthDamagePercent;
        }

        _stats._health -= totalDamage;
        _stats._health += ds_._heal;

        if (ds_._lifeSteal.GetFloat() > 0f)
        {
            hitter_.LifeSteal(ds_._lifeSteal);
        }

        if (ds_._lifeStealAmount > 0f)
        {
            hitter_.LifeSteal(totalDamage * ds_._lifeStealAmount);
        }

        //If heal make sure we don't go over maxhealth
        if (_stats._health > _stats._maxHealth)
        {
            _stats._health = new vap(_stats._maxHealth);
        }

        SpawnText(normal, tech, psychic, kinetic, ds_._heal, hitPoint_);

        if (_stats._health.GetFloat() < 1f)
        {
            _stats._health = new vap();
            Die(hitter_);
        }
    }


    public virtual void Die(Character killer_)
    {
        if (killer_ != null)
        {
            killer_.SetExperience(_level);
        }
        Die();
    }

    public virtual void Die()
    {
        _isAlive = false;
    }

    public virtual void SetExperience(uint level_)
    {
        uint exp = 0;
        exp = (uint)(level_ / _level * Global.Instance._expVariable) + level_;
        _experience += exp;
        if (_experience >= _experianceToNext)
        {
            LevelUp();
            if (this is Player)
            {
                ((Player)this)._talentPoints++;
            }
        }
    }

    public virtual void LevelUp()
    {
        _experience = 1;
       // _experianceToNext += (uint)((float)(_experianceToNext) * Global.Instance._expScale);
        _level++;
        _stats.LevelUp(_level);
        _experianceToNext = (uint)(50 + (_level * (5 * _level)));
        Global.Instance.UpdateLevel();
    }

    public void SpawnText(vap normal_, vap tech_, vap psychic_, vap kinetic_, vap heal_, Vector3 hitPoint_)
    {
        if (normal_.GetFloat() > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = normal_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.normalAttackColor;
        }

        if (tech_.GetFloat() > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = tech_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.techAttackColor;
        }

        if (psychic_.GetFloat() > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = psychic_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.psychicAttackColor;
        }

        if (kinetic_.GetFloat() > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = kinetic_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.kineticAttackColor;
        }

        if (heal_.GetFloat() > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = heal_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.healColor;
        }
    }

    public virtual void LifeSteal(vap lifeSteal_)
    {
        _stats._health += lifeSteal_;
        if (_stats._health > _stats._maxHealth)
        {
            _stats._health = _stats._maxHealth;
        }

        vap tmp = new vap();
        SpawnText(tmp, tmp, tmp, tmp, lifeSteal_, transform.position);
    }

}
