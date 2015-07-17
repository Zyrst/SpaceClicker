using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpellStats {

    public CharacterStats.Element _normal;
    public CharacterStats.Element _tech;
    public CharacterStats.Element _kinetic;
    public CharacterStats.Element _psychic;

    public float _normalDamageMultiplier;
    public float _techDamageMultiplier;
    public float _kineticDamageMultiplier;
    public float _psychicDamageMultiplier;

    [Range(0f, 1f)]
    public float _healthDamagePercent;

    public vap _heal;

    public float _healPercent;

    public vap _lifeSteal;
    [Range(0f,1f)]
    public float _lifeStealAmount = 0f;
    public float _stunTime;
    public float _slowTime;
    [Range(0f,1f)]
    public float _slowSpeed;
    public float _shieldTime;
    public float _buffTime;
    [Range(0f, 1f)]
    public float _cooldownModifier;

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
    public bool hasSlow
    {
        get { return _slowTime > 0f; }
    }

    public SpellStats() 
    {
        _normal = new CharacterStats.Element();
        _kinetic = new CharacterStats.Element();
        _tech = new CharacterStats.Element();
        _psychic = new CharacterStats.Element();

        _heal = new vap();
        _lifeSteal = new vap();
    }

    public SpellStats(SpellStats stats_)
    {
        this._normal = new CharacterStats.Element(stats_._normal);
        this._kinetic = new CharacterStats.Element(stats_._kinetic);
        this._tech = new CharacterStats.Element(stats_._tech);
        this._psychic = new CharacterStats.Element(stats_._psychic);

        this._normalDamageMultiplier = stats_._normalDamageMultiplier;
        this._techDamageMultiplier = stats_._techDamageMultiplier;
        this._kineticDamageMultiplier = stats_._kineticDamageMultiplier;
        this._psychicDamageMultiplier = stats_._psychicDamageMultiplier;

        this._healthDamagePercent = stats_._healthDamagePercent;

        this._heal = new vap(stats_._heal);

        this._healPercent = stats_._healPercent;

        this._lifeSteal = new vap(stats_._lifeSteal);
        this._lifeStealAmount = stats_._lifeStealAmount;

        this._stunTime = stats_._stunTime;
        this._slowTime = stats_._slowTime;
        this._slowSpeed = stats_._slowSpeed;
        this._shieldTime = stats_._shieldTime;
        this._buffTime = stats_._buffTime;
        this._cooldown = stats_._cooldown;
    }

    /// <summary>
    /// Add characters base stats to the spell
    /// </summary>
    /// <param name="stats_">Character combine stats</param>
    public void AddStats(CharacterStats stats_)
    {
        _normal.damage += stats_._normal.damage;
        if (_normal.damage.GetFloat() > 0)
        {
            _normal.damage *= _normalDamageMultiplier;
            _cooldown -= stats_._normal.cooldownReduction;
            _normal.crit += stats_._normal.crit;
            _normal.critMultiplier += stats_._normal.critMultiplier;
        }

        _tech.damage += stats_._tech.damage;
        if (_tech.damage.GetFloat() > 0)
        {
            _tech.damage *= _techDamageMultiplier;
            _cooldown -= stats_._tech.cooldownReduction;
            _tech.crit += stats_._tech.crit;
            _tech.critMultiplier += stats_._tech.critMultiplier;
        }

        _kinetic.damage += stats_._kinetic.damage;
        if (_kinetic.damage.GetFloat() > 0)
        {
            _kinetic.damage *= _kineticDamageMultiplier;
            _cooldown -= stats_._kinetic.cooldownReduction;
            _kinetic.crit += stats_._kinetic.crit;
            _kinetic.critMultiplier += stats_._kinetic.critMultiplier;
        }

        _psychic.damage += stats_._psychic.damage;
        if (_psychic.damage.GetFloat() > 0)
        {
            _psychic.damage *= _psychicDamageMultiplier;
            _cooldown -= stats_._psychic.cooldownReduction;
            _psychic.crit += stats_._psychic.crit;
            _psychic.critMultiplier += stats_._psychic.critMultiplier;
        }
    }
}
