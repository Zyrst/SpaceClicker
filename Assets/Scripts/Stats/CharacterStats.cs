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

    public float _baseStat;

    public float _health;
    public float _maxHealth;

    public float _multiplierHealth;
    public float _multiplierDamage;
    public float _constMultiplier;
    public float _valueMultiplier;

    public float _basePower;
    public float _powerDiv;

    public float _healthStatDist;
    public float _damageStatDist;
    
    public Element _normal;
    public Element _tech;
    public Element _kinetic;
    public Element _psychic;

    /// <summary>
    /// Calculate stats according to level 
    /// </summary>
    /// <param name="level_"> Characters new level</param>
    public void LevelUp(uint level_)
    {
        /*_maxHealth *= Mathf.Pow(Global.Instance._healthScale, level_);
        _health = _maxHealth;
        _normal.damage *= Mathf.Pow(Global.Instance._damageScale, level_);*/

        _baseStat = (_constMultiplier * level_ + (Mathf.Pow(_basePower, (level_ / _powerDiv)))) * _valueMultiplier;
        _maxHealth = (_baseStat * _multiplierHealth) * _healthStatDist;
        _health = _maxHealth;
        _normal.damage = (_baseStat * _multiplierDamage) * _damageStatDist + 1;
        
    }
}
