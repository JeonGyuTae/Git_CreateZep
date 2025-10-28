using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTone : MonoBehaviour
{
    // SingleTone���� ����� ������ �̱���_singleTone���� ���� (�ܺο��� ���� �� ���)
    static SingleTone singleTone;
    // �̱���_singleTone�� �ܺη� ���� �����ϰ� �ϴ� ���� ���� (�ܼ� ���� O, ���� X)
    public static SingleTone Instance { get { return singleTone; } }

    private void Awake()
    {
        // singleTone ��ü ���� (�� ������ SingleTone �� ����!)
        singleTone = this;
    }
}

/*
 => �̱��� ������ �⺻��
    ���� Manager �ܿ��� ������ ��ü�� ����ϰ��� �� ��� ���� ����� �Ǿ���
 */
