using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NextPlanetButton : Button {

    public Planet _nextPlanet = null;
    public GameObject _effect;

    private List<GameObject> _effectList = new List<GameObject>();

    protected override void OnEnable()
    {
        base.OnEnable();

        Sounds.OneShot(Sounds.Instance.music.questDone);

        // spawn effect
        GameObject go = GameObject.Instantiate(_effect);
        go.transform.forward = Camera.main.transform.forward;
        go.transform.position = Camera.main.ScreenToWorldPoint(GetComponent<RectTransform>().position) + new Vector3(0, 0,0);
        _effectList.Add(go);

        // when effect is done
            // enable button

        transform.localScale = new Vector3(0, 0, 0);
        if (!IsInvoking())
            Invoke("enable", 2.833f);//go.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }

    protected override void OnDisable()
    {
        if (IsInvoking())
            CancelInvoke();

        foreach (var item in _effectList)
        {
            Destroy(item.gameObject);
        }
        _effectList.Clear();

        base.OnDisable();
    }

    public void enable()
    {
        transform.localScale = new Vector3(1,1,1);
    }

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        FarmMode.Instance.backToShip();
        Sounds.OneShot(Sounds.Instance.uiSounds.navigation.eterStarmap);
        Global.Instance.SwitchScene(Global.GameType.Star);
        Starmap.Instance.SelectPlanet(_nextPlanet);
        Ship.Instance.Farm();
        FarmMode.Instance._nextPlanetButton.SetActive(false);
    }
}
