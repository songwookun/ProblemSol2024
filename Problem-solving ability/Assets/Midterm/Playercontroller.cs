using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public float speed = 5f;
    public Camera mainCamera; // ī�޶� �޾ƿ��� ����
    public float rotationDuration = 1f; // ȸ�� �ִϸ��̼� ���� �ð�
    private bool isRotating = false; // ȸ�� ������ ���θ� ��Ÿ���� �÷���
    private Quaternion targetRotation; // ��ǥ ȸ����

    void Update()
    {
        // ȸ�� ���̸� �̵��� ����
        if (!isRotating)
        {
            // WASD �Է� ����
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

            // �̵� ���� ���
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;

            // ���� ��ġ���� �̵� ���͸�ŭ �̵�
            transform.Translate(mainCamera.transform.forward * movement.z + mainCamera.transform.right * movement.x, Space.World);

            // O Ű�� ������ 45�� ȸ��
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartCoroutine(RotateCamera(90f));
            }
            // P Ű�� ������ -45�� ȸ��
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

        // sin�� cos�� ����Ͽ� ȸ���� ���� ���
        targetRotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, targetRotationY, mainCamera.transform.rotation.eulerAngles.z);

        while (elapsedTime < rotationDuration)
        {
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��Ȯ�� ������ ȸ��
        mainCamera.transform.rotation = targetRotation;
        isRotating = false;
    }
}
