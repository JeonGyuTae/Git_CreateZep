using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTone : MonoBehaviour
{
    // SingleTone으로 사용할 변수를 싱글톤_singleTone으로 선언 (외부에서 참조 시 사용)
    static SingleTone singleTone;
    // 싱글톤_singleTone을 외부로 참조 가능하게 하는 변수 선언 (단순 복사 O, 복제 X)
    public static SingleTone Instance { get { return singleTone; } }

    private void Awake()
    {
        // singleTone 객체 선언 (이 구역의 SingleTone 은 나다!)
        singleTone = this;
    }
}

/*
 => 싱글톤 패턴의 기본형
    각종 Manager 외에도 고유한 객체를 사용하고자 할 경우 좋은 대안이 되어줌
 */
