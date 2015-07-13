using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpellsMap : MonoBehaviour {

    public GameObject spellSlots;
    public GameObject spellList;

    public enum Tabs : int { Damage = 0, Healing = 1, Stun = 2 };

    public Rect _buttonRect;
    public Vector2 _buttonOffset;
    public int _perRow;

    public Tabs _latestTab = Tabs.Damage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open()
    {
        gameObject.SetActive(true);

        UpdatePlayerSpellSlots();

        SwitchTabs(Tabs.Damage);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePlayerSpellSlots()
    {
        MenuSpellSlotButton[] arr = GetComponentsInChildren<MenuSpellSlotButton>() as MenuSpellSlotButton[];
        for (int i = 0; i < Global.Instance.player._spellsArray.Length; i++)
        {
            if (Global.Instance.player._spellsArray[i] != null)
            {
                arr[i].GetComponent<Image>().sprite = Global.Instance.player._spellsArray[i]._spellImage;
                arr[i]._spell = Global.Instance.player._spellsArray[i];
            }
        }
    }

    public void SwitchTabs(Tabs tab_)
    {
        foreach (var item in spellList.GetComponentsInChildren<MenuSpellSlotButton>())
        {
            Destroy(item.gameObject);
        }

        List<SpellAttack> spells = new List<SpellAttack>();
        _latestTab = tab_;
        switch (tab_)
        {
            case Tabs.Damage:
                spells = Global.Instance._unlockedSpells.damageSpells;
                break;
            case Tabs.Healing:
                spells = Global.Instance._unlockedSpells.healSpells;
                break;
            case Tabs.Stun:
                spells = Global.Instance._unlockedSpells.stunSpells;
                break;
            default:
                break;
        }

        Rect rect = _buttonRect;
        int row = 0;
        //for (int i = 0; i < 20; i++)
        foreach (var item in spells)
        {
            // make the button
            GameObject go = new GameObject();
            go.name = "Button";
            go.AddComponent<Image>();
            go.AddComponent<MenuSpellSlotButton>();
            MenuSpellSlotButton msp = go.GetComponent<MenuSpellSlotButton>();
            go.GetComponent<Button>().targetGraphic = go.GetComponent<Image>();
            go.GetComponent<RectTransform>().SetParent(spellList.transform);
            go.GetComponent<RectTransform>().anchoredPosition = rect.position;
            go.GetComponent<RectTransform>().sizeDelta = rect.size;
            go.GetComponent<RectTransform>().localScale = Vector3.one;

            msp._spell = item;
            msp.GetComponent<Image>().sprite = item._spellImage;

            rect.position = new Vector2(rect.position.x + _buttonOffset.x, rect.position.y);
            row++;
            if (row == _perRow)
            {
                rect.position = new Vector2(_buttonRect.position.x, rect.position.y - _buttonOffset.y);
                row = 0;
            }
        }
    }
}
