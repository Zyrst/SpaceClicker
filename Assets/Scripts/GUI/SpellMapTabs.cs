using UnityEngine;
using System.Collections;

public class SpellMapTabs : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DamageTab()
    {
        GetComponentInParent<SpellsMap>().SwitchTabs(SpellsMap.Tabs.Damage);
    }

    public void HealingTab()
    {
        GetComponentInParent<SpellsMap>().SwitchTabs(SpellsMap.Tabs.Healing);
    }

    public void StunTab()
    {
        GetComponentInParent<SpellsMap>().SwitchTabs(SpellsMap.Tabs.Stun);
    }
}
