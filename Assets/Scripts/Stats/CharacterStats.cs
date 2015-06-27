using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterStats {
    [System.Serializable]
    public class Element
    {
        public vap damage = new vap();
        public float crit;
        public float critMultiplier;
        public float resistance;
        public float cooldownReduction; //Abilities


        public vap critDamage
        {
            get
            {
                return (damage * critMultiplier);
            }
        }

        public Element(Element element_)
        {
            this.damage = new vap(element_.damage);
            this.crit = element_.crit;
            this.critMultiplier = element_.critMultiplier;
            this.resistance = element_.resistance;
            this.cooldownReduction = element_.cooldownReduction;
        }

        public Element()
        { }
    }

    public vap _baseStat = new vap();

    public vap _health = new vap();
    public vap _maxHealth = new vap();

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
        _baseStat = new vap();
       /* _baseStat._values[0] = _constMultiplier * level_;
        _baseStat.Checker();

        //_baseStat._values[0] += Mathf.Pow(_basePower, (level_ / _powerDiv));
        _baseStat *= _valueMultiplier;
        */
        /*for (int i = 0; i < (int)_baseStat._prefix+1; i++)
        {
            _baseStat._values[i] = (_constMultiplier * level_ + (_baseStat._values[i] * 1.165438502f)) * _valueMultiplier;
            _baseStat.Checker();
        }*/



        //                                      1                               2                                   3
        _baseStat._values[0] = ((_constMultiplier * level_) + (Mathf.Pow(_basePower, (level_ / _powerDiv)))) * _valueMultiplier;

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\values.txt", false))
        {
            file.WriteLine("start " + level_.ToString());
        }

        _baseStat.Checker();

        _maxHealth = (_baseStat * _multiplierHealth) * _healthStatDist;
        _health = new vap(_maxHealth);
        _normal.damage = (_baseStat * _multiplierDamage) * _damageStatDist;
    }

    public CharacterStats(CharacterStats stats_)
    {
        this._normal = new Element(stats_._normal);
        this._kinetic = new Element(stats_._kinetic);
        this._tech = new Element(stats_._tech);
        this._psychic = new Element(stats_._psychic);
        this._maxHealth = new vap(stats_._maxHealth);
    }

    public CharacterStats()
    {

    }

    /// <summary>
    /// Add equipment to stats with base stats
    /// </summary>
    /// <param name="stats_">Equipment stats</param>
    public void AddStats(EquipmentStats stats_)
    {
        _normal.damage += stats_._normal.damage;
        _normal.crit += stats_._normal.crit;
        _normal.cooldownReduction += stats_._normal.cooldownReduction;
        _normal.resistance += stats_._normal.resistance;
        _normal.critMultiplier += stats_._normal.critMultiplier;

        _tech.damage += stats_._tech.damage;
        _tech.crit += stats_._tech.crit;
        _tech.cooldownReduction += stats_._tech.cooldownReduction;
        _tech.resistance += stats_._tech.resistance;
        _tech.critMultiplier += stats_._tech.critMultiplier;

        _kinetic.damage += stats_._kinetic.damage;
        _kinetic.crit += stats_._kinetic.crit;
        _kinetic.cooldownReduction += stats_._kinetic.cooldownReduction;
        _kinetic.resistance += stats_._kinetic.resistance;
        _kinetic.critMultiplier += stats_._kinetic.critMultiplier;

        _psychic.damage += stats_._psychic.damage;
        _psychic.crit += stats_._psychic.crit;
        _psychic.cooldownReduction += stats_._psychic.cooldownReduction;
        _psychic.resistance += stats_._psychic.resistance;
        _psychic.critMultiplier += stats_._psychic.critMultiplier;

        _maxHealth += stats_._health;
    }
}
