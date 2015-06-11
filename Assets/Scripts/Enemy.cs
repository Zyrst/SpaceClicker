using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public CharacterStats _stats;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c_)
    {
        Debug.Log("träffade trädet");

    }

    public void TakeDamage(DamageStats ds_)
    {
        // resistance och skit

        float normal = ds_._normal * (1f - _stats._normal.resistance);
        float tech = ds_._tech * (1f - _stats._tech.resistance);
        float psychic = ds_._psychic * (1f - _stats._psychic.resistance);
        float kinetic = ds_._kinetic * ( 1f - _stats._kinetic.resistance);

       

        _stats._health -= normal + tech + psychic + kinetic;
        if (_stats._health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        // effekter och skit

        gameObject.SetActive(false);
    }
}
