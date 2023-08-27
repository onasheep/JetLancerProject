using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEnemyVerTwo : MonoBehaviour
{
    // TODO : 오브젝트풀 보낸다 임시 ) 
    public GameObject indicator;
    public Sprite[] sprites = default; //스프라이트 변경
    public GameObject indicatorChild = default;

    public GameObject indicatorCanvas;
    public GameObject player; // target

    [SerializeField]
    private GameObject instance;
    private float defaultAngle;

    // TODO : 
    // Indicator가 적에 직접 붙어 있는 것 보다 
    // 어떤 상위 오브젝트가 Player 와 WaveManger에 있는 enemyList를 체크해서 
    // 직접 playCanvas에 찍어 주는게 좋을 것 같다. 
    private void Awake()
    {
        player = GFunc.GetRootObj("Player");
        indicatorCanvas = GFunc.GetRootObj("playCanvas");
        instance = Instantiate(indicator);
        instance.transform.SetParent(indicatorCanvas.transform);
        instance.transform.localScale = new Vector3(180, 180, 180);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Vector2 dir = new Vector2(Screen.width, Screen.height);
        defaultAngle = Vector2.Angle(new Vector2(0, 1), dir);
        indicatorChild = instance.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        SetIndicator();
        if(!gameObject.activeSelf)
        {
            indicator.SetActive(false);
        }
        if(GameManager.Instance.isGameOver)
        {
            instance.SetActive(false);
        }
    }

    public void SetIndicator()
    {
        if (!isOffScreen()) return;

        float angle = Vector2.Angle(new Vector2(0, 1), transform.position - player.transform.position);
        int sign = player.transform.position.x > transform.position.x ? -1 : 1;
        angle *= sign;

        Vector3 target = Camera.main.WorldToViewportPoint(transform.position);

        float x = target.x - 0.5f;
        float y = target.y - 0.5f;

        RectTransform indicatorRect = instance.GetComponent<RectTransform>();

        if (-defaultAngle <= angle && angle <= defaultAngle)
        {
            //Debug.Log("up");
            //anchor minY, maxY 0.96
            instance.GetComponent<SpriteRenderer>().sprite = sprites[0];
            indicatorChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -0.08f);//이건 안에 마크 위치 고정시켜줄려고 했습니다. //indicatorChild.GetComponent<RectTransform>().anchoredPosition.y);

            float anchorMinMaxY = 0.96f;

            float anchorMinMaxX = x * (anchorMinMaxY-0.5f) / y + 0.5f;

            if (anchorMinMaxX >= 0.94f) anchorMinMaxX = 0.94f;
            else if (anchorMinMaxX <= 0.06f) anchorMinMaxX = 0.06f;

            indicatorRect.anchorMin = new Vector2(anchorMinMaxX, anchorMinMaxY);
            indicatorRect.anchorMax = new Vector2(anchorMinMaxX, anchorMinMaxY);
        }
        else if (defaultAngle <= angle && angle <= 180 - defaultAngle)
        {
            //Debug.Log("right");
            //anchor minX, maxX 0.94  > 0.96
            instance.GetComponent<SpriteRenderer>().sprite = sprites[1];
            indicatorChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(-0.08f,0f);

            float anchorMinMaxX = 0.96f;

            float anchorMinMaxY = y * (anchorMinMaxX - 0.5f) / x + 0.5f;

            if (anchorMinMaxY >= 0.94f) anchorMinMaxY = 0.94f;
            else if (anchorMinMaxY <= 0.06f) anchorMinMaxY = 0.06f;

            indicatorRect.anchorMin = new Vector2(anchorMinMaxX, anchorMinMaxY);
            indicatorRect.anchorMax = new Vector2(anchorMinMaxX, anchorMinMaxY);
        }
        else if (-180 + defaultAngle <= angle && angle <= -defaultAngle)
        {
            //Debug.Log("left");
            //anchor minX, maxX 0.06 > 0.04
            instance.GetComponent<SpriteRenderer>().sprite = sprites[2];
            indicatorChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.08f, 0f); 
            float anchorMinMaxX = 0.04f;

            float anchorMinMaxY = (y * (anchorMinMaxX - 0.5f) / x) + 0.5f;

            if (anchorMinMaxY >= 0.94f) anchorMinMaxY = 0.94f;
            else if (anchorMinMaxY <= 0.06f) anchorMinMaxY = 0.06f;

            indicatorRect.anchorMin = new Vector2(anchorMinMaxX, anchorMinMaxY);
            indicatorRect.anchorMax = new Vector2(anchorMinMaxX, anchorMinMaxY);
        }
        else if (-180 <= angle && angle <= -180 + defaultAngle || 180 - defaultAngle <=angle && angle <= 180)
        {
            //Debug.Log("down");
            //anchor minY, maxY 0.04
            instance.GetComponent<SpriteRenderer>().sprite = sprites[3];
            indicatorChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0.08f);
            float anchorMinMaxY = 0.04f;

            float anchorMinMaxX = x * (anchorMinMaxY - 0.5f) / y + 0.5f;

            if (anchorMinMaxX >= 0.94f) anchorMinMaxX = 0.94f;
            else if (anchorMinMaxX <= 0.06f) anchorMinMaxX = 0.06f;

            indicatorRect.anchorMin = new Vector2(anchorMinMaxX, anchorMinMaxY);
            indicatorRect.anchorMax = new Vector2(anchorMinMaxX, anchorMinMaxY);
        }

        indicatorRect.anchoredPosition = new Vector3(0, 0, 0);
        //indicatorChild.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
    private bool isOffScreen()
    {
        Vector2 vec = Camera.main.WorldToViewportPoint(transform.position);
        if (vec.x >= 0 && vec.x <= 1 && vec.y >= 0 && vec.y <= 1)
        {
            instance.SetActive(false);
            return false;
        }
        else
        {
            instance.SetActive(true);
            return true;
        }
    }
    private void OnDisable()
    {
        // TODO 점수 처리 해줘야합니다.
        //GameManager.Instance.AddScore(10); //10 매직넘버는 score에 추가되는 숫자입니다.

        if (!gameObject.activeSelf)
        {
            instance.SetActive(false);
        }
    }
}
