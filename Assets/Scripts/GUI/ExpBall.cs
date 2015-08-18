using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class ExpBall : MonoBehaviour {
    
    public Vector3 _target;
    public float _speed;
    public float _delay;

    public uint _exp;

	// Use this for initialization
	void Start () {

        _delay = Random.Range(0f, 0.5f);
        _delay += Time.time;
        RectTransform rekt = Global.Instance._playerGUI.GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Exp").rectTransform;
        
        _target = rekt.rect.max * rekt.localScale.x;
        _target.x *= Screen.width / 1920f;
        _target.y *= Screen.height / 1080f;
	}
	
	// Update is called once per frame
    void Update()
    {

        // you spin me right round 
        transform.Rotate(0f, 0f, 360f * Time.deltaTime);

       
        if (Time.time >= _delay)
        {
            
            Vector3 calc = (_target - transform.position) * (_speed * Time.deltaTime);
            transform.position += calc;

            transform.localScale -= transform.localScale * (Time.deltaTime);

            if (Vector3.Distance(_target, transform.position) <= 20f)
            {
                Player.Instance._experience += _exp;

                if (Player.Instance._experience >= Player.Instance._experianceToNext)
                {
                    Player.Instance.LevelUp();
                    Player.Instance._talentPoints++;
                }

                Global.Instance.UpdateExpBar();
                Destroy(this.gameObject);
            }
        }

        
	}
}
