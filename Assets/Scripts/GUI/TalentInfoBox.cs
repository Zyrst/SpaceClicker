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
                Debug.LogError("SUPERFAIL - talent info box not sett");
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

    [HideInInspector]
    public TalentButton _lastButton = null;

	// Use this for initialization
	void Start () {
        _instance = instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AcceptButton()
    {
        AddStatsToPlayer();
        LevelUpTalentAndShit();
    }

    private void AddStatsToPlayer()
    {
        Player _player = Global.Instance._player;

        switch (_lastButton._talentType)
        {
            case TalentButton.TalentTypes.hpPercent:
                _player._stats._maxHealth *= _lastButton._stats._healtPercent.value;

                if (_lastButton._totalStats._healtPercent.value == 0f)
                    _lastButton._totalStats._healtPercent.value = _lastButton._stats._healtPercent.value;

                _lastButton._totalStats._healtPercent.value *= _lastButton._stats._healtPercent.value;
                break;
            case TalentButton.TalentTypes.dmgPercent:
                _player._stats._normal.damage *= _lastButton._stats._damagePercent.value;
                break;
            case TalentButton.TalentTypes.hp:
                _player._stats._maxHealth += _lastButton._stats._health;
                break;
            case TalentButton.TalentTypes.dmg:
                _player._stats._normal.damage += _lastButton._stats._normal.damage;
                break;
            case TalentButton.TalentTypes.critChans:
                _player._stats._normal.crit += _lastButton._stats._normal.crit;
                break;
            case TalentButton.TalentTypes.critPercent:
                _player._stats._normal.critMultiplier += _lastButton._stats._normal.critMultiplier;
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
        if (Global.Instance._player._unspentLevels == 0)
        {
            TalentInfoBox.Instance.acceptButton.interactable = false;
        }
        else
        {
            TalentInfoBox.Instance.acceptButton.interactable = true;
        }
    }
}
