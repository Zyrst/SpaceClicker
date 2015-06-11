using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Enemy : MonoBehaviour {
    public CharacterStats _stats;

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

    void OnTriggerEnter(Collider c_)
    {
        Debug.Log("träffade trädet");

    }

    public void TakeDamage(DamageStats ds_, Vector3 hitPoint_)
    {
        // resistance och skit

        float normal = ds_._normal * (1f - _stats._normal.resistance);
        float tech = ds_._tech * (1f - _stats._tech.resistance);
        float psychic = ds_._psychic * (1f - _stats._psychic.resistance);
        float kinetic = ds_._kinetic * ( 1f - _stats._kinetic.resistance);

       float totalDamage = normal + tech + psychic + kinetic;

       _stats._health -= totalDamage;

       SpawnText(normal, tech, psychic, kinetic, hitPoint_);

        if (_stats._health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        // effekter och skit

        gameObject.SetActive(false);
        EnemySpawner.triggers.enemyCounter--;
    }

    public void SpawnText(float normal_, float tech_, float psychic_, float kinetic_, Vector3 hitPoint_)
    {
        

        if (normal_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = normal_.ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.normalAttackColor;
        }

        if (tech_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = tech_.ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.techAttackColor;
        }

        if (psychic_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = psychic_.ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.psychicAttackColor;
        }

        if (kinetic_ > 0)
        {
            GameObject _numbers = GameObject.Instantiate(Global.Instance._prefabs.Number);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().text = kinetic_.ToString();
            _numbers.transform.position = hitPoint_;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            direction.Normalize();
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().GetComponent<Rigidbody>().AddForce(direction * 1f);
            _numbers.GetComponentsInChildren<Text>().FirstOrDefault().color = Global.Instance._colors.kineticAttackColor;
        }
    }
}
