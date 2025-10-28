using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Component_Animator 정의
    Animator animator;
    // Component_Rigidbody2D 정의 (rigidbody 는 동일한 이름이 있어 _ 붙임)
    Rigidbody2D _rigidbody;

    // 반등하는 힘(= 점프) 값 정의
    public float flapForce = 6f;
    // 전진하는 속력 값 정의
    public float forwardSpeed = 3f;
    // 사망 상태를 판별하기 위한 값 정의
    public bool isDead = false;
    // 사망 후 게임오버까지의 딜레이 값 정의
    public float gameOverDelay = 1f;

    // 점프 상태를 판별하기 위한 값 정의
    bool isFlap = false;

    // 무적 상태를 판별하기 위한 값 정의 (게임 Test 목적)
    public bool godMode = false;

    // GameManager 역할을 수행할 변수 명 정의
    GameManager gameManager;

    void Start()
    {
        // GameManager 역할을 싱글톤_GameManager으로 초기화
        gameManager = GameManager.Instance;

        /*
        해당 Script를 Component로 소유한 객체와 그 자식 객체가 Component_Animator를 소유했다면 이를 반환해주는 기능
        부모 객체와 자식 객체 모두 위 조건을 충족할 경우 부모 객체를 우선 반영함
         */
        animator = GetComponentInChildren<Animator>();
        // 해당 Script를 Component로 소유한 객체가 Component_Rigidbody2D를 소유했다면 이를 반환해주는 기능
        _rigidbody = GetComponent<Rigidbody2D>();

        // Component_Animator를 찾지 못했을 경우
        if (animator == null)
            // 콘솔창 내 오류 출력_Animator Not Found
            Debug.LogError("Not Found Console_Animator.");

        // Component_Rigidbody2D를 찾지 못했을 경우
        if (_rigidbody == null)
            // 콘솔창 내 오류 출력_Rigidbody2D Not Found
            Debug.LogError("Not Found Console_Rigidbody2D.");
    }

    void Update()
    {
        // 사망 상태일 경우
        if (isDead)
        {
            // 게임 오버 딜레이 시간 값 판별_0보다 작거나 같을 경우
            if (gameOverDelay <= 0)
            {
                // Player가 입력 신호_R을 송신했을 경우
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // 싱글톤_GameManager 내 게임 재시작 메서드 호출
                    gameManager.RestartGame();
                }

                // Player가 입력 신호_Q를 송신했을 경우
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    // 싱글톤_GameManager 내 씬 변경 메서드 호출
                    gameManager.ChangeScene();
                }
            }
            // 게임 오버 딜레이 시간 값 판별_0보다 클 경우 (= 존재할 경우)
            else
            {
                // 시간 값 차감_deltaTime 값 만큼
                gameOverDelay -= Time.deltaTime;
            }
        }
        // 사망 상태가 아닐 경우
        else
        {
            // Player가 입력 신호_Space Bar를 송신했을 경우
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 점프 상태 On
                isFlap = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // 사망 상태일 경우
        if (isDead)
            // 반환 (아래 코드 무시)
            return;

        // velocity에 Component_Rigidbody2D가 소유한 가속도 값 단순 복사(Struct)
        Vector3 velocity = _rigidbody.velocity;
        // 가속도 값_x 값을 전진하는 속력 값으로 초기화
        velocity.x = forwardSpeed;

        // 점프 상태일 경우
        if (isFlap)
        {
            // 가속도 값_y 값을 점프 값으로 재설정
            velocity.y += flapForce;
            // 점프 상태 Off
            isFlap = false;
        }

        // Rigidbody2D의 가속도 값을 앞에서 단순 복사해둔 가속도 값으로 초기화
        _rigidbody.velocity = velocity;

        // 회전 값 정의 (Component_Velocity의 y 값 기준, 최댓값 제한_-90, 90)
        float angle = Mathf.Clamp( (_rigidbody.velocity.y * 10f), -90, 90 );
        // 회전 값 적용 (Euler 방식 적용_360')
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 충돌 판정 메서드 정의
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 무적 상태일 경우
        if (godMode)
            // 반환 (아래 코드 무시)
            return;

        // 사망 상태일 경우
        if (isDead)
            // 반환 (아래 코드 무시)
            return;

        // 사망 상태 On
        isDead = true;
        // 게임 오버 딜레이 시간 값 초기화
        gameOverDelay = 1f;

        // 애니메이션 상태 변경_Condition 값에 의거
        animator.SetInteger("IsDead", 1);

        // 싱글톤_GameManager 내 게임 오버 메서드 호출
        gameManager.GameOver();
    }
}
