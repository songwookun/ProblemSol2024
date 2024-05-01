using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Vector3 originalPosition; // 처음 위치를 저장할 변수
    Quaternion originalRotation; // 처음 회전값을 저장할 변수
    bool isOpen = false; // 문이 열려 있는지 여부를 나타내는 변수

    // Start is called before the first frame update
    void Start()
    {
        // 처음 위치와 회전값 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ItemKeeper.haskeys >= 4)
            {
                if (!isOpen) // 문이 닫혀 있는 경우에만 이동하고 회전
                {
                    Vector3 newPosition = transform.position + new Vector3(0, 0, 0.8f);
                    transform.position = newPosition;
                    transform.Rotate(0, 90, 0);

                    isOpen = true; // 문이 열렸음을 표시
                }
                else // 문이 이미 열려 있는 경우에는 원래 위치와 회전값으로 돌아감
                {
                    transform.position = originalPosition;
                    transform.rotation = originalRotation;

                    isOpen = false; // 문이 다시 닫혔음을 표시
                }
            }
        }
    }
}
