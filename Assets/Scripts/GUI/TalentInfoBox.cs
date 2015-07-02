using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalentInfoBox : MonoBehaviour {

    public TalentInfoBox instance;
    private static TalentInfoBox _instance;
    public static TalentInfoBox Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("<color=red>SUPERFAIL - talent info box not sett</color>");
            }
            return _instance;
        }
    }

    public Text nameText;
    public Text levelText;

    public LevelInfo levelInfo;
    public EffectInfo currentInfo;
    public EffectInfo nextInfo;

    public Button acceptButton;

    public bool IsUp
    {
        get
        {
            return gameObject.activeSelf;
        }
        set
        {
            if (value)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    [HideInInspector]
    public TalentButton _lastButton = null;

	// Use this for initialization
	void Start () {
        _instance = instance;

        IsUp = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AcceptButton()
    {
        AddStatsToPlayer();
        LevelUpTalentAndShit();
        _lastButton.Click();

        Global.Instance._player.UpdateCombinedStats();
    }

    private void AddStatsToPlayer()
    {
        Player _player = Global.Instance._player;

        switch (_lastButton._talentType)
        {
            case TalentButton.TalentTypes.hpPercent:
                if (_player._talentStats._healtPercent.value == 0f)  // första gången
                    _player._talentStats._healtPercent.value = _lastButton._stats._healtPercent.value;
                else                                                    // alla andra gånger'
                    _player._talentStats._healtPercent.value *= _lastButton._stats._healtPercent.value;

                if (_lastButton._totalStats._healtPercent.value == 0f)
                    _lastButton._totalStats._healtPercent.value = _lastButton._stats._healtPercent.value;
                else
                    _lastButton._totalStats._healtPercent.value *= _lastButton._stats._healtPercent.value;
                break;
            case TalentButton.TalentTypes.dmgPercent:
                if (_lastButton._totalStats._damagePercent.value == 0f)  // första gången
                    _player._talentStats._damagePercent.value = _lastButton._stats._damagePercent.value;
                else                                                    // alla andra gånger'
                    _player._talentStats._damagePercent.value *= _lastButton._stats._damagePercent.value;

                if (_lastButton._totalStats._damagePercent.value == 0f)
                    _lastButton._totalStats._damagePercent.value = _lastButton._stats._damagePercent.value;
                else
                    _lastButton._totalStats._damagePercent.value *= _lastButton._stats._damagePercent.value;
                break;
            case TalentButton.TalentTypes.hp:
                _player._talentStats._health += _lastButton._stats._health;

                _lastButton._totalStats._health += _lastButton._stats._health;
                break;
            case TalentButton.TalentTypes.dmg:
                _player._talentStats._normal.damage += _lastButton._stats._normal.damage;

                _lastButton._totalStats._normal.damage += _lastButton._stats._normal.damage;
                break;
            case TalentButton.TalentTypes.critChans:
                _player._talentStats._normal.crit += _lastButton._stats._normal.crit;

                _lastButton._totalStats._normal.crit += _lastButton._stats._normal.crit;
                break;
            case TalentButton.TalentTypes.critPercent:
                _player._talentStats._normal.critMultiplier += _lastButton._stats._normal.critMultiplier;

                _lastButton._totalStats._normal.critMultiplier += _lastButton._stats._normal.critMultiplier;
                break;
            case TalentButton.TalentTypes.hpPotionChans:
                Global.Instance._potionDropChans.value += (HealthPotion._chansIncrease.value - 1f);
                break;
            case TalentButton.TalentTypes.hpPotionHealingPercent:
                Global.Instance._potionHealthPercent.value += (HealthPotion._healingIncrease.value - 1f);
                break;
            case TalentButton.TalentTypes.AllResistance:
                if (_player._talentStats._kinetic.resistance == 0f)             // kinetic
                    _player._talentStats._kinetic.resistance = _lastButton._stats._normal.resistance;
                else
                    _player._talentStats._kinetic.resistance *= _lastButton._stats._normal.resistance;
                if (_player._talentStats._tech.resistance == 0f)                // tech
                    _player._talentStats._tech.resistance = _lastButton._stats._normal.resistance;
                else
                    _player._talentStats._tech.resistance = _lastButton._stats._normal.resistance;
                if (_player._talentStats._psychic.resistance == 0f)             // psychic
                    _player._talentStats._psychic.resistance = _lastButton._stats._normal.resistance;
                else
                    _player._talentStats._psychic.resistance *= _lastButton._stats._normal.resistance;

                if (_lastButton._totalStats._normal.resistance == 0f)
                    _lastButton._totalStats._normal.resistance = _lastButton._stats._normal.resistance;
                else
                    _lastButton._totalStats._normal.resistance *= _lastButton._stats._normal.resistance;
                break;
            default:
                break;
        }
    }

    private void LevelUpTalentAndShit()
    {
        _lastButton._level++;
        Global.Instance._player._unspentLevels--;

        DeterminButtonStatus();
    }

    [System.Serializable]
    public class LevelInfo
    {
        public Text currentLevelText;
        public Text maxLevelText;
    }
    [System.Serializable]
    public class EffectInfo
    {
        public Text statInfo;
        public Text statsInfoText;
    }

    /// <summary>
    /// if the acceptbutton should be active or not
    /// </summary>
    public void DeterminButtonStatus()
    {
        if ((Global.Instance._player._unspentLevels == 0) || (_lastButton._maxLevel != 0 && _lastButton._level == _lastButton._maxLevel))
        {
            TalentInfoBox.Instance.acceptButton.interactable = false;
        }
        else
        {
            TalentInfoBox.Instance.acceptButton.interactable = true;
        }
    }
}
