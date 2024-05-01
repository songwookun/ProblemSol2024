using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Vector3 originalPosition; // ó�� ��ġ�� ������ ����
    Quaternion originalRotation; // ó�� ȸ������ ������ ����
    bool isOpen = false; // ���� ���� �ִ��� ���θ� ��Ÿ���� ����

    // Start is called before the first frame update
    void Start()
    {
        // ó�� ��ġ�� ȸ���� ����
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
                if (!isOpen) // ���� ���� �ִ� ��쿡�� �̵��ϰ� ȸ��
                {
                    Vector3 newPosition = transform.position + new Vector3(0, 0, 0.8f);
                    transform.position = newPosition;
                    transform.Rotate(0, 90, 0);

                    isOpen = true; // ���� �������� ǥ��
                }
                else // ���� �̹� ���� �ִ� ��쿡�� ���� ��ġ�� ȸ�������� ���ư�
                {
                    transform.position = originalPosition;
                    transform.rotation = originalRotation;

                    isOpen = false; // ���� �ٽ� �������� ǥ��
                }
            }
        }
    }
}
