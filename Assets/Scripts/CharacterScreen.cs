using UnityEngine;
using System.Collections;

public class CharacterScreen : MonoBehaviour {

    public GameObject InventoryBackground;
    public GameObject CharacterBackground;
    public GameObject CharacterStatsBackground;

    public float _offSet;
    public int _numInventorySlot = 5;
    public Vector3 _invStartPos = new Vector3();
    public ArrayList _inventory = new ArrayList();

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateInventorySlots()
    {
        Vector3 currentPos = _invStartPos;
        for (int j = 0; j < _numInventorySlot; j++)
        {
            for (int i = 0; i < _numInventorySlot; i++)
            {
                GameObject tmpInv = GameObject.Instantiate(Global.Instance._prefabs.InventorySlot);
                tmpInv.gameObject.GetComponent<RectTransform>().position = currentPos;
                currentPos.x += _offSet;
                tmpInv.gameObject.GetComponent<RectTransform>().parent = transform.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
                tmpInv.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                _inventory.Add(tmpInv);
            }
            currentPos.x = _invStartPos.x;
            currentPos.y -= _offSet;
        }
    }
}
