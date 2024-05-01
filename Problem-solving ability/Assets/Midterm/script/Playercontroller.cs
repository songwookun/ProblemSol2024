using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Playercontroller : MonoBehaviour
{
    public float speed;
    public Transform cameraPivot;
    public TMP_Text endText;
    public TMP_Text overText;
    public GameObject restartButton;


    float rotationDuration = 1f; // 회전 시간

    float rotationTimer; // 회전 타이머

    bool isRotating; // 회전 중 여부
    Quaternion startRotation; // 시작 회전 각도
    Quaternion targetRotation; // 목표 회전 각도

    void Update()
    {
        PlayerMove();
        CameraInput();
        CameraRotateAnimation();
    }

    void PlayerMove()
    {
        float hInput = 0; // 좌우 입력
        float vInput = 0; // 상하 입력

        if (Input.GetKey(KeyCode.W))
        {
            vInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vInput = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            hInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            hInput = 1f;
        }

        // 카메라의 전방 벡터를 기준으로 이동 방향 설정
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // 수평 이동이므로 y값은 0으로 설정
        cameraForward.Normalize(); // 정규화

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f; // 수평 이동이므로 y값은 0으로 설정
        cameraRight.Normalize(); // 정규화

        // 이동 방향 벡터 계산
        Vector3 moveDir = cameraForward * vInput + cameraRight * hInput;
        moveDir.Normalize(); // 정규화

        // 플레이어 이동
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // 이동 방향으로 회전
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void CameraInput()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isRotating)
        {
            RotateCamera(90);
        }
        else if (Input.GetKeyDown(KeyCode.P) && !isRotating)
        {
            RotateCamera(-90);
        }
    }

    void CameraRotateAnimation()
    {
        // 회전 중일 때는 회전 애니메이션 적용
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            cameraPivot.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // 회전 중에 플레이어도 함께 회전
            transform.rotation = cameraPivot.transform.rotation;

            if (rotationTimer >= rotationDuration)
            {
                isRotating = false;
            }
        }
    }

    void RotateCamera(float angle)
    {
        startRotation = cameraPivot.transform.rotation;
        targetRotation = Quaternion.Euler(0, angle, 0) * startRotation;
        isRotating = true;
        rotationTimer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            // 게임 종료 및 게임 오버 텍스트 활성화
            endText.gameObject.SetActive(true);
            // 재시작 버튼 활성화
            restartButton.SetActive(true);

        }
        else if (other.CompareTag("Enemy"))
        {
            // 게임 오버 텍스트 활성화
            overText.gameObject.SetActive(true);
            // 재시작 버튼 활성화
            restartButton.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // 씬 다시 로드하여 게임 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
