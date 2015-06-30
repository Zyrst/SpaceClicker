using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalentInfoBox : MonoBehaviour {

    public TalentInfoBox instance;
    private static TalentInfoBox _instance;
    public static TalentInfoBox Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("SUPERFAIL - talent info box not sett");
            }
            return _instance;
        }
    }

    public Text nameText;
    public Text levelText;

    public LevelInfo levelInfo;
    public EffectInfo currentInfo;
    public EffectInfo nextInfo;

	// Use this for initialization
	void Start () {
        _instance = instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [System.Serializable]
    public class LevelInfo
    {
        public Text currentLevelText;
        public Text maxLevelText;
    }
    [System.Serializable]
    public class EffectInfo
    {
        public Text statsInfoText;
    }
}
