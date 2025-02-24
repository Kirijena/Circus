using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    Image img;
    Color tempColor;

    void Start()
    {
        img = GetComponent<Image>();
        tempColor = img.color;
        tempColor.a = 1f;
        img.color = tempColor;
        StartCoroutine(FadeOut(0.05f)); // Reduced time
    }

    IEnumerator FadeOut(float seconds)
    {
        for (float a = 1f; a >= -0.05f; a -= 0.1f) // Faster step
        {
            tempColor = img.color;
            tempColor.a = a;
            img.color = tempColor;
            yield return new WaitForSeconds(seconds);
        }
        img.raycastTarget = false;
    }

    public IEnumerator FadeIn(float seconds)
    {
        img.raycastTarget = true;
        for (float a = 0f; a <= 1.05f; a += 0.1f) // Faster step
        {
            tempColor = img.color;
            tempColor.a = a;
            img.color = tempColor;
            yield return new WaitForSeconds(seconds);
        }
    }
}