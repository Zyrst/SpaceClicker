using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CharacterScreen : MonoBehaviour {

    public GameObject InventoryBackground;
    public GameObject CharacterBackground;
    public GameObject CharacterStatsBackground;
    public GameObject _EquipPopup;
    public GameObject _EquipedPopup;

    public Vector2 _offSet;
    public Vector2 _numInventorySlot;
    public Vector3 _invStartPos = new Vector3();
    public Vector3 _playerPos = Vector3.zero;
    public Quaternion _playerRot = Quaternion.identity;
    public bool _lastFrameClick = false;

    public ArrayList _inventory = new ArrayList();
    public Equipment _lastEquip;

    [HideInInspector]
    Transform _playerModel;
    [HideInInspector]
    Transform _playerCollider;

    [HideInInspector]
    Vector3 _oldPos = Vector3.zero;
    [HideInInspector]
    bool _firstTouch = false;

    private static CharacterScreen _instance = null;
    public static CharacterScreen Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CharScreen").GetComponent<CharacterScreen>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
	// Use this for initialization
	void Start () {
        CloseTalentTree();
        GenerateCharInfo();
        Instance.gameObject.SetActive(false);
        CloseSpells();
	}
	
	// Update is called once per frame
	void Update () {
        if (MouseController.Instance.buttonDown)
        {
            Ray ray = Global.Instance._uiCamera.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                try
                {
                    if (hit.collider.transform.parent.parent.tag == "Player")
                    {
                        if (!_firstTouch)
                        {
                            _oldPos = MouseController.Instance.position;
                            _firstTouch = true;
                        }
                    }

                }
                catch (System.NullReferenceException)
                {
                }
            }
            if (_firstTouch)
            {
                Vector3 delta = _oldPos - MouseController.Instance.position;
                if (delta.x != 0f)
                {
                    _playerModel.Rotate(0f, delta.x, 0f);
                }

                _oldPos = MouseController.Instance.position;
            }
        }
        else if (!MouseController.Instance.buttonDown && _firstTouch)
        {
            _firstTouch = false;
        }
	}

    void LateUpdate()
    {
        if (_lastFrameClick && !MouseController.Instance.buttonDown)
        {
            _EquipPopup.transform.gameObject.SetActive(false);
        }
        if (MouseController.Instance.buttonDown && _EquipPopup.activeSelf)
        {
            _lastFrameClick = true;

        }
        else
        {
            _lastFrameClick = false;
        }
    }

    public void Activate()
    {
        Model();
        GenerateInventorySlots();
        GenerateCharInfo();
        PutEquipmentOnTheSlots();
    }

    public void TalentTree()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "TalentTree").gameObject.SetActive(true);
        try
        {
            TalentInfoBox.Instance.talentPointText.text = Global.Instance.player._talentPoints.ToString();
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Med högsta sannolikhet så är talent tree inaktiv i hierarkin");
        }
    }

    public void CloseTalentTree()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "TalentTree").gameObject.SetActive(false);
        if (TalentInfoBox.Instance != null)
        {
            TalentInfoBox.Instance.IsUp = false;
        }
    }

    public void Spells()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "SpellsMap").GetComponent<SpellsMap>().Open();
    }

    public void CloseSpells()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "SpellsMap").GetComponent<SpellsMap>().Close();
    }

    public void GenerateInventorySlots()
    {
        EquipmentPopup.reset();
        Vector3 currentPos = Vector3.zero;
        currentPos.x = _invStartPos.x;
        currentPos.y = _invStartPos.y;
        int counter = 0;
        for (int j = 0; j < _numInventorySlot.y; j++)
        {
            for (int i = 0; i < _numInventorySlot.x; i++)
            {
                
                GameObject tmpInv = GameObject.Instantiate(Global.Instance._prefabs.InventorySlot);
                //tmpInv.gameObject.GetComponent<RectTransform>().SetParent(transform.GetComponentInChildren<Canvas>().GetComponent<RectTransform>());
                tmpInv.gameObject.GetComponent<RectTransform>().SetParent(
                    transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "InventoryBackground").GetComponent<RectTransform>());
                tmpInv.gameObject.GetComponent<RectTransform>().localPosition = currentPos;
                currentPos.x += _offSet.x;
                tmpInv.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                try
                {
                    tmpInv.gameObject.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "EquipSprite").sprite = Global.Instance.player._inventoryArray[counter].gameObject.GetComponent<Equipment>()._sprite.sprite;
                }
                catch (System.NullReferenceException) {}
  
                _inventory.Add(tmpInv);
                counter++;
            }
            currentPos.x = _invStartPos.x;
            currentPos.y -= _offSet.y;
        }
    }

    public void RemoveInventorySlots()
    {
        foreach (var item in _inventory)
        {
            Destroy((GameObject)item);
        }
        _inventory.Clear();
    }

    public void EquipLast()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.equipGear);

       // Debug.Log("EquipLast");
        ResetInventorySprite(_lastEquip);
        Global.Instance.player._equipped.Equip(_lastEquip);
        GenerateCharInfo();
    }

    public void PopItUp(Equipment equi_)
    {
        _EquipPopup.gameObject.SetActive(true);
        Vector3 temp = MouseController.Instance.position;
        //temp.z = 100f; //Distance to plane
        temp.y += 40f;
       // _EquipPopup.transform.position = Global.Instance._uiCamera.ScreenToWorldPoint(temp);
        _EquipPopup.transform.position = temp;
        _lastEquip = equi_;
        EquipmentStatsPopUp(equi_);
    }

    public void ClosePopup()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        _EquipPopup.gameObject.SetActive(false);
    }

    public void EquipedPop(Equipment equi_)
    {
        _EquipedPopup.gameObject.SetActive(true);
        string info = equi_._stats._name;
        switch (equi_._element)
        {
            case Equipment.ElementType.Tech:
                if (equi_._stats._tech.damage.GetFloat() > 0f)
                {
                    info += System.Environment.NewLine + "(Tec.) Damage: " + equi_._stats._tech.damage.GetString();
                }
                break;
            case Equipment.ElementType.Kinetic:
                if (equi_._stats._kinetic.damage.GetFloat() > 0f)
                {
                    info += System.Environment.NewLine + "(Kin.) Damage: " + equi_._stats._kinetic.damage.GetString();
                }
                break;
            case Equipment.ElementType.Pshycic:
                if (equi_._stats._psychic.damage.GetFloat() > 0f)
                {
                    info += System.Environment.NewLine + "(Psy.) Damage: " + equi_._stats._psychic.damage.GetString();
                }
                break;
            case Equipment.ElementType.Normal:
                if (equi_._stats._normal.damage.GetFloat() > 0f)
                {
                    info += System.Environment.NewLine + "(Nor.) Damage: " + equi_._stats._normal.damage.GetString();
                }
                break;
            default:
                break;
        }

        if (equi_._stats._health.GetFloat() > 0f)
        {
            info += System.Environment.NewLine + "Healh Upgrade : " + equi_._stats._health.GetString();
        }

        _EquipedPopup.GetComponentInChildren<Text>().text = info;
        
    }

    public void EquipedPopClose()
    {
        _EquipedPopup.gameObject.SetActive(false);
    }

    public void Sell()
    {
        //Global.Instance.Gold += _lastEquip._sellValue;
       // Destroy(Global.Instance._player._inventoryArray.FirstOrDefault(x => x.name == _lastEquip.ToString()));
    }

    public void ResetInventorySprite(Equipment equi_)
    {

        for (int i = 0; i < Global.Instance.player._inventoryArray.Length; i++)
        {
            if (Global.Instance.player._inventoryArray[i] == equi_.gameObject)
            {
                object[] arr = _inventory.ToArray();
                ((GameObject)arr[i]).GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "EquipSprite").sprite = null;
            }
        }
    }

    /// <summary>
    /// Generates the Character stats in CharScreen
    /// </summary>
    public void GenerateCharInfo()
    {

        string info = "Level : " + Global.Instance.player._level.ToString();
        info += System.Environment.NewLine + "Health: " +  Global.Instance.player._combinedStats._maxHealth.GetString();
        
        info+= System.Environment.NewLine + System.Environment.NewLine + "Click";
        info += System.Environment.NewLine + "Click Damage : " + Global.Instance.player._combinedStats._normal.damage.GetString();
        info += System.Environment.NewLine + "Click Crit Chance " + (Global.Instance.player._combinedStats._normal.crit * 100) + "%";
        info += System.Environment.NewLine + "Click Crit multiplier : " + Global.Instance.player._combinedStats._normal.critMultiplier;

        info += System.Environment.NewLine + System.Environment.NewLine + "<color=blue>Tech</color>";
        info += System.Environment.NewLine + "Tech Damage : "  + Global.Instance.player._combinedStats._tech.damage.GetString();
        info += System.Environment.NewLine + "Tech Crit Chance : " + (Global.Instance.player._combinedStats._tech.crit * 100) + "%";
        info += System.Environment.NewLine + "Tech Crit multiplier : " + Global.Instance.player._combinedStats._tech.critMultiplier;

        info += System.Environment.NewLine + System.Environment.NewLine + "<color=yellow>Kinetic</color>";
        info += System.Environment.NewLine + "Kinetic Damage : " + Global.Instance.player._combinedStats._kinetic.damage.GetString();
        info += System.Environment.NewLine + "Kinetic Crit Chance : " + (Global.Instance.player._combinedStats._kinetic.crit * 100) + "%";
        info += System.Environment.NewLine + "Kinetic Crit multiplier : " + Global.Instance.player._combinedStats._kinetic.critMultiplier;

        info += System.Environment.NewLine + System.Environment.NewLine + "<color=purple>Psychic</color>";
        info += System.Environment.NewLine + "Psychic Damage : " + Global.Instance.player._combinedStats._psychic.damage.GetString();
        info += System.Environment.NewLine + "Kinetic Crit Chance : " + (Global.Instance.player._combinedStats._psychic.crit * 100) + "%";
        info += System.Environment.NewLine + "Kinetic Crit multiplier : " + Global.Instance.player._combinedStats._psychic.critMultiplier;

        info += System.Environment.NewLine + System.Environment.NewLine + "Resistance";
        info += System.Environment.NewLine + "Normal Resistance : " + Global.Instance.player._combinedStats._normal.resistance;
        info += System.Environment.NewLine + "Tech Resistance : " + Global.Instance.player._combinedStats._tech.resistance;
        info += System.Environment.NewLine + "Kinetic Resistance : " + Global.Instance.player._combinedStats._kinetic.resistance;
        info += System.Environment.NewLine + "Psychic Resistance : " + Global.Instance.player._combinedStats._psychic.resistance;

        float cooldown = Global.Instance.player._combinedStats._psychic.cooldownReduction + Global.Instance.player._combinedStats._normal.cooldownReduction +
            Global.Instance.player._combinedStats._kinetic.cooldownReduction + Global.Instance.player._combinedStats._tech.cooldownReduction;

        info += System.Environment.NewLine + System.Environment.NewLine + "Cooldown Reduction : " + cooldown;

        GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "CharInfo").text = info;

        for (int i = 0; i < Global.Instance.player._spellsArray.Length; i++)
        {
            if (Global.Instance.player._spellsArray[i] != null)
                GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Spell" + i).GetComponent<Image>().sprite = Global.Instance.player._spellsArray[i]._spellImage.sprite;
        }
    }

    public void Model()
    {
        _playerPos = Global.Instance.player.transform.position;
        _playerRot = Global.Instance.player.transform.rotation;

        Global.Instance.player.gameObject.SetActive(true);
        _playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "maincharacter_combat_animation_idle_01Slow").transform;

        _playerCollider = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "colliders").transform;


        _playerModel.position = GetComponentsInChildren<RectTransform>().FirstOrDefault(x => x.name == "CharPos").transform.position;
        _playerCollider.position = GetComponentsInChildren<RectTransform>().FirstOrDefault(x => x.name == "CharPos").transform.position;
        _playerModel.LookAt(Global.Instance._uiCamera.transform.position + new Vector3(0,-20,0));
        _playerModel.localScale = new Vector3(13, 13, 13);
        _playerCollider.localScale = new Vector3(13, 13, 13);

        Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = false;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = false;
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = false;
    }

    public void ResetModel()
    {

        _playerModel.position = _playerPos;
        _playerCollider.position = _playerPos;
        _playerModel.rotation = _playerRot;
        _playerModel.localScale = new Vector3(1, 1, 1);
        _playerCollider.localScale = new Vector3(1, 1, 1);
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = true;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = true;
    //    Global.Instance._player.gameObject.SetActive(false);
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = true;

    }

    /// <summary>
    /// Diffrence between gear
    /// </summary>
    /// <param name="equip_">Equipment</param>
    public void EquipmentStatsPopUp(Equipment equip_)
    {
        string info = equip_._stats._name;
        info += System.Environment.NewLine + "Rarity: " + equip_.GetRarity();
        #region NothingEquipped
        if (Global.Instance.player._equipped._chest == null && equip_._type == Equipment.EquipmentType.Chest || Global.Instance.player._equipped._head == null && equip_._type == Equipment.EquipmentType.Head || 
            Global.Instance.player._equipped._legs == null && equip_._type == Equipment.EquipmentType.Legs || Global.Instance.player._equipped._weapon == null && equip_._type == Equipment.EquipmentType.Weapon)
        {
            if(equip_._stats._normal.damage.GetFloat() > 0f)
                info += System.Environment.NewLine + "Normal Damage :  " + equip_._stats._normal.damage.GetString() + "     +" + equip_._stats._normal.damage.GetString();
            if (equip_._stats._normal.crit > 0f)
                info += System.Environment.NewLine + "Click Crit Chance :  " + (equip_._stats._normal.crit * 100) + "% " + "     +" + (equip_._stats._normal.crit * 100) + "% "; 
            if(equip_._stats._normal.critMultiplier >0f)
                info += System.Environment.NewLine + "Click Crit Multiplier :  " + equip_._stats._normal.critMultiplier.ToString() + "     +" + equip_._stats._normal.critMultiplier.ToString(); 
            if(equip_._stats._normal.resistance > 0f)
                info += System.Environment.NewLine + "Click Resistance :  " + equip_._stats._normal.resistance.ToString() + "     +" + equip_._stats._normal.resistance.ToString();
            GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "StatsText").text = info;
            return;
        }
        #endregion
        #region CalcDamage
        /* Vap for different damage types */
        vap calcN = new vap();
        vap calcT = new vap();
        vap calcK = new vap();
        vap calcP = new vap();
        //Click damage
        switch (equip_._type)
        {
            case Equipment.EquipmentType.Weapon:
                calcN = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._weapon._stats._normal.damage);
                calcT = vap.Minus(equip_._stats._tech.damage, Global.Instance.player._equipped._weapon._stats._tech.damage);
                calcK = vap.Minus(equip_._stats._kinetic.damage, Global.Instance.player._equipped._weapon._stats._kinetic.damage);
                calcP = vap.Minus(equip_._stats._psychic.damage, Global.Instance.player._equipped._weapon._stats._psychic.damage);
                break;
            case Equipment.EquipmentType.Head:
                calcN = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._head._stats._normal.damage);
                calcT = vap.Minus(equip_._stats._tech.damage, Global.Instance.player._equipped._head._stats._tech.damage);
                calcK = vap.Minus(equip_._stats._kinetic.damage, Global.Instance.player._equipped._head._stats._kinetic.damage);
                calcP = vap.Minus(equip_._stats._psychic.damage, Global.Instance.player._equipped._head._stats._psychic.damage);
                break;
            case Equipment.EquipmentType.Chest:
                calcN = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._chest._stats._normal.damage);
                calcT = vap.Minus(equip_._stats._tech.damage, Global.Instance.player._equipped._chest._stats._tech.damage);
                calcK = vap.Minus(equip_._stats._kinetic.damage, Global.Instance.player._equipped._chest._stats._kinetic.damage);
                calcP = vap.Minus(equip_._stats._psychic.damage, Global.Instance.player._equipped._chest._stats._psychic.damage);
                break;
            case Equipment.EquipmentType.Legs:
                calcN = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._legs._stats._normal.damage);
                calcT = vap.Minus(equip_._stats._tech.damage, Global.Instance.player._equipped._legs._stats._tech.damage);
                calcK = vap.Minus(equip_._stats._kinetic.damage, Global.Instance.player._equipped._legs._stats._kinetic.damage);
                calcP = vap.Minus(equip_._stats._psychic.damage, Global.Instance.player._equipped._legs._stats._psychic.damage);
                break;
        }

        if(calcN.GetFloat() > 0f || calcN.GetFloat() < 0f)
        {
            info += System.Environment.NewLine + "(Nor.)Damage: " + equip_._stats._normal.damage.GetString();
            info += "   ";
            if (calcN.GetFloat() > 0f)
            {
                info += "+" + calcN.GetString();
            }
            else
                info += calcN.GetString();
        }

        if (calcT.GetFloat() > 0f || calcT.GetFloat() < 0f)
        {
            info += System.Environment.NewLine + "(Tech)Damage: " + equip_._stats._tech.damage.GetString();
            info += "   ";
            if (calcT.GetFloat() > 0f)
            {
                info += "+" + calcT.GetString();
            }
            else
                info += calcT.GetString();
        }

        if (calcK.GetFloat() > 0f || calcK.GetFloat() < 0f)
        {
            info += System.Environment.NewLine + "(Kin.)Damage: " + equip_._stats._kinetic.damage.GetString();
            info += "   ";
            if (calcK.GetFloat() > 0f)
            {
                info += "+" + calcK.GetString();
            }
            else
                info += calcK.GetString();
        }

        if (calcP.GetFloat() > 0f || calcP.GetFloat() < 0f)
        {
            info += System.Environment.NewLine + "(Psy.)Damage: " + equip_._stats._psychic.damage.GetString();
            info += "   ";
            if (calcP.GetFloat() > 0f)
            {
                info += "+" + calcP.GetString();
            }
            else
                info += calcP.GetString();
        }
        #endregion
        #region CalcCritChance
        /* Damage crit - <type><Crit>*/
         float calcNC = 0f;
         float calcTC = 0f;
         float calcKC = 0f;
         float calcPC = 0f;

         switch (equip_._type)
         {
             case Equipment.EquipmentType.Weapon:
                  calcNC = equip_._stats._normal.crit - Global.Instance.player._equipped._weapon._stats._normal.crit;
                  calcTC = equip_._stats._tech.crit - Global.Instance.player._equipped._weapon._stats._tech.crit;
                  calcKC = equip_._stats._kinetic.crit - Global.Instance.player._equipped._weapon._stats._kinetic.crit;
                  calcPC = equip_._stats._psychic.crit - Global.Instance.player._equipped._weapon._stats._psychic.crit;
                  break;
             case Equipment.EquipmentType.Head:
                  calcNC = equip_._stats._normal.crit - Global.Instance.player._equipped._head._stats._normal.crit;
                  calcTC = equip_._stats._tech.crit - Global.Instance.player._equipped._head._stats._tech.crit;
                  calcKC = equip_._stats._kinetic.crit - Global.Instance.player._equipped._head._stats._kinetic.crit;
                  calcPC = equip_._stats._psychic.crit - Global.Instance.player._equipped._head._stats._psychic.crit;
                  break;
             case Equipment.EquipmentType.Chest:
                  calcNC = equip_._stats._normal.crit - Global.Instance.player._equipped._chest._stats._normal.crit;
                  calcTC = equip_._stats._tech.crit - Global.Instance.player._equipped._chest._stats._tech.crit;
                  calcKC = equip_._stats._kinetic.crit - Global.Instance.player._equipped._chest._stats._kinetic.crit;
                  calcPC = equip_._stats._psychic.crit - Global.Instance.player._equipped._chest._stats._psychic.crit;
                  break;
             case Equipment.EquipmentType.Legs:
                  calcNC = equip_._stats._normal.crit - Global.Instance.player._equipped._legs._stats._normal.crit;
                  calcTC = equip_._stats._tech.crit - Global.Instance.player._equipped._legs._stats._tech.crit;
                  calcKC = equip_._stats._kinetic.crit - Global.Instance.player._equipped._legs._stats._kinetic.crit;
                  calcPC = equip_._stats._psychic.crit - Global.Instance.player._equipped._legs._stats._psychic.crit;
                  break;
         }

        if(calcNC > 0f || calcNC < 0f)
        {
            info += System.Environment.NewLine + "(Nor.)Crit. Chance: " + (equip_._stats._normal.crit * 100f) + "%";
            info += "    ";
            if (calcNC > 0f)
                info += "+" + (calcNC * 100f).ToString() + "%";
            else
                info += (calcNC * 100f).ToString() + "%";
        }

        if (calcTC > 0f || calcTC < 0f)
        {
            info += System.Environment.NewLine + "(Tech)Crit. Chance: " + (equip_._stats._tech.crit * 100f) + "%";
            info += "    ";
            if (calcTC > 0f)
                info += "+" + (calcTC * 100f).ToString() + "%";
            else
                info += (calcTC * 100f).ToString() + "%";
        }

        if (calcKC > 0f || calcKC < 0f)
        {
            info += System.Environment.NewLine + "(Kin.)Crit. Chance: " + (equip_._stats._kinetic.crit * 100f) + "%";
            info += "    ";
            if (calcKC > 0f)
                info += "+" + (calcKC * 100f).ToString() + "%";
            else
                info += (calcKC * 100f).ToString() + "%";
        }

        if (calcPC > 0f || calcPC < 0f)
        {
            info += System.Environment.NewLine + "(Psy.)Crit. Chans: " + (equip_._stats._psychic.crit * 100f) + "%";
            info += "    ";
            if (calcPC > 0f)
                info += "+" + (calcPC * 100f).ToString() + "%";
            else
                info += (calcPC * 100f).ToString() + "%";
        }
        #endregion
        #region CalcCritMulti
        calcNC = 0f;
        calcTC = 0f;
        calcKC = 0f;
        calcPC = 0f;

        switch (equip_._type)
        {
            case Equipment.EquipmentType.Weapon:
                calcNC = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._weapon._stats._normal.critMultiplier;
                calcTC = equip_._stats._tech.critMultiplier - Global.Instance.player._equipped._weapon._stats._tech.critMultiplier;
                calcKC = equip_._stats._kinetic.critMultiplier - Global.Instance.player._equipped._weapon._stats._kinetic.critMultiplier;
                calcPC = equip_._stats._psychic.critMultiplier - Global.Instance.player._equipped._weapon._stats._psychic.critMultiplier;
                break;
            case Equipment.EquipmentType.Head:
                calcNC = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._head._stats._normal.critMultiplier;
                calcTC = equip_._stats._tech.critMultiplier - Global.Instance.player._equipped._head._stats._tech.critMultiplier;
                calcKC = equip_._stats._kinetic.critMultiplier - Global.Instance.player._equipped._head._stats._kinetic.critMultiplier;
                calcPC = equip_._stats._psychic.critMultiplier - Global.Instance.player._equipped._head._stats._psychic.critMultiplier;
                break;
            case Equipment.EquipmentType.Chest:
                calcNC = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._chest._stats._normal.critMultiplier;
                calcTC = equip_._stats._tech.critMultiplier - Global.Instance.player._equipped._chest._stats._tech.critMultiplier;
                calcKC = equip_._stats._kinetic.critMultiplier - Global.Instance.player._equipped._chest._stats._kinetic.critMultiplier;
                calcPC = equip_._stats._psychic.critMultiplier - Global.Instance.player._equipped._chest._stats._psychic.critMultiplier;
                break;
            case Equipment.EquipmentType.Legs:
                calcNC = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._legs._stats._normal.critMultiplier;
                calcTC = equip_._stats._tech.critMultiplier - Global.Instance.player._equipped._legs._stats._tech.critMultiplier;
                calcKC = equip_._stats._kinetic.critMultiplier - Global.Instance.player._equipped._legs._stats._kinetic.critMultiplier;
                calcPC = equip_._stats._psychic.critMultiplier - Global.Instance.player._equipped._legs._stats._psychic.critMultiplier;
                break;
            default:
                break;
        }
        if (calcNC > 0f || calcNC < 0f)
        {
            info += System.Environment.NewLine + "(Nor.)Crit. Damage: " + equip_._stats._normal.critMultiplier;
            info += "    ";
            if (calcNC > 0)
                info += "+" + calcNC.ToString();
            else
                info += calcNC.ToString();
        }

        if (calcTC > 0f || calcTC < 0f)
        {
            info += System.Environment.NewLine + "(Tech)Crit. Damage: " + equip_._stats._tech.critMultiplier;
            info += "    ";
            if (calcTC > 0)
                info += "+" + calcTC.ToString();
            else
                info += calcTC.ToString();
        }

        if (calcKC > 0f || calcKC < 0f)
        {
            info += System.Environment.NewLine + "(Kin.)Crit. Damage: " + equip_._stats._kinetic.critMultiplier;
            info += "    ";
            if (calcKC > 0)
                info += "+" + calcKC.ToString();
            else
                info += calcKC.ToString();
        }

        if (calcPC > 0f || calcPC < 0f)
        {
            info += System.Environment.NewLine + "(Psy.)Crit. Damage: " + equip_._stats._psychic.critMultiplier;
            info += "    ";
            if (calcPC > 0)
                info += "+" + calcPC.ToString();
            else
                info += calcPC.ToString();
        }
