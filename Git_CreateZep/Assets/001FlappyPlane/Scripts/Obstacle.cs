using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Obstacle ������ ������ �� ���� ��½�ų �ִ� ����
    public float highPosY = 1f;
    // Obstacle ������ ������ �� ���� �����ų �ִ� ����
    public float lowPosY = -1f;

    // Obstacles ������ ���� �ּڰ� ����
    public float holeSizeMin = 1f;
    // Obstacles ������ ���� �ִ� ����
    public float holeSizeMax = 3f;

    // Obstacles�� Component_Transform ������
    public Transform topObject;
    public Transform bottomObject;

    // Obstacle ������ ������ ���� ���� ����
    public float widthPadding = 4f;

    // GameManager ������ ������ ���� �� ����
    GameManager gameManager;

    private void Start()
    {
        // GameManager ������ �̱���_GameManager���� �ʱ�ȭ
        gameManager = GameManager.Instance;
    }

    // Obstacle ���� ���� ���� �޼���
    public Vector3 SetRandomPlace(Vector3 lastPosition, int obstacleCount)
    {
        // Obstacles ������ ���� ���� ����
        float holeSize = Random.Range(holeSizeMin, holeSizeMax);
        // Obstacles�� ��ġ�� �����ϱ� ���� ���� ����
        float halfHoleSize = holeSize / 2;

        // Obstacle_TopObject�� ������ �⺻ ���̰� ����
        topObject.localPosition = new Vector3(0, halfHoleSize);
        // Obstacle_BottomObject�� ������ �⺻ ���̰� ����
        bottomObject.localPosition = new Vector3(0, -halfHoleSize);

        // Obstacle ������ ������ ��ġ ���� (���� ���� �� �ʱ�ȭ)
        Vector3 placePosition = lastPosition + new Vector3(widthPadding, 0);
        // Obstacles�� ���� ��ġ�� ���� (�ּڰ� : LowPosY, �ִ� : HighPosY)
        placePosition.y = Random.Range(lowPosY, highPosY);

        // ���� ������ Obstacle ������ ��ġ �̵�
        transform.position = placePosition;

        // ��ȯ_Obstacle ������ ��ġ ��
        return placePosition;
    }

    // Trigger�� ������� ��� ȣ���� �޼���
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player�� Component_Collider ����
        Player player = collision.GetComponent<Player>();

        // Player�� Null ���°� �ƴ� ���
        if (player != null)
        {
            // ���� �߰� �޼��� ȣ�� (�Ű� ������ : 1��)
            gameManager.AddScore(1);
        }
    }
}
