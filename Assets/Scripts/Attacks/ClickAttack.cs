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
                    // hit enemy
                    if ((hit.collider.transform.parent.parent.tag == "Enemy" && _canDealDamage) || (_lastTarget != null && _lastTarget != hit.collider.gameObject))
                    {
                        _canDealDamage = false;
                        _lastTarget = hit.collider.gameObject;
                        CharacterStats cs = gameObject.GetComponent<Player>()._stats;
                        hit.collider.transform.parent.parent.gameObject.GetComponent<Enemy>().TakeDamage(DamageStats.GenerateFromCharacterStats(cs), hit.point);
                    }
                }
                catch (System.NullReferenceException e) { }
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
