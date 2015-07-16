﻿using UnityEngine;
using System.Collections;

public class ClickAttack : BaseAttack {
    public bool _canDealDamage = true;
    public GameObject _lastTarget = null;
    public int _hitCount = 0;
    public int _critHitCount;
    public int _critHitSoundStart;
    public int _hitSound = 1;

    public bool _stunned = false;
    public float _stunTime = 0f;

    public bool _holdingSpell = false;

    public static class Swipes {  
        public static float tech = 0.8f; 
        public static float kinetic = 0.4f;
        public static float pshycic = 0f; 
    };

    private FMOD.Studio.EventInstance _swipeSoundEvent;
    private FMOD.Studio.ParameterInstance _swipeSoundVariable;
    

	// Use this for initialization
	void Start () {

        _swipeSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.swipe);
        _swipeSoundEvent.getParameter("Spec", out _swipeSoundVariable);
	}
	
	// Update is called once per frame
	void Update () {
        // LMB
        if (MouseController.Instance.clickButtonDown && !_stunned && !_holdingSpell && GetComponentInParent<Character>()._isAlive)
        {
            // mouseon the ground
            Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                try
                {
                    //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.blue,10f);
                    // hit enemy
                    if (hit.collider.transform.parent.parent.tag == "Enemy")
                    {
                        if ((_canDealDamage || (_lastTarget != null && _lastTarget != hit.collider.gameObject.transform.parent.parent.gameObject))
                            && hit.collider.gameObject.GetComponentInParent<Character>()._isAlive)
                        {
                            if (_lastTarget == hit.collider.gameObject.transform.parent.parent.gameObject)
                                _hitCount++;
                            else
                            {
                                _hitCount = 0;
                                _hitSound = 0;
                            }

                            GetComponent<Player>().Animator.SetInteger("attack", Random.Range(0,2));
                            GetComponent<Player>().Animator.SetTrigger("attack_start");
                                
                            //Debug.Log("Hit count: " + _hitCount);
                            _canDealDamage = false;
                            //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 10f);
                            _lastTarget = hit.collider.transform.parent.parent.gameObject;
                            CharacterStats cs = gameObject.GetComponent<Player>()._combinedStats;

                            // dom här två är samma sak ^⌄

                            CharacterStats comb = GetComponent<Player>()._combinedStats;
                            // play sound, fix crit later
                            if (comb._tech.damage > comb._psychic.damage && comb._tech.damage > comb._kinetic.damage)               // tech is störst
                            {
                                _swipeSoundVariable.setValue(Swipes.tech);
                                _swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                            }
                            else if (comb._kinetic.damage > comb._psychic.damage && comb._kinetic.damage > comb._tech.damage)       // kinetic is störst
                            {
                                _swipeSoundVariable.setValue(Swipes.kinetic);
                                _swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                            }
                            else if (comb._psychic.damage > comb._kinetic.damage && comb._psychic.damage > comb._tech.damage)       // pshycic is störst
                            {
                                _swipeSoundVariable.setValue(Swipes.pshycic);
                                _swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                            }
                            else
                            {
                                _swipeSoundVariable.setValue(Swipes.pshycic);
                                _swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                            }


                            if (_hitCount < _critHitCount)
                            {
                                if (_hitCount >= (_critHitCount / _critHitSoundStart))
                                {
                                    //Build up sound for crit
                                    //audio.PlayOneShot(_tempHitSounds[_hitSound]);
                                    _hitSound++;
                                }
                                //else
                                 //   audio.PlayOneShot(_tempHitSounds[0]);
                                hit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, false), hit.point, Global.Instance.player);
                            }
                                
                            else if (_hitCount == _critHitCount)
                            {
                                hit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, true), hit.point, Global.Instance.player);
                                //audio.PlayOneShot(_tempHitSounds[5]);
                                _hitSound = 0;
                                _hitCount = 0;
                            }
                        }
                    }
                    else
                    {
                        _canDealDamage = true;
                    }
                }
                catch (System.NullReferenceException) { _canDealDamage = true; }
            }
            else if(!_holdingSpell)
            {
                _canDealDamage = true;
            }
        }
        else if(_stunned)
        {
            _stunTime -= Time.deltaTime;
            if (_stunTime <= 0f)
            {
                _stunned = false;
                _canDealDamage = true;
            }
        }
        else if(!_holdingSpell)
        {
            _canDealDamage = true;
        }
	}

    public void Stunned(float stunTime_)
    {
        _stunned = true;
        _stunTime = stunTime_;
        _canDealDamage = false;
    }

    public void HoldingSpell()
    {
        _holdingSpell = true;
        _canDealDamage = false;
    }

    public void ReleasedSpell()
    {
        _holdingSpell = false;
    }
}
