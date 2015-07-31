using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SpellAttack : BaseAttack {

    private float _cdModifierTimer = 0f;
    private float _cooldownModifier = 0f;
    public void StartCooldownModifier(float modifier_, float time_)
    {
        _cdModifierTimer = time_;
        _cooldownModifier = modifier_;

        StartCoroutine(cdModifierTracker(this));
    }

    static IEnumerator cdModifierTracker(SpellAttack spell_)
    {
        while (spell_._cdModifierTimer > 0f)
        {
            spell_._cdModifierTimer -= Time.deltaTime;
            yield return null;
        }

        spell_._cooldownModifier = 0f;
    }

    public SpellStats _stats = new SpellStats();
    public SpellStats _combinedStats = new SpellStats();
    public SpriteRef _spellImage;

    [HideInInspector]
    public GameObject _slot;
    [HideInInspector]
    public Image _slotImage;

    private bool _clicked = false;
    private bool _cd = false;
    private float _coolDown = 0f;
    [HideInInspector]
    public Vector3 _startGUIPos;
    [HideInInspector]
    public Vector3 _followerDiff = Vector3.zero;

    [HideInInspector]
    public bool _stunned = false;
    [HideInInspector]
    public float _stunTime = 0f;
    [HideInInspector]
    public float _buffTime = 0f;

    public enum SpellType : int { Damage = 0, Heal = 1, Stun = 2, Shield = 3, TimeBuff = 4 };
    public enum SpellTarget : int { Single = 0, Adjacent = 1, EnemiesAndPlayer = 2, Enemies = 3 };
    public enum SpellTrigger : int { Drag = 0, Click = 1 };

    public SpellType _type = SpellType.Damage;
    public SpellTarget _target = SpellTarget.Single;
    public SpellTrigger _trigger = SpellTrigger.Drag;

    

    private FMOD.Studio.EventInstance _holdSound;
    private FMOD.Studio.EventInstance _readySound;
    private FMOD.Studio.EventInstance _useSound;
    private FMOD.Studio.EventInstance _takeSound;

	// Use this for initialization
	void Start () {
        _slot =  Global.Instance.player.getSpellSlot(this);
        if (_slot == null)
        {
            Debug.Log("slot är null");
        }
        if (_spellImage == null)
        {
            Debug.Log("_spellImage är null");
        }
        switch (_type)
        {
            case SpellType.Damage:
                
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.damage.ready);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.damage.take);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.damage.use);
                break;
            case SpellType.Heal:
               // _holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.heal.hold);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.heal.ready);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.heal.take);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.heal.use);
                break;
            case SpellType.Stun:
               // _holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.stun.hold);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.stun.ready);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.stun.take);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.stun.use);
                break;
            default:
                break;
        }

        CheckSpellSpecificSounds();
	}

    public void Init()
    {
        _slotImage = _slot.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Image");
        _slotImage.sprite = _spellImage.sprite;

        _startGUIPos = _slot.transform.position;
    }
	
	// Update is called once per frame
    void Update()
    {
        if (_clicked)
        {
            FollowMouse();
        }
        Cooldown();
        Stun();

        if (_buffTime > 0f)
        {
            _buffTime -= Time.deltaTime;
            if (_buffTime <= 0)
            {
                // undo buff
                Global.Instance.player.UpdateCombinedStats();
            }
        }
	}

    public void Clicked()
    {
        if (_trigger == SpellTrigger.Click && !_cd)
        {
            if (_type == SpellType.Shield)
            {
                Global.Instance.player._shield.Raise(_combinedStats._shieldTime);
                Global.Instance.player._shield.spellRef = this;
            }
            else if (_type == SpellType.TimeBuff)
            {
                Global.Instance.player._combinedStats._normal.damage *= _combinedStats._normalDamageMultiplier != 0f ? _combinedStats._normalDamageMultiplier : 1f;
                Global.Instance.player._combinedStats._tech.damage *= _combinedStats._techDamageMultiplier != 0f ? _combinedStats._techDamageMultiplier : 1f;
                Global.Instance.player._combinedStats._psychic.damage *= _combinedStats._psychicDamageMultiplier != 0f ? _combinedStats._psychicDamageMultiplier : 1f;
                Global.Instance.player._combinedStats._kinetic.damage *= _combinedStats._kineticDamageMultiplier != 0f ? _combinedStats._kineticDamageMultiplier : 1f;

                Global.Instance.player._combinedStats._normal.resistance += _combinedStats._normal.resistance;
                Global.Instance.player._combinedStats._tech.resistance += _combinedStats._tech.resistance;
                Global.Instance.player._combinedStats._kinetic.resistance += _combinedStats._kinetic.resistance;
                Global.Instance.player._combinedStats._psychic.resistance += _combinedStats._psychic.resistance;

                if (_stats._cooldownModifier > 0f)
                {
                    Player player = GetComponentInParent<Player>();
                    for (int i = 0; i < player._spellsArray.Length; i++)
                    {
                        if (player._spellsArray[i] != null && player._spellsArray[i] != this)
                        {
                            Debug.Log("modi: " + _stats._cooldownModifier + " time: " + _combinedStats._cooldown);
                            player._spellsArray[i].StartCooldownModifier(_stats._cooldownModifier, _combinedStats._cooldown);
                        }
                    }
                }

                _buffTime = _combinedStats._buffTime;
            }

            _cd = true;
            _coolDown = 0f;
            _slotImage.color = new Color(0.5f, 0.5f, 0.5f);

            ResetGUI();

            UseSpell(Global.Instance.GetAllEnemies().ToArray()[0] as Character, transform.position);
        }
        else if (!_clicked && !_cd && !_stunned)
        {
            try
            {
                //play take sound
                _takeSound.start();
                _holdSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                if (IsInvoking("PlayHoldSound"))
                {
                    CancelInvoke("PlayHoldSound");
                }
                Invoke("PlayHoldSound", (float)Sounds.GetLength(_takeSound) / 1000f);
            }
            catch (System.NullReferenceException) { }

            // start animation
            // then hold animation
            Global.Instance.player.Animator.SetTrigger("ability_start");
            switch (_type)
            {
                case SpellType.Damage:
                    Global.Instance.player.Animator.SetInteger("ability", 0);
                    break;
                case SpellType.Heal:
                    Global.Instance.player.Animator.SetInteger("ability", 1);
                    break;
                case SpellType.Stun:
                    Global.Instance.player.Animator.SetInteger("ability", 1);
                    break;
                default:
                    break;
            }

            _startGUIPos = _slot.transform.position;
            _followerDiff = MouseController.Instance.position - _slot.transform.position;
            _clicked = true;
            MouseController.Instance.locked = true;
            //Reduce color on slot a bit
            _slotImage.color = new Color(0.7f, 0.7f, 0.7f);
            //Color the trail renderer depending which type of spell
            switch (_type)
            {
                case SpellType.Damage:
                    FarmMode.Instance.GetComponentInChildren<TrailRenderer>().material.SetColor("_Color", Color.red);
                    break;
                case SpellType.Heal:
                    FarmMode.Instance.GetComponentInChildren<TrailRenderer>().material.SetColor("_Color", Color.green);
                    break;
                case SpellType.Stun:
                    FarmMode.Instance.GetComponentInChildren<TrailRenderer>().material.SetColor("_Color", Color.blue);
                    break;
                default:
                    break;
            }
            GetComponentInParent<ClickAttack>().HoldingSpell();
            Invoke("ResetCanDealDamage", 0.1f);
        }
    }

    public void FollowMouse()
    {
      /*  _slot.transform.position = new Vector3(_followerDiff.x + MouseController.Instance.position.x, 
            _followerDiff.y + MouseController.Instance.position.y,
            _slot.transform.position.z);*/
       
        
        if (!MouseController.Instance.buttonDown)
        {
            ResetGUI();
            _slotImage.color = new Color(1f, 1f, 1f);
            // reset animation 
            Global.Instance.player.Animator.SetTrigger("idle");
            Global.DebugOnScreen("PLAY IDLE");
        }
        else
        {
            CheckHit();
        }
    }

    public void CheckHit()
    {
            // mouse on the ground
            Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                try
                {
                    // hit enemy or player
                    if (hit.collider.transform.parent.parent.tag == "Enemy" && hit.collider.transform.parent.parent.GetComponent<Character>()._isAlive 
                        || hit.collider.transform.parent.parent.tag == "Player" && hit.collider.transform.parent.parent.GetComponent<Character>()._isAlive)
                    {
                        _slotImage.color = new Color(0.5f, 0.5f, 0.5f);
                        _cd = true;
                        ResetGUI();

                        // do damage
                        UseSpell(hit.collider.transform.parent.parent.gameObject.GetComponent<Character>(), hit.point);

                        // play use sound
                        try
                        {
                            _holdSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                            if (IsInvoking("PlayHoldSound"))
                            {
                                CancelInvoke("PlayHoldSound");
                            }
                        }
                        catch (System.NullReferenceException) {}
                        _useSound.start();

                        // do animation
                        Global.Instance.player.Animator.SetTrigger("ability_throw");

                        
                    }
                    else if (!hit.collider.transform.parent.parent.GetComponent<Character>()._isAlive)      // it was dead
                    {
                        ResetGUI();

                        // reset animation 
                        Global.Instance.player.Animator.SetTrigger("ability_throw");
                        Global.DebugOnScreen("PLAY IDLE");
                    }
                   
                }
                catch (System.NullReferenceException) { }
            }
    }

    public void ResetGUI()
    {
        _clicked = false;
        MouseController.Instance.locked = false;
        _slot.transform.position = _startGUIPos;
        GetComponentInParent<ClickAttack>().ReleasedSpell();
        FarmMode.Instance.GetComponentInChildren<TrailRenderer>().material.SetColor("_Color", Color.white);

        // stop hold sound
        try
        {
            _holdSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            if (IsInvoking("PlayHoldSound"))
            {
                CancelInvoke("PlayHoldSound");
            }
        }
        catch (System.NullReferenceException) { }
    }

    public void Cooldown()
    {
        if (_cd)
        {
            _coolDown += Time.deltaTime;
            if (_coolDown >= (_combinedStats._cooldown * (1f - _cooldownModifier)))
            {
                ResetCooldown();
                try
                {
                    // play ready sound
                    _readySound.start();
                }
                catch (System.NullReferenceException)
                {}
            }
        }
       
    }

    public void ResetCooldown()
    {
        try
        {
            _cd = false;
            _coolDown = 0f;
            _slotImage.color = new Color(1f, 1f, 1f);
        }
        catch (System.NullReferenceException) {}
    }

    public void Stun()
    {
        if (_stunned)
        {
            _stunTime -= Time.deltaTime;
            if (_stunTime <= 0f)
            {
                _stunned = false;
                if(!_cd)
                    _slotImage.color = new Color(1f, 1f, 1f);
            }
        }
    }

    public void ResetCanDealDamage()
    {
        MouseController.Instance._locked = false;
    }

    public void CombineSpellStats()
    {
        Global.DebugOnScreen("Combining spellstats");
        _combinedStats = new SpellStats(_stats);
        _combinedStats.AddStats(Global.Instance.player._combinedStats);
    }

    public SpellStats GetTempSpellStats()
    {
        SpellStats tmp = new SpellStats(_stats);
        tmp.AddStats(Global.Instance.player._combinedStats);

        return tmp;
    }

    public void Stunned(float stunTime_)
    {
        _stunned = true;
        _stunTime = stunTime_;
        _slotImage.color = new Color(0.5f, 0.5f, 0.5f);

    }

    public void PlayHoldSound()
    {
        try { _holdSound.start(); }
        catch (System.NullReferenceException) { }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void UseSpell(Character char_, Vector3 hitpoint_)
    {
        if (_type == SpellType.Shield)
        {

        }
        else
        {
            switch (_target)
            {
                case SpellTarget.Single:
                    {
                        char_.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hitpoint_, Global.Instance.player);
                    }
                    break;
                case SpellTarget.Adjacent:
                    {
                        Enemy[] enemies = (Enemy[])Global.Instance.GetAllEnemies().ToArray(typeof(Enemy));
                        int enemy = 0;
                        bool foundIt = false;
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            if (enemies[i] == char_)
                            {
                                enemy = i;
                                foundIt = true;
                            }
                        }

                        char_.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hitpoint_, Global.Instance.player);

                        if (foundIt)
                        {
                            if (enemy - 1 >= 0)
                            {
                                enemies[enemy - 1].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), enemies[enemy - 1].GetComponent<Transform>().position, Global.Instance.player);
                            }
                            if (enemy + 1 < enemies.Length)
                            {
                                enemies[enemy + 1].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), enemies[enemy + 1].GetComponent<Transform>().position, Global.Instance.player);
                            }
                        }
                    }
                    break;
                case SpellTarget.EnemiesAndPlayer:
                    {
                            Enemy[] enemies = (Enemy[])Global.Instance.GetAllEnemies().ToArray(typeof(Enemy));
                            for (int i = 0; i < enemies.Length; i++)
                            {
                                enemies[i].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), enemies[i].transform.position, Global.Instance.player);
                            }

                            Global.Instance.player.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), 
                                Global.Instance.player.transform.position, Global.Instance.player.GetComponent<Character>());
                    }
                    break;
                case SpellTarget.Enemies:
                    {
                        char_.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hitpoint_, Global.Instance.player);

                        Enemy[] enemies = (Enemy[])Global.Instance.GetAllEnemies().ToArray(typeof(Enemy));
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            if (enemies[i] != char_)
                            {
                                enemies[i].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), enemies[i].GetComponent<Transform>().position, Global.Instance.player);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void CheckSpellSpecificSounds()
    {
        switch (gameObject.name)
        {
                /*Tech*/
            case "Granade":
               // _holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.granadeReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.granadeTake);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.granadeUse);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.granadeReady);

                //Debug.Log("granade ljud");
                break;
            case "Lightning Discharge" :
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.lightningUse);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.lightningTake);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.tech.lightningReady);
                break;
                /*Psychic*/
            case "Drain Life":
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.psychic.drainReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.psychic.drainTake);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.psychic.drainUse);
                break;
            case "Mindfray" :
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.psychic.mindfrayReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.psychic.mindfrayTake);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.psychic.mindfrayUse);
                break;
                /*Kinetic*/
            case "Tremor" :
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.tremorUse);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.tremorTake);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.tremorReady);
                break;
            case "Supersonic Throw":
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.supersonicUse);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.supersonicTake);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.supersonicReady);
                break;
            case "Seismic Slam":
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.siesmicUse);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.siesmicTake);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.kinetic.siesmicReady);
                break;
                /*Extension*/
            case "Adrenaline Rush" :
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.adrenalineUse);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.adrenalineReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.adrenalineTake);
                break;
            case "Overload":
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.overloadUse);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.overloadReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.overloadTake);
                break;
            case "Overpower" :
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.overpowerUse);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.overpowerReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.overpowerTake);
                break;
            case "Protective Shield":
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.protShieldUse);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.protShieldReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.protShieldTake);
                break;
            case "Shockwave" :
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.shockwaveUse);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.shockwaveReady);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.extensions.shockwaveTake);
                break;
                /*Base*/
            case "Heavy Strike":
                _holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.abilities.Base.damage.hold);
                break;
            default:
                break;
        }
    }
}
