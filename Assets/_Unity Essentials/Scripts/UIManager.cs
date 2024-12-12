

using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GradientGrid gradientGrid; // Reference to the GradientGrid script

    public void SetColorChangeInterval(float interval)
    {
        Debug.Log("called.................................");
        if (gradientGrid != null)
        {
            gradientGrid.colorChangeInterval = interval;
            Debug.Log($"Color change interval set to {interval} seconds.");
        }
        else
        {
            Debug.LogError("GradientGrid reference is not assigned.");
        }
    }
}
