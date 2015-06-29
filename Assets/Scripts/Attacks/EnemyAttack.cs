﻿using UnityEngine;
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

	// Use this for initialization
	void Start () {
        _attackTimer = Random.Range(0f, 2f);
        shield = gameObject.GetComponentInChildren<CharacterGUI>().Shield;
	}
	
	// Update is called once per frame
	void Update () {
        _attackTimer += Time.deltaTime;

        if (_attackTimer > _cooldownTimer)
        {
            Attack();
            _attackTimer = 0f;
        }
	}

    public void Attack()
    {
        if (Global.Instance.PlayerAlive())
        {
            try
            {
                GetComponentInChildren<Animator>().SetTrigger("AttackTrigger");
            }
            catch (System.NullReferenceException) { }
            if (!_nextAttackIsShield)
            {
                Global.Instance._player.TakeDamage(DamageStats.GenerateFromCharacterStats(gameObject.GetComponent<Enemy>()._stats, false), gameObject.GetComponent<Enemy>());
            }
            else
            {
                shield.gameObject.SetActive(true);
                _shieldUp = true;
                Invoke("ResetShield", _shieldTime);
            }
        }
        if (!_nextAttackIsShield)
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
    }
}
