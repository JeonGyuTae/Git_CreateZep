using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // �ش� Script�� ���� ��ü�� ������ ��� ����
    public Transform target;
    // �ش� Script�� ���� ��ü�� ��ġ ��_X �� ����
    float offsetX;

    void Start()
    {
        // ������ ����� ã�� ������ ���
        if (target == null)
        {
            // ���� �޼��� ���_Target Not Found
            Debug.LogError("Target is Null");
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }

        // ��ü�� ��ġ ��_X �� �ʱ�ȭ (��ü�� ��ġ ��_X �� - ������ ����� ��ġ ��_X ��) -> �� ���� ������� X ���� ������
        offsetX = transform.position.x - target.position.x;
    }

    void Update()
    {
        // ������ ����� ã�� ������ ���
        if (target == null)
        {
            // ���� �޼��� ���_Target Not Found
            Debug.LogError("Target is Null");
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }

        // ��ü�� ��ġ �� ���� (Transform�� ���� ���� �Ұ���, 1ȸ ���� �ʿ�)
        Vector3 pos = transform.position;
        // ��ü�� ��ġ �� �ʱ�ȭ (=> 1ȸ ���� �۾�)
        pos.x = target.position.x + offsetX;
        // ��ü�� ��ġ �� ����
        transform.position = pos;

    }
}
