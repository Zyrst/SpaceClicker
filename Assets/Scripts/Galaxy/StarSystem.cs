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

    /// <summary>
    /// never ever use! (resets the seed)
    /// </summary>
    public void GenerateNumberOfPlanets()
    {
        Random.seed = _seed;
        _numberOfPlanets = Random.Range(3, 9);
    }
}
