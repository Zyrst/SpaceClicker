using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class EnemyAttack : MonoBehaviour {
    [HideInInspector]
    public float _attackTimer;
    [HideInInspector]
    GameObject shield;

    public float _cooldownTimer;

    public bool _nextAttackIsShield = false;
    public bool _shieldUp = false;
    public float _shieldTime = 2f;
    public float _shieldDamageMulti = 3f;
    public bool _stunned = false;

    public float _stunTime = 0f;

    private float _startAttackAnimationTime = 0f;
    private float _startShieldAnimationTime = 0f;

    public bool _animationIsTriggered = false;

	// Use this for initialization
	void Start () {
        _attackTimer = Random.Range(0f, 2f);
        shield = gameObject.GetComponentInChildren<CharacterGUI>().Shield;

        Animator anim = GetComponentInChildren<Animator>();

        try
        {
            RuntimeAnimatorController ac = anim.runtimeAnimatorController;          // get animator controller
            for (int i = 0; i < ac.animationClips.Length; i++)                      // for each clip in the controller
            {
                if (ac.animationClips[i].name == "Attack")                          // name of the clip (not state machine)
                {
                    float attackSpeed = 2f;
                    anim.SetFloat("AttackSpeed", attackSpeed);                      // set speed of the clip
                    _startAttackAnimationTime = ac.animationClips[i].length / attackSpeed;      // lenght with speed in mind
                }
                else if (ac.animationClips[i].name == "Shield")
                {
                    float shieldSpeed = 2f;
                    anim.SetFloat("ShieldSpeed", shieldSpeed);
                    _startShieldAnimationTime = ac.animationClips[i].length / shieldSpeed;
                }
            }
        }
        catch (System.NullReferenceException) { }
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Enemy>()._isAlive)
        {
            if (!_stunned)
            {
                _attackTimer += Time.deltaTime;

                // start attack animation
                if (Global.Instance.PlayerAlive() && GetComponent<Enemy>()._isAlive && !_animationIsTriggered)
                {
                    if (!_nextAttackIsShield)
                    {
                        if (_attackTimer > (_cooldownTimer - _startAttackAnimationTime))
                        {
                            try
                            {
                                GetComponentInChildren<Animator>().SetTrigger("AttackTrigger");
                                _animationIsTriggered = true;
                            }
                            catch (System.NullReferenceException) { }
                        }
                    }
                    else
                    {
                        if (_attackTimer > (_cooldownTimer - _startShieldAnimationTime))
                        {
                            try
                            {
                                GetComponentInChildren<Animator>().SetTrigger("ShieldTrigger");
                                _animationIsTriggered = true;
                            }

                            catch (System.NullReferenceException) { }
                        }
                    }
                }

                if (_attackTimer > _cooldownTimer)
                {
                    Attack();
                    _attackTimer = 0f;
                    _animationIsTriggered = false;
                }
            }
            else
            {
                _stunTime -= Time.deltaTime;
                if (_stunTime <= 0)
                    _stunned = false;
            }
        }
	}

    public void Attack()
    {
        if (Global.Instance.PlayerAlive() && GetComponent<Enemy>()._isAlive)
        {
            if (!_nextAttackIsShield)           // do regular attack
            {
                Global.Instance._player.TakeDamage(DamageStats.GenerateFromCharacterStats(gameObject.GetComponent<Enemy>()._stats, false), gameObject.GetComponent<Enemy>());
            }
            else
            {
                // do shield
                shield.gameObject.SetActive(true);
                _shieldUp = true;
                GetComponent<Enemy>()._shieldUp = true;

                Sounds.OneShot(Sounds.Instance.enemySounds.shieldSounds.start);      // shield start sound
                            
                                                                    // start the loop sound after ^^ is finshed
                Invoke("StartShieldLoop", (float)Sounds.GetLength(Sounds.Instance.enemySounds.shieldSounds.start)/1000f);    

                Invoke("ResetShield", _shieldTime);                 // stop loop and play stop
            }
        }
        if (!_nextAttackIsShield && GetComponent<Enemy>()._isAlive)     // determine next attack, can never get two shields in a row
        {
            int result = Random.Range(0, 2);
            if (result == 1)                                            // next attack is shield
            {
                _nextAttackIsShield = true;
                GetComponentInChildren<CharacterGUI>().CooldownBar.GetComponent<Image>().color = Color.blue;
            }
        }
        else
        {
            _nextAttackIsShield = false;
            GetComponentInChildren<CharacterGUI>().CooldownBar.GetComponent<Image>().color = Color.white;
        }
    }

    public void StartShieldLoop()
    {
        FMOD_StudioEventEmitter sound = GetComponent<FMOD_StudioEventEmitter>();        // shield loop sound
        try
        {
            sound.asset = Sounds.Instance.enemySounds.shieldSounds.loop;
            sound.Play();
        } catch (System.NullReferenceException){
        }
    }

    public void ResetShield()
    {
        FMOD_StudioEventEmitter sound = GetComponent<FMOD_StudioEventEmitter>();        // shield loop sound
        try
        {
            sound.Stop();
        } catch (System.NullReferenceException){
        }
        Sounds.OneShot(Sounds.Instance.enemySounds.shieldSounds.stop);

        shield.gameObject.SetActive(false);
        _shieldUp = false;
        GetComponent<Enemy>()._shieldUp = false;
    }

    public void Stunned(float stunTime_)
    {
        _stunTime = stunTime_;
        _stunned = true;
        _attackTimer = 0f;
    }
}
