using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject deActiveObj;
    public GameObject activeObj;

    public GameObject middleObj; // ������ �������� ������Ʈ �Դϴ�.
    private float animationDuration = 3f; // �ִϸ��̼� ���� �ð� (��)
    private float targetWidth = 500f; // ��ǥ �ʺ�

    private RectTransform rectTransform; // UI �̹����� RectTransform
    private float startTime; // �ִϸ��̼� ���� �ð�
    public bool animationStarted = false; // �ִϸ��̼��� ���۵Ǿ����� ����

    //public GameObject unlockSound;
    

    public Image image1;
    public Image image2;
    public GameObject objectToDeactivate; // ��Ȱ��ȭ�� ���� ������Ʈ �߰�
    private float fadeDuration = 1.0f;
    private float displayDuration = 2.0f;
    

    private bool hasStarted = false;

    private void Start()
    {
        //unlockSound = GameObject.Find("OneSoundOn");
        activeObj.SetActive(false);
        rectTransform = middleObj.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
        StartCoroutine(StartFadeInOut());

    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 )
        {
            if (Input.anyKeyDown && !animationStarted)
            {
                deActiveObj.SetActive(false);
                activeObj.SetActive(true);  //TODO public ���� �޾ƿ��°� ��ũ��Ʈ�� �޾ƿ��� �ٲ����
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MiddleMove();

        }
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

        objectToDeactivate.SetActive(false); // ���� ������Ʈ ��Ȱ��ȭ

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


    void MiddleMove()
    {
        if (Input.anyKeyDown && !animationStarted) // �ƹ� Ű�� ������ �ִϸ��̼��� ���۵��� �ʾҴٸ�
        {
            animationStarted = true; // �ִϸ��̼� ���� �÷��� ����
            startTime = Time.time; // �ִϸ��̼� ���� �ð� ���
        }

        if (animationStarted) // �ִϸ��̼� ���۵Ǿ��� ���� �۵�
        {
            float elapsedTime = Time.time - startTime; // ��� �ð� ���

            if (elapsedTime <= animationDuration) // �ִϸ��̼� ���� ���� ���
            {
                float normalizedTime = elapsedTime / animationDuration; // ����ȭ�� �ð� ���
                float newWidth = Mathf.Lerp(rectTransform.sizeDelta.x, targetWidth, normalizedTime); // �ʺ� ���� ���
                rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y); // �̹��� ũ�� ������Ʈ
            }
            else // �ִϸ��̼��� �Ϸ�� ���
            {
                ////TODO public ���� �޾ƿ��°� ��ũ��Ʈ�� �޾ƿ��� �ٲ����
                //rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y); // ��ǥ �ʺ�� ũ�� ����
                //animationStarted = false; // �ִϸ��̼� ���� �÷��� ����
                /* Do nothing */
            }
        }
    }

}
