using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class MenuSpellSlotButton : Button {

    public SpellAttack _spell;
    public bool _held = false;
    private Vector2 _startPos;
    private Vector2 _mouseDelta;
    private GameObject _slot;
    private int _slotNum = 0;
    public bool _firstClick = true;
	
	// Update is called once per frame
	void Update () {
        if (_held)
        {
            _slotNum = 0;
            GetComponent<RectTransform>().position = new Vector2(MouseController.Instance.position.x, MouseController.Instance.position.y) + _mouseDelta;

            // check overlap with spellslots
            foreach (var item in GetComponentInParent<SpellsMap>().spellSlots.GetComponentsInChildren<MenuSpellSlotButton>())
            {
                Rect me = new Rect(new Vector2(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y), GetComponent<RectTransform>().rect.size/2f);
                Rect other = new Rect(new Vector2(item.GetComponent<RectTransform>().position.x, item.GetComponent<RectTransform>().position.y), item.GetComponent<RectTransform>().rect.size/2f);

                _slot = null;
                if (me.Overlaps(other))
                {
                    GetComponent<RectTransform>().position = item.GetComponent<RectTransform>().position;
                    _slot = item.gameObject;
                    break;
                }
                _slotNum++;
            }
        }
	}

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        GetComponentInParent<SpellsMap>().spellInfo.gameObject.GetComponent<SpellInfo>().Activate(_spell);
        Sounds.OneShot(Sounds.Instance.uiSounds.Button);
    }
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (transform.parent.name == "SpellsList")
        {
            // pickup and follow mouse
            _startPos = GetComponent<RectTransform>().position;
            _mouseDelta = GetComponent<RectTransform>().position - MouseController.Instance.position;
            _held = true;
        }
        if (_firstClick)
        {
            Sounds.OneShot(Sounds.Instance.uiSounds.pickUpAbility);
            _firstClick = false;
        }
    }

    
    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _firstClick = true;
        if (transform.parent.name == "SpellsList")
        {
            if (_slot != null)
            {
                GameObject old = null;
                // remove old spell
                if (Global.Instance.player._spellsArray[_slotNum] != null)
                {
                    old = Global.Instance.player._spellsArray[_slotNum].gameObject;
                    Global.Instance.player._spellsArray[_slotNum]._cdDoneImage.gameObject.SetActive(true);
                    Global.Instance.player._spellsArray[_slotNum]._cdImage.gameObject.SetActive(true);

                }

                // create new instance of selected spell
                GameObject go = GameObject.Instantiate(_spell.gameObject);
                go.name = _spell.gameObject.name;
                Global.Instance.player._spellsArray[_slotNum] = go.GetComponent<SpellAttack>();


                Global.Instance.player._spellSlotArray[_slotNum
                    ].GetComponentsInChildren<Image>(true).FirstOrDefault(
                        x => x.name == "Image").sprite = go.GetComponent<SpellAttack>()._spellImage.sprite;

                go.transform.parent = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Spells").transform;
                if (go.GetComponent<SpellAttack>()._slot != null)
                    go.GetComponent<SpellAttack>()._slot = null;

                go.GetComponent<SpellAttack>()._slot = Global.Instance.player._spellSlotArray[_slotNum];
                go.GetComponent<SpellAttack>().Init();

                if (old != null)
                {
                    Destroy(old);
                }
                Sounds.OneShot(Sounds.Instance.uiSounds.changeAbility);
            }

            
            GetComponent<RectTransform>().position = _startPos;
            _held = false;
        }

        GetComponentInParent<SpellsMap>().UpdatePlayerSpellSlots();
    }
}
