using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float speed = 5f;
    public Camera mainCamera; // 카메라를 받아오는 변수
    public float rotationDuration = 1f; // 회전 애니메이션 지속 시간
    private bool isRotating = false; // 회전 중인지 여부를 나타내는 플래그
    private Quaternion targetRotation; // 목표 회전값

    void Update()
    {
        // 회전 중이면 이동을 막음
        if (!isRotating)
        {
            // WASD 입력 감지
            float horizontalInput = 0f;
            float verticalInput = 0f;

            if (Input.GetKey(KeyCode.W))
                verticalInput = 1f;
            if (Input.GetKey(KeyCode.S))
                verticalInput = -1f;
            if (Input.GetKey(KeyCode.A))
                horizontalInput = -1f;
            if (Input.GetKey(KeyCode.D))
                horizontalInput = 1f;

            // 이동 방향 계산
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;

            // 현재 위치에서 이동 벡터만큼 이동
            transform.Translate(mainCamera.transform.forward * movement.z + mainCamera.transform.right * movement.x, Space.World);

            // O 키를 누르면 45도 회전
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartCoroutine(RotateCamera(90f));
            }
            // P 키를 누르면 -45도 회전
            else if (Input.GetKeyDown(KeyCode.P))
            {
                StartCoroutine(RotateCamera(-90f));
            }
        }
    }

    IEnumerator RotateCamera(float angle)
    {
        isRotating = true;
        float elapsedTime = 0f;
        Quaternion startRotation = mainCamera.transform.rotation;
        float targetRotationY = mainCamera.transform.rotation.eulerAngles.y + angle;

        // sin과 cos를 사용하여 회전할 각도 계산
        targetRotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, targetRotationY, mainCamera.transform.rotation.eulerAngles.z);

        while (elapsedTime < rotationDuration)
        {
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 각도로 회전
        mainCamera.transform.rotation = targetRotation;
        isRotating = false;
    }
}
