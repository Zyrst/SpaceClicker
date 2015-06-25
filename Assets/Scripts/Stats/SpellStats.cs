using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpellStats {

    public CharacterStats.Element _normal;
    public CharacterStats.Element _tech;
    public CharacterStats.Element _kinetic;
    public CharacterStats.Element _psychic;
    public vap _heal;
    public vap _lifeSteal;
    public float _stunTime;
    public float _cooldown;

    public bool hasDamage
    {
        get { return _normal.damage.GetFloat() > 0f || _tech.damage.GetFloat() > 0f || _kinetic.damage.GetFloat() > 0f || _psychic.damage.GetFloat() > 0f; }
    }
    public bool hasHeal
    {
        get { return _heal.GetFloat() > 0f; }
    }
    public bool hasLifeSteal
    {
        get { return _lifeSteal.GetFloat() > 0f; }
    }
    public bool hasStun
    {
        get { return _stunTime > 0f; }
    }

    public SpellStats() { }

    public SpellStats(SpellStats stats_)
    {
        this._normal = new CharacterStats.Element(stats_._normal);
        this._kinetic = new CharacterStats.Element(stats_._kinetic);
        this._tech = new CharacterStats.Element(stats_._tech);
        this._psychic = new CharacterStats.Element(stats_._psychic);

        this._heal = stats_._heal;
        this._lifeSteal = stats_._lifeSteal;
        this._stunTime = stats_._stunTime;
        this._cooldown = stats_._cooldown;
    }

    /// <summary>
    /// Add characters base stats to the spell
    /// </summary>
    /// <param name="stats_">Character combine stats</param>
    public void AddStats(CharacterStats stats_)
    {
        if (_normal.damage.GetFloat() > 0)
        {
            _normal.damage += stats_._normal.damage;
            _cooldown -= stats_._normal.cooldownReduction;
            _normal.crit += stats_._normal.crit;
            _normal.critMultiplier += stats_._normal.critMultiplier;
        }

        if (_tech.damage.GetFloat() > 0)
        {
            _tech.damage += stats_._tech.damage;
            _cooldown -= stats_._tech.cooldownReduction;
            _tech.crit += stats_._tech.crit;
            _tech.critMultiplier += stats_._tech.critMultiplier;
        }

        if (_kinetic.damage.GetFloat() > 0)
        {
            _kinetic.damage += stats_._kinetic.damage;
            _cooldown -= stats_._kinetic.cooldownReduction;
            _kinetic.crit += stats_._kinetic.crit;
            _kinetic.critMultiplier += stats_._kinetic.critMultiplier;
        }

        if (_psychic.damage.GetFloat() > 0)
        {
            _psychic.damage += stats_._psychic.damage;
            _cooldown -= stats_._psychic.cooldownReduction;
            _psychic.crit += stats_._psychic.crit;
            _psychic.critMultiplier += stats_._psychic.critMultiplier;
        }
    }
}
