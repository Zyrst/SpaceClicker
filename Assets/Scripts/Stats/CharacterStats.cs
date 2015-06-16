using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterStats {
    [System.Serializable]
    public class Element
    {
        public float damage;
        public float crit;
        public float critMultiplier;
        public float resistance;

        public float critDamage
        {
            get
            {
                return (damage * critMultiplier);
            }
        }
    }

    public float _health;
    public float _maxHealth;
    public Element _normal;
    public Element _tech;
    public Element _kinetic;
    public Element _psychic;

    public void LevelUp()   {   LevelUp(1);     }

    public void LevelUp(uint level_)
    {
        _maxHealth *= Mathf.Pow(Global.Instance._healthScale, level_);
        _health = _maxHealth;
        _normal.damage *= Mathf.Pow(Global.Instance._damageScale, level_);
    }

}
