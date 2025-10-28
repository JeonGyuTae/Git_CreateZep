using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // TextMeshProGUGI_ScoreText ����
    public TextMeshProUGUI scoreText;
    // TextMeshProGUGI_RestartText ����
    public TextMeshProUGUI restartText;

    void Start()
    {
        // ScoreText�� ã�� ������ ���
        if (scoreText == null)
        {
            // ���� �޼��� ���_ScoreText Not Found
            Debug.LogError("Score Text is Null");
        }

        // RestartText�� ã�� ������ ���
        if (restartText == null)
        {
            // ���� �޼��� ���_RestartText Not Found
            Debug.LogError("Restart Text is Null");
        }

        // RestartText ��Ȱ��ȭ (�����)
        restartText.gameObject.SetActive(false);
    }

    // ���� ���� �� ȣ���� �޼���
    public void SetRestart()
    {
        // RestartText Ȱ��ȭ
        restartText.gameObject.SetActive(true);
    }

    // ������ �߰��� ��� ȣ���� �޼���
    public void UpdateScore(int score)
    {
        // ScoreText ������Ʈ�� ����
        scoreText.text = score.ToString();
    }
}
