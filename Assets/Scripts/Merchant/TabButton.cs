using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TabButton : UnityEngine.UI.Button {

    public Tabs.TabType _buttonType = Tabs.TabType.Buy;

    public ColorBlock _col;

    public TabButton()
    {
    }

	// Use this for initialization
	protected override void Start () {
        switch (_buttonType)
        {
            case Tabs.TabType.Buy:
                Highlight();
                break;
            case Tabs.TabType.Upgrade:
                break;
            case Tabs.TabType.MysteryItem:
                break;
            case Tabs.TabType.Preview:
                break;
            default:
                break;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //base.OnPointerClick(eventData);
        Highlight();
    }

    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        //base.OnPointerUp(eventData);
        if (Merchant.Instance.tabs == null)
        {
            Debug.Log("tabs == null");
            if (Merchant.Instance.tabs.GetComponent<Tabs>() == null )
            {
                Debug.Log("compo is null");
            }
        }
        Merchant.Instance.tabs.GetComponent<Tabs>().OpenTab(_buttonType);
    }

    public void Highlight()
    {
        UnHighlightAll();

        ColorBlock temp = _col;
        temp.normalColor = _col.pressedColor;
        GetComponent<Button>().colors = temp;
    }

    public static void UnHighlightAll()
    {
        for (int i = 0; i < Merchant.Instance.tabs.GetComponent<Tabs>().allTabButtons.Length; i++)
        {
            Merchant.Instance.tabs.GetComponent<Tabs>().allTabButtons[i].colors = Merchant.Instance.tabs.GetComponent<Tabs>().allTabButtons[i]._col;
        }
    }
}
