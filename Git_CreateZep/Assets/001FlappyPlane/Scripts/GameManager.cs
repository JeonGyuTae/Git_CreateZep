using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // gameManager 변수를 싱글톤_GameManager으로 선언 (외부에서 참조 시 사용)
    static GameManager gameManager;
    // 싱글톤_GameManager을 외부로 참조 가능하게 하는 변수 선언 (단순 복사 O, 복제 X)
    public static GameManager Instance { get { return gameManager; } }

    // 현재 점수 정의
    private int currentScore = 0;

    // UIManager 정의
    UIManager uiManager;
    // 외부에서 사용 시 참조용 변수 정의
    public UIManager UIManager { get { return uiManager; } }

    private void Awake()
    {
        // gameManager 객체 선언 (이 구역의 GameManager 는 나다! => 싱글톤 패턴의 기본형)
        gameManager = this;
        // Component_UIManager 참조
        uiManager = FindObjectOfType<UIManager>();
    }

    public void Start()
    {
        // UI_점수 초기화 (0점)
        uiManager.UpdateScore(0);
    }

    // 게임 오버 시 호출할 메서드 정의
    public void GameOver()
    {
        // UIManager 내 메서드 출력_"Restart 할거야?"
        uiManager.SetRestart();
    }

    // 게임을 재시작할 경우 호출할 메서드 정의
    public void RestartGame()
    {
        // 현재 실행중인 Scene의 name을 사용하여 씬 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 점수를 추가할 경우 호출할 메서드 정의
    public void AddScore(int score)
    {
        // 현재 점수에서 추가할 점수 합산
        currentScore += score;
        // UIManager 내 메서드 출력_"현재(합산한) 점수"
        uiManager.UpdateScore(currentScore);
    }

    public void ChangeScene()
    {
        // 현재 실행중인 Scene의 name을 사용하여 씬 로드
        SceneManager.LoadScene("000Main");
    }
}
