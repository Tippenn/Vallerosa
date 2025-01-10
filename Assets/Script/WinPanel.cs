using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public Graphic uiElement;
    public float duration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;

        // Get the original color
        Color startColor = uiElement.color;
        Color endColor = startColor;
        endColor.a = 1f; // Target full alpha

        // Set initial alpha to 0
        startColor.a = 0f;
        uiElement.color = startColor;

        // Gradually lerp the alpha from 0 to 1
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            uiElement.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure final alpha is exactly 1
        uiElement.color = endColor;
    }
}
