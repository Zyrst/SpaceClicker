using UnityEngine;
using System.Collections;

public class SwipeHitEffect : MonoBehaviour {

	public float _lifeTime;

	// Use this for initialization
	void Start () {
		_lifeTime = 0.1f;
		Vector3 newPos = transform.position;
		newPos.y += 1f;
		transform.position = newPos;
	
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<Light> ().range += Time.deltaTime * 20;

		_lifeTime -= Time.deltaTime;

		if (_lifeTime <= 0) {
			Destroy (gameObject);
		}
	
	}
}
