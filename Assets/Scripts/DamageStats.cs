using UnityEngine;
using System.Collections;

public class DamageStats {
    public static DamageStats GenerateFromCharacterStats(CharacterStats stats_)
    {
        DamageStats ds = new DamageStats();

        ds._normal = stats_._normal.damage;
        ds._kinetic = stats_._kinetic.damage;
        ds._tech = stats_._tech.damage;
        ds._psychic = stats_._psychic.damage;

        /* Make it crit*/
        float crit = Random.Range(0f, 1f);
        if (crit <= stats_._normal.crit)
        {
            ds._normal = stats_._normal.critDamage;
        }

        crit = Random.Range(0f, 1f);
        if (crit <= stats_._tech.crit)
        {
            ds._tech = stats_._tech.critDamage;
        }

        crit = Random.Range(0f, 1f);
        if (crit <= stats_._psychic.crit)
        {
            ds._psychic = stats_._psychic.critDamage;
        }

        crit = Random.Range(0f, 1f);
        if (crit <= stats_._kinetic.crit)
        {
            ds._kinetic = stats_._kinetic.critDamage;
        }


        return ds;
    }

    public float _normal;
    public float _tech;
    public float _kinetic;
    public float _psychic;
}
