using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDeath : MonoBehaviour {

    public Image _screenImage;
    public Text _cdText;
    public Color _targetColor;

    private Color _originalColor;
    private Color _lastColor;

    public float _colorDelta;

    public float _inTime;
    public float _realTime;
    public float _time;

    private int dir = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        _realTime += Time.deltaTime;
        _time += Time.deltaTime * dir;

        _lastColor.a = _colorDelta * _time;
        _screenImage.color = _lastColor;

        _cdText.text = ((int)Mathf.Ceil((_inTime - _realTime))).ToString();

        if (_cdText.text == "1")
        {
            dir = 0;
        }

        if (_cdText.text == "0")
        {
            _screenImage.color = _originalColor;
            gameObject.SetActive(false);
        }
	
	}

    public void Died(float time_)
    {
        dir = 1;
        _time = 0;
        _realTime = _time;
        _inTime = time_;
        _originalColor = _screenImage.color;
        _lastColor = _originalColor;

        // (current - target) / tid
        _colorDelta = (_targetColor.a - _originalColor.a) / time_;
    }
}
