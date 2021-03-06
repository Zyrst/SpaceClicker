﻿using UnityEngine;
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
                Debug.Log("<color=red>SUPERFAIL - talent info box not set</color>");

            }
            return _instance;
        }
    }

    public TalentInfoBox()
    {
        _instance = this;
    }

    public Text nameText;
    public Text levelText;

    public LevelInfo levelInfo;
    public EffectInfo currentInfo;
    public EffectInfo nextInfo;

    public Button acceptButton;

    public Text talentPointText;

    public enum Arms : int { Right = 0, Left = 1, Top = 2, Down = 3, NoArm = 4 };
    public uint[] pointsPerArm = new uint[] { 0, 0, 0, 0, 0 };

    public bool IsUp
    {
        get
        {
            //Global.DebugOnScreen(gameObject.activeSelf ? "true" : "false" , 60);
            return gameObject.activeSelf;
        }
        set
        {
            //Global.DebugOnScreen(value ? "true" : "false", 60);
            if (value)
            {
                gameObject.SetActive(true);
                talentPointText.text = Global.Instance.player._talentPoints.ToString();
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AcceptButton()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        AddStatsToPlayer();
        LevelUpTalentAndShit();
        _lastButton.Click();

        // add points per arm
        pointsPerArm[(int)_lastButton._arm]++;

        Global.Instance.player.UpdateCombinedStats();
        talentPointText.text = Global.Instance.player._talentPoints.ToString();
    }

    private void AddStatsToPlayer()
    {
        Player _player = Global.Instance.player;

        switch (_lastButton._talentType)
        {
            case TalentButton.TalentTypes.hpPercent:
                if (_player._talentStats._healtPercent.value == 0f)  // första gången
                    _player._talentStats._healtPercent.value = _lastButton._stats._healtPercent.value;
                else                                                    // alla andra gånger'
                    _player._talentStats._healtPercent.value += _lastButton._stats._healtPercent.value;

                if (_lastButton._totalStats._healtPercent.value == 0f)
                    _lastButton._totalStats._healtPercent.value = _lastButton._stats._healtPercent.value;
                else
                    _lastButton._totalStats._healtPercent.value += _lastButton._stats._healtPercent.value;
                break;
            case TalentButton.TalentTypes.dmgPercent:
                #region MyRegion
		        switch (_lastButton._element)
	            {
                    case TalentButton.Elements.All:
                        if (_lastButton._totalStats._normal._damagePercent.value == 0f)  // första gången
                            _player._talentStats._tech._damagePercent.value = _lastButton._stats._normal._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._tech._damagePercent.value += _lastButton._stats._normal._damagePercent.value;

                        if (_lastButton._totalStats._normal._damagePercent.value == 0f)  // första gången
                            _player._talentStats._kinetic._damagePercent.value = _lastButton._stats._normal._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._kinetic._damagePercent.value += _lastButton._stats._normal._damagePercent.value;

                        if (_lastButton._totalStats._normal._damagePercent.value == 0f)  // första gången
                            _player._talentStats._psychic._damagePercent.value = _lastButton._stats._normal._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._psychic._damagePercent.value += _lastButton._stats._normal._damagePercent.value;

                        if (_lastButton._totalStats._normal._damagePercent.value == 0f)
                            _lastButton._totalStats._normal._damagePercent.value = _lastButton._stats._normal._damagePercent.value;
                        else
                            _lastButton._totalStats._normal._damagePercent.value += _lastButton._stats._normal._damagePercent.value;
                        break;
                    case TalentButton.Elements.Normal:
                        if (_lastButton._totalStats._normal._damagePercent.value == 0f)  // första gången
                            _player._talentStats._normal._damagePercent.value = _lastButton._stats._normal._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._normal._damagePercent.value += _lastButton._stats._normal._damagePercent.value;

                        if (_lastButton._totalStats._normal._damagePercent.value == 0f)
                            _lastButton._totalStats._normal._damagePercent.value = _lastButton._stats._normal._damagePercent.value;
                        else
                            _lastButton._totalStats._normal._damagePercent.value += _lastButton._stats._normal._damagePercent.value;
                        break;
                    case TalentButton.Elements.Tech:
                        if (_lastButton._totalStats._tech._damagePercent.value == 0f)  // första gången
                            _player._talentStats._tech._damagePercent.value = _lastButton._stats._tech._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._tech._damagePercent.value += _lastButton._stats._tech._damagePercent.value;

                        if (_lastButton._totalStats._tech._damagePercent.value == 0f)
                            _lastButton._totalStats._tech._damagePercent.value = _lastButton._stats._tech._damagePercent.value;
                        else
                            _lastButton._totalStats._tech._damagePercent.value += _lastButton._stats._tech._damagePercent.value;
                        break;
                    case TalentButton.Elements.Kinetic:
                        if (_lastButton._totalStats._kinetic._damagePercent.value == 0f)  // första gången
                            _player._talentStats._kinetic._damagePercent.value = _lastButton._stats._kinetic._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._kinetic._damagePercent.value += _lastButton._stats._kinetic._damagePercent.value;

                        if (_lastButton._totalStats._kinetic._damagePercent.value == 0f)
                            _lastButton._totalStats._kinetic._damagePercent.value = _lastButton._stats._kinetic._damagePercent.value;
                        else
                            _lastButton._totalStats._kinetic._damagePercent.value += _lastButton._stats._kinetic._damagePercent.value;
                        break;
                    case TalentButton.Elements.Pshycic:
                        if (_lastButton._totalStats._psychic._damagePercent.value == 0f)  // första gången
                            _player._talentStats._psychic._damagePercent.value = _lastButton._stats._psychic._damagePercent.value;
                        else                                                    // alla andra gånger'
                            _player._talentStats._psychic._damagePercent.value += _lastButton._stats._psychic._damagePercent.value;

                        if (_lastButton._totalStats._psychic._damagePercent.value == 0f)
                            _lastButton._totalStats._psychic._damagePercent.value = _lastButton._stats._psychic._damagePercent.value;
                        else
                            _lastButton._totalStats._psychic._damagePercent.value += _lastButton._stats._psychic._damagePercent.value;
                        break;
                    default:
                        break;
	            } 
	#endregion
                break;
            case TalentButton.TalentTypes.hp:
                _player._talentStats._health += _lastButton._stats._health;

                _lastButton._totalStats._health += _lastButton._stats._health;
                break;
            case TalentButton.TalentTypes.dmg:
                _player._talentStats._normal.damage += _lastButton._stats._normal.damage;

                _lastButton._totalStats._normal.damage += _lastButton._stats._normal.damage;
                break;
            case TalentButton.TalentTypes.critPercent:
                #region MyRegion
		        switch (_lastButton._element)
	            {
                    case TalentButton.Elements.All:
                        _player._talentStats._normal.critMultiplier.value += _lastButton._stats._normal.critMultiplier.value;
                        _lastButton._totalStats._normal.critMultiplier.value += _lastButton._stats._normal.critMultiplier.value;
                        break;
                    case TalentButton.Elements.Normal:
                        _player._talentStats._normal.critMultiplier.value += _lastButton._stats._normal.critMultiplier.value;
                        _lastButton._totalStats._normal.critMultiplier.value += _lastButton._stats._normal.critMultiplier.value;
                        break;
                    case TalentButton.Elements.Tech:
                        _player._talentStats._tech.critMultiplier.value += _lastButton._stats._tech.critMultiplier.value;
                        _lastButton._totalStats._tech.critMultiplier.value += _lastButton._stats._tech.critMultiplier.value;
                        break;
                    case TalentButton.Elements.Kinetic:
                        _player._talentStats._kinetic.critMultiplier.value += _lastButton._stats._kinetic.critMultiplier.value;
                        _lastButton._totalStats._kinetic.critMultiplier.value += _lastButton._stats._kinetic.critMultiplier.value;
                        break;
                    case TalentButton.Elements.Pshycic:
                        _player._talentStats._psychic.critMultiplier.value += _lastButton._stats._psychic.critMultiplier.value;
                        _lastButton._totalStats._psychic.critMultiplier.value += _lastButton._stats._psychic.critMultiplier.value;
                        break;
                    default:
                        break;
	            } 
	#endregion
                break;
            case TalentButton.TalentTypes.critChans:
                #region MyRegion
                switch (_lastButton._element)
                {
                    case TalentButton.Elements.All:
                        _player._talentStats._normal.crit.value += _lastButton._stats._normal.crit.value;
                        _lastButton._totalStats._normal.crit.value += _lastButton._stats._normal.crit.value;
                        break;
                    case TalentButton.Elements.Normal:
                        _player._talentStats._normal.crit.value += _lastButton._stats._normal.crit.value;
                        _lastButton._totalStats._normal.crit.value += _lastButton._stats._normal.crit.value;
                        break;
                    case TalentButton.Elements.Tech:
                        _player._talentStats._tech.crit.value += _lastButton._stats._tech.crit.value;
                        _lastButton._totalStats._tech.crit.value += _lastButton._stats._tech.crit.value;
                        break;
                    case TalentButton.Elements.Kinetic:
                        _player._talentStats._kinetic.crit.value += _lastButton._stats._kinetic.crit.value;
                        _lastButton._totalStats._kinetic.crit.value += _lastButton._stats._kinetic.crit.value;
                        break;
                    case TalentButton.Elements.Pshycic:
                        _player._talentStats._psychic.crit.value += _lastButton._stats._psychic.crit.value;
                        _lastButton._totalStats._psychic.crit.value += _lastButton._stats._psychic.crit.value;
                        break;
                    default:
                        break;
                }  
	        #endregion
                break;
            case TalentButton.TalentTypes.hpPotionChans:
                Global.Instance._potionDropChans.value += Global.Instance._potionChansIncrease.value;
                break;
            case TalentButton.TalentTypes.hpPotionHealingPercent:
                Global.Instance._potionHealthPercent.value += Global.Instance._PotionHealingIncrease.value;
                break;
            case TalentButton.TalentTypes.AllResistance:
                if (_player._talentStats._kinetic.resistance.value == 0f)             // kinetic
                    _player._talentStats._kinetic.resistance.value = _lastButton._stats._normal.resistance.value;
                else
                    _player._talentStats._kinetic.resistance.value += _lastButton._stats._normal.resistance.value;

                if (_player._talentStats._tech.resistance.value == 0f)                // tech
                    _player._talentStats._tech.resistance.value = _lastButton._stats._normal.resistance.value;
                else
                    _player._talentStats._tech.resistance.value += _lastButton._stats._normal.resistance.value;

                if (_player._talentStats._psychic.resistance.value == 0f)             // psychic
                    _player._talentStats._psychic.resistance.value = _lastButton._stats._normal.resistance.value;
                else
                    _player._talentStats._psychic.resistance.value += _lastButton._stats._normal.resistance.value;

                if (_lastButton._totalStats._normal.resistance.value == 0f)
                    _lastButton._totalStats._normal.resistance.value = _lastButton._stats._normal.resistance.value;
                else
                    _lastButton._totalStats._normal.resistance.value += _lastButton._stats._normal.resistance.value;
                break;
            default:
                break;
        }
    }

    private void LevelUpTalentAndShit()
    {
        _lastButton._level++;
        Global.Instance.player._talentPoints--;

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
        if ((Global.Instance.player._talentPoints == 0) || (_lastButton._maxLevel != 0 && _lastButton._level == _lastButton._maxLevel))
        {
            TalentInfoBox.Instance.acceptButton.interactable = false;
        }
        else
        {
            TalentInfoBox.Instance.acceptButton.interactable = true;
        }
    }
}
