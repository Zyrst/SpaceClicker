using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SpellAttack : BaseAttack {

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

    public enum SpellType : int { Damage = 0, Heal = 1, Stun = 2, Shield = 3 };
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
                _holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.damageSpell.hold);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.damageSpell.ready);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.damageSpell.take);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.damageSpell.use);
                break;
            case SpellType.Heal:
                //_holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.healingSpell.hold);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.healingSpell.ready);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.healingSpell.take);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.healingSpell.use);
                break;
            case SpellType.Stun:
                //_holdSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.stunSpell.hold);
                _readySound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.stunSpell.ready);
                _takeSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.stunSpell.take);
                _useSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.stunSpell.use);
                break;
            default:
                break;
        }
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
	}

    public void Clicked()
    {
        if (_type == SpellType.Shield)
        {
            Global.Instance.player._shield.Raise(_combinedStats._shieldTime);
            Global.Instance.player._shield.spellRef = this;

            _cd = true;
            _slotImage.color = new Color(0.5f, 0.5f, 0.5f);
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
            catch (System.NullReferenceException) {}

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
            GetComponentInParent<ClickAttack>().HoldingSpell();
            Invoke("ResetCanDealDamage", 0.1f);
        }
    }

    public void FollowMouse()
    {
        _slot.transform.position = new Vector3(_followerDiff.x + MouseController.Instance.position.x, 
            _followerDiff.y + MouseController.Instance.position.y,
            _slot.transform.position.z);
        if (!MouseController.Instance.buttonDown)
        {
            ResetGUI();

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
                    if (hit.collider.transform.parent.parent.tag == "Enemy" || hit.collider.transform.parent.parent.tag == "Player" && hit.collider.transform.parent.parent.GetComponent<Character>()._isAlive)
                    {
                        _slotImage.color = new Color(0.5f, 0.5f, 0.5f);
                        _cd = true;
                        ResetGUI();

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

                        // do damage
                        UseSpell(hit.collider.transform.parent.parent.gameObject.GetComponent<Character>(), hit);
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
            if (_coolDown >= _combinedStats._cooldown)
            {
                ResetCooldown();
                try
                {
                    // play ready sound
                    _readySound.start();
                }
                catch (System.NullReferenceException)
                { }
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

    public void Stunned(float stunTime_)
    {
        _stunned = true;
        _stunTime = stunTime_;
        _slotImage.color = new Color(0.5f, 0.5f, 0.5f);

    }

    public void PlayHoldSound()
    {
        _holdSound.start();
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void UseSpell(Character char_, RaycastHit hit_)
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
                        char_.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);
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

                        char_.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);

                        if (foundIt)
                        {
                            if (enemy - 1 >= 0)
                            {
                                enemies[enemy - 1].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);
                            }
                            if (enemy + 1 < enemies.Length)
                            {
                                enemies[enemy + 1].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);
                            }
                        }
                    }
                    break;
                case SpellTarget.EnemiesAndPlayer:
                    {
                        Enemy[] enemies = (Enemy[])Global.Instance.GetAllEnemies().ToArray(typeof(Enemy));
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            enemies[i].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);
                        }

                        Global.Instance.player.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);
                    }
                    break;
                case SpellTarget.Enemies:
                    {
                        char_.TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);

                        Enemy[] enemies = (Enemy[])Global.Instance.GetAllEnemies().ToArray(typeof(Enemy));
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            if (enemies[i] != char_)
                            {
                                enemies[i].TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit_.point, Global.Instance.player);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
