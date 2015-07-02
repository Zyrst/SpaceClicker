using UnityEngine;
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

        RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            Debug.Log("name animaiton: " + ac.animationClips[i].name);
            if (ac.animationClips[i].name == "Attack")        //If it has the same name as your clip
            {
                float attackSpeed = 2f;
                anim.SetFloat("AttackSpeed", attackSpeed);
                _startAttackAnimationTime = ac.animationClips[i].length / attackSpeed;
            }
            else if (ac.animationClips[i].name == "Shield")
            {
                float shieldSpeed = 2f;
                anim.SetFloat("ShieldSpeed", shieldSpeed);
                _startShieldAnimationTime = ac.animationClips[i].length / shieldSpeed;
            }
        }
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
            if (!_nextAttackIsShield)
            {
                Global.Instance._player.TakeDamage(DamageStats.GenerateFromCharacterStats(gameObject.GetComponent<Enemy>()._stats, false), gameObject.GetComponent<Enemy>());
            }
            else
            {
                shield.gameObject.SetActive(true);
                _shieldUp = true;
                GetComponent<Enemy>()._shieldUp = true;
                Invoke("ResetShield", _shieldTime);
            }
        }
        if (!_nextAttackIsShield && GetComponent<Enemy>()._isAlive)
        {
            int result = Random.Range(0, 2);
            if (result == 1)
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

    public void ResetShield()
    {
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
