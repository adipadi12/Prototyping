using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    static float timeTillMaxDiffculty = 30;

    public static float GetDiffcultyPercentage()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad /  timeTillMaxDiffculty); 
    }
}
