using UnityEngine;
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

    public Vector3 _lastRay;
    public Vector3 _hitPoint;

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
            //Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            //RaycastHit hit = new RaycastHit();
            if (MouseCuboid.hit)//Physics.Raycast(ray, out hit))
            {
                try
                {
                    _hitPoint = MouseCuboid.collider.transform.position;
                    if (MouseCuboid.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        //if (MouseCuboid.collider.gameObject.GetComponentInParent<Character>()._isAlive) Global.DebugOnScreen(_canDealDamage ? "får slå" : "får inte slå");
                        if ((_canDealDamage || (_lastTarget != null && _lastTarget != MouseCuboid.collider.gameObject.transform.parent.parent.gameObject))
                            && MouseCuboid.collider.gameObject.GetComponentInParent<Character>()._isAlive)
                        {
                            if (_lastTarget == MouseCuboid.collider.gameObject.transform.parent.parent.gameObject)
                                _hitCount++;
                            else
                            {
                                _hitCount = 0;
                                _hitSound = 0;
                            }

                            GetComponent<Player>().Animator.SetInteger("attack", Random.Range(0,2));
                            GetComponent<Player>().Animator.SetTrigger("attack_start");
                                
                            _canDealDamage = false;
                            _lastTarget = MouseCuboid.collider.transform.parent.parent.gameObject;

                            _swipeSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.playerSounds.swipe);
                            _swipeSoundEvent.getParameter("Spec", out _swipeSoundVariable);

                            CharacterStats cs = GetComponent<Player>()._combinedStats;
                            // play sound, fix crit later
                            if (cs._tech.damage > cs._psychic.damage && cs._tech.damage > cs._kinetic.damage)               // tech is störst
                            {
                                _swipeSoundVariable.setValue(Swipes.tech);
                                //_swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                                _swipeSoundEvent.release();
                            }
                            else if (cs._kinetic.damage > cs._psychic.damage && cs._kinetic.damage > cs._tech.damage)       // kinetic is störst
                            {
                                _swipeSoundVariable.setValue(Swipes.kinetic);
                                //_swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                                _swipeSoundEvent.release();
                            }
                            else if (cs._psychic.damage > cs._kinetic.damage && cs._psychic.damage > cs._tech.damage)       // pshycic is störst
                            {
                                _swipeSoundVariable.setValue(Swipes.pshycic);
                                //_swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                                _swipeSoundEvent.release();
                            }
                            else
                            {
                                _swipeSoundVariable.setValue(Swipes.pshycic);
                                //_swipeSoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                                _swipeSoundEvent.start();
                                _swipeSoundEvent.release();
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
                                MouseCuboid.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, false), _hitPoint, Global.Instance.player);
                            }
                                
                            else if (_hitCount == _critHitCount)
                            {
                                MouseCuboid.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, true), _hitPoint, Global.Instance.player);
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
                catch (System.Exception) { _canDealDamage = true; }
            }
            else if(!_holdingSpell)
            {
                //Global.DebugOnScreen("HOLDINGSPELL");
                _canDealDamage = true;
            }
            //Try to see if we missed a target
            //if (_lastRay != _hitPoint)
            //{
            //    RaycastHit lineHit = new RaycastHit();
            //    if (Physics.Linecast(_hitPoint, _lastRay, out lineHit))
            //    {
            //        Debug.DrawLine(_hitPoint ,_lastRay, Color.green,5f);
            //        try
            //        {
            //            if (lineHit.collider.transform.parent.parent.tag == "Enemy")
            //            {
            //                if (_lastTarget != lineHit.collider.gameObject.transform.parent.parent.gameObject && _canDealDamage)
            //                {
            //                    CharacterStats cs = GetComponent<Player>()._combinedStats;
            //                    lineHit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, false), hit.point, Global.Instance.player);
            //                }
            //            }
                           
            //        }
            //        catch (System.NullReferenceException) { }
            //     }
            //}
        }
        else if(_stunned)
        {
            _stunTime -= Time.deltaTime;
            if (_stunTime <= 0f)
            {
                _stunned = false;
                _canDealDamage = true;
                Global.Instance.player._isHoldingSpell = false;
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
