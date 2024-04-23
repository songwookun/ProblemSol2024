using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 10f;
    public float BulletRotationSpeed;
    public Queue<GameObject> list;
    public Vector3 BulletDirection;

    // Start is called before the first frame update
    void Start()
    {
        if (BulletDirection != Vector3.zero)
        {
            BulletDirection.Normalize();
        }
    }

    public void Init(Vector3 InitialPos, Queue<GameObject> queue, Vector3 direction)
    {
        transform.position = InitialPos;
        list = queue;
        BulletDirection = direction.normalized; // 입력된 방향 벡터를 정규화하여 저장
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += BulletDirection * BulletSpeed * Time.deltaTime;

        transform.Rotate(BulletDirection, BulletRotationSpeed * Time.deltaTime);

        // 현재 오브젝트의 위치를 기준으로 주변에 있는 모든 Collider와의 충돌을 검사
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("RedCube"))
            {
                Destroy(collider.gameObject);
                // 충돌한 물체가 플레이어일 때, 현재 오브젝트를 파괴
                gameObject.SetActive(false);
                list.Enqueue(gameObject);
                break; // 더 이상 검사하지 않음
            }
            else if (collider.CompareTag("GreenBox"))
            {
                gameObject.SetActive(false);
                list.Enqueue(gameObject);
            }

        }
    }
}
