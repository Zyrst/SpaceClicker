﻿using UnityEngine;
using System.Collections;

public class MouseCuboid : MonoBehaviour
{
    public static Collider collider = null;
    public static bool hit = false;

    private static MouseCuboid midPoint = null;
    private Vector3 lastPoint;
    private bool mayUpdate = true;

    public MouseCuboid()
    {
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
        if (mayUpdate)
        {
            lastPoint = transform.position;

            transform.forward = Camera.main.transform.forward;
            transform.position = MouseController.Instance.worldPosition;

            if (midPoint == null)
            {
                midPoint = GameObject.Instantiate(this as MouseCuboid);
                midPoint.transform.parent = transform;
                midPoint.mayUpdate = false;
            }

            midPoint.transform.forward = transform.forward;
            Vector3 pos = midPoint.transform.position;
            pos = (transform.position + lastPoint) / 2f;
            midPoint.transform.position = pos;
            midPoint.transform.localScale = transform.localScale;

            Vector3 scale = midPoint.transform.localScale;
            scale.x *= Vector3.Distance(lastPoint, transform.position) > scale.x ? Vector3.Distance(lastPoint, transform.position) : scale.x;

            if (scale.x <= 0.5f)    scale.x = 0.5f;
            
            midPoint.transform.localScale = scale;

            midPoint.GetComponent<Collider>().enabled = false;
            midPoint.GetComponent<Collider>().enabled = true;
        }
	}

    void OnTriggerEnter(Collider col_)
    {
        //Global.DebugOnScreen("träffade någonting");
        collider = col_;

        hit = true;
    }

    void OnTriggerStay(Collider col_)
    {
        collider = col_;

        hit = true;
    }

    void OnTriggerExit(Collider col_)
    {
        hit = false;
    }
}
