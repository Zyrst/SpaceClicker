using UnityEngine;
using System.Collections;

public class NumberScipt : MonoBehaviour {

    public float _killTime = 1f;
    public float _currentTime = 0f;

	// Use this for initialization
	void Start () {
        transform.forward = Camera.main.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _killTime)
        {
            Destroy(gameObject  );
        }
	}
}
