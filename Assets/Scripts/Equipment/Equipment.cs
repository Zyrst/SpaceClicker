using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Equipment : MonoBehaviour {

    public EquipmentStats _stats = new EquipmentStats();
    public Sprite _sprite = new Sprite();

    public enum EquipmentType
    {
        Weapon = 0, Head = 1, Chest = 2, Legs = 3
    }

    public enum ElementType : int { Tech = 0, Kinetic = 1, Pshycic = 2, Normal = 3 };
    public enum Rareness : int { Green = 0, Blue = 2, Purple = 5, Orange = 10 };

    public EquipmentType _type;
    public ElementType _element;
    public Rareness _rareness;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Popup()
    {
        CharacterScreen.Instance.PopItUp(this);
    }
}
