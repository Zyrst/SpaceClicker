using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CharacterScreen : MonoBehaviour {

    public GameObject InventoryBackground;
    public GameObject CharacterBackground;
    public GameObject CharacterStatsBackground;
    public GameObject _EquipPopup;

    public Vector2 _offSet;
    public Vector2 _numInventorySlot;
    public Vector3 _invStartPos = new Vector3();
    public bool _lastFrameClick = false;

    public ArrayList _inventory = new ArrayList();
    public Equipment _lastEquip;

    private static CharacterScreen _instance = null;
    public static CharacterScreen Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CharScreen").GetComponent<CharacterScreen>();
            }
            return _instance;
        }
    }
	// Use this for initialization
	void Start () {
        Instance.gameObject.SetActive(false);
        CloseTalentTree();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if (_lastFrameClick && !MouseController.Instance.buttonDown)
        {
            _EquipPopup.transform.gameObject.SetActive(false);
        }
        if (MouseController.Instance.buttonDown && _EquipPopup.activeSelf)
        {
            _lastFrameClick = true;

        }
        else
        {
            _lastFrameClick = false;
        }
    }

    public void TalentTree()
    {
        GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "TalentTree").gameObject.SetActive(true);
    }

    public void CloseTalentTree()
    {
        GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "TalentTree").gameObject.SetActive(false);
    }

    public void GenerateInventorySlots()
    {
        EquipmentPopup.Reset();
        Vector3 currentPos = Vector3.zero;
        currentPos.x = _invStartPos.x;
        currentPos.y = _invStartPos.y;
        int counter = 0;
        for (int j = 0; j < _numInventorySlot.y; j++)
        {
            for (int i = 0; i < _numInventorySlot.x; i++)
            {
                
                GameObject tmpInv = GameObject.Instantiate(Global.Instance._prefabs.InventorySlot);
                //tmpInv.gameObject.GetComponent<RectTransform>().SetParent(transform.GetComponentInChildren<Canvas>().GetComponent<RectTransform>());
                tmpInv.gameObject.GetComponent<RectTransform>().SetParent(
                    transform.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "InventoryBackground").GetComponent<RectTransform>());
                tmpInv.gameObject.GetComponent<RectTransform>().localPosition = currentPos;
                currentPos.x += _offSet.x;
                tmpInv.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                try
                {
                    tmpInv.gameObject.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "EquipSprite").sprite = Global.Instance._player._inventoryArray[counter].gameObject.GetComponent<Equipment>()._sprite;
                }
                catch (System.NullReferenceException) {}
  
                _inventory.Add(tmpInv);
                counter++;
            }
            currentPos.x = _invStartPos.x;
            currentPos.y -= _offSet.y;
        }
    }

    public void RemoveInventorySlots()
    {
        foreach (var item in _inventory)
        {
            Destroy((GameObject)item);
        }
        _inventory.Clear();
    }

    public void EquipLast()
    {
        Debug.Log("EquipLast");
        ResetInventorySprite(_lastEquip);
        Global.Instance._player._equipped.Equip(_lastEquip);
    }

    public void PopItUp(Equipment equi_)
    {
        _EquipPopup.gameObject.SetActive(true);
        Vector3 temp = MouseController.Instance.position;
        temp.y += 40f;
        _EquipPopup.transform.position = temp;
        _lastEquip = equi_;
    }

    public void ClosePopup()
    {
        _EquipPopup.gameObject.SetActive(false);
    }

    public void ResetInventorySprite(Equipment equi_)
    {

        for (int i = 0; i < Global.Instance._player._inventoryArray.Length; i++)
        {
            if (Global.Instance._player._inventoryArray[i] == equi_.gameObject)
            {
                object[] arr = _inventory.ToArray();
                ((GameObject)arr[i]).GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "EquipSprite").sprite = null;
            }
        }
    }

    public void GenerateCharInfo()
    {
        
        string info = "Health: " +  Global.Instance._player._combinedStats._maxHealth.GetString();
        info += System.Environment.NewLine + "Click Damage : " + Global.Instance._player._combinedStats._normal.damage.GetString();
        info += System.Environment.NewLine + "Click Crit Chance" + Global.Instance._player._combinedStats._normal.crit.ToString();
        info += System.Environment.NewLine + "Tech Damage : " +  Global.Instance._player._combinedStats._tech.damage.GetString();
        info += System.Environment.NewLine + "Kinetic Damage : " + Global.Instance._player._combinedStats._kinetic.damage.GetString();
        info += System.Environment.NewLine + "Psychic Damage : " + Global.Instance._player._combinedStats._psychic.damage.GetString();


        GetComponentsInChildren<Text>().FirstOrDefault(x => x.name == "CharInfo").text = info;
    }

}
