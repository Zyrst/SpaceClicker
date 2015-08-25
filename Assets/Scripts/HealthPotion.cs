using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {

    public float _lifeTime = 20f;
    [HideInInspector]
    public float _timer = 0f;

    public bool staticPotion = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!staticPotion)
        {
            _timer += Time.deltaTime;
            if (_timer >= _lifeTime)
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
                    /*foreach (var item in GetComponentInChildren<Transform>())
                    {
                        if (hit.collider.transform == item)
                        {
                            Collect();
                            break;
                        }
                    }*/
                }
            }
        }
       
	}

    public void Collect()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.medpackPickup);

        Global.Instance.player._stats._health += (Global.Instance.player._combinedStats._maxHealth * Global.Instance._potionHealthPercent.value);
        if ((Global.Instance.player._stats._health) > (Global.Instance.player._combinedStats._maxHealth)) 
            Global.Instance.player._stats._health = new vap(Global.Instance.player._combinedStats._maxHealth);

        GameObject go = GameObject.Instantiate(Global.Instance._prefabs._effects[0]);
        go.transform.parent = Global.Instance._player.transform;
        go.transform.position = Global.Instance._player.transform.position;
        go.GetComponent<ParticleSystem>().Play();
        Global.Instance._player._healEffect = go;
        foreach (var item in gameObject.GetComponentsInChildren<Transform>())
        {
            item.gameObject.SetActive(false);
        }
        
        Invoke("DestroyPotion", 1f);

        
    }

    public static GameObject Create(Vector3 pos_, Vector3 force_)
    {
        GameObject potion = GameObject.Instantiate(Global.Instance._prefabs.HealthPotion);
        potion.transform.position = pos_;
       // potion.GetComponent<Rigidbody>().AddForce(force_);
        return potion;

    }

    public void DestroyPotion()
    {
        GameObject.Destroy(gameObject);
    }
}
