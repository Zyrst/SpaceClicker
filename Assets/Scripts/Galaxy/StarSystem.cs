using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarSystem : MonoBehaviour
{
    public uint _llevel = 0;
    public uint _ulevel = 0;

    // for ze planetz
    public int _seed;

    public int _numberOfPlanets;

    public enum StarBackgrounds : int { White = 0, Blue = 1, Green = 2, Yellow = 3, Orange = 4, Red = 5, Maroon = 6, Death = 7 };

    public int _starColor = (int)StarBackgrounds.Yellow;
    public StarBackgrounds _starBackground = StarBackgrounds.Yellow;

    private float _rotate = 0.5f;
    private float _speedBoost = 0f;

    private bool _move = false;

    private Vector3 lastframepos = Vector3.zero;

    /// <summary>
    /// never ever use! (resets the seed)
    /// </summary>
    public void GenerateNumberOfPlanets()
    {
        Random.seed = _seed;
        _numberOfPlanets = Random.Range(3, 9);
    }

    public void GenerateRotationAndStuff()
    {
        _rotate = Random.Range(-1f, 1f);
        _speedBoost = Random.Range(0f, 0.07f);
    }

    void Update()
    {
        if (_move)
        {

            transform.Translate(0, Time.deltaTime * _rotate + (_speedBoost * _rotate), 0);
            transform.Rotate(0, 0, Time.deltaTime * _rotate + (_speedBoost * _rotate));

        }

        RectTransform rect = GetComponentInChildren<Text>().GetComponent<RectTransform>();
        rect.localPosition = new Vector3(140f, 23f, 0f);
    }
}
