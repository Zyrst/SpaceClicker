using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CharacterGUI : MonoBehaviour {
    public GameObject HealthBar;
    public GameObject LevelText;
    public GameObject HealthText;
    public Character character;
    public GameObject Shield;
    public GameObject CooldownBar;

    
	// Use this for initialization
	void Start () {
        ResetDir();
	}

    public void ResetDir()
    {
        try
        {
            transform.forward = Camera.main.transform.forward;
        }
        catch (System.NullReferenceException) { }
    }
	
	// Update is called once per frame
	void Update () {
        float scale = vap.GetScale(character._stats._health, character._stats._maxHealth);

        if (character is Player)
        {
            scale = vap.GetScale(character._stats._health, ((Player)character)._combinedStats._maxHealth);
        }

        GameObject lifebar = HealthBar.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Life").gameObject;

        lifebar.transform.localScale = new Vector3(scale, lifebar.transform.localScale.y, lifebar.transform.localScale.z);

        if (character is Enemy)
        {
            LevelText.GetComponent<Text>().text = character._level.ToString();
            HealthText.GetComponent<Text>().text = character._stats._health.GetString();
            CooldownBar.GetComponent<Image>().transform.localScale = new Vector3(GetComponentInParent<EnemyAttack>()._attackTimer / GetComponentInParent<EnemyAttack>()._cooldownTimer, 1, 1);
        }
	}
}
