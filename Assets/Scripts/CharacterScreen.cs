using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CharacterScreen : MonoBehaviour {

    public GameObject InventoryBackground;
    public GameObject CharacterBackground;
    public GameObject CharacterStatsBackground;
    public GameObject _EquipPopup;

    public Vector2 _offSet;
    public Vector2 _numInventorySlot;
    public Vector3 _invStartPos = new Vector3();
    public Vector3 _playerPos = Vector3.zero;
    public Quaternion _playerRot = Quaternion.identity;
    public bool _lastFrameClick = false;

    public ArrayList _inventory = new ArrayList();
    public Equipment _lastEquip;

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
        EquipmentPopup.Reset();
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
                    tmpInv.gameObject.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "EquipSprite").sprite = Global.Instance.player._inventoryArray[counter].gameObject.GetComponent<Equipment>()._sprite;
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
        Debug.Log("EquipLast");
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
                GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Spell" + i).GetComponent<Image>().sprite = Global.Instance.player._spellsArray[i]._spellImage;
        }
    }

    public void Model()
    {
        _playerPos = Global.Instance.player.transform.position;
        _playerRot = Global.Instance.player.transform.rotation;

        Global.Instance.player.gameObject.SetActive(true);
        Transform playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Maincharacter_combat_model_01").transform;

        
        playerModel.position = GetComponentsInChildren<RectTransform>().FirstOrDefault(x => x.name == "CharPos").transform.position;
        playerModel.LookAt(Global.Instance._uiCamera.transform.position);
        playerModel.localScale = new Vector3(50, 50, 50);

        Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = false;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = false;
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = false;
    }

    public void ResetModel()
    {
        Transform playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Maincharacter_combat_model_01").transform;

        playerModel.position = _playerPos;
        playerModel.rotation = _playerRot;
        playerModel.localScale = new Vector3(2, 2, 2);
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
        if (Global.Instance.player._equipped._chest == null && equip_._type == Equipment.EquipmentType.Chest || Global.Instance.player._equipped._head == null && equip_._type == Equipment.EquipmentType.Head || 
            Global.Instance.player._equipped._legs == null && equip_._type == Equipment.EquipmentType.Legs || Global.Instance.player._equipped._weapon == null && equip_._type == Equipment.EquipmentType.Weapon)
        {
            if(equip_._stats._normal.damage.GetFloat() > 0f)
                info += System.Environment.NewLine + "Click Damage :  " + equip_._stats._normal.damage.GetString() + "     +" + equip_._stats._normal.damage.GetString();
            if (equip_._stats._normal.crit > 0f)
                info += System.Environment.NewLine + "Click Crit Chance :  " + (equip_._stats._normal.crit * 100) + "% " + "     +" + (equip_._stats._normal.crit * 100) + "% "; 
            if(equip_._stats._normal.critMultiplier >0f)
                info += System.Environment.NewLine + "Click Crit Multiplier :  " + equip_._stats._normal.critMultiplier.ToString() + "     +" + equip_._stats._normal.critMultiplier.ToString(); 
            if(equip_._stats._normal.resistance > 0f)
                info += System.Environment.NewLine + "Click Resistance :  " + equip_._stats._normal.resistance.ToString() + "     +" + equip_._stats._normal.resistance.ToString();
            GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "StatsText").text = info;
            return;
        }




        vap calc = new vap();
 
        switch (equip_._type)
        {
            case Equipment.EquipmentType.Weapon:
                calc = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._weapon._stats._normal.damage);
                break;
            case Equipment.EquipmentType.Head:
                calc = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._head._stats._normal.damage);
                break;
            case Equipment.EquipmentType.Chest:
                calc = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._chest._stats._normal.damage);
                break;
            case Equipment.EquipmentType.Legs:
                calc = vap.Minus(equip_._stats._normal.damage, Global.Instance.player._equipped._legs._stats._normal.damage);
                break;
            default:
                calc = equip_._stats._normal.damage;
                break;
        }

        if(calc.GetFloat() > 0f || calc.GetFloat() < 0f)
        {
            info += System.Environment.NewLine + "Click damage: " + equip_._stats._normal.damage.GetString();
            info += "   ";
            if (calc.GetFloat() > 0)
            {
                info += "+" + calc.GetString();
            }
            else
                info += calc.GetString();
        }

         float calcF = 0f;
         switch (equip_._type)
         {
             case Equipment.EquipmentType.Weapon:
                  calcF = equip_._stats._normal.crit - Global.Instance.player._equipped._weapon._stats._normal.crit;
                  break;
             case Equipment.EquipmentType.Head:
                  calcF = equip_._stats._normal.crit - Global.Instance.player._equipped._head._stats._normal.crit;
                  break;
             case Equipment.EquipmentType.Chest:
                  calcF = equip_._stats._normal.crit - Global.Instance.player._equipped._chest._stats._normal.crit;
                  break;
             case Equipment.EquipmentType.Legs:
                  calcF = equip_._stats._normal.crit - Global.Instance.player._equipped._legs._stats._normal.crit;
                  break;
             default:
                  calcF = equip_._stats._normal.crit;
                  break;
         }

        if(calcF > 0f || calcF < 0f)
        {
            info += System.Environment.NewLine + "Click crit chance : " + (equip_._stats._normal.crit * 100f) + "%";
            info += "    ";
            if (calcF > 0)
                info += "+" + (calcF * 100f).ToString() + "%";
            else
                info += (calcF * 100f).ToString() + "%";
        }

        calcF = 0f;
        switch (equip_._type)
        {
            case Equipment.EquipmentType.Weapon:
                calcF = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._weapon._stats._normal.critMultiplier;
                break;
            case Equipment.EquipmentType.Head:
                calcF = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._head._stats._normal.critMultiplier;
                break;
            case Equipment.EquipmentType.Chest:
                calcF = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._chest._stats._normal.critMultiplier;
                break;
            case Equipment.EquipmentType.Legs:
                calcF = equip_._stats._normal.critMultiplier - Global.Instance.player._equipped._legs._stats._normal.critMultiplier;
                break;
            default:
                break;
        }
        if (calcF > 0f || calcF < 0f)
        {
            info += System.Environment.NewLine + "Click crit chance : " + equip_._stats._normal.critMultiplier;
            info += "    ";
            if (calcF > 0)
                info += "+" + calcF.ToString();
            else
                info += calcF.ToString();
        }
            
        



        GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "StatsText").text = info;
    }

    public void PutEquipmentOnTheSlots()
    {
        Global.Instance.player._equipped._chestSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._chest._sprite;
        Global.Instance.player._equipped._legsSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._legs._sprite;
        Global.Instance.player._equipped._weaponSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._weapon._sprite;
        Global.Instance.player._equipped._headSlot.GetComponent<Image>().sprite = Global.Instance.player._equipped._head._sprite;
    }
}
