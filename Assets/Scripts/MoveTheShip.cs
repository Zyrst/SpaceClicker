using UnityEngine;
using System.Collections;

public class MoveTheShip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = GetComponent<RectTransform>().localPosition;
        if (pos.x <= -1200f)
        {
            pos.x = 1400;
            pos.y = Random.Range(-300, 300);
        }
        pos.x -= Time.deltaTime * 10;
        GetComponent<RectTransform>().localPosition = pos;
	}
}
