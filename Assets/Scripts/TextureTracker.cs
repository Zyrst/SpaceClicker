using UnityEngine;
using System.Collections;

public class TextureTracker : MonoBehaviour {

    private static TextureTracker _instance;
    public static TextureTracker Instance
    {
        get
        {
            return _instance;
        }
    }

    public TextureTracker()
    {
        _instance = this;
    }

    [System.Serializable]
    public class Arena
    {
        public Texture[] _postAp;
        public Texture[] _mech;
    }

    public Arena _arena;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
