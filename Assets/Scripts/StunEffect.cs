using UnityEngine;
using System.Collections;

public class StunEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.LookAt(Global.Instance._gameCamera.transform);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, 300f * Time.deltaTime));
	}
}
