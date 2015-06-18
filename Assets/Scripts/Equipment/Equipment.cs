using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Equipment : MonoBehaviour {

    public CharacterStats _stats = new CharacterStats();
    public Sprite _sprite = new Sprite();

    public enum EquipmentType
    {
        Weapon,Head,Chest,Legs
    }

    public EquipmentType _type;

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
