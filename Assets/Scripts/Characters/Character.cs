﻿using UnityEngine;
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
    public GameObject _healEffect;

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
        vap healthDamagePercent = new vap();

        if (ds_._healPercent > 0f)
            ds_._heal += _stats._maxHealth * (ds_._healPercent * 0.01f);

        if (ds_._healthDamagePercent > 0f)
        {
            totalDamage += hitter_.maxHealth * ds_._healthDamagePercent;
            healthDamagePercent = hitter_.maxHealth * ds_._healthDamagePercent;
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

        SpawnText(normal, tech, psychic, kinetic, ds_._heal, healthDamagePercent, hitPoint_);

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
            killer_.SetExperience(_level, this);
        }
        Die();
    }

    public virtual void Die()
    {
        _isAlive = false;
    }

    public virtual void SetExperience(uint level_, Character killer_)
    {
        if (this as Player == null)
        {
            return;
        }

        uint exp = 0;
        exp = (uint)(level_ / _level * Global.Instance._expVariable) + level_;
        RectTransform playerTrans = Global.Instance._playerGUI.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();

        int rr = Mathf.CeilToInt(exp / 10);
        for (int i = 0; i < rr+1; i++)
        {
            GameObject go = GameObject.Instantiate(Global.Instance._prefabs.ExpBall);

            uint xp = (exp > 10 ? 10 : exp);
            exp -= 10;

            go.GetComponent<ExpBall>()._exp = xp;

            go.GetComponent<RectTransform>().SetParent(playerTrans);
            go.transform.position = Global.Instance._gameCamera.WorldToScreenPoint(killer_.transform.position);
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

    public void SpawnText(vap normal_, vap tech_, vap psychic_, vap kinetic_, vap heal_, vap healthDamagePercent_, Vector3 hitPoint_)
    {
        #region Old
        /*if (normal_.GetFloat() > 0)
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
        */
        #endregion
        vap total = normal_ + tech_ + kinetic_ + psychic_;
        if (total.GetFloat() > 0f)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = total.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(2f, 3f), Random.Range(-1f, 1f));
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);

            if (normal_ > tech_ && normal_ > psychic_ && normal_ > kinetic_)
            {
                _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.normalAttackColor;
            }
            else if (tech_ > normal_ && tech_ > psychic_ && tech_ > kinetic_)
            {
                _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.techAttackColor;
            }
            else if (kinetic_ > normal_ && kinetic_ > tech_ && kinetic_ > psychic_)
            {
                _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.kineticAttackColor;
            }
            else if (psychic_ > normal_ && psychic_ > tech_ && psychic_ > kinetic_)
            {
                _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.psychicAttackColor;
            }
            else
            {
                _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.normalAttackColor;
            }

            

        }
        if (heal_.GetFloat() > 0)
        {
            //Numbers
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = heal_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.healColor;
            //Particle effect
            _healEffect = GameObject.Instantiate(Global.Instance._prefabs._effects[0]);
            _healEffect.transform.position = gameObject.transform.position;
            _healEffect.transform.parent = gameObject.transform;
            _healEffect.GetComponent<ParticleSystem>().Play(true);
        }

        if (healthDamagePercent_.GetFloat() > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = healthDamagePercent_.GetString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.orange;
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
        SpawnText(tmp, tmp, tmp, tmp, lifeSteal_, tmp, transform.position);
    }

    public void CheckEffect()
    {
        if (_healEffect != null)
        {
            //Debug.Log(_healEffect.GetComponent<ParticleSystem>().isPlaying);
            if (!_healEffect.GetComponent<ParticleSystem>().isPlaying)
            {
                Destroy(_healEffect);
                _healEffect = null;
            }
        }
    }
}
