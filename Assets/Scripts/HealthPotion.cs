using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {

    public float _lifeTime = 5f;
    [HideInInspector]
    public float _timer = 0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _timer += Time.deltaTime;
        if( _timer >= _lifeTime)
        {
            GameObject.Destroy(gameObject);
        }

        if (MouseController.Instance.buttonDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform == transform)
                {
                    Collect();
                }
                foreach (var item in GetComponentInChildren<Transform>())
                {
                    if (hit.collider.transform == item)
                    {
                        Collect();
                        break;
                    }
                }
            }
        }
	}

    public void Collect()
    {
        Global.Instance._player._stats._health += (Global.Instance._player._stats._maxHealth * 0.1f);
        if ((Global.Instance._player._stats._health) > (Global.Instance._player._stats._maxHealth)) 
            Global.Instance._player._stats._health = new vap(Global.Instance._player._stats._maxHealth);

        GameObject.Destroy(gameObject);
    }

    public static GameObject Create(Vector3 pos_, Vector3 force_)
    {
        GameObject potion = GameObject.Instantiate(Global.Instance._prefabs.HealthPotion);
        potion.transform.position = pos_;
        potion.GetComponent<Rigidbody>().AddForce(force_);
        return potion;

    }
}
