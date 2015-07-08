using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyTab : Tabs {

    public Vector3 _buttonOffset;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateNewEquipment()
    {
        foreach (var item in GetComponentsInChildren<BuyEquipmentButton>())
        {
            if (item.GetComponentInChildren<Equipment>() != null)
            {
                Destroy(item.GetComponentInChildren<Equipment>().gameObject);
            }
            item.GetComponent<Button>().interactable = true;
            BuyEquipmentGeneration.SetEquipmentOnSlot(item);
        }
        Debug.Log("New equipment has been generated");
    }
}
