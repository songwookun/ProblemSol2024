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
        BulletDirection = direction.normalized; // �Էµ� ���� ���͸� ����ȭ�Ͽ� ����
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += BulletDirection * BulletSpeed * Time.deltaTime;

        transform.Rotate(BulletDirection, BulletRotationSpeed * Time.deltaTime);

        // ���� ������Ʈ�� ��ġ�� �������� �ֺ��� �ִ� ��� Collider���� �浹�� �˻�
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("RedCube"))
            {
                Destroy(collider.gameObject);
                // �浹�� ��ü�� �÷��̾��� ��, ���� ������Ʈ�� �ı�
                gameObject.SetActive(false);
                list.Enqueue(gameObject);
                break; // �� �̻� �˻����� ����
            }
            else if (collider.CompareTag("GreenBox"))
            {
                gameObject.SetActive(false);
                list.Enqueue(gameObject);
            }

        }
    }
}
