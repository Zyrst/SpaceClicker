using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Enemy : Character
{
    public bool _shieldUp = false;
    [HideInInspector]
    public int _animCounter = 0;
    [HideInInspector]
    public int _lastAnim = 0;
    [HideInInspector]
    public int _myNumDeath = 0;

    GameObject _myPotion;
    private FMOD.Studio.EventInstance _takingDamageSoundEvent;

    public classType _myClass = classType.assassin;

    public enum classType : int { sage = 0 , assassin = 1, tank = 2}

     
	// Use this for initialization
    void Start()
    {
        _level = Global.Instance.GetEnemyLevel();
        //_stats.LevelUp(_level);
        
        
        Debug.Log(gameObject.name);
        if (gameObject.name.Contains("Tank"))
        {
            _myClass = classType.tank;
            _stats._healthStatDist = 0.7f;
            _stats._damageStatDist = 0.8f;
            _stats._baseCooldownTimer *= 1f;
            GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "ClassIcon").sprite = Sprites.Instance.classIcons.Tank.sprite;
        }
        else
        {
            _myClass = (classType)Random.Range(0, 2);
            switch (_myClass)
            {
                case classType.sage:
                    _stats._healthStatDist = 0.5f;
                    _stats._damageStatDist = 0.8f;
                    _stats._baseCooldownTimer *= 0.8f;
                    GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "ClassIcon").sprite = Sprites.Instance.classIcons.Sage.sprite;
                    break;
                case classType.assassin:
                    _stats._healthStatDist = 0.3f;
                    _stats._damageStatDist = 0.8f;
                    _stats._baseCooldownTimer *= 0.6f;
                    GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "ClassIcon").sprite = Sprites.Instance.classIcons.Assassin.sprite;
                    break;
                default:
                    break;
            }
        }
        
        
        _stats._baseStat._values[0] = (_stats._constMultiplier*_level + ( Mathf.Pow(_stats._basePower,(_level/_stats._powerDiv)))) * _stats._valueMultiplier;
        _stats._baseStat.Checker();
        _stats._baseStat = _stats._baseStat * (1f / ((EnemySpawner._enemiesSpawn / 2f) + 0.5f));
        _stats._baseStat.Checker();

        _stats._maxHealth = (_stats._baseStat * _stats._multiplierHealth) * _stats._healthStatDist;
        _stats._health = new vap(_stats._maxHealth);
        _stats._normal.damage = (_stats._baseStat * _stats._multiplierDamage) * _stats._damageStatDist;

        Transform tr = transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Model");
        tr.LookAt(Global.Instance.player.transform);

        float rnd = Random.Range(0f, 1f);
       // if (rnd >= 1f - Global.Instance._potionDropChans.value)
        {
            _myPotion = HealthPotion.Create(tr.position - (tr.forward * 3f), Vector3.zero);
            GameObject tmp = GameObject.Instantiate(Global.Instance._prefabs.LootCrate);
            tmp.transform.position = _myPotion.transform.position;
            tmp.transform.localScale = new Vector3(15, 15, 15);
            tmp.GetComponent<LootCrate>().Activate(gameObject,_myPotion);
        }
            
       // Quaternion rot = transform.rotation;
        //rot.z = 0f;
	}
	
	// Update is called once per frame
	void Update () {
       /* Vector3 rot = Vector3.up;

        transform.Rotate(rot * 0f * Time.deltaTime);*/
        
	}

    public override void Die()
    {
        try { 
            GetComponentInChildren<Animator>().SetTrigger("DeadTrigger"); 
        }
        catch (System.NullReferenceException) { }
        // effekter och skits
        
        base.Die();
        _myNumDeath = Global.Instance.EnemiesAlive();

        // spawn goldcoin
        Vector3 dir = (Vector3.up * 10f) + -(transform.position - Global.Instance.player.transform.position);
        GoldCoin.Create(transform.position, dir * 20f).GetComponent<GoldCoin>()._value = Global.Instance._player._level >= 19 ? (uint) (_level/50) + 2 : 1 ;
            
       // Debug.Log("EnemisAlive: " + Global.Instance.EnemiesAlive());
        Invoke("Kill", 2f);
        GetComponentInChildren<CharacterGUI>().gameObject.SetActive(false);
    }

    public void Kill()
    {

        if (Global.Instance.EnemiesAlive() == 0 && _myNumDeath == 0)
        {
            //Debug.Log("Triggered new wave");
            EnemySpawner.triggers.newWave();
        }
        gameObject.SetActive(false);
    }

    public override void TakeDamage(DamageStats ds_, Vector3 hitPoint_, Character hitter_)
    {
        if (_isAlive)
        {
            if (!_shieldUp)
            {
                try
                {
                    int rnd = Random.Range(1,3);
                    //Try to make so not the same hit animation gets trigged 
                    if (_lastAnim == rnd)
                    {
                        _animCounter++;
                        if (_animCounter == 3)
                        {
                            rnd--;
                            if (rnd == 0)
                            {
                                rnd = 2;
                            }
                            _animCounter = 0;
                        }
                    }
                    else
                    {
                        _animCounter = 0;
                        _lastAnim = rnd;
                    }
                    GetComponentInChildren<Animator>().SetInteger("Hit",rnd);
                    GetComponentInChildren<Animator>().SetTrigger("HitTrigger");
                }

                catch (System.NullReferenceException) { }

                vap oldHP = new vap(_stats._health);

                base.TakeDamage(ds_, hitPoint_, hitter_);

                // play taking damage sound 
                if (!_isAlive)
                {
                    _takingDamageSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.enemySounds.damage_heavy); 
                    _takingDamageSoundEvent.start();
                }
                else if (vap.GetScale(oldHP, _stats._maxHealth) >= 0.35f && vap.GetScale(_stats._health, _stats._maxHealth) <= 0.35f)
                {
                    _takingDamageSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.enemySounds.damage_medium); 
                    _takingDamageSoundEvent.start();
                }
                else if (vap.GetScale(oldHP, _stats._maxHealth) >= 0.85f && vap.GetScale(_stats._health, _stats._maxHealth) <= 0.85f)
                {
                    _takingDamageSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.enemySounds.damage_light); 
                    _takingDamageSoundEvent.start();
                }

                if (ds_._stunTime > 0f)
                {
                    GetComponent<EnemyAttack>().Stunned(ds_._stunTime);
                    GetComponentInChildren<Animator>().SetTrigger("StunTrigger");
                    GetComponentInChildren<Animator>().SetInteger("Hit", 0);
                }
                if (ds_._slowTime > 0f)
                {
                    GetComponent<EnemyAttack>().Slow(ds_._slowTime, ds_._slowSpeed);
                    GetComponentInChildren<Animator>().SetTrigger("StunTrigger");
                    GetComponentInChildren<Animator>().SetInteger("Hit", 0);
                }
            }
            else
            {
                CharacterStats _shieldStats = new CharacterStats(_stats);
                vap _shieldVap = _shieldStats._normal.damage + _shieldStats._tech.damage + _shieldStats._kinetic.damage + _shieldStats._psychic.damage;
                _shieldVap *= GetComponent<EnemyAttack>()._shieldDamageMulti;

                // hitta vilken typ av sköld det är
                // ^^ jävligt viktigt
                _shieldStats = new CharacterStats();
                _shieldStats._tech.damage = _shieldVap;

                // play shield damage sound
                Sounds.OneShot(Sounds.Instance.enemySounds.shieldSounds.damage);
                GetComponent<EnemyAttack>().ResetShield();



                Global.Instance.player.TakeDamage(DamageStats.GenerateFromCharacterStats(_shieldStats, false), gameObject.GetComponent<Enemy>());
                GetComponent<EnemyAttack>().ResetShield();
                if (GetComponent<EnemyAttack>().IsInvoking())
                {
                    GetComponent<EnemyAttack>().CancelInvoke();
                }
            }
        }
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
