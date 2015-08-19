using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.IO;

public class CharacterCreation : MonoBehaviour {
    
    public GameObject[] _hairStyles;
    public Material[] _hairMaterials;
    public Color[] _rainbow;
    public Texture[] _skinColor;
    public Texture[] _eyes;

    [HideInInspector]
    public Vector3 _playerPos;
    [HideInInspector]
    public Quaternion _playerRot;

    [HideInInspector]
    bool _firstTouch = false;
    [HideInInspector]
    Vector3 _oldPos = Vector3.zero;

    [HideInInspector]
    public int _currentHairStyle = 0;
    [HideInInspector]
    public int _currentHairColor = 0;
    [HideInInspector]
    public int _currentSkin = 0;
    [HideInInspector]
    public int _currentEyes = 0;

    Transform _playerModel;
    Transform _playerCollider;

    public GameObject _hairColorText;
    public GameObject _hairText;
    public GameObject _skinText;
    public GameObject _eyesText;

    private static CharacterCreation _instance = null;
    public static CharacterCreation Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CharCreate").GetComponent<CharacterCreation>();
                //_instance = Resources.FindObjectsOfTypeAll<CharacterCreation>()[0];
            }
            return _instance;
        }
    }
	// Use this for initialization
	void Start () {
       
	}
 
	// Update is called once per frame
	void Update () {
        if (MouseController.Instance.buttonDown)
        {
            Ray ray = Global.Instance._uiCamera.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                try
                {
                    if (hit.collider.transform.parent.parent.tag == "Player")
                    {
                        if (!_firstTouch)
                        {
                            _oldPos = MouseController.Instance.position;
                            _firstTouch = true;
                        }
                    }

                }
                catch (System.NullReferenceException)
                {
                }
            }
            if (_firstTouch)
            {
                Vector3 delta = _oldPos - MouseController.Instance.position;
                if (delta.x != 0f)
                {
                    _playerModel.Rotate(0f, delta.x, 0f);
                }

                _oldPos = MouseController.Instance.position;
            }
        }
        else if (!MouseController.Instance.buttonDown && _firstTouch)
        {
            _firstTouch = false;
        }
	}

    /// <summary>
    /// Initialize player to right place and set to "standard" hair combo
    /// </summary>
    public void Init()
    {
        _playerPos = Global.Instance.player.transform.position;
        _playerRot = Global.Instance.player.transform.rotation;

        Global.Instance.player.gameObject.SetActive(true);

        _playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "maincharacter_combat_animation_idle_01Slow").transform;
        _playerCollider = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "colliders").transform;
        _playerModel.position = GetComponentsInChildren<RectTransform>().FirstOrDefault(x => x.name == "CharPos").transform.position;
        _playerCollider.position = _playerModel.position;
        _playerModel.transform.rotation = new Quaternion(1f, 133f, -2.4f, 0f);
        _playerModel.localScale = new Vector3(20, 20, 20);
        _playerCollider.localScale = _playerModel.localScale;

        Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = false;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = false;
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = false;
        foreach (var item in _hairMaterials)
        {
            item.color = _rainbow[_currentHairColor];
        }
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "MainC").GetComponent<SkinnedMeshRenderer>().material.SetTexture("_Base", _skinColor[_currentSkin]);
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "MainC").GetComponent<SkinnedMeshRenderer>().material.SetTexture("_Eyes", _eyes[_currentEyes]);


        _hairColorText.GetComponentInChildren<Text>().text = "Hair Color " + (_currentHairColor + 1);
        _hairText.GetComponentInChildren<Text>().text = "Hair " + (_currentHairStyle + 1);
        _skinText.GetComponentInChildren<Text>().text = "Skin " + (_currentSkin + 1);
        _eyesText.GetComponentInChildren<Text>().text = "Eyes " + (_currentEyes + 1);
    }

    /// <summary>
    /// Change hairstyle
    /// </summary>
    /// <param name="val_">Right or left in array</param>
    public void ChangeHair(int val_)
    {
        _hairStyles[_currentHairStyle].SetActive(false);
        _currentHairStyle += val_;
        if (_currentHairStyle < 0)
            _currentHairStyle = _hairStyles.Length - 1;
        else if (_currentHairStyle >= _hairStyles.Length)
            _currentHairStyle = 0;
        _hairStyles[_currentHairStyle].SetActive(true);
        _hairMaterials[_currentHairStyle].color = _rainbow[_currentHairColor];
        _hairText.GetComponentInChildren<Text>().text = "Hair " + (_currentHairStyle + 1); 
    }
    
    /// <summary>
    /// Change hair color
    /// </summary>
    /// <param name="val_">Right or left in array</param>
    public void ChangeHairColor(int val_)
    {
        _currentHairColor += val_;
        if (_currentHairColor < 0)
            _currentHairColor = _rainbow.Length - 1;
        else if (_currentHairColor >= _rainbow.Length)
            _currentHairColor = 0;
        _hairMaterials[_currentHairStyle].color = _rainbow[_currentHairColor];
        _hairColorText.GetComponentInChildren<Text>().text = "Hair Color " + (_currentHairColor + 1); 
    }
    /// <summary>
    /// Change skin
    /// </summary>
    /// <param name="val_">Right or left in array</param>
    public void ChangeSkin(int val_)
    {
        _currentSkin += val_;
        if (_currentSkin < 0)
            _currentSkin = _skinColor.Length - 1;
        else if (_currentSkin >= _skinColor.Length)
            _currentSkin = 0;
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "MainC").GetComponent<SkinnedMeshRenderer>().material.SetTexture("_Base", _skinColor[_currentSkin]);
        _skinText.GetComponentInChildren<Text>().text = "Skin " + (_currentSkin + 1);
    }
    /// <summary>
    /// Change eyes
    /// </summary>
    /// <param name="val_">Right or left in array</param>
    public void ChangeEyes(int val_)
    {
        _currentEyes += val_;
        if(_currentEyes < 0 )
            _currentEyes = _eyes.Length - 1;
        else if(_currentEyes >= _eyes.Length)
            _currentEyes = 0;
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "MainC").GetComponent<SkinnedMeshRenderer>().material.SetTexture("_Eyes", _eyes[_currentEyes]);
        _eyesText.GetComponentInChildren<Text>().text = "Eyes " + (_currentEyes + 1);
    }

    public void Rotate()
    {
        _playerModel.transform.eulerAngles = new Vector3(0, GetComponentInChildren<Slider>().value, 0);
    }

   
    /// <summary>
    /// Presses accept button, resets everything and writes to file that you have done charcreate
    /// </summary>
    public void Accept()
    {

        _playerModel.position = _playerPos;
        _playerModel.rotation = _playerRot;
        _playerModel.localScale = new Vector3(1, 1, 1);
        _playerCollider.position = _playerModel.position;
        _playerCollider.localScale = _playerModel.localScale;

        Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = true;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = true;
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = true;
        Global.Instance.player.gameObject.SetActive(false);

        string fileName = "Save.txt";
        if (File.Exists(fileName))
        {
            StreamWriter sw = File.CreateText("Save.txt");
            sw.WriteLine("CharCreation: 1");
            sw.Close();
           
        }


        Ship.Instance.ExitCharCreation();
    }

}
