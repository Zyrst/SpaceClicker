using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class PlayerGUI : MonoBehaviour {
    public GameObject HealthBar;
    public Player player;

    
	// Use this for initialization
	void Start () {
        transform.forward = Camera.main.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
        float scale = player._stats._health / player._stats._maxHealth;
        GameObject lifebar = HealthBar.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Life").gameObject;

        lifebar.transform.localScale = new Vector3(scale, lifebar.transform.localScale.y, lifebar.transform.localScale.z);
	}
}
