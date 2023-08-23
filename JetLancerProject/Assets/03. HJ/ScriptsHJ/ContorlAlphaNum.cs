using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContorlAlphaNum : MonoBehaviour
{
    public GameObject refObj; //������ ������Ʈ
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

        yield return FadeImage(imageObj, 0f, 0.7f); // 1�ʰ� �������
        yield return new WaitForSeconds(displayDuration);
        //yield return FadeImage(imageObj, 1f, 0f); //1�ʰ� ��ο�����

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
