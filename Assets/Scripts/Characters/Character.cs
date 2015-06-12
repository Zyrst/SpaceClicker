using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Character : MonoBehaviour {

    public CharacterStats _stats = new CharacterStats();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void TakeDamage(DamageStats ds_)
    {
        TakeDamage(ds_, transform.position);
    }
    public virtual void TakeDamage(DamageStats ds_, Vector3 hitPoint_)
    {

    }

    public virtual void Die()
    {
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
