using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Equipment : MonoBehaviour {

    public EquipmentStats _stats = new EquipmentStats();
    public SpriteRef _sprite;

    public enum EquipmentType
    {
        Weapon = 0, Head = 1, Chest = 2, Legs = 3
    }

    public enum ElementType : int { Tech = 0, Kinetic = 1, Pshycic = 2, Normal = 3 };
    public enum Rareness : int { Green = 0, Blue = 2, Purple = 5, Orange = 10 };

    public EquipmentType _type;
    public ElementType _element;
    public Rareness _rareness;

    public uint _cost = 0;
    public uint _sellValue = 0;

	// Use this for initialization
	void Start () {
        _sellValue = 8;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Popup()
    {
        CharacterScreen.Instance.PopItUp(this);
    }

    public void EqupipedPop()
    {
        CharacterScreen.Instance.EquipedPop(this);
    }

    public string GetRarity()
    {
        string ret = "";
        switch (_rareness)
        {
            case Rareness.Green:
                ret = "Common";
                break;
            case Rareness.Blue:
                ret = "Uncommon";
                break;
            case Rareness.Purple:
                ret = "Dank Purple";
                break;
            case Rareness.Orange:
                ret = "Orange Juice";
                break;
            default:
                break;
        }
        return ret;
    }
}
