using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalentButtonButton : Button {

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        GetComponent<TalentButton>().Click();
    }
}
