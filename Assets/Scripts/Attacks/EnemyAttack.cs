using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    public float _attackTimer;
    public float _cooldownTimer;

	// Use this for initialization
	void Start () {
	
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
        if(Global.Instance.PlayerAlive())
            Global.Instance._player.TakeDamage(DamageStats.GenerateFromCharacterStats(gameObject.GetComponent<Enemy>()._stats));
    }
}
