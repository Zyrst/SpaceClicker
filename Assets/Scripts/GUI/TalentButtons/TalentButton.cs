using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalentButton : MonoBehaviour {

    public string _name = "";

    public CharacterStats _stats = new CharacterStats();
    public CharacterStats _totalStats = new CharacterStats();

    public uint _level = 0;
    public uint _maxLevel = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        TalentInfoBox.Instance.nameText.text = _name;
        TalentInfoBox.Instance.levelText.text = _level.ToString();

        TalentInfoBox.Instance.levelInfo.currentLevelText.text = _level.ToString();
        TalentInfoBox.Instance.levelInfo.maxLevelText.text = _maxLevel.ToString();
    }
}
