using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

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
    public int _currentHairStyle = 0;
    [HideInInspector]
    public int _currentHairColor = 0;
    [HideInInspector]
    public int _currentSkin = 0;
    [HideInInspector]
    public int _currentEyes = 0;

    private static CharacterCreation _instance = null;
    public static CharacterCreation Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CharCreate").GetComponent<CharacterCreation>();
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

    public void Init()
    {
        _playerPos = Global.Instance.player.transform.position;
        _playerRot = Global.Instance.player.transform.rotation;

        Global.Instance.player.gameObject.SetActive(true);
        Transform playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "maincharacter_combat_animation_idle_01Slow").transform;


        playerModel.position = GetComponentsInChildren<RectTransform>().FirstOrDefault(x => x.name == "CharPos").transform.position;
        //playerModel.LookAt(Global.Instance._uiCamera.transform.position);
        playerModel.transform.rotation = new Quaternion(1f, 133f, -2.4f, 0f);
        playerModel.localScale = new Vector3(20, 20, 20);

        Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = false;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = false;
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = false;
        
    }

    /// <summary>
    /// Changes hairstyle
    /// </summary>
    public void ChangeHair()
    {

        _hairStyles[_currentHairStyle].SetActive(false);
        _currentHairStyle++;
        if (_currentHairStyle >= _hairStyles.Length)
            _currentHairStyle = 0;
        _hairStyles[_currentHairStyle].SetActive(true);
        _hairMaterials[_currentHairStyle].color = _rainbow[_currentHairColor];

        
    }

    /// <summary>
    /// Changes hair color
    /// </summary>
    public void ChangeHairColor()
    {
        _currentHairColor++;
        if (_currentHairColor >= _rainbow.Length)
            _currentHairColor = 0;
        _hairMaterials[_currentHairStyle].color = _rainbow[_currentHairColor];
    }

    /// <summary>
    /// Change skin
    /// </summary>
    public void ChangeSkin()
    {
        _currentSkin++;
        if (_currentSkin >= _skinColor.Length)
            _currentSkin = 0;
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "MainC").GetComponent<SkinnedMeshRenderer>().material.SetTexture("_Base", _skinColor[_currentSkin]);
    }

    /// <summary>
    /// Change eyes
    /// </summary>
    public void ChangeEyes()
    {
        _currentEyes++;
        if (_currentEyes >= _eyes.Length)
            _currentEyes = 0;
        Global.Instance.player.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.name == "MainC").GetComponent<SkinnedMeshRenderer>().material.SetTexture("_Eyes", _eyes[_currentEyes]);
    }

    public void Rotate()
    {
        Transform playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "maincharacter_combat_animation_idle_01Slow").transform;
        playerModel.Rotate(new Vector3(0, GetComponentInChildren<Slider>().value, 0));
        playerModel.transform.eulerAngles = new Vector3(0, GetComponentInChildren<Slider>().value, 0);
    }

   


    public void Accept()
    {

        Transform playerModel = Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "maincharacter_combat_animation_idle_01Slow").transform;

        playerModel.position = _playerPos;
        playerModel.rotation = _playerRot;
        playerModel.localScale = new Vector3(1, 1, 1);

        Global.Instance.player.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "GUI").GetComponentInChildren<Canvas>().enabled = true;
        Global.Instance.player.GetComponent<ClickAttack>().enabled = true;
        Global.Instance._playerGUI.GetComponentInChildren<Canvas>().enabled = true;
        Global.Instance.player.gameObject.SetActive(false);

        Ship.Instance.ExitCharCreation();
    }
}
