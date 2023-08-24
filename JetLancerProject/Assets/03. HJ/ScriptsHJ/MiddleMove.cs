using UnityEngine;
using UnityEngine.UI;

public class MiddleMove : MonoBehaviour
{
    public float animationDuration = 3f; // �ִϸ��̼� ���� �ð� (��)
    public float targetWidth = 500f; // ��ǥ �ʺ�

    private RectTransform rectTransform; // UI �̹����� RectTransform
    private float startTime; // �ִϸ��̼� ���� �ð�
    private bool animationStarted = false; // �ִϸ��̼��� ���۵Ǿ����� ����

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // UI �̹����� RectTransform ������Ʈ ��������
        rectTransform.sizeDelta = new Vector2(0,rectTransform.sizeDelta.y);
    }

    private void Update()
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
                rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y); // ��ǥ �ʺ�� ũ�� ����
                animationStarted = false; // �ִϸ��̼� ���� �÷��� ����
            }
        }
    }
}
