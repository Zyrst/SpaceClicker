﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SpellInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate(SpellAttack spell_)
    {
        gameObject.SetActive(true);
        SpellStats tmpStat = spell_.GetTempSpellStats();
        Text txt = gameObject.GetComponentInChildren<Text>();
        string info = spell_.gameObject.name;

        #region SpellTargets
        switch (spell_._target)
        {
            case SpellAttack.SpellTarget.Single:
                info += System.Environment.NewLine + "Attack Type: Single";
                break;
            case SpellAttack.SpellTarget.Adjacent:
                info += System.Environment.NewLine + "Attack Type: Adjacent";
                break;
            case SpellAttack.SpellTarget.EnemiesAndPlayer:
                info += System.Environment.NewLine + "Attack Type: Enemies and Player";
                break;
            case SpellAttack.SpellTarget.Enemies:
                info += System.Environment.NewLine + "Attack Type: AoE";
                break;
        }
        #endregion


        #region SpellType
        switch (spell_._type)
        {
            case SpellAttack.SpellType.Damage:
                break;
            case SpellAttack.SpellType.Heal:
                info += System.Environment.NewLine + "Type: Heal";
                break;
            case SpellAttack.SpellType.Stun:
                info += System.Environment.NewLine + "Type: Stun";
                break;
            case SpellAttack.SpellType.Shield:
                info += System.Environment.NewLine + "Type: Shield";
                break;
            case SpellAttack.SpellType.TimeBuff:
                info += System.Environment.NewLine + "Type: Time Buff";
                break;
        }
        #endregion

        #region Heal and Shield
        if (spell_._type == SpellAttack.SpellType.Shield)
        {
            info += System.Environment.NewLine + "Shield Duration: "  + tmpStat._shieldTime;
        }
        
        
        if (tmpStat.hasHeal)
        {
            info += System.Environment.NewLine + "Heals: " + tmpStat._heal.GetString();
        }

        if (tmpStat._healPercent > 0f)
        {
            info += System.Environment.NewLine + "Heals: " + tmpStat._healPercent + "%";
        }
        #endregion

        #region Damage
        if (tmpStat.hasDamage)
        {
            if (tmpStat._normal.damage.GetFloat() > 0f)
            {
                info += System.Environment.NewLine + "(Nor.) Damage: " + tmpStat._normal.damage.GetString();
            }

            if (tmpStat._tech.damage.GetFloat() > 0f)
            {
                info += System.Environment.NewLine + "(Tec.) Damage: " + tmpStat._tech.damage.GetString();
            }

            if (tmpStat._kinetic.damage.GetFloat() > 0f)
            {
                info += System.Environment.NewLine + "(Kin.) Damage: " + tmpStat._kinetic.damage.GetString();
            }

            if (tmpStat._psychic.damage.GetFloat() > 0f)
            {
                info += System.Environment.NewLine + "(Psy.) Damage: " + tmpStat._psychic.damage.GetString();
            }

            if (tmpStat._healthDamagePercent > 0f)
            {
                vap hlthdmgperc = new vap(tmpStat._healthDamagePercent * Global.Instance._player._stats._health);
                info += System.Environment.NewLine + "(Pure) Damage: " + hlthdmgperc.GetString();
            }
        }
        #endregion

        #region Stun and Lifesteal + Health Damage
        if (tmpStat.hasStun)
        {
            info += System.Environment.NewLine + "Stun Duration: " + tmpStat._stunTime;
        }

        if (tmpStat.hasLifeSteal || tmpStat._lifeStealAmount > 0f)
        {
            info += System.Environment.NewLine + "Lifesteal: Yes";
        }

        if (tmpStat._healthDamagePercent > 0f)
        {
            info += System.Environment.NewLine + "Damage % from Player Health: Yes";
        }
        #endregion

        #region Slow
        if (tmpStat.hasSlow)
        {
            info += System.Environment.NewLine + "Slow time: " + tmpStat._slowTime;
        }
        #endregion

        if (spell_._trigger == SpellAttack.SpellTrigger.Click)
        {
            info += System.Environment.NewLine + "Activates on Click";
        }
        //TODO : Read description for a spell


        txt.text = info;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}