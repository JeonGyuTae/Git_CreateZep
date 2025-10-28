using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    // Obstacle ������ ���� �� ���� (�⺻ ��)
    public int obstacleCount = 0;
    // Obstacle ������ ������ ��ġ �� ����
    public Vector3 obstacleLastPosition = Vector3.zero;

    // Obstacle ������ ���� �� ���� (���� ��)
    public int numBgCount = 5;

    void Start()
    {
        // ���� �� Component_Obstacle(Script)�� �޸� ��� ��ü �˻� �� ����
        Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
        // Obstacle ������ ������ ��ġ �� �ʱ�ȭ (���� �տ� ��ġ�� ��ü�� ��ġ ��)
        obstacleLastPosition = obstacles[0].transform.position;
        // Obstacle ������ ���� �� �ʱ�ȭ
        obstacleCount = obstacles.Length;

        // �ݺ���_Obstacle ������ ���� ����ŭ
        for (int i = 0; i < obstacleCount; i++)
        {
            // Obstacle ������ ������ ��ġ �� �ʱ�ȭ (���� �ڿ� ��ġ�� ��ü�� ��ġ ��) -> ��ü�� �ٽ� ��ġ�� �� ���� ��ġ ���� Ȱ��
            obstacleLastPosition = obstacles[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }

    // Trigger�� �浹���� ��� ȣ���� �޼��� (Trigger�̱⿡ ������ �浹�� �ƴ� �浹 ������ ��ȯ)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ��ü�� Unity Tag�� 'Background' �� ���
        if (collision.CompareTag("Background"))
        {
            // ��ü�� ���� ���� �� ���� �� ����
            float widthOfBgObject = ((BoxCollider2D)collision).size.x;

            // �浹�� ��ü�� ��ġ �� ���� �� ���� �� ����
            Vector3 pos = collision.transform.position;

            // �浹�� ��ü�� �̵���ų ���� ���� ���� (�⺻ �� * ���� ����ŭ)
            pos.x += widthOfBgObject * numBgCount;
            // �浹�� ��ü �̵� (������ ������ �� ��ŭ)
            collision.transform.position = pos;

            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }

        // Obstacle ������ Component_Collider ����
        Obstacle obstacle = collision.GetComponent<Obstacle>();
        // Obstacle ���� �� ���
        if (obstacle)
        {
            // Obstacle ������ ��ġ �� �ʱ�ȭ (���� �ڿ� ��ġ�� ��ü�� ��ġ ��) -> �̵��ϴ� ��ü�̰� ���� ���̰� ��� �����ϱ⿡ ������ �۾�
            obstacleLastPosition = obstacle.SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }
}
