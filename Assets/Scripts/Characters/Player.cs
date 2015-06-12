using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Player : Character {

    public bool _isAlive = true;

    public SpellAttack[] _spellsArray = new SpellAttack[4];
    public GameObject[] _spellSlotArray = new GameObject[4];

	// Use this for initialization
	void Start () {
        _stats._health = _stats._maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void TakeDamage(DamageStats ds_)
    {
        TakeDamage(ds_, transform.position + new Vector3(0, 4, 0));
    }

    public override void TakeDamage(DamageStats ds_, Vector3 hitPoint_)
    {
        float normal = ds_._normal * (1f - _stats._normal.resistance);
        float tech = ds_._tech * (1f - _stats._tech.resistance);
        float psychic = ds_._psychic * (1f - _stats._psychic.resistance);
        float kinetic = ds_._kinetic * (1f - _stats._kinetic.resistance);

        float totalDamage = normal + tech + psychic + kinetic;

        _stats._health -= totalDamage;

        SpawnText(normal, tech, psychic, kinetic, hitPoint_);

        if (_stats._health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        _isAlive = false;

        Global.Instance.PlayerDied();
        gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Cylinder").gameObject.SetActive(false);
    }

    public void Reset(float time_)
    {
        Invoke("ResetNow", time_);
    }

    public void ResetNow()
    {
        gameObject.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Cylinder").gameObject.SetActive(true);
        _stats._health = _stats._maxHealth;

        _isAlive = true;
    }

    public GameObject getSpellSlot(SpellAttack spell_)
    {
        for (int i = 0; i < _spellsArray.Length; i++)
        {
            if (_spellsArray[i] == spell_)
            {
                return _spellSlotArray[i];
            }
        }
        return null;
    }


    //public void SpawnText(float normal_, float tech_, float psychic_, float kinetic_, Vector3 hitPoint_)
    //{
    //    if (normal_ > 0)
    //    {
    //        GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = normal_.ToString();
    //        _numbers.transform.position = hitPoint_;
    //        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    //        direction.Normalize();
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.normalAttackColor;
    //    }

    //    if (tech_ > 0)
    //    {
    //        GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = tech_.ToString();
    //        _numbers.transform.position = hitPoint_;
    //        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    //        direction.Normalize();
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.techAttackColor;
    //    }

    //    if (psychic_ > 0)
    //    {
    //        GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = psychic_.ToString();
    //        _numbers.transform.position = hitPoint_;
    //        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    //        direction.Normalize();
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.psychicAttackColor;
    //    }

    //    if (kinetic_ > 0)
    //    {
    //        GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = kinetic_.ToString();
    //        _numbers.transform.position = hitPoint_;
    //        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    //        direction.Normalize();
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
    //        _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.kineticAttackColor;
    //    }
    //}
}
