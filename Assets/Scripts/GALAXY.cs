using UnityEngine;
using System.Collections;

public class Galaxy : MonoBehaviour {

    private static Galaxy _instance = null;
    public static Galaxy Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GALAXY").GetComponent<Galaxy>();
            }
            return _instance;
        }
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Generate()
    {
        Starmap.Instance.Generate(0, 100, 9001);
    }
}
