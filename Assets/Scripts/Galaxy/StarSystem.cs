using UnityEngine;
using System.Collections;

public class StarSystem : MonoBehaviour
{
    public uint _llevel = 0;
    public uint _ulevel = 0;

    // for ze planetz
    public int _seed;

    public int _numberOfPlanets;

    public enum StarBackgrounds : int { Yellow = 0, Green = 1, Red = 2, Blue = 3 };
    public static Color[] StarColor = { Color.yellow, Color.green, Color.red, Color.blue };

    public int _starColor = (int)StarBackgrounds.Yellow;
    public StarBackgrounds _starBackground = StarBackgrounds.Yellow;

    private float _rotate = 0.5f;
    private float _speedBoost = 0f;

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
        transform.Translate(0, Time.deltaTime * _rotate + (_speedBoost * _rotate), 0);
        transform.Rotate(0, 0, Time.deltaTime * _rotate + (_speedBoost * _rotate)); 

    }
}
