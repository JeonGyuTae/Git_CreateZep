using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // TextMeshProGUGI_ScoreText 정의
    public TextMeshProUGUI scoreText;
    // TextMeshProGUGI_RestartText 정의
    public TextMeshProUGUI restartText;

    void Start()
    {
        // ScoreText를 찾지 못했을 경우
        if (scoreText == null)
        {
            // 에러 메세지 출력_ScoreText Not Found
            Debug.LogError("Score Text is Null");
        }

        // RestartText를 찾지 못했을 경우
        if (restartText == null)
        {
            // 에러 메세지 출력_RestartText Not Found
            Debug.LogError("Restart Text is Null");
        }

        // RestartText 비활성화 (숨기기)
        restartText.gameObject.SetActive(false);
    }

    // 게임 오버 시 호출할 메서드
    public void SetRestart()
    {
        // RestartText 활성화
        restartText.gameObject.SetActive(true);
    }

    // 점수를 추가할 경우 호출할 메서드
    public void UpdateScore(int score)
    {
        // ScoreText 업데이트용 정의
        scoreText.text = score.ToString();
    }
}
