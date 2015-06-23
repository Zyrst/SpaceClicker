using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SpellAttack : BaseAttack {

    public SpellStats _stats = new SpellStats();
    public SpellStats _combinedStats = new SpellStats();
    public Sprite _spellImage;
    public GameObject _slot;
    public Image _slotImage;

    private bool _clicked = false;
    private bool _cd = false;
    private float _coolDown = 0f;
    public Vector3 _startGUIPos;
    public Vector3 _followerDiff = Vector3.zero;

	// Use this for initialization
	void Start () {
       _slot =  Global.Instance._player.getSpellSlot(this);
       if (_slot == null)
       {
           Debug.Log("slot är null");
       }
       if (_spellImage == null)
       {
           Debug.Log("_spellImage är null");
       }
       _slotImage = _slot.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Image");
      _slotImage.sprite = _spellImage;

       _startGUIPos = _slot.transform.position;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (_clicked)
        {
            FollowMouse();
        }
        Cooldown();
	}

    public void Clicked()
    {
        if (!_clicked && !_cd)
        {
            _startGUIPos = _slot.transform.position;
            _followerDiff = MouseController.Instance.position - _slot.transform.position;
            _clicked = true;
            MouseController.Instance.locked = true;
            Invoke("ResetCanDealDamage", 0.1f);
        }
    }

    public void FollowMouse()
    {
        _slot.transform.position = new Vector3(_followerDiff.x + MouseController.Instance.position.x, 
            _followerDiff.y + MouseController.Instance.position.y,
            _slot.transform.position.z);
        if (!MouseController.Instance.buttonDown)
        {
            ResetGUI();
        }
        else
        {
            CheckHit();
        }
    }

    public void CheckHit()
    {
            // mouseon the ground
            Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                try
                {
                    // hit enemy
                    if (hit.collider.transform.parent.parent.tag == "Enemy" || hit.collider.transform.parent.parent.tag == "Player" && hit.collider.transform.parent.parent.GetComponent<Character>()._isAlive)
                    {
                        Debug.Log("collide");
                        hit.collider.transform.parent.parent.gameObject.GetComponent<Character>().TakeDamage(DamageStats.GenerateFromSpellStats(_combinedStats), hit.point, Global.Instance._player);
                        _cd = true; 
                        _slotImage.color = new Color(0.5f, 0.5f, 0.5f);
                    }
                    ResetGUI();
                }
                catch (System.NullReferenceException) { }
            }
    }

    public void ResetGUI()
    {
        _clicked = false;
        MouseController.Instance.locked = false;
        _slot.transform.position = _startGUIPos;
    }

    public void Cooldown()
    {
        if (_cd)
        {
            _coolDown += Time.deltaTime;
            if (_coolDown >= _combinedStats._cooldown)
            {
                _cd = false;
                _coolDown = 0f;
                _slotImage.color = new Color(1f, 1f, 1f);
            }
        }
       
    }

    public void ResetCanDealDamage()
    {
        MouseController.Instance._locked = false;
    }

    public void CombineSpellStats()
    {
        _combinedStats = new SpellStats(_stats);
        _combinedStats.AddStats(Global.Instance._player._combinedStats);
    }
}
