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
        Vector3 pos = Vector3.zero;

        if (Global.Instance._player._equipped._chest != null)
        {
            counter++;
            BuyEquipmentGeneration.ConnectEquipmentToTheButton(GetComponentsInChildren<BuyEquipmentButton>()[0], Global.Instance._player._equipped._chest);
            GetComponentsInChildren<BuyEquipmentButton>()[0]._equipText.text = "Equipped";
        }
        if (Global.Instance._player._equipped._head != null)
        {
            counter++;
            if (counter % 2 == 0)
                pos.x = 475f;
            else
            {
                if (counter != 2 && counter != 1)
                {
                    pos.y -= 200f;
                }
                pos.x = 0f;
            }
            if (GetComponentsInChildren<BuyEquipmentButton>().Length < counter)
            {
                BuyEquipmentButton but = makeAButton(pos);
                BuyEquipmentGeneration.ConnectEquipmentToTheButton(but, Global.Instance._player._equipped._head);
                but._equipText.text = "Equipped";
            }
            else
            {
                Debug.LogError("NÅGOT GICK JÄVLIGT FEL, DET SKALL INTE FINNAS NÅGON BUYEQUIPMENTBUTTON HÄR");
            }
        }
        if (Global.Instance._player._equipped._legs != null)
        {
            counter++;
            if (counter % 2 == 0)
                pos.x = 475f;
            else
            {
                if (counter != 2 && counter != 1)
                {
                    pos.y -= 200f;
                }
                pos.x = 0f;
            }
            if (GetComponentsInChildren<BuyEquipmentButton>().Length < counter)
            {
                BuyEquipmentButton but = makeAButton(pos);
                BuyEquipmentGeneration.ConnectEquipmentToTheButton(but, Global.Instance._player._equipped._legs);
                but._equipText.text = "Equipped";
            }
            else
            {
                Debug.LogError("NÅGOT GICK JÄVLIGT FEL, DET SKALL INTE FINNAS NÅGON BUYEQUIPMENTBUTTON HÄR");
            }
        }
        if (Global.Instance._player._equipped._weapon != null)
        {
            counter++;
            if (counter % 2 == 0)
                pos.x = 475f;
            else
            {
                if (counter != 2 && counter != 1)
                {
                    pos.y -= 200f;
                }
                pos.x = 0f;
            }
            if (GetComponentsInChildren<BuyEquipmentButton>().Length < counter)
            {
                BuyEquipmentButton but = makeAButton(pos);
                BuyEquipmentGeneration.ConnectEquipmentToTheButton(but, Global.Instance._player._equipped._weapon);
                but._equipText.text = "Equipped";
            }
            else
            {
                Debug.LogError("NÅGOT GICK JÄVLIGT FEL, DET SKALL INTE FINNAS NÅGON BUYEQUIPMENTBUTTON HÄR");
            }
        }

        foreach (var item in Global.Instance._player._inventoryArray)
        {
            counter ++;
            if (item == null)
            {
                break;
            }
            if (counter % 2 == 0)
                pos.x = 475f;
            else
            {
                if (counter != 2 && counter != 1)
                {
                    pos.y -= 200f;
                }
                pos.x = 0f;
            }
            if (GetComponentsInChildren<BuyEquipmentButton>().Length < counter)
            {
                BuyEquipmentButton but = makeAButton(pos);
                BuyEquipmentGeneration.ConnectEquipmentToTheButton(but, item.GetComponent<Equipment>());
                but._equipText.text = "";
            }
            else
            {
                BuyEquipmentGeneration.ConnectEquipmentToTheButton(GetComponentsInChildren<BuyEquipmentButton>()[counter-1], item.GetComponent<Equipment>());
            }
        }
        _scrollerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(_scrollerPanel.GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(pos.y) + 200f);
    }

    private BuyEquipmentButton makeAButton(Vector3 pos_)
    {
        Global.DebugOnScreen("trying to make the button\tx: " + pos_.x + " y: " + pos_.y);
        // generate button
        Object button = (GetComponentsInChildren<BuyEquipmentButton>()[0].gameObject);
        GameObject go = (GameObject)GameObject.Instantiate(button, Vector3.zero, ((GameObject)button).transform.rotation);
        go.name = "Button";
        go.GetComponent<RectTransform>().SetParent(_scrollerPanel.GetComponent<RectTransform>());


        go.GetComponent<RectTransform>().position = Vector3.zero;
        go.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos_.x, pos_.y, pos_.z);
        go.GetComponent<RectTransform>().localScale = Vector3.one;

        return go.GetComponent<BuyEquipmentButton>();
    }

    void OnDisable()
    {
        bool first = true;
        foreach (var item in GetComponentsInChildren<BuyEquipmentButton>(true))
        {
            if (!first)
            {
                Global.DebugOnScreen("Remove ONE BUTTON FROM PREVIEW TAB");
                Destroy(item.gameObject);
            }
            first = false;
        }
    }
}
