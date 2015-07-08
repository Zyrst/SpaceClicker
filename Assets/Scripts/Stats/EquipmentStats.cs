using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipmentStats {

    public CharacterStats.Element _normal = new CharacterStats.Element();
    public CharacterStats.Element _tech = new CharacterStats.Element();
    public CharacterStats.Element _kinetic = new CharacterStats.Element();
    public CharacterStats.Element _psychic = new CharacterStats.Element();

    public float _multiplierHealth;
    public float _multiplierDamage;
    public float _constMultiplier;
    public float _valueMultiplier;

    public float _basePower;
    public float _powerDiv;

    public uint _level;

    //Increase player health
    public vap _health = new vap();

    public vap _baseStat = new vap();

    public string _name = "Lovely big guns";
}
