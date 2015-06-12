using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SpellAttack : BaseAttack {

    public SpellStats _stats = new SpellStats();
    public Sprite _spellImage;
    public GameObject _slot;

    private bool _clicked = false;
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
       _slot.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Image").sprite = _spellImage;

       _startGUIPos = _slot.transform.position;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (_clicked)
        {
            FollowMouse();
        }
	}

    public void Clicked()
    {
        if (!_clicked)
        {
            _startGUIPos = _slot.transform.position;
            _followerDiff = MouseController.Instance.position - _slot.transform.position;
            _clicked = true;
            MouseController.Instance.locked = true;
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
                    if (hit.collider.transform.parent.parent.tag == "Enemy" || hit.collider.transform.parent.parent.tag == "Player")
                    {
                        Debug.Log("collide");
                        hit.collider.transform.parent.parent.gameObject.GetComponent<Character>().TakeDamage(DamageStats.GenerateFromSpellStats(_stats), hit.point);
                        
                    }
                    ResetGUI();
                }
                catch (System.NullReferenceException e) { }
            }
    }

    public void ResetGUI()
    {
        _clicked = false;
        MouseController.Instance.locked = false;
        _slot.transform.position = _startGUIPos;
    }
}
