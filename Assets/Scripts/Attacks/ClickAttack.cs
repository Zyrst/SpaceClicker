using UnityEngine;
using System.Collections;

public class ClickAttack : BaseAttack {
    public bool _canDealDamage = true;
    public GameObject _lastTarget = null;
    public int _hitCount = 0;
    public int _critHitCount;
    public int _critHitSoundStart;
    public int _hitSound = 1;

    public AudioClip[] _tempHitSounds = new AudioClip[5];
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // LMB
        if (MouseController.Instance.clickButtonDown)
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
                        if (_canDealDamage || (_lastTarget != null && _lastTarget != hit.collider.gameObject.transform.parent.parent.gameObject))
                        {
                            if (_lastTarget == hit.collider.gameObject.transform.parent.parent.gameObject)
                                _hitCount++;
                            else
                            {
                                _hitCount = 0;
                                _hitSound = 0;
                            }
                                
                            //Debug.Log("Hit count: " + _hitCount);
                            _canDealDamage = false;
                            //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 10f);
                            _lastTarget = hit.collider.transform.parent.parent.gameObject;
                            CharacterStats cs = gameObject.GetComponent<Player>()._combinedStats;
                            AudioSource audio = Global.Instance._player.GetComponent<AudioSource>();
                            if (_hitCount < _critHitCount)
                            {
                                if (_hitCount >= (_critHitCount / _critHitSoundStart))
                                {
                                    //Build up sound for crit
                                    audio.PlayOneShot(_tempHitSounds[_hitSound]);
                                    _hitSound++;
                                }
                                else
                                    audio.PlayOneShot(_tempHitSounds[0]);
                                hit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, false), hit.point, Global.Instance._player);
                            }
                                
                            else if (_hitCount == _critHitCount)
                            {
                                hit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs, true), hit.point, Global.Instance._player);
                                audio.PlayOneShot(_tempHitSounds[5]);
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
            else
            {
                _canDealDamage = true;
            }
        }
        else
        {
            _canDealDamage = true;
        }
	}
}
