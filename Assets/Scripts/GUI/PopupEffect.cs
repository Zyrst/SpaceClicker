using UnityEngine;
using System.Collections;

public class PopupEffect : MonoBehaviour {

    public float _time = 1f;
    public float _timer = 0f;

    private bool _doIt = true;
    private Vector3 _original = new Vector3(1, 1, 1);

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (_doIt)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                transform.localScale = _original;
                _doIt = false;
            }
            else
            {
                transform.localScale += _original * Time.deltaTime / _time;
            }
        }

	}

    void OnAwake()
    {
        _original = transform.localScale;
    }

    void OnEnable()
    {
        _doIt = true;
        _timer = _time;

        transform.localScale *= 0f;
    }
}
