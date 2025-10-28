using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    // Obstacle 묶음의 개수 값 정의 (기본 값)
    public int obstacleCount = 0;
    // Obstacle 묶음의 마지막 위치 값 정의
    public Vector3 obstacleLastPosition = Vector3.zero;

    // Obstacle 묶음의 개수 값 정의 (임의 값)
    public int numBgCount = 5;

    void Start()
    {
        // 게임 내 Component_Obstacle(Script)가 달린 모든 객체 검색 후 참조
        Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
        // Obstacle 묶음의 마지막 위치 값 초기화 (가장 앞에 위치한 객체의 위치 값)
        obstacleLastPosition = obstacles[0].transform.position;
        // Obstacle 묶음의 개수 값 초기화
        obstacleCount = obstacles.Length;

        // 반복문_Obstacle 묶음의 개수 값만큼
        for (int i = 0; i < obstacleCount; i++)
        {
            // Obstacle 묶음의 마지막 위치 값 초기화 (가장 뒤에 위치한 객체의 위치 값) -> 객체를 다시 배치할 때 다음 위치 값에 활용
            obstacleLastPosition = obstacles[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }

    // Trigger와 충돌했을 경우 호출할 메서드 (Trigger이기에 물리적 충돌이 아닌 충돌 정보만 반환)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체의 Unity Tag가 'Background' 일 경우
        if (collision.CompareTag("Background"))
        {
            // 객체의 가로 길이 값 참조 후 정의
            float widthOfBgObject = ((BoxCollider2D)collision).size.x;

            // 충돌한 객체의 위치 값 참조 후 변수 값 정의
            Vector3 pos = collision.transform.position;

            // 충돌한 객체를 이동시킬 가로 길이 정의 (기본 값 * 변수 값만큼)
            pos.x += widthOfBgObject * numBgCount;
            // 충돌한 객체 이동 (위에서 정의한 값 만큼)
            collision.transform.position = pos;

            // 반환 (아래 코드 무시)
            return;
        }

        // Obstacle 묶음의 Component_Collider 참조
        Obstacle obstacle = collision.GetComponent<Obstacle>();
        // Obstacle 묶음 일 경우
        if (obstacle)
        {
            // Obstacle 묶음의 위치 값 초기화 (가장 뒤에 위치한 객체의 위치 값) -> 이동하는 객체이고 가로 길이가 모두 동일하기에 가능한 작업
            obstacleLastPosition = obstacle.SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }
}
