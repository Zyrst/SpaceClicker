using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Player : Character {

    
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
        base.TakeDamage(ds_, hitPoint_);
        
    }

    public override void Die()
    {
        base.Die();
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
    public override void SetExperience(uint level_)
    {
        base.SetExperience(level_);
        Global.Instance.UpdateExpBar();
    }

}
