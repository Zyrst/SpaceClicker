using UnityEngine;
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

        #region SpellTargets
        switch (spell_._target)
        {
            case SpellAttack.SpellTarget.Single:
                info += System.Environment.NewLine + "AttackType: Single";
                break;
            case SpellAttack.SpellTarget.Adjacent:
                info += System.Environment.NewLine + "AttackType: Adjacent";
                break;
            case SpellAttack.SpellTarget.EnemiesAndPlayer:
                info += System.Environment.NewLine + "AttackType: Enemies and Player";
                break;
            case SpellAttack.SpellTarget.Enemies:
                info += System.Environment.NewLine + "AttackType: AoE";
                break;
        }
        #endregion
        if (spell_._trigger == SpellAttack.SpellTrigger.Click)
        {
            info += System.Environment.NewLine + "Activates on Click";
        }

        if (spell_._type == SpellAttack.SpellType.Shield)
        {
            info += System.Environment.NewLine + "Shield Duration: "  + tmpStat._shieldTime;
        }
        
        
        if (tmpStat.hasHeal)
        {
            info += System.Environment.NewLine + tmpStat._heal.GetString();
        }

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
        }
        #endregion

        if (tmpStat.hasStun)
        {
            info += System.Environment.NewLine + "Stun Duration: " + tmpStat._stunTime;
        }

        if (tmpStat.hasLifeSteal)
        {
            info += System.Environment.NewLine + "Lifesteal: Yes";
        }

        txt.text = info;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
