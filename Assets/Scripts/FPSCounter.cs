using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI FPSDisplay;

    public enum DisplayMode {  FPS, MS }
    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    [SerializeField, Range(0.2f,2f)]
    float sampleDuration = 1.0f;
    int frames;
    float duration, bestDuration = float.MaxValue, worstDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime; //to check for time passed between current and previous frame
        frames++;
        duration += frameDuration; //introduced to make fps counter less erratic so as to calculate an avg fps over time

        if (frameDuration < bestDuration)
        {
            bestDuration = frameDuration;
        }

        if (duration > worstDuration) {
            worstDuration = frameDuration;
        }

        if (duration >= sampleDuration)
        {
            if (displayMode == DisplayMode.FPS)
            {
                FPSDisplay.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", //frame / duration = frame rate also shows FPS till no decimal places
                    1f / bestDuration, //indicating where FPS was the best
                    frames / duration,
                    1f / worstDuration); //indicating where FPS was the worst
            }
            else
            {
                FPSDisplay.SetText(
                    "MS\n{0:1}\n{1:1}\n{2:1}",
                    1000f * bestDuration,
                    1000f * duration / frames,
                    1000f * worstDuration
                    );
            }
            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }
}
