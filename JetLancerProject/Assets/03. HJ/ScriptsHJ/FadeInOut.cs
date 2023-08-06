using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public GameObject objectToDeactivate; // 비활성화할 게임 오브젝트 추가
    public float fadeDuration = 1.0f;
    public float displayDuration = 2.0f;

    private bool hasStarted = false;

    private void Start()
    {
        StartCoroutine(StartFadeInOut());
    }

    private IEnumerator StartFadeInOut()
    {
        if (hasStarted)
            yield break;

        hasStarted = true;

        yield return FadeImage(image1, 0f, 1f);
        yield return new WaitForSeconds(displayDuration);
        yield return FadeImage(image1, 1f, 0f);

        yield return FadeImage(image2, 0f, 1f);
        yield return new WaitForSeconds(displayDuration);
        yield return FadeImage(image2, 1f, 0f);

        objectToDeactivate.SetActive(false); // 게임 오브젝트 비활성화

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
