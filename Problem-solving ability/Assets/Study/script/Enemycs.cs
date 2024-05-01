using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed = 2f; // 적의 이동 속도를 설정하는 변수

    private Transform playerTransform; // 플레이어의 Transform을 받아올 변수

    void Update()
    {
        // 플레이어의 Transform을 가져옵니다.
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 적의 위치에서 플레이어 시점으로 레이캐스트를 발사합니다.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit))
        {
            // 레이캐스트가 플레이어와 충돌한 경우 플레이어 쪽으로 이동합니다.
            if (hit.collider.CompareTag("Player"))
            {
                // 적이 플레이어 쪽을 향해 이동하는 방향을 계산합니다.
                Vector3 direction = playerTransform.position - transform.position;
                // 이동 방향을 정규화하여 적의 이동 속도에 곱한 후 시간에 따라 이동합니다.
                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime);
            }
        }
    }
}
