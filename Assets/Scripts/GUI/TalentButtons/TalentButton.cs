using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalentButton : MonoBehaviour {
    [System.Serializable]
    public class SmallStuff
    {
        
    }

    public string _name = "";
    public TalentTypes _talentType = TalentTypes.hpPercent;

    public TalentStats _stats = new TalentStats();
    public TalentStats _totalStats = new TalentStats();
    public SmallStuff _smallStuff = new SmallStuff();

    public uint _level = 0;
    public uint _maxLevel = 0;
    public string maxLevel
    {
        get
        {
            return _maxLevel == 0 ? "INF" : _maxLevel.ToString();
        }
    }

    public enum TalentTypes { hpPercent, dmgPercent, hp, dmg, critChans, critPercent, hpPotionChans, hpPotionHealingPercent, AllResistance };


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        TalentInfoBox.Instance.IsUp = true;

        TalentInfoBox.Instance._lastButton = this;
        TalentInfoBox.Instance.DeterminButtonStatus();

        TalentInfoBox.Instance.nameText.text = _name;
        TalentInfoBox.Instance.levelText.text = _level.ToString();

        TalentInfoBox.Instance.levelInfo.currentLevelText.text = _level.ToString();
        TalentInfoBox.Instance.levelInfo.maxLevelText.text = maxLevel;

        SeeWhatTypeItIs();
    }

    public void SeeWhatTypeItIs()
    {
        switch (_talentType)
        {
            case TalentTypes.hpPercent:         // HP%
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Health:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._healtPercent.PerCent;

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Health:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._healtPercent.PerCent;
                break;
            case TalentTypes.dmgPercent:        // DMG%
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Damage:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._damagePercent.PerCent;

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Damage:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._damagePercent.PerCent;
                break;
            case TalentTypes.hp:                // HP
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Health:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._health.GetFloat();

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Health:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._health.GetFloat();
                break;
            case TalentTypes.dmg:               // DMG
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Damage:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.damage.GetFloat();

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Damage:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.damage.GetFloat();
                break;
            case TalentTypes.critChans:       // crit%
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Critical:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.critMultiplier + "%";

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Critical:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.critMultiplier + "%";
                break;
            case TalentTypes.critPercent:              // crit%
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Crit. Damage:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.crit + "%";

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Crit. Damage:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.crit + "%";
                break;
            case TalentTypes.hpPotionChans:     // potion drop%
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Potion Drop:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + Global.Instance._potionDropChans.PerCent;
                
                TalentInfoBox.Instance.nextInfo.statInfo.text = "Potion Drop:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + HealthPotion._chansIncrease.PerCent;
                break;
            case TalentTypes.hpPotionHealingPercent:    // healing %
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Potion Healing:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + Global.Instance._potionHealthPercent.PerCent;

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Potion Healing:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + HealthPotion._healingIncrease.PerCent;
                break;
            case TalentTypes.AllResistance:             // resistance %
                TalentInfoBox.Instance.currentInfo.statInfo.text = "Resistance:";
                TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + (_totalStats._normal.resistance == 0f ? "0%" : ((_totalStats._normal.resistance - 1f) * 100f).ToString("N2") + "%");

                TalentInfoBox.Instance.nextInfo.statInfo.text = "Resistance:";
                TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + ((_stats._normal.resistance-1f) * 100f).ToString("N2") + "%";

                break;
            default:
                break;
        }
    }
}
