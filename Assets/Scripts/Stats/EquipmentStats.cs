using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipmentStats {

    public CharacterStats.Element _normal = new CharacterStats.Element();
    public CharacterStats.Element _tech = new CharacterStats.Element();
    public CharacterStats.Element _kinetic = new CharacterStats.Element();
    public CharacterStats.Element _psychic = new CharacterStats.Element();

    //Increase player health
    public vap _health = new vap();

    public string _name = "Gorilla Pants of Death";
}
