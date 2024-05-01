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


    float rotationDuration = 1f; // ȸ�� �ð�

    float rotationTimer; // ȸ�� Ÿ�̸�

    bool isRotating; // ȸ�� �� ����
    Quaternion startRotation; // ���� ȸ�� ����
    Quaternion targetRotation; // ��ǥ ȸ�� ����

    void Update()
    {
        PlayerMove();
        CameraInput();
        CameraRotateAnimation();
    }

    void PlayerMove()
    {
        float hInput = 0; // �¿� �Է�
        float vInput = 0; // ���� �Է�

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

        // ī�޶��� ���� ���͸� �������� �̵� ���� ����
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // ���� �̵��̹Ƿ� y���� 0���� ����
        cameraForward.Normalize(); // ����ȭ

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f; // ���� �̵��̹Ƿ� y���� 0���� ����
        cameraRight.Normalize(); // ����ȭ

        // �̵� ���� ���� ���
        Vector3 moveDir = cameraForward * vInput + cameraRight * hInput;
        moveDir.Normalize(); // ����ȭ

        // �÷��̾� �̵�
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        // �̵� �������� ȸ��
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
        // ȸ�� ���� ���� ȸ�� �ִϸ��̼� ����
        if (isRotating)
        {
            rotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);
            cameraPivot.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // ȸ�� �߿� �÷��̾ �Բ� ȸ��
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
            // ���� ���� �� ���� ���� �ؽ�Ʈ Ȱ��ȭ
            endText.gameObject.SetActive(true);
            // ����� ��ư Ȱ��ȭ
            restartButton.SetActive(true);

        }
        else if (other.CompareTag("Enemy"))
        {
            // ���� ���� �ؽ�Ʈ Ȱ��ȭ
            overText.gameObject.SetActive(true);
            // ����� ��ư Ȱ��ȭ
            restartButton.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // �� �ٽ� �ε��Ͽ� ���� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
