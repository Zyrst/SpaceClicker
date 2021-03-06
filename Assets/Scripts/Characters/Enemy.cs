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

    public Texture[] _skins;

    GameObject _myPotion;
    private FMOD.Studio.EventInstance _takingDamageSoundEvent;
    public bool _isBoss;

    public classType _myClass = classType.assassin;

    public enum classType : int { sage = 0 , assassin = 1, tank = 2}

     
	// Use this for initialization
    void Start()
    {
        _level = Global.Instance.GetEnemyLevel();
        //_stats.LevelUp(_level);
        
        
        if (gameObject.name.Contains("Tank"))
        {
            _myClass = classType.tank;
            _stats._healthStatDist = 0.7f;
            _stats._damageStatDist = 0.5f;
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
                    _stats._damageStatDist = 0.5f;
                    _stats._baseCooldownTimer *= 0.8f;
                    GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "ClassIcon").sprite = Sprites.Instance.classIcons.Sage.sprite;
                    break;
                case classType.assassin:
                    _stats._healthStatDist = 0.3f;
                    _stats._damageStatDist = 0.5f;
                    _stats._baseCooldownTimer *= 0.6f;
                    GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "ClassIcon").sprite = Sprites.Instance.classIcons.Assassin.sprite;
                    break;
                default:
                    break;
            }
        }

        vap tmpVap = new vap();
        tmpVap._values[0] = _stats._constMultiplier * _level;
        tmpVap.Checker();
     //   Debug.Log(Global.Instance._prevLevels[_level - Starmap.Instance._minLevel].GetFloat());
        _stats._baseStat = (tmpVap + Global.Instance._prevLevels[ _level - Starmap.Instance._minLevel ]) * _stats._valueMultiplier;
       
        _stats._baseStat.Checker();
        
        
        if (Global.Instance._player._miniBoss && EnemySpawner.triggers._bossSpawn._enemy == this)
        {
            _stats._baseStat._values[0] *= 2f;
            _stats._baseStat.Checker();
            transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Model").localScale = new Vector3(2f, 2f, 2f);
            transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "colliders").localScale = new Vector3(1f, 2f, 2f);
            GetComponentInChildren<CharacterGUI>().transform.localPosition = new Vector3(0f, 4f, 0f);
            _isBoss = true;

        }
            
        _stats._baseStat = _stats._baseStat * (1f / ((EnemySpawner._enemiesSpawn / 2f) + 0.5f));
        _stats._baseStat.Checker();

        _stats._maxHealth = (_stats._baseStat * _stats._multiplierHealth) * _stats._healthStatDist;
        _stats._health = new vap(_stats._maxHealth);
        _stats._normal.damage = (_stats._baseStat * _stats._multiplierDamage) * _stats._damageStatDist;

        Transform tr = transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "Model");
        tr.LookAt(Global.Instance.player.transform);

        float rnd = Random.Range(0f, 1f);
        if (rnd >= 1f - Global.Instance._potionDropChans.value && !_isBoss)
        {
            _myPotion = HealthPotion.Create(tr.position - (tr.forward * 3f), Vector3.zero);
            GameObject tmp = GameObject.Instantiate(Global.Instance._prefabs.LootCrate);
            tmp.transform.position = _myPotion.transform.position;
            tmp.transform.position += new Vector3(0f, 0.5f, 0f);
            tmp.transform.localScale = new Vector3(15, 15, 15);
            tmp.GetComponent<LootCrate>().Activate(gameObject,_myPotion);
        }

        //More than one texture for the enemy
        //Find the skin meshed renderer on Body
        if (_skins.Length != 0)
        {
            try
            {
                GetComponentsInChildren<SkinnedMeshRenderer>().FirstOrDefault(x => x.name == "Body").material.mainTexture = _skins[Random.Range(0, _skins.Length)];
            }
            catch (System.NullReferenceException) { }
        }
       // Quaternion rot = transform.rotation;
        //rot.z = 0f;
	}
	
	// Update is called once per frame
	void Update () {
       /* Vector3 rot = Vector3.up;

        transform.Rotate(rot * 0f * Time.deltaTime);*/
        base.CheckEffect();
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
        if (_isBoss)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector3 dir = (Vector3.up * 15f ) + -(transform.position - Global.Instance.player.transform.position);
                GoldCoin.Create(new Vector3(transform.position.x, transform.position.y + (0.5f * i), transform.position.z), dir * 5f).GetComponent<GoldCoin>()._value = Global.Instance._player._level >= 19 ? (uint)(_level / 50) + 2 : 1;
            }
            Global.Instance._player._miniBoss = false;
            _isBoss = false;
            //Doesn't sit on top of the boss
            GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "colliders").gameObject.SetActive(false);
        }
        else
        {
            Vector3 dir = (Vector3.up * 15f) + -(transform.position - Global.Instance.player.transform.position);
            GoldCoin.Create(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), dir * 20f).GetComponent<GoldCoin>()._value = Global.Instance._player._level >= 19 ? (uint)(_level / 50) + 2 : 1;
        }
        
       // Debug.Log("EnemisAlive: " + Global.Instance.EnemiesAlive());
       
        Invoke("Kill", 2f);
        GetComponentInChildren<CharacterGUI>().gameObject.SetActive(false);
    }

    /// <summary>
    /// never ever call
    /// </summary>
    public void Kill()
    {
        if (Global.Instance.EnemiesAlive() == 0 && _myNumDeath == 0)
        {
            //Debug.Log("Triggered new wave");
            Sounds.OneShot(Sounds.Instance.music.clearLevel);
            EnemySpawner.triggers.newWave();
        }
        
        gameObject.SetActive(false);
    }

    /// <summary>
    /// kills it now
    /// </summary>
    public void KillIt()
    {
        _isAlive = false;
        _myNumDeath = Global.Instance.EnemiesAlive();

        if (Global.Instance.EnemiesAlive() == 0 && _myNumDeath == 0)
        {
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
				GameObject he = GameObject.Instantiate(Global.Instance._prefabs._effects[3]);
				he.transform.position = transform.position;
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
                    if (Global.Instance.player._miniBoss)
                        _takingDamageSoundEvent.setParameterValue("Boss", 1f);
                    _takingDamageSoundEvent.start();
                }
                else if (vap.GetScale(oldHP, _stats._maxHealth) >= 0.35f && vap.GetScale(_stats._health, _stats._maxHealth) <= 0.35f)
                {
                    _takingDamageSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.enemySounds.damage_medium);
                    if(Global.Instance.player._miniBoss)
                        _takingDamageSoundEvent.setParameterValue("Boss", 1f);
                    _takingDamageSoundEvent.start();
                }
                else if (vap.GetScale(oldHP, _stats._maxHealth) >= 0.85f && vap.GetScale(_stats._health, _stats._maxHealth) <= 0.85f)
                {
                    _takingDamageSoundEvent = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.enemySounds.damage_light);
                    if (Global.Instance.player._miniBoss)
                        _takingDamageSoundEvent.setParameterValue("Boss", 1f);
                    _takingDamageSoundEvent.start();
                }

                if (ds_._stunTime > 0f)
                {
                    GetComponent<EnemyAttack>().Stunned(ds_._stunTime);
                    GetComponentInChildren<Animator>().SetTrigger("StunTrigger");
                    GetComponentInChildren<Animator>().SetInteger("Hit", 0);
                    GameObject go = GameObject.Instantiate(Global.Instance._prefabs._effects[1]);
                    go.transform.parent = transform;
                    go.transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
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