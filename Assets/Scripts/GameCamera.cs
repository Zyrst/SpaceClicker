using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    private Vector3 _originalPos = Vector3.zero;
    private Quaternion _originalRot = new Quaternion();

    public float _shakeStartIntensity = 0.05f;
    public float _shakeIntensity = 0f;
    public float _shakeDecay = 0.002f;
    public float _shakeDist = 0.1f;

    public bool _shake = false;

	// Use this for initialization
    void Start()
    {
        _originalPos = transform.position;
        _originalRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update()
    {
        if (_shake && _shakeIntensity > 0f)
        {
            _shakeIntensity -= _shakeDecay;

            transform.position = _originalPos + Random.insideUnitSphere * _shakeIntensity;
            transform.rotation = new Quaternion(
                            _originalRot.x + Random.Range(-_shakeIntensity, _shakeIntensity) * _shakeDist,
                            _originalRot.y + Random.Range(-_shakeIntensity, _shakeIntensity) * _shakeDist,
                            _originalRot.z + Random.Range(-_shakeIntensity, _shakeIntensity) * _shakeDist,
                            _originalRot.w + Random.Range(-_shakeIntensity, _shakeIntensity) * _shakeDist);
        }
        else
	    {
            _shake = false;
            transform.position = _originalPos;
            transform.rotation = _originalRot;
        }
    }

    public void Shake()
    {
        Debug.Log("gör en shake");

        if (!_shake)
        {
            _originalPos = transform.position;
            _originalRot = transform.rotation;
        }
        _shakeIntensity = _shakeStartIntensity;

        _shake = true;
    }
}
