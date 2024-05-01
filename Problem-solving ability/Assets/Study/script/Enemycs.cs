using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // ���� �̵� �ӵ��� �����ϴ� ����

    private Transform playerTransform; // �÷��̾��� Transform�� �޾ƿ� ����

    void Update()
    {
        // �÷��̾��� Transform�� �����ɴϴ�.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // ���� ��ġ���� �÷��̾� �������� ����ĳ��Ʈ�� �߻��մϴ�.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit))
        {
            // ����ĳ��Ʈ�� �÷��̾�� �浹�� ��� �÷��̾� ������ �̵��մϴ�.
            if (hit.collider.CompareTag("Player"))
            {
                // ���� �÷��̾� ���� ���� �̵��ϴ� ������ ����մϴ�.
                Vector3 direction = playerTransform.position - transform.position;
                // �̵� ������ ����ȭ�Ͽ� ���� �̵� �ӵ��� ���� �� �ð��� ���� �̵��մϴ�.
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
            }
        }
    }
}
