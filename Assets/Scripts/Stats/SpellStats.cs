using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpellStats {
    [System.Serializable]
    public class Element
    {
        public float damage;
        public float crit;
        public float critMultiplier;

        public float critDamage
        {
            get
            {
                return (damage * critMultiplier);
            }
        }
    }

    public Element _normal;
    public Element _tech;
    public Element _kinetic;
    public Element _psychic;
    public float _heal;
    public float _lifeSteal;
    public float _stunTime;
    public float _cooldown;

    public bool hasDamage
    {
        get { return _normal.damage > 0f || _tech.damage > 0f || _kinetic.damage > 0f || _psychic.damage > 0f; }
    }
    public bool hasHeal
    {
        get { return _heal > 0f; }
    }
    public bool hasLifeSteal
    {
        get { return _lifeSteal > 0f; }
    }
    public bool hasStun
    {
        get { return _stunTime > 0f; }
    }
}
