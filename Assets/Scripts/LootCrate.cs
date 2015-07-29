using UnityEngine;
using System.Collections;

public class LootCrate : MonoBehaviour {

    public GameObject _enemyOwner;
    public GameObject _potion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (MouseController.Instance.buttonDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit) && !IsInvoking())
            {
                if (hit.collider.transform == transform && !_enemyOwner.GetComponent<Character>()._isAlive)
                {
                    _potion.GetComponent<HealthPotion>().staticPotion = false;
                    Vector3 dir = (Vector3.up * 25f) + -(transform.position - Global.Instance.player.transform.position);
                    _potion.GetComponent<Rigidbody>().AddForce(dir * 20f);
                    Destroy();
                }
            }
        }
	}

    /// <summary>
    /// Associate a potion and enemy with loot crate
    /// </summary>
    /// <param name="enemy_">Enemy who needs to killed to activate crate</param>
    /// <param name="potion_">Potion inside the loot crate</param>
    public void Activate(GameObject enemy_, GameObject potion_)
    {
        _enemyOwner = enemy_;
        _potion = potion_;
    }

    /// <summary>
    /// Hides the lootcrate
    /// </summary>
    public void Destroy()
    {
        gameObject.SetActive(false);

        //And spawn effects
    }

    /// <summary>
    /// Destroy both lootcrate and potion
    /// </summary>
    public void UltimateDestroy()
    {
        GameObject.Destroy(gameObject);
        GameObject.Destroy(_potion.gameObject);
    }

    /// <summary>
    /// Activates the potion and destroys the lootcrate
    /// </summary>
    public void ActivatePotion()
    {
        
        GameObject.Destroy(gameObject);
    }
}
