using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalentButton : MonoBehaviour {

    public string _name = "";
    public TalentTypes _talentType = TalentTypes.hpPercent;

    public TalentStats _stats = new TalentStats();
    public TalentStats _totalStats = new TalentStats();

    public uint _level = 0;
    public uint _maxLevel = 0;
    public string maxLevel
    {
        get
        {
            return _maxLevel == 0 ? "INF" : _maxLevel.ToString();
        }
    }

    public enum TalentTypes { hpPercent, dmgPercent, hp, dmg, critChans, critPercent };


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
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
            default:
                break;
        }
    }
}
