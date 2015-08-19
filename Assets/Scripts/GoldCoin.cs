using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GoldCoin : MonoBehaviour
{

    public uint _value = 0;
    public float _lifetime = 15f;

    [HideInInspector]
    public float _timer = 0f;

    public Vector3 _target;
    public float _speed;
    public bool _collect = false;


    // Use this for initialization
    void Start()
    {
        _target = Camera.main.ScreenToWorldPoint(Global.Instance._playerGUI.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "GoldCoin").rectTransform.position);

    }

    // Update is called once per frame
    void Update()
    {
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
        if (_collect)
        {
            Vector3 calc = (_target - transform.position) * (_speed * Time.deltaTime);
            transform.position += calc;

            transform.Rotate(Random.Range(0f, 20f), Random.Range(0f, 20f), Random.Range(0f, 20f));
            if (Vector3.Distance(_target, transform.position) <= 3f)
            {
                Global.Instance.Gold += _value;
                Sounds.OneShot(Sounds.Instance.uiSounds.coinCollect);
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void Collect()
    {
        _collect = true;
        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.useGravity = false;
        GetComponent<BoxCollider>().enabled = false;

        /*Flyg till guldikonen*/
    }

    public static GameObject Create(Vector3 pos_, Vector3 force_)
    {
        GameObject _gold = GameObject.Instantiate(Global.Instance._prefabs.GoldCoin);

        pos_ += new Vector3(0f, 1f, 0f);
        _gold.transform.position = pos_;

        _gold.GetComponent<Rigidbody>().AddForce(force_);

        return _gold;
    }

}