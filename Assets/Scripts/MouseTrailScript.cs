using UnityEngine;
using System.Collections;

public class MouseTrailScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = MouseController.Instance.worldPosition;
        if (MouseController.Instance.buttonDown)
        {
            gameObject.GetComponent<TrailRenderer>().enabled = true;
        }
        else
            gameObject.GetComponent<TrailRenderer>().enabled = false;
	}
}
