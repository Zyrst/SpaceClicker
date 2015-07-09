using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MysteryTab : Tabs {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateNewItem()
    {
        foreach (var item in GetComponentsInChildren<BuyEquipmentButton>(true))
        {
            if (item.GetComponentInChildren<Equipment>() != null)
            {
                Destroy(item.GetComponentInChildren<Equipment>().gameObject);
            }
            item.GetComponent<Button>().interactable = true;

            Equipment equi = BuyEquipmentGeneration.GenerateOne();
            equi.transform.parent = item.transform;

            item._equipmentImage.sprite = equi._sprite;
        }
        Debug.Log("New Mystery Item has been generated");
    }
}
