using UnityEngine;
using System.Collections;

public class MouseCuboid : MonoBehaviour
{
    public static Collider collider = null;
    public static bool hit = false;

    private static int twoFrames = 2;
    private static MouseCuboid midPoint = null;
    private Vector3 lastPoint;

    private static  int ID = 0;
    private int id = 0;

    public MouseCuboid()
    {
        id = ID++;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
        if (id == 1)
        {
            lastPoint = transform.position;

            transform.forward = Camera.main.transform.forward;
            transform.position = MouseController.Instance.worldPosition;

            twoFrames--;
            if (twoFrames == 0)
            {
                hit = false;
            }

            if (midPoint == null)
            {
                midPoint = GameObject.Instantiate(this as MouseCuboid);
                midPoint.transform.parent = transform;
            }


            midPoint.transform.forward = transform.forward;
            Vector3 pos = midPoint.transform.position;
            pos = (transform.position + lastPoint) / 2f;
            midPoint.transform.position = pos;
            midPoint.transform.localScale = transform.localScale;

            Vector3 scale = midPoint.transform.localScale;
            scale.x *= Vector3.Distance(lastPoint, transform.position) > scale.x ? Vector3.Distance(lastPoint, transform.position) : scale.x;
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
        twoFrames = 2;
    }

    void OnTriggerStay(Collider col_)
    {
        collider = col_;

        hit = true;
        twoFrames = 2;
    }
}
