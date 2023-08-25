using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject deActiveObj;

    // 못찾아와서 Update문에서 강제로 찾아온다.
    public GameObject activeObj;
    public GameObject middleObj; // 검은색 펼쳐지는 오브젝트 입니다.
    
    private float animationDuration = 3f; // 애니메이션 진행 시간 (초)
    private float targetWidth = 500f; // 목표 너비

    private RectTransform rectTransform; // UI 이미지의 RectTransform
    private float startTime; // 애니메이션 시작 시간
    public bool animationStarted = false; // 애니메이션이 시작되었는지 여부

    //public GameObject unlockSound;
    

    public Image image1;
    public Image image2;
    public GameObject objectToDeactivate; // 비활성화할 게임 오브젝트 추가
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
        if(middleObj.IsValid() == false && activeObj.IsValid() == false && SceneManager.GetActiveScene().buildIndex == 0)
        {
            GameObject MiddleUi = GFunc.GetRootObj("MiddleUI");
            middleObj = MiddleUi.FindChildObj("middleImg");
            activeObj = MiddleUi.FindChildObj("AllText");
        }


        if (SceneManager.GetActiveScene().buildIndex == 0 )
        {
            if (Input.anyKeyDown && !animationStarted)
            {
                deActiveObj.SetActive(false);
                activeObj.SetActive(true);  //TODO public 으로 받아오는거 스크립트로 받아오게 바꿔야함
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


    void MiddleMove()
    {
        if (Input.anyKeyDown && !animationStarted) // 아무 키나 누르고 애니메이션이 시작되지 않았다면
        {
            animationStarted = true; // 애니메이션 시작 플래그 설정
            startTime = Time.time; // 애니메이션 시작 시간 기록
        }

        if (animationStarted) // 애니메이션 시작되었을 때만 작동
        {
            float elapsedTime = Time.time - startTime; // 경과 시간 계산

            if (elapsedTime <= animationDuration) // 애니메이션 진행 중인 경우
            {
                float normalizedTime = elapsedTime / animationDuration; // 정규화된 시간 계산
                float newWidth = Mathf.Lerp(rectTransform.sizeDelta.x, targetWidth, normalizedTime); // 너비 보간 계산
                rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y); // 이미지 크기 업데이트
            }
            else // 애니메이션이 완료된 경우
            {
                ////TODO public 으로 받아오는거 스크립트로 받아오게 바꿔야함
                //rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y); // 목표 너비로 크기 조정
                //animationStarted = false; // 애니메이션 종료 플래그 설정
                /* Do nothing */
            }
        }
    }

}
