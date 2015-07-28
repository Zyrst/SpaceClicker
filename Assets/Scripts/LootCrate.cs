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
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform == transform && !_enemyOwner.GetComponent<Character>()._isAlive)
                {
                    Destroy();
                    _potion.GetComponent<HealthPotion>().staticPotion = false;
                }
            }
        }
	}

    public void Activate(GameObject enemy_, GameObject potion_)
    {
        _enemyOwner = enemy_;
        _potion = potion_;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
        //And spawn effects
    }

    public void UltimateDestroy()
    {
        GameObject.Destroy(gameObject);
        GameObject.Destroy(_potion.gameObject);
    }
}