#endregion
        #region CalcHealth
        vap health = new vap();

        switch (equip_._type)
        {
            case Equipment.EquipmentType.Weapon:
                health = vap.Minus(equip_._stats._health, Global.Instance.player._equipped._weapon._stats._health);
                break;
            case Equipment.EquipmentType.Head:
                health = vap.Minus(equip_._stats._health, Global.Instance.player._equipped._head._stats._health);
                break;
            case Equipment.EquipmentType.Chest:
                health = vap.Minus(equip_._stats._health, Global.Instance.player._equipped._chest._stats._health);
                break;
            case Equipment.EquipmentType.Legs:
                health = vap.Minus(equip_._stats._health, Global.Instance.player._equipped._legs._stats._health);
                break;
            default:
                break;
        }

        if (health.GetFloat() > 0f || health.GetFloat() < 0f)
        {
            info += System.Environment.NewLine + "Health: " + equip_._stats._health.GetString();
            info += "   ";
            if (health.GetFloat() > 0f)
            {
                info += "+" + health.GetString();
            }
            else
                info += health.GetString();
        }
        #endregion
        GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "StatsText").text = info;
    }

    public void PutEquipmentOnTheSlots()
    {
        Global.Instance.player._equipped._chestSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._chest._sprite.sprite;
        Global.Instance.player._equipped._legsSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._legs._sprite.sprite;
        Global.Instance.player._equipped._weaponSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._weapon._sprite.sprite;
        Global.Instance.player._equipped._headSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._head._sprite.sprite;
    }

    public void Rotate()
    {
        _playerModel.transform.eulerAngles = new Vector3(0, GetComponentInChildren<Slider>().value, 0);
    }
}
