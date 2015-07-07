using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SpellAttack : BaseAttack {

    public SpellStats _stats = new SpellStats();
    public SpellStats _combinedStats = new SpellStats();
    public Sprite _spellImage;
    public GameObject _slot;
    public Image _slotImage;

    private bool _clicked = false;
    private bool _cd = false;
    private float _coolDown = 0f;
    public Vector3 _startGUIPos;
    public Vector3 _followerDiff = Vector3.zero;

    public bool _stunned = false;
    public float _stunTime = 0f;

    public enum SpellType : int { Damage = 0, Heal = 1, Stun = 2 };
    public SpellType _type = SpellType.Damage;

    public FMOD.Studio.EventInstance _holdSound;
    public FMOD.Studio.EventInstance _readySound;
    public FMOD.Studio.EventInstance _useSound;
    public FMOD.Studio.EventInstance _takeSound;

	// Use this for initialization
	void Start () {
        _slot =  Global.Instance._player.getSpellSlot(this);
        if (_slot == null)
        {
            Debug.Log("slot är null");
        }
        if (_spellImage == null)
        {
            Debug.Log("_spellImage är null");
        }
        _slotImage = _slot.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Image");
        _slotImage.sprite = _spellImage;

        _startGUIPos = _slot.transform.position;

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
        if (!_clicked && !_cd && !_stunned)
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
            //GetComponentInParent<ClickAttack>()._canDealDamage = true;
        }
        else
        {
            CheckHit();
          //  GetComponentInParent<ClickAttack>()._canDealDamage = false;
            
        }
    }

    public void CheckHit()
    {
            // mouseon the ground
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

                        hit.collider.transform.parent.parent.gameObject.GetComponent<Character>().TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit.point, Global.Instance._player);
                    }
                    else if (!hit.collider.transform.parent.parent.GetComponent<Character>()._isAlive)
                    {
                        ResetGUI();
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
                // play ready sound
                _readySound.start();
            }
        }
       
    }

    public void ResetCooldown()
    {
        _cd = false;
        _coolDown = 0f;
        _slotImage.color = new Color(1f, 1f, 1f);
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
        _combinedStats = new SpellStats(_stats);
        _combinedStats.AddStats(Global.Instance._player._combinedStats);
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

}
