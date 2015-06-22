using UnityEngine;
using System.Collections;

public class ClickAttack : BaseAttack {
    public bool _canDealDamage = true;
    public GameObject _lastTarget = null;

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
                    Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue,10f);
                    // hit enemy
                    if (hit.collider.transform.parent.parent.tag == "Enemy")
                    {
                        if (_canDealDamage || (_lastTarget != null && _lastTarget != hit.collider.gameObject.transform.parent.parent.gameObject))
                        {
                            _canDealDamage = false;
                            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 10f);
                            _lastTarget = hit.collider.transform.parent.parent.gameObject;
                            CharacterStats cs = gameObject.GetComponent<Player>()._stats;
                            hit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs), hit.point, Global.Instance._player);
                        }
                    }
                    else
                    {
                        _canDealDamage = true;
                    }
                }
                catch (System.NullReferenceException) { }
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
