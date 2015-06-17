using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Player : Character {
    [System.Serializable]
    public class EquipmentOnPlayer
    {
        public Weapon _weapon = null;
        public Head _head = null;
        public Chest _chest = null;
        public Legs _legs = null;

        public GameObject _weaponSlot;
        public GameObject _headSlot;
        public GameObject _chestSlot;
        public GameObject _legsSlot;

        public void Equip(Equipment equi_)
        {
            switch (equi_._type)
            {
                case Equipment.EquipmentType.Weapon:
                   
                    for (int i = 0; i < Global.Instance._player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance._player._inventoryArray[i] == equi_.gameObject)
                        {
                            //Change to equipped 
                            Global.Instance._player._inventoryArray[i].gameObject.transform.parent = Global.Instance._player._equipmentObject.transform;
                            //Set old to inventoy
                            if (_weapon == null)
	                        {
                                Global.Instance._player._inventoryArray[i] = null;
	                        }
                            else
                            {
                                Global.Instance._player._inventoryArray[i] = _weapon.gameObject;
                                _weapon.transform.parent = Global.Instance._player._inventoryObject.transform;
                            }
                            _weaponSlot.GetComponent<Image>().sprite = equi_._sprite;
                            break;
                        }
                    }
                    _weapon = (Weapon)equi_;
                        break;
                case Equipment.EquipmentType.Head:
                        _head = (Head)equi_;
                        for (int i = 0; i < Global.Instance._player._inventoryArray.Length; i++)
                        {
                            if (Global.Instance._player._inventoryArray[i] == equi_.gameObject)
                            {
                                Global.Instance._player._inventoryArray[i] = null;
                                break;
                            }
                        }
                    break;
                case Equipment.EquipmentType.Chest:
                    _chest = (Chest)equi_;
                    for (int i = 0; i < Global.Instance._player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance._player._inventoryArray[i] == equi_.gameObject)
                        {
                            Global.Instance._player._inventoryArray[i] = null;
                            break;
                        }
                    }
                    break;
                case Equipment.EquipmentType.Legs:
                    _legs = (Legs)equi_;
                    for (int i = 0; i < Global.Instance._player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance._player._inventoryArray[i] == equi_.gameObject)
                        {
                            Global.Instance._player._inventoryArray[i] = null;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            CharacterScreen.Instance.RemoveInventorySlots();
            CharacterScreen.Instance.GenerateInventorySlots();
        }

    }
    
    public SpellAttack[] _spellsArray = new SpellAttack[4];
    public GameObject[] _spellSlotArray = new GameObject[4];
    public GameObject[] _inventoryArray = new GameObject[32];
    public EquipmentOnPlayer _equipped = new EquipmentOnPlayer();
    public GameObject _equipmentObject;
    public GameObject _inventoryObject;
   
	// Use this for initialization
	void Start () {
        _stats._health = _stats._maxHealth;
        LevelUp();
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
