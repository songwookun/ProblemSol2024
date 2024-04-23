using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject BulletPrefab; //�Ѿ� ������
    public Transform reddot; //�Ѿ��� �߻��ϴ� ��ġ 
    public Queue<GameObject> queue; //�Ѿ��� ���� ť

    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<GameObject>(); //ť �ʱ�ȭ
        for (int i = 0; i < 10; i++) //�Ѿ� 10���� �̸� ������ ť�� ����
        {
            GameObject obj = Instantiate(BulletPrefab); // �Ѿ� �������� �����Ͽ� ����
            obj.GetComponent<Bullet>().Init(reddot.position, queue, reddot.transform.forward);//Bullet��ũ��Ʈ ����, Init �޼��� ȣ�� ��ġ ���� ����
            queue.Enqueue(obj); //������ ������ �Ѿ� ��ü�� ť�� �߰�
            obj.SetActive(false);// ť�� �� ��ü�� ��Ȱ��ȭ
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //���콺 ���� Ŭ��������
        {
            if (queue.Count > 0) //�Ѿ��� 1���� ���Ҵ��� Ȯ��
            {
                GameObject bullet = queue.Dequeue(); // ť���� �Ѿ��� ����
                bullet.GetComponent<Bullet>().Init(reddot.position, queue, reddot.transform.forward);//Bullet��ũ��Ʈ ����, Init �޼��� ȣ�� ��ġ ���� ����
                bullet.SetActive(true); //�Ѿ� Ȱ��ȭ
            }
            else //�Ѿ��� ���� ���
            {
                Debug.Log("�Ѿ��� ��� �����Ǿ����ϴ�.");
            }
        }
    }
}
