using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Component_Animator ����
    Animator animator;
    // Component_Rigidbody2D ���� (rigidbody �� ������ �̸��� �־� _ ����)
    Rigidbody2D _rigidbody;

    // �ݵ��ϴ� ��(= ����) �� ����
    public float flapForce = 6f;
    // �����ϴ� �ӷ� �� ����
    public float forwardSpeed = 3f;
    // ��� ���¸� �Ǻ��ϱ� ���� �� ����
    public bool isDead = false;
    // ��� �� ���ӿ��������� ������ �� ����
    public float gameOverDelay = 1f;

    // ���� ���¸� �Ǻ��ϱ� ���� �� ����
    bool isFlap = false;

    // ���� ���¸� �Ǻ��ϱ� ���� �� ���� (���� Test ����)
    public bool godMode = false;

    // GameManager ������ ������ ���� �� ����
    GameManager gameManager;

    void Start()
    {
        // GameManager ������ �̱���_GameManager���� �ʱ�ȭ
        gameManager = GameManager.Instance;

        /*
        �ش� Script�� Component�� ������ ��ü�� �� �ڽ� ��ü�� Component_Animator�� �����ߴٸ� �̸� ��ȯ���ִ� ���
        �θ� ��ü�� �ڽ� ��ü ��� �� ������ ������ ��� �θ� ��ü�� �켱 �ݿ���
         */
        animator = GetComponentInChildren<Animator>();
        // �ش� Script�� Component�� ������ ��ü�� Component_Rigidbody2D�� �����ߴٸ� �̸� ��ȯ���ִ� ���
        _rigidbody = GetComponent<Rigidbody2D>();

        // Component_Animator�� ã�� ������ ���
        if (animator == null)
            // �ܼ�â �� ���� ���_Animator Not Found
            Debug.LogError("Not Found Console_Animator.");

        // Component_Rigidbody2D�� ã�� ������ ���
        if (_rigidbody == null)
            // �ܼ�â �� ���� ���_Rigidbody2D Not Found
            Debug.LogError("Not Found Console_Rigidbody2D.");
    }

    void Update()
    {
        // ��� ������ ���
        if (isDead)
        {
            // ���� ���� ������ �ð� �� �Ǻ�_0���� �۰ų� ���� ���
            if (gameOverDelay <= 0)
            {
                // Player�� �Է� ��ȣ_R�� �۽����� ���
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // �̱���_GameManager �� ���� ����� �޼��� ȣ��
                    gameManager.RestartGame();
                }

                // Player�� �Է� ��ȣ_Q�� �۽����� ���
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    // �̱���_GameManager �� �� ���� �޼��� ȣ��
                    gameManager.ChangeScene();
                }
            }
            // ���� ���� ������ �ð� �� �Ǻ�_0���� Ŭ ��� (= ������ ���)
            else
            {
                // �ð� �� ����_deltaTime �� ��ŭ
                gameOverDelay -= Time.deltaTime;
            }
        }
        // ��� ���°� �ƴ� ���
        else
        {
            // Player�� �Է� ��ȣ_Space Bar�� �۽����� ���
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ���� ���� On
                isFlap = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // ��� ������ ���
        if (isDead)
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;

        // velocity�� Component_Rigidbody2D�� ������ ���ӵ� �� �ܼ� ����(Struct)
        Vector3 velocity = _rigidbody.velocity;
        // ���ӵ� ��_x ���� �����ϴ� �ӷ� ������ �ʱ�ȭ
        velocity.x = forwardSpeed;

        // ���� ������ ���
        if (isFlap)
        {
            // ���ӵ� ��_y ���� ���� ������ �缳��
            velocity.y += flapForce;
            // ���� ���� Off
            isFlap = false;
        }

        // Rigidbody2D�� ���ӵ� ���� �տ��� �ܼ� �����ص� ���ӵ� ������ �ʱ�ȭ
        _rigidbody.velocity = velocity;

        // ȸ�� �� ���� (Component_Velocity�� y �� ����, �ִ� ����_-90, 90)
        float angle = Mathf.Clamp( (_rigidbody.velocity.y * 10f), -90, 90 );
        // ȸ�� �� ���� (Euler ��� ����_360')
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // �浹 ���� �޼��� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ������ ���
        if (godMode)
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;

        // ��� ������ ���
        if (isDead)
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;

        // ��� ���� On
        isDead = true;
        // ���� ���� ������ �ð� �� �ʱ�ȭ
        gameOverDelay = 1f;

        // �ִϸ��̼� ���� ����_Condition ���� �ǰ�
        animator.SetInteger("IsDead", 1);

        // �̱���_GameManager �� ���� ���� �޼��� ȣ��
        gameManager.GameOver();
    }
}
