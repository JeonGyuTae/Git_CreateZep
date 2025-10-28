using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // gameManager ������ �̱���_GameManager���� ���� (�ܺο��� ���� �� ���)
    static GameManager gameManager;
    // �̱���_GameManager�� �ܺη� ���� �����ϰ� �ϴ� ���� ���� (�ܼ� ���� O, ���� X)
    public static GameManager Instance { get { return gameManager; } }

    // ���� ���� ����
    private int currentScore = 0;

    // UIManager ����
    UIManager uiManager;
    // �ܺο��� ��� �� ������ ���� ����
    public UIManager UIManager { get { return uiManager; } }

    private void Awake()
    {
        // gameManager ��ü ���� (�� ������ GameManager �� ����! => �̱��� ������ �⺻��)
        gameManager = this;
        // Component_UIManager ����
        uiManager = FindObjectOfType<UIManager>();
    }

    public void Start()
    {
        // UI_���� �ʱ�ȭ (0��)
        uiManager.UpdateScore(0);
    }

    // ���� ���� �� ȣ���� �޼��� ����
    public void GameOver()
    {
        // UIManager �� �޼��� ���_"Restart �Ұž�?"
        uiManager.SetRestart();
    }

    // ������ ������� ��� ȣ���� �޼��� ����
    public void RestartGame()
    {
        // ���� �������� Scene�� name�� ����Ͽ� �� �ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ������ �߰��� ��� ȣ���� �޼��� ����
    public void AddScore(int score)
    {
        // ���� �������� �߰��� ���� �ջ�
        currentScore += score;
        // UIManager �� �޼��� ���_"����(�ջ���) ����"
        uiManager.UpdateScore(currentScore);
    }

    public void ChangeScene()
    {
        // ���� �������� Scene�� name�� ����Ͽ� �� �ε�
        SceneManager.LoadScene("000Main");
    }
}
