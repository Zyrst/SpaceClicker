﻿using UnityEngine;
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
            Global.Instance._player.UpdateCombinedStats();
        }

    }
    
    public SpellAttack[] _spellsArray = new SpellAttack[4];
    public GameObject[] _spellSlotArray = new GameObject[4];

    public GameObject _inventoryObject;
    public GameObject[] _inventoryArray = new GameObject[32];

    public EquipmentOnPlayer _equipped = new EquipmentOnPlayer();
    public GameObject _equipmentObject;

    public CharacterStats _combinedStats = new CharacterStats();

    public uint _unspentLevels = 0;

	// Use this for initialization
	void Start () {
        LevelUp();
        UpdateCombinedStats();
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

    public override void TakeDamage(DamageStats ds_, Vector3 hitPoint_, Character hitter_)
    {
        //Calculate damage with resistance from the characters stats
        vap normal = ds_._normal * (1f - _combinedStats._normal.resistance);
        vap tech = ds_._tech * (1f - _combinedStats._tech.resistance);
        vap psychic = ds_._psychic * (1f - _combinedStats._psychic.resistance);
        vap kinetic = ds_._kinetic * (1f - _combinedStats._kinetic.resistance);

        vap totalDamage = normal + tech + psychic + kinetic;

        _stats._health -= totalDamage;
        _stats._health += ds_._heal;

        //If heal make sure we don't go over maxhealth
        if (_stats._health > _combinedStats._maxHealth)
            _stats._health = new vap(_combinedStats._maxHealth);
        if (ds_._stunTime > 0f)
        {
            GetComponent<ClickAttack>().Stunned(ds_._stunTime);
            foreach (var item in _spellsArray)
            {
                item.Stunned(ds_._stunTime);
            }
        }

        SpawnText(normal, tech, psychic, kinetic, ds_._heal, hitPoint_);
        
        if (_stats._health._values[0] < 1f)
        {
            _stats._health = new vap();
            Die(hitter_);
        }
    }

    /// <summary>
    /// Player is dead
    /// </summary>
    public override void Die()
    {
        base.Die();
        Global.Instance.PlayerDied();
        gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Model").gameObject.SetActive(false);
    }

    public override void LevelUp()
    {
        base.LevelUp();
        UpdateCombinedStats();
    }
    public void Reset(float time_)
    {
        Invoke("ResetNow", time_);
    }

    public void ResetNow()
    {
        gameObject.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Model").gameObject.SetActive(true);
        _stats._health = new vap(_stats._maxHealth);

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

    public void SortInventory()
    {        
        for (int i = 0; i < _inventoryArray.Length; i++)
        {
            if (_inventoryArray[i] == null)
            {
                for (int j = i; j < _inventoryArray.Length; j++)
                {
                    if (_inventoryArray[j] != null)
                    {
                        _inventoryArray[i] = _inventoryArray[j];
                        _inventoryArray[j] = null;
                    }
                }
            }
            
        }
    }

    /// <summary>
    /// Update stats of character with gear and base stats
    /// </summary>
    public void UpdateCombinedStats()
    {
        _combinedStats = new CharacterStats(_stats);
        if (_equipped._chest != null)
            _combinedStats.AddStats(_equipped._chest._stats);
        if (_equipped._head != null)
            _combinedStats.AddStats(_equipped._head._stats);
        if (_equipped._legs != null)
            _combinedStats.AddStats(_equipped._legs._stats);
        if (_equipped._weapon != null)
            _combinedStats.AddStats(_equipped._weapon._stats);

        for (int i = 0; i < _spellsArray.Length; i++)
        {
            if(_spellsArray[i] != null)
                _spellsArray[i].CombineSpellStats();
        }

        _stats._health = new vap(_combinedStats._maxHealth);
    }

    public override void LifeSteal(vap lifeSteal_)
    {
        _stats._health += lifeSteal_;
        if (_stats._health > _combinedStats._maxHealth)
        {
            _stats._health = _combinedStats._maxHealth;
        }

        vap tmp = new vap();
        SpawnText(tmp, tmp, tmp, tmp, lifeSteal_, transform.position);
    }
}
