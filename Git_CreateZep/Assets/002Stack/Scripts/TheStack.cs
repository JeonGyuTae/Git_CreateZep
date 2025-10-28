using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheStack : MonoBehaviour
{
    // 객체의 크기 정의
    private const float BoundSize = 3.5f;
    // 객체가 이동하는 값 정의
    private const float MovingBoundsSize = 3f;
    // 속력 정의
    private const float StackMovingSpeed = 5.0f;
    // 객체 속력 정의
    private const float BlockMovingSpeed = 3.5f;
    // 벗어난 길이의 최소 값 정의 (이 값보다 낮으면 게임 오버)
    private const float ErrorMargin = 0.1f;

    // Prefab으로 복제할 객체 정의
    public GameObject originBlock = null;

    // 이전의 객체 위치 값 정의
    private Vector3 prevBlockPosition;
    // 객체의 최종 위치 값 정의 <- 카메라의 유연한 화면 전환을 위해 사용
    private Vector3 desiredPosition;
    // 생성할 객체의 크기 정의
    private Vector3 stackBounds = new Vector2(BoundSize, BoundSize);

    /*   이동 처리 후의 최종 값들을 정의한 부분   */
    // 최종 객체 값 정의
    Transform lastBlock = null;
    // 최종 위치 값 정의
    float blockTransition = 0f;
    // 최근 위치 값 정의
    float secondaryPosition = 0f;

    // 적층한 객체의 개수 정의 => 최초 생성 시 +1 하기에 -1 로 정의
    int stackCount = -1;
    public int Score { get { return stackCount; } }
    // 적층에 성공한 횟수 정의
    int comboCount = 0;
    public int Combo { get { return comboCount; } }

    private int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    // 이전 색상 정의
    public Color prevColor;
    // 다음 색상 정의
    public Color nextColor;

    // 이동 방향 체크_X축
    bool isMovingX = false;

    int bestScore = 0;
    public int BestScore { get => bestScore; }

    int bestCombo = 0;
    public int BestCombo { get => bestCombo; }

    private const string BestScoreKey = "BestScore";
    private const string BestComboKey = "BestCombo";

    void Start()
    {
        // Prefab으로 복제할 객체를 찾지 못했을 경우
        if (originBlock == null)
        {
            // 에러 메세지 출력_OriginObject Not Found
            Debug.LogError("OriginBlock is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        // 이전 색상 값 갱신_랜덤 색상 메서드 호출
        prevColor = GetRandomColor();
        // 다음 색상 값 갱신_랜덤 색상 메서드 호출
        nextColor = GetRandomColor();

        // 이전 객체의 위치 값 갱신_Vector3.Down(= Y 값 -1)
        prevBlockPosition = Vector3.down;

        // 객체 생성 메서드 호출
        Spawn_Block();
    }

    void Update()
    {
        // Player가 입력 신호_마우스 좌클릭(0)을 송신했을 경우
        if (Input.GetMouseButtonDown(0))
        {
            // 객체 적층 메서드가 호출되었을 경우
            if (PlaceBlock())
            {
                // 객체 생성 메서드 호출
                Spawn_Block();
            }
            // 객체 적층 메서드가 호출되지 않았을 경우
            else
            {
                // 게임 오버
                Debug.Log("Game Over");

                UpdateScore();
            }
        }

        // 객체 이동 메서드 호출
        MoveBlock();
        // 객체의 위치 값 갱신_러프하게(출발 지점, 도착 지점, 이동 FPS % 값 / 카메라의 부드러운 전환을 위함)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
    }

    // 객체 생성 메서드
    bool Spawn_Block()
    {
        // Last Block 이 존재할 경우
        if (lastBlock != null)
            // 이전 객체의 위치 값 갱신 (최근 객체의 Local 위치 값) <- 아래로 생성할 예정이므로 부모 객체 내부 좌표를 사용함
            prevBlockPosition = lastBlock.localPosition;

        // 새로 생성할 객체 정의
        GameObject newBlock = null;
        // 새로 생성할 객체의 Transform 정의
        Transform newTrans = null;

        // 새로 생성할 객체 초기화 <<- Instantiate (매개 변수 값을 복제하여 반환해주는 함수)
        newBlock = Instantiate(originBlock);

        // 새로 생성한 객체를 찾지 못했을 경우
        if (newBlock == null)
        {
            // 에러 메세지 출력_Object Instantiate Fail
            Debug.LogError("NewBlock Instantiate Failed");
            // 반환_false (아래 코드 무시)
            return false;
        }

        // 매개 변수의 색상 변경 메서드 호출
        ColorChange(newBlock);

        // 새로 생성한 객체의 Transform 참조
        newTrans = newBlock.transform;
        // 위 객체를 해당 Script를 지닌 객체의 자식 객체로 갱신
        newTrans.parent = this.transform;
        // 위 객체의 위치 값 갱신_이전 객체의 Vector3.Up(= Y 값 +1)
        newTrans.localPosition = prevBlockPosition + Vector3.up;
        // 위 객체의 회전 값 갱신_Quaternion 초기 값(= 회전이 없는 상태)
        newTrans.localRotation = Quaternion.identity;
        // 위 객체의 크기 값 갱신_이전 객체의 Vector3 값 <- 객체 크기 변동이 게임의 주 로직이기에 가장 중요한 코드
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        // 적층한 객체의 개수 갱신_+1
        stackCount++;

        // 최종 위치 값 갱신_적층한 횟수 * Y 값 -1
        desiredPosition = Vector3.down * stackCount;
        // 최종 위치 값 초기화
        blockTransition = 0f;

        // 최종 객체 값 초기화
        lastBlock = newTrans;

        // 이동 방향 반전_X축 X
        isMovingX = !isMovingX;

        // 반환_true
        return true;
    }

    // 랜덤 색상 생성 메서드
    Color GetRandomColor()
    {
        // RGB_R 값 정의_랜덤 범위_100 ~ 250 / 255
        float r = Random.Range(100f, 250f) / 255f;
        // RGB_G 값 정의_랜덤 범위_100 ~ 250 / 255
        float g = Random.Range(100f, 250f) / 255f;
        // RGB_B 값 정의_랜덤 범위_100 ~ 250 / 255
        float b = Random.Range(100f, 250f) / 255f;

        // 반환_RGB
        return new Color(r, g, b);
    }

    // 매개변수의 색상 변경 메서드
    void ColorChange(GameObject go)
    {
        // 색상 값 정의_러프하게(이전 색상 값, 다음 색상 값, 색상 전환 % 값)
        /* 
            ★ 코드 해석
            {(stackCount % 11)} : 0 ~ 9까지, 총 10개의 단계를 거쳐 색상이 완전히 변하게 됨 (이전 색상부터, 9단계를 거쳐 다음 색상으로 점차 변화)
            { / 10f} : 0 ~ 1 까지의 값으로 출력되어야 함
            ex) 11단계 => 0.0f, 0.1f, 0.2f, 0.3f, ... , 0.9f, 1.0f
            Lerp 코드의 3번째 값은 % 수치를 연산하는 것이지만
            현재 게임에서는 색상이 완전히 동일해야 변화하는 구조임으로
            0 ~ 1 까지 즉, 100%로 전환되어야 함
         */
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 10) / 9f);

        // 매개 변수의 Component_Renderer 참조
        Renderer rn = go.GetComponent<Renderer>();

        // 매개변수를 찾지 못했을 경우
        if (rn == null)
        {
            // 에러 메세지 출력_Renderer Not Found
            Debug.LogError("Renderer is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        // 색상 변경(Material)_러프하게 정의한 색상 값
        rn.material.color = applyColor;
        // 메인 카메라 배경 색상 변경_러프하게 정의한 색상 값 - 10%
        Camera.main.backgroundColor = applyColor - new Color(0.2f, 0.2f, 0.2f);

        // 러프하게 정의한 색상 값과 다음 색상 값이 동일할 경우
        if (applyColor.Equals(nextColor) == true)
        {
            // 이전 색상 값 갱신
            prevColor = nextColor;
            // 다음 색상 값 갱신_랜덤 색상 메서드 호출
            nextColor = GetRandomColor();
        }
    }

    // 객체 이동 메서드
    void MoveBlock()
    {
        // 최종 위치 값 갱신_60FPS * 객체 속력
        blockTransition += Time.deltaTime * BlockMovingSpeed;

        // 최종 위치 갱신 값 정의_삼각함수 양수 값(0 ~ 객체 길이 사이를 최종 위치 값만큼 갱신) - (객체 길이 / 2) <- 시작 위치를 맞추기 위해 {객체 길이 / 2} 계산
        float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

        // 이동 방향이 X축일 경우
        if (isMovingX)
        {
            // 최종 객체 위치 값 갱신_Vector3(최종 위치 갱신 값 * 객체가 이동하는 값, 적층한 객체의 개수, 최근 객체의 위치 값)
            lastBlock.localPosition = new Vector3(movePosition * MovingBoundsSize, stackCount, secondaryPosition);
        }
        // 이동 방향이 X축이 아닐 경우
        else
        {
            // 최종 객체 위치 값 갱신_Vector3(최근 객체의 위치 값, 적층한 객체의 개수, 최종 위치 갱신 값 * 객체가 이동하는 값)
            lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, -movePosition * MovingBoundsSize);
        }
    }

    // 객체 적층 메서드
    bool PlaceBlock()
    {
        // 최종 위치 값 갱신
        Vector3 lastPosition = lastBlock.localPosition;

        // 이동 방향이 X축일 경우
        if (isMovingX)
        {
            // 벗어난 길이 값 정의
            float deltaX = prevBlockPosition.x - lastPosition.x;
            bool isNegativeNum = (deltaX < 0) ? true : false;

            // 절대값 반환_벗어난 길이 값
            deltaX = Mathf.Abs(deltaX);
            // 벗어난 길이 값이 최소 값보다 높을 경우
            if (deltaX > ErrorMargin)
            {
                // 객체의 크기 연산
                stackBounds.x -= deltaX;
                // 객체의 크기가 0 이하일 경우
                if (stackBounds.x <= 0)
                {
                    // 반환_false
                    return false;
                }
                // 두 객체 사이의 중심 값 정의
                float middle = (prevBlockPosition.x + lastPosition.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.x = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaX / 2f;
                CreateRubble(
                    new Vector3(
                        isNegativeNum
                        ? lastPosition.x + stackBounds.x / 2 + rubbleHalfScale
                        : lastPosition.x - stackBounds.x / 2 - rubbleHalfScale
                        , lastPosition.y
                        , lastPosition.z
                        ),
                    new Vector3(deltaX, 1, stackBounds.y)
                );

                comboCount = 0;
            }
            // 벗어난 길이 값이 최소 값보다 낮을 경우
            else
            {
                // 콤보 연산 메서드 호출
                CheckCombo();
                // 최종 객체 위치 값 갱신
                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }


        }
        // 이동 방향이 X축이 아닐 경우
        else
        {
            float deltaZ = prevBlockPosition.z - lastPosition.z;
            bool isNegativeNum = (deltaZ < 0) ? true : false;

            deltaZ = Mathf.Abs(deltaZ);
            if (deltaZ > ErrorMargin)
            {
                stackBounds.y -= deltaZ;
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = (prevBlockPosition.z + lastPosition.z) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 tempPosition = lastBlock.localPosition;
                tempPosition.z = middle;
                lastBlock.localPosition = lastPosition = tempPosition;

                float rubbleHalfScale = deltaZ / 2f;
                CreateRubble(
                    new Vector3(
                        lastPosition.x,
                        lastPosition.y,
                        isNegativeNum
                        ? lastPosition.z + stackBounds.y / 2 + rubbleHalfScale
                        : lastPosition.z - stackBounds.y / 2 - rubbleHalfScale
                        ),
                    new Vector3(stackBounds.x, 1, deltaZ)
                );

                comboCount = 0;
            }
            else
            {
                CheckCombo();

                lastBlock.localPosition = prevBlockPosition + Vector3.up;
            }
        }
        
        secondaryPosition = (isMovingX) ? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    // 콤보 연산 메서드
    void CheckCombo()
    {
        comboCount++;

        if (comboCount > maxCombo)
            maxCombo = comboCount;

        if ( (comboCount % 5) == 0)
        {
            Debug.Log("5 Combo Success!!");
            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x =
                (stackBounds.x > BoundSize) ? BoundSize : stackBounds.x;
            stackBounds.y =
                (stackBounds.y > BoundSize) ? BoundSize : stackBounds.y;
        }
    }

    void UpdateScore()
    {
        if (bestScore < stackCount)
        {
            Debug.Log("최고 점수 갱신");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }
}
