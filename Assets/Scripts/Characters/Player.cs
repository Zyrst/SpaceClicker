using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Threading;

[System.Serializable]
public class Player : Character {
    [System.Serializable]
    public class EquipmentOnPlayer
    {
        public Equipment _weapon = null;
        public Equipment _head = null;
        public Equipment _chest = null;
        public Equipment _legs = null;

        public GameObject _weaponSlot;
        public GameObject _headSlot;
        public GameObject _chestSlot;
        public GameObject _legsSlot;

        public void Equip(Equipment equi_)
        {
            switch (equi_._type)
            {
                case Equipment.EquipmentType.Weapon:
                    for (int i = 0; i < Global.Instance.player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance.player._inventoryArray[i] == equi_.gameObject)
                        {
                            //Change to equipped 
                            Global.Instance.player._inventoryArray[i].gameObject.transform.parent = Global.Instance.player._equipmentObject.transform;
                            //Set old to inventoy
                            if (_weapon == null)
	                        {
                                Global.Instance.player._inventoryArray[i] = null;
	                        }
                            else
                            {
                                Global.Instance.player._inventoryArray[i] = _weapon.gameObject;
                                _weapon.transform.parent = Global.Instance.player._inventoryObject.transform;
                            }
                            _weaponSlot.GetComponent<Image>().sprite = equi_._sprite.sprite;
                            break;
                        }
                    }
                    _weapon = equi_;
                    break;
                case Equipment.EquipmentType.Head:
                    for (int i = 0; i < Global.Instance.player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance.player._inventoryArray[i] == equi_.gameObject)
                        {
                            //Change to equipped 
                            Global.Instance.player._inventoryArray[i].gameObject.transform.parent = Global.Instance.player._equipmentObject.transform;
                            //Set old to inventoy
                            if (_head == null)
	                        {
                                Global.Instance.player._inventoryArray[i] = null;
	                        }
                            else
                            {
                                Global.Instance.player._inventoryArray[i] = _head.gameObject;
                                _head.transform.parent = Global.Instance.player._inventoryObject.transform;
                            }
                            _headSlot.GetComponent<Image>().sprite = equi_._sprite.sprite;
                            break;
                        }
                    }
                    _head = equi_;
                    break;
                case Equipment.EquipmentType.Chest:
                    for (int i = 0; i < Global.Instance.player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance.player._inventoryArray[i] == equi_.gameObject)
                        {
                            //Change to equipped 
                            Global.Instance.player._inventoryArray[i].gameObject.transform.parent = Global.Instance.player._equipmentObject.transform;
                            //Set old to inventoy
                            if (_chest == null)
                            {
                                Global.Instance.player._inventoryArray[i] = null;
                            }
                            else
                            {
                                Global.Instance.player._inventoryArray[i] = _chest.gameObject;
                                _chest.transform.parent = Global.Instance.player._inventoryObject.transform;
                            }
                            _chestSlot.GetComponent<Image>().sprite = equi_._sprite.sprite;
                            break;
                        }
                    }
                    _chest = equi_;
                    break;
                case Equipment.EquipmentType.Legs:
                    for (int i = 0; i < Global.Instance.player._inventoryArray.Length; i++)
                    {
                        if (Global.Instance.player._inventoryArray[i] == equi_.gameObject)
                        {
                            //Change to equipped 
                            Global.Instance.player._inventoryArray[i].gameObject.transform.parent = Global.Instance.player._equipmentObject.transform;
                            //Set old to inventoy
                            if (_legs == null)
                            {
                                Global.Instance.player._inventoryArray[i] = null;
                            }
                            else
                            {
                                Global.Instance.player._inventoryArray[i] = _legs.gameObject;
                                _legs.transform.parent = Global.Instance.player._inventoryObject.transform;
                            }
                            _legsSlot.GetComponent<Image>().sprite = equi_._sprite.sprite;
                            break;
                        }
                    }
                    _legs = equi_;
                    break;
                default:
                    break;
            }
            CharacterScreen.Instance.RemoveInventorySlots();
            try
            {
                CharacterScreen.Instance.GenerateInventorySlots();
            }
            catch (System.Exception) { Debug.Log("inte inne i char. screen"); }
            Global.Instance.player.UpdateCombinedStats();
        }

    }

    public GameObject GUIPrefab;
    
    public SpellAttack[] _spellsArray = new SpellAttack[4];
    public GameObject[] _spellSlotArray = new GameObject[4];

    public GameObject _inventoryObject;
    public GameObject[] _inventoryArray = new GameObject[32];

    public EquipmentOnPlayer _equipped = new EquipmentOnPlayer();
    public GameObject _equipmentObject;

    public CharacterStats _combinedStats = new CharacterStats();
    public TalentStats _talentStats = new TalentStats();

    public uint _talentPoints = 0;

    public bool _showHit = false;
    public float _showHitTimer = 0f;


    public bool _miniBoss = false;
    public static Player Instance = null;

    public string _levelUpMessage = "";

    public bool _isHoldingSpell = false;


    public override vap maxHealth
    {
        get
        {
            return _combinedStats._maxHealth;
        }
    }

    public class Shield
    {
        public bool isUp = false;
        public SpellAttack spellRef = null;
        private float timer;
        /// <summary>
        /// raises the shield for 'duration_' amount of time
        /// </summary>
        /// <param name="duration_">if <= 0, infinite duration</param>
        public void Raise(float duration_)
        {
            isUp = true;
            Global.Instance.player.GetComponentInChildren<CharacterGUI>().Shield.SetActive(true);
            timer = duration_;
            if (duration_ <= 0)
            {
                timer = float.NaN;
            }
        }
        public void Lower()
        {
            isUp = false;
            Global.Instance.player.GetComponentInChildren<CharacterGUI>().Shield.SetActive(false);
        }

        public void Update()
        {
            if (!float.IsNaN(timer))
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    Lower();
                }
            }
            
        }
    }
    public Shield _shield = new Shield();

    public Player()
    {
        Instance = this;
    }

    public Animator Animator
    {
        get
        {
            return GetComponentsInChildren<Animator>(true).FirstOrDefault();
        }
    }

	// Use this for initialization
	void Start () {
        Animator.SetTrigger("idle");
        _stats._prevLevel._values[0] = Global.Instance._playerLevelModifier;
        LevelUp();
        UpdateCombinedStats();
        Global.DebugOnScreen("PLAYER START()");
       

        int ii = 0;
        foreach (var item in _spellsArray)
        {
            if (item != null)
            {
                item._slot = _spellSlotArray[ii++];
                item.Init();
                item._slotImage.sprite = item._spellImage.sprite;
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if (_shield.isUp)
        {
            _shield.Update();
        }

        if (_showHit)
        {
            _showHitTimer += Time.deltaTime;
            
            if (_showHitTimer >= 0.3f)
            {
                _showHit = false;
                _showHitTimer = 0f;
            }
        }
        base.CheckEffect();
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
        if (_shield.isUp)
        {
            hitter_.TakeDamage(DamageStats.GenerateFromSpellStats(_shield.spellRef._combinedStats));
            _shield.Lower();
        }
        else
        {
            //Calculate damage with resistance from the characters stats
            vap normal = ds_._normal * (1f - _combinedStats._normal.resistance);
            vap tech = ds_._tech * (1f - _combinedStats._tech.resistance);
            vap psychic = ds_._psychic * (1f - _combinedStats._psychic.resistance);
            vap kinetic = ds_._kinetic * (1f - _combinedStats._kinetic.resistance);

            vap totalDamage = normal + tech + psychic + kinetic;
            vap healthDamagePercent = new vap();
            if (ds_._healthDamagePercent > 0f)
            {
                totalDamage += hitter_.maxHealth * ds_._healthDamagePercent;
                healthDamagePercent = hitter_.maxHealth * ds_._healthDamagePercent;
            }

            _stats._health -= totalDamage;
            if (ds_._healPercent > 0f)
                ds_._heal += _combinedStats._maxHealth * (ds_._healPercent * 0.01f);
            _stats._health += ds_._heal;

            if (ds_._healthDamagePercent > 0f)
            {
                totalDamage += hitter_.maxHealth * ds_._healthDamagePercent;
            }

            //If heal, make sure we don't go over maxhealth
            if (_stats._health > _combinedStats._maxHealth)
                _stats._health = new vap(_combinedStats._maxHealth);
            if (ds_._stunTime > 0f)
            {
                GetComponent<ClickAttack>().Stunned(ds_._stunTime);
                foreach (var item in _spellsArray)
                {
                    if (item != null)
                    {
                        item.Stunned(ds_._stunTime);
                    }
                }
            }

            // play hit animation
            Animator.SetTrigger("hit_start");

            //Draw hit texture on gui
            _showHit = true;
            OnGUI();
            Sounds.OneShot(Sounds.Instance.playerSounds.takeDamage);
            SpawnText(normal, tech, psychic, kinetic, ds_._heal, healthDamagePercent, hitPoint_);

            if (_stats._health._values[0] < 1f)
            {
                _stats._health = new vap();
                Die(hitter_);
            }
        }
    }

    public void CheckPlanetLevel()
    {
        if (Global.Instance._gameType != Global.GameType.Farm)
        {
            return;
        }
        if (Global.Instance._planet._maxLevel + 3 <= _level)
        {
            for (int i = 0; i < Starmap.Instance._planetList.Count; i++)
            {
                if (Starmap.Instance._planetList[i] == Starmap.Instance._selectedPlanet && i != Starmap.Instance._planetList.Count-1)
                {
                    FarmMode.Instance._nextPlanetButton.SetActive(true);
                    FarmMode.Instance._nextPlanetButton.GetComponent<NextPlanetButton>()._nextPlanet = Starmap.Instance._planetList[i + 1];
                }
            }
        }
    }

    void OnGUI()
    {
        if(_showHit){
            try{
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Global.Instance._textures._hitEffect);
            }
            catch (System.NullReferenceException) { }
        }
            
    }

    /// <summary>
    /// Player is dead
    /// </summary>
    public override void Die()
    {
        base.Die();
        Global.Instance.PlayerDied();
        Sounds.OneShot(Sounds.Instance.playerSounds.dies);
        // play die animation
        Animator.SetTrigger("die_start");
        
    }

    public override void LevelUp()
    {
        base.LevelUp();
        UpdateCombinedStats();
        
        if (_level % 3 == 0)
        {
            _miniBoss = true;
        }
        
        if (_level != 1)
        {
            Sounds.OneShot(Sounds.Instance.music.levelUp);
            GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Ding").text = _levelUpMessage;
            StartCoroutine(RemoveDing());
        }

        Global.Instance.effects.levelUp.Start();
            
        // see if player is 3 levels higher than enemies on planet
        CheckPlanetLevel();
    }

    IEnumerator RemoveDing()
    {
        yield return new WaitForSeconds(1);
        GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "Ding").text = "";
    }
    public void Reset(float time_)
    {
        if (IsInvoking())
        {
            CancelInvoke();
        }
        Invoke("ResetNow", time_);
    }

    public void ResetNow()
    {
        // trigger idle animation
        Animator.SetTrigger("idle");

        _stats._health = new vap(_combinedStats._maxHealth);

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
    public override void SetExperience(uint level_, Character killer_)
    {
        base.SetExperience(level_, killer_);
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
        //Global.DebugOnScreen("kör combine stats");
        _combinedStats = new CharacterStats(_stats);

        // equipment
        if (_equipped._chest != null)
            _combinedStats.AddStats(_equipped._chest._stats);
        if (_equipped._head != null)
            _combinedStats.AddStats(_equipped._head._stats);
        if (_equipped._legs != null)
            _combinedStats.AddStats(_equipped._legs._stats);
        if (_equipped._weapon != null)
            _combinedStats.AddStats(_equipped._weapon._stats);


        // talents
                        // hp
        if (_talentStats._healtPercent.value > 0f)
            _combinedStats._maxHealth += _combinedStats._maxHealth * _talentStats._healtPercent.value;
        _combinedStats._maxHealth += _talentStats._health;

                        // dmg
        if (_talentStats._normal._damagePercent.value > 0f)
            _combinedStats._normal.damage +=  _combinedStats._normal.damage * _talentStats._normal._damagePercent.value;
        _combinedStats._normal.damage += _talentStats._normal.damage;

        if (_talentStats._tech._damagePercent.value > 0f)
            _combinedStats._tech.damage += _combinedStats._tech.damage * _talentStats._tech._damagePercent.value;
        _combinedStats._tech.damage += _talentStats._tech.damage;

        if (_talentStats._kinetic._damagePercent.value > 0f)
            _combinedStats._kinetic.damage += _combinedStats._kinetic.damage * _talentStats._kinetic._damagePercent.value;
        _combinedStats._kinetic.damage += _talentStats._kinetic.damage;

        if (_talentStats._psychic._damagePercent.value > 0f)
            _combinedStats._psychic.damage += _combinedStats._psychic.damage * _talentStats._psychic._damagePercent.value;
        _combinedStats._psychic.damage += _talentStats._psychic.damage;

                        // crit
        if (_talentStats._normal.critMultiplier.value > 0f)
            _combinedStats._normal.critMultiplier += _combinedStats._normal.critMultiplier * _talentStats._normal.critMultiplier.value;
        _combinedStats._normal.crit += _talentStats._normal.crit.value;

        if (_talentStats._tech.critMultiplier.value > 0f)
            _combinedStats._tech.critMultiplier += _combinedStats._tech.critMultiplier * _talentStats._tech.critMultiplier.value;
        _combinedStats._tech.crit += _talentStats._tech.crit.value;

        if (_talentStats._kinetic.critMultiplier.value > 0f)
            _combinedStats._kinetic.critMultiplier += _combinedStats._kinetic.critMultiplier * _talentStats._kinetic.critMultiplier.value;
        _combinedStats._kinetic.crit += _talentStats._kinetic.crit.value;

        if (_talentStats._psychic.critMultiplier.value > 0f)
            _combinedStats._psychic.critMultiplier += _combinedStats._psychic.critMultiplier * _talentStats._psychic.critMultiplier.value;
        _combinedStats._psychic.crit += _talentStats._psychic.crit.value;

        _stats._health = new vap(_combinedStats._maxHealth);

        // spells
        for (int i = 0; i < _spellsArray.Length; i++)
        {
            if(_spellsArray[i] != null)
                _spellsArray[i].CombineSpellStats();
        }
    }

    public override void LifeSteal(vap lifeSteal_)
    {
        _stats._health += lifeSteal_;
        if (_stats._health > _combinedStats._maxHealth)
        {
            _stats._health = _combinedStats._maxHealth;
        }

        vap tmp = new vap();
        SpawnText(tmp, tmp, tmp, tmp, lifeSteal_, tmp, transform.position);
    }

    void OnEnable()
    {
        try
        {
            GetComponentInChildren<CharacterGUI>().ResetDir();
        }
        catch (System.NullReferenceException)
        {
        }        
        
    }

    void OnDisable()
    {
       
    }

    public void PreSave()
    {
        Global.Instance._player.gameObject.SetActive(true);

        Transform[] Parr = Global.Instance._player.gameObject.GetComponentsInChildren<Transform>(true);
        /*for (int i = 0; i < Parr.Length; i++)
        {
            bool found = false;
            foreach (var item in gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "GUI").GetComponentsInChildren<Transform>(true))
            {
                if (Parr[i].gameObject == item.gameObject || Parr[i].name == "GUI")
	            {
                    found = true;
	            }
            }
            if (!found)
            {
                if (Parr[i].GetComponent<StoreInformation>() == null)
                {
                    Parr[i].gameObject.AddComponent<StoreInformation>();
                }
            }
        }*/
        for (int i = 0; i < Parr.Length; i++)
        {
            Parr[i].gameObject.SetActive(true);
        }

        foreach (var item in GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentsInChildren<Transform>(true))
        {
            if (item.GetComponent<StoreInformation>() == null)
            {
                item.gameObject.AddComponent<StoreInformation>();
            }
        }
        foreach (var item in GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Inventory").GetComponentsInChildren<Transform>(true))
        {
            if (item.GetComponent<StoreInformation>() == null)
            {
                item.gameObject.AddComponent<StoreInformation>();
            }
        }
        foreach (var item in GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Equipment").GetComponentsInChildren<Transform>(true))
        {
            if (item.GetComponent<StoreInformation>() == null)
            {
                item.gameObject.AddComponent<StoreInformation>();
            }
        }
        foreach (var item in GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Model").GetComponentsInChildren<Transform>(true))
        {
            if (item.GetComponent<StoreInformation>() == null)
            {
                item.gameObject.AddComponent<StoreInformation>();
            }
        }
    }

    public void PostSave()
    {
        gameObject.SetActive(false);
    }

    public void PreLoad()
    {
        Transform[] arr = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

    public void PostLoad()
    {
        gameObject.SetActive(false);

        foreach (var item in _spellsArray)
        {
            if (item != null)
            {
                item._slotImage.sprite = item._spellImage.sprite;
            }
        }

       /* try
        {
            Destroy(gameObject.GetComponentsInChildren<CharacterGUI>(true).FirstOrDefault().gameObject);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("nu försvann guit innan jag kunde ta bort det");
        }

        GameObject go = GameObject.Instantiate(GUIPrefab);
        go.transform.parent = transform;

        go.GetComponent<CharacterGUI>().character = this;

        Transform[] arr = go.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < _spellSlotArray.Length; i++)
        {
            //Debug.Log("hallå eller?");
            foreach (var item in go.GetComponentsInChildren<Transform>(true))
            {
                //Debug.Log(item.name);
                if (item.name == "Spellslot " + i)
                {
                    //Debug.Log("ja");
                    _spellSlotArray[i] = item.gameObject;
                }
            }
        }

        for (int i = 0; i < _spellsArray.Length; i++)
        {
            if (_spellsArray[i] != null)
            {
                _spellsArray[i]._slot = _spellSlotArray[i];
                _spellsArray[i]._slotImage = _spellSlotArray[i].GetComponentsInChildren<Image>().FirstOrDefault(x => x.gameObject.name == "Image");
            }
        }*/
    }
}
