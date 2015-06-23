using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CharacterGUI : MonoBehaviour {
    public GameObject HealthBar;
    public GameObject LevelText;
    public GameObject HealthText;
    public Character character;

    
	// Use this for initialization
	void Start () {
        transform.forward = Camera.main.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        float scale = character._stats._health / character._stats._maxHealth;
        GameObject lifebar = HealthBar.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Life").gameObject;

        lifebar.transform.localScale = new Vector3(scale, lifebar.transform.localScale.y, lifebar.transform.localScale.z);

        if (character is Enemy)
        {
            LevelText.GetComponent<Text>().text = character._level.ToString();
            HealthText.GetComponent<Text>().text = ((int)(System.Math.Floor(character._stats._health+0.5f))).ToString();
        }
	}
}
