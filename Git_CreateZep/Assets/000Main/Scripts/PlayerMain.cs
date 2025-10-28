using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    // Rigidbody2D ����
    Rigidbody2D _rigidbody;
    // SpriteRenderer ����
    SpriteRenderer _renderer;

    // Player �̵� �ӷ� ����
    float moveSpeed = 20f;
    // Player �̵� ���� ����_X ��
    bool isLeft = false;

    /* ī�޶� ���� �ڵ�
    public Transform cameraTransform;
    public Vector2 cameraMinBounds;
    public Vector2 cameraMaxBounds;
    public float cameraFollowSpeed = 5f;
     */

    void Start()
    {
        // Rigidbody2D ����
        _rigidbody = GetComponent<Rigidbody2D>();
        // SpriteRenderer ����_�ڽ� ��ü
        _renderer = GetComponentInChildren<SpriteRenderer>();

        // Rigidbody2D�� ã�� ������ ���
        if (_rigidbody == null)
        {
            // �ܼ�â �� ���� ���_Rigidbody Not Found
            Debug.LogError("Rigidbody2D Not Found");
        }

        // SpriteRenderer�� ã�� ������ ���
        if (_renderer == null)
        {
            // �ܼ�â �� ���� ���_SpriteRenderer Not Found
            Debug.LogError("Renderer Not Found");
        }
    }

    void Update()
    {
        // Rigidbody2D or SpriteRenderer�� ã�� ������ ���
        if (_rigidbody == null || _renderer == null)
        {
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }

        // ĳ���� ���� ��ȯ �޼��� ȣ��
        Rotate();
        // ĳ���� �̵� �޼��� ȣ��
        Move();

        /* ī�޶� ���� �ڵ�
        FollowCamera();
         */

        /*
        if (targetObject != null)
        {
            targetObject.gameObject.SetActive(true);
            Debug.Log($"{objectNameInCanvas} ������Ʈ�� Ȱ��ȭ�߽��ϴ�.");
        }
        else
        {
            Debug.LogWarning($"{objectNameInCanvas} ������Ʈ�� Canvas ������ ã�� �� �����ϴ�.");
        }
         */
    }

    // ĳ���� ���� ��ȯ �޼���
    void Rotate()
    {
        // Player �̵� ���� ����_����
        float dirX = Input.GetAxisRaw("Horizontal");

        // �̵� ������ ����(����)�� ���
        if (dirX < 0)
            // bool �� ����
            isLeft = true;
        // �̵� ������ ���(����)�� ���
        else if (dirX > 0)
            // bool �� ����
            isLeft = false;

        // Player �̵� ���� ����_X ��
        _renderer.flipX = isLeft;

        /*
        // Player�� �Է� ��ȣ_A�� �۽����� ���_���� 1ȸ
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Player�� ������ �ٶ󺸰� ���� ���
            if (!isLeft)
            {
                // ���� ��ȯ
                isLeft = !isLeft;
            }
        }

        // Player�� �Է� ��ȣ_D�� �۽����� ���_���� 1ȸ
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Player�� ������ �ٶ󺸰� ���� ���
            if (isLeft)
            {
                // ���� ��ȯ
                isLeft = !isLeft;
            }
        }

        // SpriteRenderer ���� ����
        _renderer.flipX = isLeft;
         */
    }

    // ĳ���� �̵� �޼���
    void Move()
    {
        // Player �̵� ���� ����
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),
                                    Input.GetAxisRaw("Vertical"));
        // Player �̵� ������ �ӵ� ����_��ü ���� 1
        input.Normalize();

        // Player �̵� �� ����_Player �̵� ������ �ӵ� * Player �̵� �ӷ� * 60 fps
        Vector2 targetPos = _rigidbody.position + input * moveSpeed * Time.deltaTime;
        // Player �̵�
        _rigidbody.MovePosition(targetPos);

        /*
        // Play�� ��ġ �� ����
        Vector3 position = transform.position;

        // Player�� �Է� ��ȣ_W�� �۽����� ���
        if (Input.GetKey(KeyCode.W))
        {
            // Player �̵�_Y �� + �ӷ�
            position.y += moveSpeed * Time.deltaTime;
        }

        // Player�� �Է� ��ȣ_S�� �۽����� ���
        if (Input.GetKey(KeyCode.S))
        {
            // Player �̵�_Y �� - �ӷ�
            position.y -= moveSpeed * Time.deltaTime;
        }

        // Player�� �Է� ��ȣ_A�� �۽����� ���
        if (Input.GetKey(KeyCode.A))
        {
            // Player �̵�_X �� - �ӷ�
            position.x -= moveSpeed * Time.deltaTime;
        }

        // Player�� �Է� ��ȣ_D�� �۽����� ���
        if (Input.GetKey(KeyCode.D))
        {
            // Player �̵�_X �� + �ӷ�
            position.x += moveSpeed * Time.deltaTime;
        }

        // ���� Player�� ��ġ �� ����
        transform.position = position;
         */
    }

    // �浹 ���� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ����� Tag�� Area A�� ���
        if (collision.gameObject.CompareTag("Area A"))
        {
            // ���_A���� ����
            Debug.Log("A ������ �����߽��ϴ�.");
        }

        // �浹�� ����� Tag�� Area B�� ���
        if (collision.gameObject.CompareTag("Area B"))
        {
            // ���_B���� ����
            Debug.Log("B ������ �����߽��ϴ�.");
        }

        // �浹�� ����� Tag�� TriggerFlappyPlane�� ���
        if (collision.gameObject.CompareTag("TriggerFlappyPlane"))
        {
            // ���_��Ŭ�� �� ���� ����_Flappy Plane
            Debug.Log("Flappy Plane ������ �����߽��ϴ�.\nSpaceBar �Է� �� 'Flappy Plane'�������� �����մϴ�.");
        }
    }

    // �浹 ��
    private void OnTriggerStay2D(Collider2D collision)
    {
        // �浹�� ����� Tag�� TriggerFlappyPlane �̰� SpaceBar �� �Է����� ���
        if (collision.gameObject.CompareTag("TriggerFlappyPlane") && Input.GetKeyDown(KeyCode.Space))
        {
            // ���_���� ����_Flappy Plane
            Debug.Log("'FlappyPlane' �������� �����մϴ�.");

            // �� ��ȯ_FlappyPlane
            SceneManager.LoadScene("001FlappyPlane");
        }
    }

    // �浹 Ż�� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �浹�� ����� Tag�� Area A�� ���
        if (collision.gameObject.CompareTag("Area A"))
        {
            // ���_A���� Ż��
            Debug.Log("A �������� Ż���߽��ϴ�.");
        }

        // �浹�� ����� Tag�� Area B�� ���
        if (collision.gameObject.CompareTag("Area B"))
        {
            // ���_B���� Ż��
            Debug.Log("B �������� Ż���߽��ϴ�.");
        }

        // �浹�� ����� Tag�� TriggerFlappyPlane�� ���
        if (collision.gameObject.CompareTag("TriggerFlappyPlane"))
        {
            // ���_TriggerFlappyPlane Ż��
            Debug.Log("FlappyPlane �������� Ż���߽��ϴ�.");
        }
    }

    /*
    // ī�޶� ����
    void FollowCamera()
    {
        // ī�޶� ã�� ������ ���
        if (cameraTransform == null)
        {
            // ��ȯ (�Ʒ� �ڵ� ����)
            return;
        }
        
        // ��
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);
        // ��
        targetPos.x = Mathf.Clamp(targetPos.x, cameraMinBounds.x, cameraMaxBounds.x);
        // ��
        targetPos.y = Mathf.Clamp(targetPos.y, cameraMinBounds.y, cameraMaxBounds.y);

        // ��
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
        
    
        // ��� ����
    }
     */
}
