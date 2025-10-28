using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Obstacle 묶음이 생성될 때 랜덤 상승시킬 최댓값 정의
    public float highPosY = 1f;
    // Obstacle 묶음이 생성될 때 랜덤 하향시킬 최댓값 정의
    public float lowPosY = -1f;

    // Obstacles 사이의 넓이 최솟값 정의
    public float holeSizeMin = 1f;
    // Obstacles 사이의 넓이 최댓값 정의
    public float holeSizeMax = 3f;

    // Obstacles의 Component_Transform 가져옴
    public Transform topObject;
    public Transform bottomObject;

    // Obstacle 묶음이 생성될 가로 간격 정의
    public float widthPadding = 4f;

    // GameManager 역할을 수행할 변수 명 정의
    GameManager gameManager;

    private void Start()
    {
        // GameManager 역할을 싱글톤_GameManager으로 초기화
        gameManager = GameManager.Instance;
    }

    // Obstacle 묶음 랜덤 생성 메서드
    public Vector3 SetRandomPlace(Vector3 lastPosition, int obstacleCount)
    {
        // Obstacles 사이의 공간 넓이 정의
        float holeSize = Random.Range(holeSizeMin, holeSizeMax);
        // Obstacles의 위치를 정의하기 위한 변수 정의
        float halfHoleSize = holeSize / 2;

        // Obstacle_TopObject가 생성될 기본 높이값 정의
        topObject.localPosition = new Vector3(0, halfHoleSize);
        // Obstacle_BottomObject가 생성될 기본 높이값 정의
        bottomObject.localPosition = new Vector3(0, -halfHoleSize);

        // Obstacle 묶음이 생성될 위치 정의 (가로 간격 값 초기화)
        Vector3 placePosition = lastPosition + new Vector3(widthPadding, 0);
        // Obstacles의 랜덤 위치값 정의 (최솟값 : LowPosY, 최댓값 : HighPosY)
        placePosition.y = Random.Range(lowPosY, highPosY);

        // 랜덤 생성할 Obstacle 묶음의 위치 이동
        transform.position = placePosition;

        // 반환_Obstacle 묶음의 위치 값
        return placePosition;
    }

    // Trigger를 통과했을 경우 호출할 메서드
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player의 Component_Collider 참조
        Player player = collision.GetComponent<Player>();

        // Player가 Null 상태가 아닐 경우
        if (player != null)
        {
            // 점수 추가 메서드 호출 (매개 변숫값 : 1점)
            gameManager.AddScore(1);
        }
    }
}
