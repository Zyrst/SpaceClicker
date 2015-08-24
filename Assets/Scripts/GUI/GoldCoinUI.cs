using UnityEngine;
using System.Collections;

public class GoldCoinUI : MonoBehaviour {

    public static GoldCoinUI Instance = null;
    public Animator _animator;

    public bool _glow = false;
    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Check(uint gold_)
    {
        if (gold_ >= 75 && !_glow)
        {
            _animator.SetTrigger("Flash");
            _glow = true;
        }
        else if (gold_ < 75 && _glow)
        {
            _glow = false;
        }
    }
}
