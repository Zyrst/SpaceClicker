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
    public Elements _element = Elements.All;

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
    public enum Elements { All, Normal, Tech, Kinetic, Pshycic };


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
                #region per element
		        TalentInfoBox.Instance.currentInfo.statInfo.text = "Damage:";
                TalentInfoBox.Instance.nextInfo.statInfo.text = "Damage:";
                switch (_element)
	            {
                    case Elements.All:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal._damagePercent.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal._damagePercent.PerCent;
                        break;
                    case Elements.Normal:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal._damagePercent.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal._damagePercent.PerCent;
                        break;
                    case Elements.Tech:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._tech._damagePercent.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._tech._damagePercent.PerCent;
                        break;
                    case Elements.Kinetic:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._kinetic._damagePercent.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._kinetic._damagePercent.PerCent;
                        break;
                    case Elements.Pshycic:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._psychic._damagePercent.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._psychic._damagePercent.PerCent;
                        break;
                    default:
                        break;
	            } 
	#endregion
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
            case TalentTypes.critPercent:       // crit%
                #region per element
		        TalentInfoBox.Instance.currentInfo.statInfo.text = "Crit. Damage:";
                TalentInfoBox.Instance.nextInfo.statInfo.text = "Crit. Damage:";
                switch (_element)
	            {
                    case Elements.All:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.critMultiplier.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.critMultiplier.PerCent;
                        break;
                    case Elements.Normal:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.critMultiplier.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.critMultiplier.PerCent;
                        break;
                    case Elements.Tech:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._tech.critMultiplier.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._tech.critMultiplier.PerCent;
                        break;
                    case Elements.Kinetic:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._kinetic.critMultiplier.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._kinetic.critMultiplier.PerCent;
                        break;
                    case Elements.Pshycic:
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._psychic.critMultiplier.PerCent;
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._psychic.critMultiplier.PerCent;
                        break;
                    default:
                        break;
	            } 
	#endregion
                break;
            case TalentTypes.critChans:              // crit%
                #region per element
		        TalentInfoBox.Instance.currentInfo.statInfo.text = "Critical:";
                TalentInfoBox.Instance.nextInfo.statInfo.text = "Critical:";
                switch (_element)
	            {
                    case Elements.All:
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.crit.PerCent;
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.crit.PerCent;
                        break;
                    case Elements.Normal:
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._normal.crit.PerCent;
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._normal.crit.PerCent;
                        break;
                    case Elements.Tech:
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._tech.crit.PerCent;
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._tech.crit.PerCent;
                        break;
                    case Elements.Kinetic:
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._kinetic.crit.PerCent;
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._kinetic.crit.PerCent;
                        break;
                    case Elements.Pshycic:
                        TalentInfoBox.Instance.nextInfo.statsInfoText.text = "+" + _stats._psychic.crit.PerCent;
                        TalentInfoBox.Instance.currentInfo.statsInfoText.text = "+" + _totalStats._psychic.crit.PerCent;
                        break;
                    default:
                        break;
	            } 
	#endregion
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
