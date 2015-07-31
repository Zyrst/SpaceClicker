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
    public class SkinTexture
    {
        public Texture _skin0;
        public Texture _skin1;
        public Texture _skin2;
        public Texture _skin3;
        public Texture _skin4;
        public Texture _skin5;
        public Texture _skin6;
    }

    [System.Serializable]
    public class EyeTextures
    {
        public Texture _eye0;
        public Texture _eye1;
        public Texture _eye2;
        public Texture _eye3;
        public Texture _eye4;
        public Texture _eye5;
        public Texture _eye6;
        public Texture _eye7;
    }

    public SkinTexture _skins = new SkinTexture();
    public EyeTextures _eyes = new EyeTextures();



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
