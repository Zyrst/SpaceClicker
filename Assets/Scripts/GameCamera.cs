using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    private Vector3 _originalPos = Vector3.zero;
    private Quaternion _originalRot = new Quaternion();

    public float _shakeDuration = 0.5f;
    public float _shakeTime = 0f;
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
        if (_shake && _shakeTime > 0f)
        {
            _shakeTime -= Time.deltaTime;

            transform.position = _originalPos; // +Random.insideUnitSphere * _shakeTime;
            transform.rotation = new Quaternion(
                            _originalRot.x + Random.Range(-_shakeTime, _shakeTime) * _shakeDist * Time.deltaTime,
                            _originalRot.y + Random.Range(-_shakeTime, _shakeTime) * _shakeDist * Time.deltaTime,
                            _originalRot.z + Random.Range(-_shakeTime, _shakeTime) * _shakeDist * Time.deltaTime,
                            _originalRot.w + Random.Range(-_shakeTime, _shakeTime) * _shakeDist * Time.deltaTime);
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
        if (!_shake)
        {
            _originalPos = transform.position;
            _originalRot = transform.rotation;
        }
        _shakeTime = _shakeDuration;

        _shake = true;
    }
}
