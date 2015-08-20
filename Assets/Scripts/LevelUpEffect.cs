using UnityEngine;
using System.Collections;

public class LevelUpEffect : MonoBehaviour {

	public float _lifeTime;

	// Use this for initialization
	void Start () {
		_lifeTime = 0.8f;
	
	}
	
	// Update is called once per frame
	void Update () {

		_lifeTime -= Time.deltaTime;

		GetComponent<Light> ().range += Time.deltaTime * 20;

		if (_lifeTime <= 0) {
			Destroy (gameObject);
		}
	
	}
}
