using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ScreenFading : MonoBehaviour
{
    public Image fadeImage;
    public Color fadeColor;
    private float fadeAmount;


    private void Awake () {
        FadeIn();
    }

    public IEnumerator FadeScreen(float fadeSpeed, bool fadeToBlack) {
        if(fadeToBlack) {
            while (fadeImage.color.a < 1) {
                fadeAmount = fadeImage.color.a + (fadeSpeed * Time.deltaTime);
                fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                yield return null;
            }
        }
        else {
            while (fadeImage.color.a > 0) {
                fadeAmount = fadeImage.color.a - (fadeSpeed * Time.deltaTime);
                fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                yield return null;
            }
        }
    }

    public void FadeIn () {
        StartCoroutine(FadeScreen(0.5f, false));
    }

    public void FadeOut () {
        StartCoroutine(FadeScreen(0.5f, true));
    }
}
