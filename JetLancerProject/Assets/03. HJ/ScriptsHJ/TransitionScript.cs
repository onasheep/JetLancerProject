using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionScript : MonoBehaviour
{
    
    public GameObject ani1;
    public GameObject ani2;

    private GameObject player;
    private GameObject playCanvas;
    public GameObject burnStart;
    public GameObject countdown;

    //=========================================================0824 승리 결과 작업을 여기서 처리해줍니다.
    private Rigidbody2D playerRigid;
    void Start()
    {   
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            playCanvas = GFunc.GetRootObj(RDefine.PLAYER_CANVAS);
            player = GFunc.GetRootObj(RDefine.PLAYER);
            playerRigid = player.GetComponent<Rigidbody2D>();
            StartCoroutine(AliveObj());
            StopCoroutine(AliveObj());
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && SceneManager.GetActiveScene().buildIndex == 0) // Title씬일때만 활성화
        {
            ani1.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1) //CharacterSelect 씬일때만 활성화
        {
            ani2.SetActive(true);
        }//Title 와 캐릭터 선택 씬에서 트랜지션 애니메이션 오브젝트를 출력해주빈디ㅏㄷ
    }

    IEnumerator AliveObj()
    {
        yield return new WaitForSeconds(1.3f);
        burnStart.SetActive(true);
        countdown.SetActive(true);
        yield return new WaitForSeconds(3f);
        playCanvas.SetActive(true);
        burnStart.SetActive(false);
        GameManager.Instance.isEngague = false;
        playerRigid.constraints = RigidbodyConstraints2D.None;
    }

}
