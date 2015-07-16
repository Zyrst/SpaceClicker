using UnityEngine;
using System.Collections;

public class DamageStats {
    public static DamageStats GenerateFromCharacterStats(CharacterStats stats_, bool alwaysCrit_)
    {
        DamageStats ds = new DamageStats();

        ds._normal = new vap(stats_._normal.damage);
        ds._kinetic = new vap(stats_._kinetic.damage);
        ds._tech = new vap(stats_._tech.damage);
        ds._psychic = new vap(stats_._psychic.damage);

        if (alwaysCrit_)
        {
            ds._normal = new vap(stats_._normal.critDamage);
            ds._tech = new vap(stats_._tech.critDamage);
            ds._psychic = new vap(stats_._psychic.critDamage);
            ds._kinetic = new vap(stats_._kinetic.critDamage);

            Global.Instance.ShakeCamera();
        }
        else
        {
            bool didaCrit = false;
            /* Make it crit*/
            float crit = Random.Range(0f, 1f);
            if (crit <= stats_._normal.crit)
            {
                ds._normal = new vap(stats_._normal.critDamage);
                if (stats_._normal.critDamage > stats_._normal.damage)
                didaCrit = true;
            }

            crit = Random.Range(0f, 1f);
            if (crit <= stats_._tech.crit)
            {
                ds._tech = new vap(stats_._tech.critDamage);
                if (stats_._tech.critDamage > stats_._tech.damage)
                didaCrit = true;
            }

            crit = Random.Range(0f, 1f);
            if (crit <= stats_._psychic.crit)
            {
                ds._psychic = new vap(stats_._psychic.critDamage);
                if (stats_._psychic.critDamage > stats_._psychic.damage)
                didaCrit = true;
            }

            crit = Random.Range(0f, 1f);
            if (crit <= stats_._kinetic.crit)
            {
                ds._kinetic = new vap(stats_._kinetic.critDamage);
                if (stats_._kinetic.critDamage > stats_._kinetic.damage)
                    didaCrit = true;
            }

            if (didaCrit)
            {
                Global.Instance.ShakeCamera();
            }
        }
       
        return ds;
    }

    public static DamageStats GenerateFromSpellStats(SpellStats stats_)
    {
        DamageStats ds = new DamageStats();

        ds._normal = stats_._normal.damage;
        ds._kinetic = stats_._kinetic.damage;
        ds._tech = stats_._tech.damage;
        ds._psychic = stats_._psychic.damage;

        ds._healPercent = stats_._healPercent;

        bool didaCrit = false;
        /* Make it crit*/
        float crit = Random.Range(0f, 1f);
        if (crit <= stats_._normal.crit)
        {
            ds._normal = stats_._normal.critDamage;
            didaCrit = true;
        }

        crit = Random.Range(0f, 1f);
        if (crit <= stats_._tech.crit)
        {
            ds._tech = stats_._tech.critDamage;
            didaCrit = true;
        }

        crit = Random.Range(0f, 1f);
        if (crit <= stats_._psychic.crit)
        {
            ds._psychic = stats_._psychic.critDamage;
            didaCrit = true;
        }

        crit = Random.Range(0f, 1f);
        if (crit <= stats_._kinetic.crit)
        {
            ds._kinetic = stats_._kinetic.critDamage;
            didaCrit = true;
        }

        if (didaCrit)
        {
            Global.Instance.ShakeCamera();
        }

        ds._heal = stats_._heal;
        ds._lifeSteal = stats_._lifeSteal;
        ds._stunTime = stats_._stunTime;
        ds._slowTime = stats_._slowTime;
        return ds;
    }

    public vap _normal = new vap();
    public vap _tech = new vap();
    public vap _kinetic = new vap();
    public vap _psychic = new vap();
    public vap _heal = new vap();
    public vap _lifeSteal = new vap();
    public float _stunTime = 0f;
    public float _slowTime = 0f;
    public float _healPercent = 0;
}
