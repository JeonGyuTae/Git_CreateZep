using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    // Rigidbody2D 정의
    Rigidbody2D _rigidbody;
    // SpriteRenderer 정의
    SpriteRenderer _renderer;

    // Player 이동 속력 정의
    float moveSpeed = 20f;
    // Player 이동 방향 정의_X 값
    bool isLeft = false;

    /* 카메라 관련 코드
    public Transform cameraTransform;
    public Vector2 cameraMinBounds;
    public Vector2 cameraMaxBounds;
    public float cameraFollowSpeed = 5f;
     */

    void Start()
    {
        // Rigidbody2D 참조
        _rigidbody = GetComponent<Rigidbody2D>();
        // SpriteRenderer 참조_자식 객체
        _renderer = GetComponentInChildren<SpriteRenderer>();

        // Rigidbody2D를 찾지 못했을 경우
        if (_rigidbody == null)
        {
            // 콘솔창 내 오류 출력_Rigidbody Not Found
            Debug.LogError("Rigidbody2D Not Found");
        }

        // SpriteRenderer를 찾지 못했을 경우
        if (_renderer == null)
        {
            // 콘솔창 내 오류 출력_SpriteRenderer Not Found
            Debug.LogError("Renderer Not Found");
        }
    }

    void Update()
    {
        // Rigidbody2D or SpriteRenderer를 찾지 못했을 경우
        if (_rigidbody == null || _renderer == null)
        {
            // 반환 (아래 코드 무시)
            return;
        }

        // 캐릭터 방향 전환 메서드 호출
        Rotate();
        // 캐릭터 이동 메서드 호출
        Move();

        /* 카메라 관련 코드
        FollowCamera();
         */

        /*
        if (targetObject != null)
        {
            targetObject.gameObject.SetActive(true);
            Debug.Log($"{objectNameInCanvas} 오브젝트를 활성화했습니다.");
        }
        else
        {
            Debug.LogWarning($"{objectNameInCanvas} 오브젝트를 Canvas 내에서 찾을 수 없습니다.");
        }
         */
    }

    // 캐릭터 방향 전환 메서드
    void Rotate()
    {
        // Player 이동 방향 정의_가로
        float dirX = Input.GetAxisRaw("Horizontal");

        // 이동 방향이 음수(좌측)일 경우
        if (dirX < 0)
            // bool 값 갱신
            isLeft = true;
        // 이동 방향이 양수(우측)일 경우
        else if (dirX > 0)
            // bool 값 갱신
            isLeft = false;

        // Player 이동 방향 갱신_X 값
        _renderer.flipX = isLeft;

        /*
        // Player가 입력 신호_A를 송신했을 경우_최초 1회
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Player가 우측을 바라보고 있을 경우
            if (!isLeft)
            {
                // 방향 전환
                isLeft = !isLeft;
            }
        }

        // Player가 입력 신호_D를 송신했을 경우_최초 1회
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Player가 좌측을 바라보고 있을 경우
            if (isLeft)
            {
                // 방향 전환
                isLeft = !isLeft;
            }
        }

        // SpriteRenderer 방향 갱신
        _renderer.flipX = isLeft;
         */
    }

    // 캐릭터 이동 메서드
    void Move()
    {
        // Player 이동 방향 정의
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),
                                    Input.GetAxisRaw("Vertical"));
        // Player 이동 방향의 속도 고정_전체 방향 1
        input.Normalize();

        // Player 이동 값 정의_Player 이동 방향의 속도 * Player 이동 속력 * 60 fps
        Vector2 targetPos = _rigidbody.position + input * moveSpeed * Time.deltaTime;
        // Player 이동
        _rigidbody.MovePosition(targetPos);

        /*
        // Play의 위치 값 참조
        Vector3 position = transform.position;

        // Player가 입력 신호_W를 송신했을 경우
        if (Input.GetKey(KeyCode.W))
        {
            // Player 이동_Y 값 + 속력
            position.y += moveSpeed * Time.deltaTime;
        }

        // Player가 입력 신호_S를 송신했을 경우
        if (Input.GetKey(KeyCode.S))
        {
            // Player 이동_Y 값 - 속력
            position.y -= moveSpeed * Time.deltaTime;
        }

        // Player가 입력 신호_A를 송신했을 경우
        if (Input.GetKey(KeyCode.A))
        {
            // Player 이동_X 값 - 속력
            position.x -= moveSpeed * Time.deltaTime;
        }

        // Player가 입력 신호_D를 송신했을 경우
        if (Input.GetKey(KeyCode.D))
        {
            // Player 이동_X 값 + 속력
            position.x += moveSpeed * Time.deltaTime;
        }

        // 현재 Player의 위치 값 갱신
        transform.position = position;
         */
    }

    // 충돌 시작 시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 대상의 Tag가 Area A일 경우
        if (collision.gameObject.CompareTag("Area A"))
        {
            // 출력_A구역 진입
            Debug.Log("A 구역에 진입했습니다.");
        }

        // 충돌한 대상의 Tag가 Area B일 경우
        if (collision.gameObject.CompareTag("Area B"))
        {
            // 출력_B구역 진입
            Debug.Log("B 구역에 진입했습니다.");
        }

        // 충돌한 대상의 Tag가 TriggerFlappyPlane일 경우
        if (collision.gameObject.CompareTag("TriggerFlappyPlane"))
        {
            // 출력_좌클릭 시 게임 진입_Flappy Plane
            Debug.Log("Flappy Plane 구역에 진입했습니다.\nSpaceBar 입력 시 'Flappy Plane'게임으로 진입합니다.");
        }
    }

    // 충돌 중
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 충돌한 대상의 Tag가 TriggerFlappyPlane 이고 SpaceBar 를 입력했을 경우
        if (collision.gameObject.CompareTag("TriggerFlappyPlane") && Input.GetKeyDown(KeyCode.Space))
        {
            // 출력_게임 진입_Flappy Plane
            Debug.Log("'FlappyPlane' 게임으로 진입합니다.");

            // 씬 전환_FlappyPlane
            SceneManager.LoadScene("001FlappyPlane");
        }
    }

    // 충돌 탈출 시
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 충돌한 대상의 Tag가 Area A일 경우
        if (collision.gameObject.CompareTag("Area A"))
        {
            // 출력_A구역 탈출
            Debug.Log("A 구역에서 탈출했습니다.");
        }

        // 충돌한 대상의 Tag가 Area B일 경우
        if (collision.gameObject.CompareTag("Area B"))
        {
            // 출력_B구역 탈출
            Debug.Log("B 구역에서 탈출했습니다.");
        }

        // 충돌한 대상의 Tag가 TriggerFlappyPlane일 경우
        if (collision.gameObject.CompareTag("TriggerFlappyPlane"))
        {
            // 출력_TriggerFlappyPlane 탈출
            Debug.Log("FlappyPlane 구역에서 탈출했습니다.");
        }
    }

    /*
    // 카메라 추적
    void FollowCamera()
    {
        // 카메라를 찾지 못했을 경우
        if (cameraTransform == null)
        {
            // 반환 (아래 코드 무시)
            return;
        }
        
        // ㅇ
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);
        // ㅇ
        targetPos.x = Mathf.Clamp(targetPos.x, cameraMinBounds.x, cameraMaxBounds.x);
        // ㅇ
        targetPos.y = Mathf.Clamp(targetPos.y, cameraMinBounds.y, cameraMaxBounds.y);

        // ㅇ
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, cameraFollowSpeed * Time.deltaTime);
        
    
        // 경계 설정
    }
     */
}
