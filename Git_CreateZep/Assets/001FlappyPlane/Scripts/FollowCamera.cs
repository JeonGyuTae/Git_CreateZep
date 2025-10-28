using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // 해당 Script를 지닌 객체가 추적할 대상 정의
    public Transform target;
    // 해당 Script를 지닌 객체의 위치 값_X 값 정의
    float offsetX;

    void Start()
    {
        // 추적할 대상을 찾지 못했을 경우
        if (target == null)
        {
            // 에러 메세지 출력_Target Not Found
            Debug.LogError("Target is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        // 객체의 위치 값_X 값 초기화 (객체의 위치 값_X 값 - 추적할 대상의 위치 값_X 값) -> 이 값을 기반으로 X 값을 갱신함
        offsetX = transform.position.x - target.position.x;
    }

    void Update()
    {
        // 추적할 대상을 찾지 못했을 경우
        if (target == null)
        {
            // 에러 메세지 출력_Target Not Found
            Debug.LogError("Target is Null");
            // 반환 (아래 코드 무시)
            return;
        }

        // 객체의 위치 값 참조 (Transform은 직접 변조 불가능, 1회 가공 필요)
        Vector3 pos = transform.position;
        // 객체의 위치 값 초기화 (=> 1회 가공 작업)
        pos.x = target.position.x + offsetX;
        // 객체의 위치 값 갱신
        transform.position = pos;

    }
}
