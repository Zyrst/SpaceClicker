﻿using UnityEngine;
using System.Collections;

public class Merchant : MonoBehaviour {

    public Merchant _instance = null;
    private static Merchant instance = null;
    public static Merchant Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("<color=red>Merchant är null</color>");
                instance = GameObject.Find("Merchant").GetComponent<Merchant>();
            }

            return instance;
        }
    }

    public GameObject tabs;
    public GameObject mainBox;
    public GameObject ShopKeep;

    public Merchant()
    {
        instance = _instance;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        gameObject.SetActive(true);
        tabs.GetComponent<Tabs>().OpenTab(Tabs.TabType.Buy);

        GetComponentsInChildren<BuyTab>(true)[0].GenerateNewEquipment();
        GetComponentsInChildren<MysteryTab>(true)[0].GenerateNewItem();

        Music.Instance._menuTheme.StartMerchant();
    }

    public void Close()
    {
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
        Music.Instance._menuTheme.ExitMerchant();
        gameObject.SetActive(false);
    }
}
