using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorTransition : MonoBehaviour
{
    public static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        throw new ArgumentException("Invalid hex color string");
    }

    public static string ColorToHex(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }

    public static Color InterpolateColor(Color startColor, Color endColor, float t)
    {
        return Color.Lerp(startColor, endColor, t);
    }

    public static List<Color> GenerateColorSteps(string startHex, string endHex, float seconds, int fps = 60)
    {
        int totalFrames = Mathf.CeilToInt(seconds * fps);
        Color startColor = HexToColor(startHex);
        Color endColor = HexToColor(endHex);

        List<Color> steps = new List<Color>();

        for (int frame = 0; frame <= totalFrames; frame++)
        {
            float t = (float)frame / totalFrames;
            steps.Add(InterpolateColor(startColor, endColor, t));
        }

        return steps;
    }

    // Example usage in Unity
    public float transitionSeconds = 3.0f;
    public string startHex = "#FF0000"; // Red
    public string endHex = "#00FF00";   // Green

    private List<Color> colorSteps;
    private int currentFrame;
    private Renderer objectRenderer;
    private float frameTime;
    private float timer;

    void Start()
    {
        colorSteps = GenerateColorSteps(startHex, endHex, transitionSeconds);
        objectRenderer = GetComponent<Renderer>();
        frameTime = 1.0f / 60.0f; // Assuming 60 FPS
        timer = 0;
        currentFrame = 0;
    }

    void Update()
    {
        if (currentFrame < colorSteps.Count)
        {
            timer += Time.deltaTime;

            if (timer >= frameTime)
            {
                timer -= frameTime;
                objectRenderer.material.color = colorSteps[currentFrame];
                currentFrame++;
            }
        }
    }
}
