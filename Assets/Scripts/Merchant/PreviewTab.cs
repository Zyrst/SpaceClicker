using UnityEngine;
using System.Collections;

public class PreviewTab : Tabs {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateButtons()
    {
        int counter = 0;
        foreach (var item in Global.Instance._player._inventoryArray)
        {
            counter ++;
            Vector3 pos = Vector3.zero;
            if (item == null)
            {
                break;
            }
            if (GetComponentsInChildren<BuyEquipmentButton>().Length < counter)
            {
                Global.Instance.DebugOnScreen("trying to make the buttons");
                // generate button
                Object button = (GetComponentsInChildren<BuyEquipmentButton>()[0].gameObject);
                GameObject go = (GameObject)GameObject.Instantiate(button, ((GameObject)button).transform.position, ((GameObject)button).transform.rotation);
                go.name = "Button" + counter;
                go.GetComponent<RectTransform>().SetParent(_scrollerPanel.GetComponent<RectTransform>());

                if (counter % 2 == 0)
                    pos.x = 475f;
                else
                {
                    if (counter != 2)
                    {
                        //pos.y -= 200f;
                    }
                    pos.x = 0f;
                }

                go.GetComponent<RectTransform>().localScale = Vector3.one;
                go.GetComponent<RectTransform>().localPosition = pos;


                Global.Instance.DebugOnScreen("x: " + pos.x + " y: " + pos.y + "\t" + "x: " + go.GetComponent<RectTransform>().localPosition.x + " y: " + go.GetComponent<RectTransform>().localPosition.y);
            }
        }
    }
}
