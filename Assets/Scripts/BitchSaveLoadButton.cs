using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BitchSaveLoadButton : Button {

    public bool trueIfSave = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (trueIfSave)
        {
            SaveLoadSystem.Save();
        }
        else
        {
            SaveLoadSystem.Load();
        }

    }
}
