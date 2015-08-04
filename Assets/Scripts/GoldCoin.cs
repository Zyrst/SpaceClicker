using UnityEngine;
using System.Collections;

public class GoldCoin : MonoBehaviour {

    public uint _value = 0;
    public float _lifetime = 15f;

    [HideInInspector]
    public float _timer = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _timer += Time.deltaTime;
        if (_timer >= _lifetime)
        {
            Collect();
        }
        else
        {
            
            //Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            Ray ray = Global.Instance._gameCamera.ScreenPointToRay(MouseController.Instance.position);
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
        Global.Instance.Gold += _value;
        Sounds.OneShot(Sounds.Instance.uiSounds.coinCollect);
        GameObject.Destroy(gameObject);
    }

    public static GameObject Create(Vector3 pos_, Vector3 force_)
    {
        GameObject _gold = GameObject.Instantiate(Global.Instance._prefabs.GoldCoin);

        _gold.transform.position = pos_;

        _gold.GetComponent<Rigidbody>().AddForce(force_ * 2);

        return _gold;
    }
}
