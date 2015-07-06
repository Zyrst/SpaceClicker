using UnityEngine;
using System.Collections;

public class Tabs : MonoBehaviour {

    public enum TabType : int { Buy = 0, Upgrade = 1, MysteryItem = 2, Preview = 3 };

    public Tabs[] allTabs = new Tabs[4];
    public TabButton[] allTabButtons = new TabButton[4];

    public TabType _tabType;

    public GameObject _scrollerPanel;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenTab(TabType type_)
    {
        TabButton.UnHighlightAll();
        DisableAll();

        allTabButtons[(int)type_].Highlight();
        allTabs[(int)type_].gameObject.SetActive(true);

        switch (type_)
        {
            case TabType.Buy:
                break;
            case TabType.Upgrade:
                break;
            case TabType.MysteryItem:
                break;
            case TabType.Preview:
                break;
            default:
                break;
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < allTabs.Length; i++)
        {
            allTabs[i].gameObject.SetActive(false);
        }
    }
}
