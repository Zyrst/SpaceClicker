using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Character : MonoBehaviour {
    
    public CharacterStats _stats = new CharacterStats();
    public bool _isAlive = true;
    public uint _level = 0;
    public uint _experience = 1;
    public uint _experianceToNext = 100;
    
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
        float normal = ds_._normal * (1f - _stats._normal.resistance);
        float tech = ds_._tech * (1f - _stats._tech.resistance);
        float psychic = ds_._psychic * (1f - _stats._psychic.resistance);
        float kinetic = ds_._kinetic * (1f - _stats._kinetic.resistance);

        float totalDamage = normal + tech + psychic + kinetic;

        _stats._health -= totalDamage;
        _stats._health += ds_._heal;

        //If heal make sure we don't go over maxhealth
        if (_stats._health > _stats._maxHealth)
            _stats._health = _stats._maxHealth;

        SpawnText(normal, tech, psychic, kinetic, ds_._heal, hitPoint_);

        if (_stats._health < 1f)
        {
            _stats._health = 0f;
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
        }
    }

    public void LevelUp()
    {
        _experience = 1;
       // _experianceToNext += (uint)((float)(_experianceToNext) * Global.Instance._expScale);
        _level++;
        _stats.LevelUp(_level);
        _experianceToNext = (uint)(50 + (_level * (5 * _level)));
        Global.Instance.UpdateLevel();
    }

    public void SpawnText(float normal_, float tech_, float psychic_, float kinetic_, float heal_, Vector3 hitPoint_)
    {
        if (normal_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = ((int)(normal_)).ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.normalAttackColor;
        }

        if (tech_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = ((int)(tech_)).ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.techAttackColor;
        }

        if (psychic_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = ((int)(psychic_)).ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.psychicAttackColor;
        }

        if (kinetic_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = ((int)(kinetic_)).ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.kineticAttackColor;
        }

        if (heal_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = ((int)(heal_)).ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.healColor;
        }
    }
}
