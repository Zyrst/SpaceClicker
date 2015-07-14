using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class SpriteRef : MonoBehaviour {

    public Sprite sprite;

    public string path
    {
        get
        {
            return AssetDatabase.GetAssetPath(sprite);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {

    }
}
