using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyEquipmentGeneration {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void SetEquipmentOnSlot(BuyEquipmentButton slot_)
    {
        Equipment equi = GenerateOne();

        ConnectEquipmentToTheButton(slot_, equi);

        equi.transform.parent = slot_.transform;
    }

    public static void ConnectEquipmentToTheButton(BuyEquipmentButton slot_, Equipment equipment_)
    {
        slot_._equipmentImage.sprite = equipment_._sprite.sprite;
        slot_._levelText.text = equipment_._stats._level.ToString();
        slot_._nameText.text = equipment_._stats._name;
        slot_._typeText.text = System.Enum.GetName(typeof(Equipment.EquipmentType), equipment_._type);

        switch (equipment_._rareness)
        {
            case Equipment.Rareness.Green:
                slot_.GetComponent<Image>().color = Global.Instance._colors.green;
                break;
            case Equipment.Rareness.Blue:
                slot_.GetComponent<Image>().color = Global.Instance._colors.blue;
                break;
            case Equipment.Rareness.Purple:
                slot_.GetComponent<Image>().color = Global.Instance._colors.purple;
                break;
            case Equipment.Rareness.Orange:
                slot_.GetComponent<Image>().color = Global.Instance._colors.orange;
                break;
            default:
                break;
        }

        string critMult = "";
        string crit = "";
        string damage = "";
        switch (equipment_._element)
        {
            case Equipment.ElementType.Tech:
                critMult = "(Tech)Crit. Damage:\t+ " + (equipment_._stats._tech.critMultiplier * 100f).ToString() + "%";
                crit = "(Tech)Crit. Chans:\t\t+ " + (equipment_._stats._tech.crit * 100f).ToString() + "%";
                damage = "(Tech)Damage:\t\t\t+ " + equipment_._stats._tech.damage.GetString();
                break;
            case Equipment.ElementType.Kinetic:
                critMult = "(Kin.)Crit. Damage:\t+ " + (equipment_._stats._kinetic.critMultiplier * 100f).ToString() + "%";
                crit = "(Kin.)Crit. Chans:\t\t+ " + (equipment_._stats._kinetic.crit * 100f).ToString() + "%";
                damage = "(Kin.)Damage:\t\t\t+ " + equipment_._stats._kinetic.damage.GetString();
                break;
            case Equipment.ElementType.Pshycic:
                critMult = "(Psy.)Crit. Damage:\t+ " + (equipment_._stats._psychic.critMultiplier * 100f).ToString() + "%";
                crit = "(Psy.)Crit. Chans:\t\t+ " + (equipment_._stats._psychic.crit * 100f).ToString() + "%";
                damage = "(Psy.)Damage:\t\t\t+ " + equipment_._stats._psychic.damage.GetString();
                break;
            case Equipment.ElementType.Normal:
                critMult = "(Nor.)Crit. Damage:\t+ " + (equipment_._stats._normal.critMultiplier * 100f).ToString() + "%";
                crit = "(Nor.)Crit. Chans:\t\t+ " + (equipment_._stats._normal.crit * 100f).ToString() + "%";
                damage = "(Nor.)Damage:\t\t\t+ " + equipment_._stats._normal.damage.GetString();
                break;
            default:
                break;
        }
        string health = "Health:\t\t\t\t\t\t+ " + equipment_._stats._health.GetString();

        slot_._infoText.text = critMult + "\n" + crit + "\n" + damage + "\n" + health;

        slot_._goldText.text = equipment_._cost.ToString();
    }

    public static Equipment GenerateOne()
    {
        var go = new GameObject();
        go.AddComponent<Equipment>();
        Equipment ret = go.GetComponent<Equipment>();
        ret.name = "Clementine";

        ret._stats._multiplierHealth = Global.Instance.player._stats._multiplierHealth;
        ret._stats._multiplierDamage = Global.Instance.player._stats._multiplierDamage;
        ret._stats._constMultiplier = Global.Instance.player._stats._constMultiplier;
        ret._stats._valueMultiplier = Global.Instance.player._stats._valueMultiplier;
        ret._stats._basePower = Global.Instance.player._stats._basePower;
        ret._stats._powerDiv = Global.Instance.player._stats._powerDiv;

        ret._rareness = Equipment.Rareness.Green;
        float ITEMSTATMULTIPLIER = 0f;
        switch (Random.Range(0, 4))
	    {
            case 0:
                ret._rareness = Equipment.Rareness.Green;
                ITEMSTATMULTIPLIER = 0.075f;
                break;
            case 1:
                ret._rareness = Equipment.Rareness.Blue;
                ITEMSTATMULTIPLIER = 0.15f;
                break;
            case 2:
                ret._rareness = Equipment.Rareness.Purple;
                ITEMSTATMULTIPLIER = 0.21f;
                break;
            case 3:
                ret._rareness = Equipment.Rareness.Orange;
                ITEMSTATMULTIPLIER = 0.3f;
                break;
		    default:
                break;
	    }

        ret._stats._level = Global.Instance.player._level;                                                         // level
                                                                                                                    // base
        ret._stats._baseStat._values[0] = (ret._stats._constMultiplier *
            (ret._stats._level + (int)ret._rareness) + (Mathf.Pow(ret._stats._basePower, (ret._stats._level + (int)ret._rareness) / ret._stats._powerDiv))) * ITEMSTATMULTIPLIER;
        ret._stats._baseStat.Checker();

        ret._stats._health = (ret._stats._baseStat / 2f) * Global.Instance.player._stats._multiplierHealth;        // health

        ret._element = (Equipment.ElementType)Random.Range(0,4);                                                    // element

        ret._type = (Equipment.EquipmentType)Random.Range(0,4);                                                     // type

        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            // crint mult
            switch (ret._element)
            {
                case Equipment.ElementType.Tech:
                    ret._stats._tech.critMultiplier = 0.5f;
                    break;
                case Equipment.ElementType.Kinetic:
                    ret._stats._kinetic.critMultiplier = 0.5f;
                    break;
                case Equipment.ElementType.Pshycic:
                    ret._stats._psychic.critMultiplier = 0.5f;
                    break;
                case Equipment.ElementType.Normal:
                    ret._stats._normal.critMultiplier = 0.5f;
                    break;
                default:
                    break;
            }
        }
        else if (rand == 1)
        {
            // crit chans
            switch (ret._element)
            {
                case Equipment.ElementType.Tech:
                    ret._stats._tech.crit = 0.1f;
                    break;
                case Equipment.ElementType.Kinetic:
                    ret._stats._kinetic.crit = 0.1f;
                    break;
                case Equipment.ElementType.Pshycic:
                    ret._stats._psychic.crit = 0.1f;
                    break;
                case Equipment.ElementType.Normal:
                    ret._stats._normal.crit = 0.1f;
                    break;
                default:
                    break;
            }
        }
        else if (rand == 3)
        {
            // båda
            switch (ret._element)
            {
                case Equipment.ElementType.Tech:
                    ret._stats._tech.critMultiplier = 0.25f;
                    ret._stats._tech.crit = 0.05f;
                    break;
                case Equipment.ElementType.Kinetic:
                    ret._stats._kinetic.critMultiplier = 0.25f;
                    ret._stats._kinetic.crit = 0.05f;
                    break;
                case Equipment.ElementType.Pshycic:
                    ret._stats._psychic.critMultiplier = 0.25f;
                    ret._stats._psychic.crit = 0.05f;
                    break;
                case Equipment.ElementType.Normal:
                    ret._stats._normal.critMultiplier = 0.25f;
                    ret._stats._normal.crit = 0.05f;
                    break;
                default:
                    break;
            }
        }

        //damage 

        switch (ret._element)
        {
            case Equipment.ElementType.Tech:
                ret._stats._tech.damage = (ret._stats._baseStat / 2f) * Global.Instance.player._stats._multiplierDamage;
                if (ret._stats._tech.damage.GetFloat() > 0f && ret._stats._tech.damage.GetFloat() < 1f)
                {
                    ret._stats._tech.damage._values[0] = 1;
                }
                
                break;
            case Equipment.ElementType.Kinetic:
                ret._stats._kinetic.damage = (ret._stats._baseStat / 2f) * Global.Instance.player._stats._multiplierDamage;
                if (ret._stats._kinetic.damage.GetFloat() > 0f && ret._stats._kinetic.damage.GetFloat() < 1f)
                {
                    ret._stats._kinetic.damage._values[0] = 1;
                }
                break;
            case Equipment.ElementType.Pshycic:
                ret._stats._psychic.damage = (ret._stats._baseStat / 2f) * Global.Instance.player._stats._multiplierDamage;
                if (ret._stats._psychic.damage.GetFloat() > 0f && ret._stats._psychic.damage.GetFloat() < 1f)
                {
                    ret._stats._psychic.damage._values[0] = 1;
                }
                break;
            case Equipment.ElementType.Normal:
                ret._stats._normal.damage = (ret._stats._baseStat / 2f) * Global.Instance.player._stats._multiplierDamage;
                if (ret._stats._normal.damage.GetFloat() > 0f && ret._stats._normal.damage.GetFloat() < 1f)
                {
                    ret._stats._normal.damage._values[0] = 1;
                }
                break;
            default:
                break;
        }

        switch (ret._type)
        {
            case Equipment.EquipmentType.Weapon:
                ret._stats._name = "Weapon";
                ret._sprite = Sprites.Instance.equipment.Weapon;
                break;
            case Equipment.EquipmentType.Head:
                ret._stats._name = "Hat";
                ret._sprite = Sprites.Instance.equipment.Head;
                break;
            case Equipment.EquipmentType.Chest:
                ret._stats._name = "Shirt";
                ret._sprite = Sprites.Instance.equipment.Chest;
                break;
            case Equipment.EquipmentType.Legs:
                ret._stats._name = "Pantalones";
                ret._sprite = Sprites.Instance.equipment.Legs;
                break;
            default:
                break;
        }

        ret._cost = 10;

        return ret;
    }
}
