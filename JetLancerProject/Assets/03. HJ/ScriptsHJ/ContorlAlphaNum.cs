using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContorlAlphaNum : MonoBehaviour
{
    public GameObject refObj; //참조할 오브젝트
    public Image imageObj;

    [SerializeField]
    private float fadeDuration = 1.0f;
    private float displayDuration = 2.0f;
    private bool hasStarted = false;

    private void Update()
    {
        if (!refObj.activeSelf)
        {
            StartCoroutine(StartFadeInOut());
        }
    }


    private IEnumerator StartFadeInOut()
    {
        if (hasStarted)
            yield break;

        hasStarted = true;

        yield return FadeImage(imageObj, 0f, 0.7f); // 1초간 밝아지고
        yield return new WaitForSeconds(displayDuration);
        //yield return FadeImage(imageObj, 1f, 0f); //1초간 어두워지고

        hasStarted = false;
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float targetAlpha)
    {
        Color color = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }
}
