using UnityEngine;
using System.Collections;

public class LevelUpAnim : MonoBehaviour {
    SpriteRenderer _spriteRen;
    float _timer;
	// Use this for initialization
	void Start () {
        _spriteRen = GetComponent<SpriteRenderer>();
        _timer = GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        
	}
	
	// Update is called once per frame
	void Update () {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            Destroy(this.gameObject);
	}
}
