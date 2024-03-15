using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    // �Ѿ� ������
    public GameObject BulletPrefab;

    // �Ѿ� Linked List �����
    BulletLinkedList LinkedList;

    BulletLinkedList dequeueLinkedList;

    // �Ѿ� Linked List
    class BulletLinkedList
    {
        Queue<Node> BulletQueue = new Queue<Node>(); // �Ѿ� ��� ����
        Node queueBackNode; // Queue�� ���� �ڿ� �ִ� ���

        GameObject prefabObj;   // �Ѿ� Linked List ���� �� ������ ������ ������Ʈ
        int NodeCount;          // ��� ī��Ʈ

        bool log = false;

        // ������ �� �ʱ�ȭ : ���� ���� ���(������ ������Ʈ, �߰��� ��� ����)
        public BulletLinkedList(GameObject prefab, int Count)
        {
            prefabObj = prefab;
            NodeCount = 0;

            for (int i = 0; i < Count; i++)
            {
                Enqueue(i);
            }

        }

        public void writeLog(bool set) => log = set;

        // ������ �� �ʱ�ȭ : Queue���� �������� ���
        public BulletLinkedList()
        {
            NodeCount = 0;
        }

        // ��� �߰� �� Queue �ֱ�	
        void Enqueue(int Data)
        {
            if (NodeCount == 0) // ó�� ��� �����ϴ� ���
            {
                // Queue�� ���� ������ (������ ��) ��� �ֱ�
                BulletQueue.Enqueue(new Node(Instantiate(prefabObj), Data));

                // ���� ��尡 1���� ������ Queue�� ���� ��?�� �ִ� ��Ұ� �� ��·��
                queueBackNode = BulletQueue.Peek();
            }
            else // ��尡 1�� �̻��� �ִ� ���
            {
                // ���ο� ��� ���� �� �ӽ� ����
                Node newNode = new Node(Instantiate(prefabObj), Data);
                BulletQueue.Enqueue(newNode);

                // ���� ���߿� ���� ��带 ���� ������ ��忡 ������
                queueBackNode.Next = newNode;

                // ���� ������ ��带 Queue�� ���� ��?�� �ִ� ��ҷ� ����
                queueBackNode = newNode;
            }

            NodeCount++; // ��� ���� ����
            DebugNode();
        }

        public void Enqueue(Node node)
        {
            if (NodeCount == 0) // ��尡 ���� ���
            {
                // Queue�� ��� �ֱ�
                BulletQueue.Enqueue(node);

                // ���� ��尡 1���� ������ Queue�� ���� ��?�� �ִ� ��Ұ� �� ��·��
                queueBackNode = BulletQueue.Peek();
            }
            else // ��尡 1�� �̻��� �ִ� ���
            {
                // �ֱٿ� ���� ��� ����
                Node newNode = node;
                BulletQueue.Enqueue(newNode);

                // �� ���� ���� ��带 �ֱٿ� ���� ��忡 ������
                queueBackNode.Next = newNode;

                // �ֱٿ� ���� ��带 Queue�� ���� ��?�� �ִ� ��ҷ� ����
                queueBackNode = newNode;
            }

            NodeCount++; // ��� ���� ����
            DebugNode();
        }

        // ��� ���� �� Queue ����
        public Node Dequeue()
        {
            if (NodeCount != 0) // node�� ���� ������
            {
                Node dequeueNode = BulletQueue.Dequeue();

                GameObject dequeueBullet = dequeueNode.nodeBullet;
                dequeueBullet.SetActive(true);
                dequeueBullet.transform.position = new Vector3(-8, 0, 0);

                dequeueNode.Next = null;

                NodeCount--;
                if (NodeCount != 0) DebugNode();
                return dequeueNode;
            }
            else return null; //node�� ���� ���� ������
        }

        // �Ѿ� �� ��� �΋H���� �� �ӽ� ������ Queue���� ��� ����
        public Node Dequeue(bool isTrigged)
        {
            if (NodeCount != 0) // node�� ���� ������
            {
                Node dequeueNode = BulletQueue.Dequeue();

                GameObject dequeueBullet = dequeueNode.nodeBullet;
                dequeueBullet.SetActive(false);
                dequeueBullet.transform.position = new Vector3(-8, 0, 0);

                dequeueNode.Next = null;

                NodeCount--;
                if (NodeCount != 0) DebugNode();
                return dequeueNode;
            }
            else return null; //node�� ���� ���� ������
        }

        public Node compare()
        {
            if (NodeCount == 0) return null;

            Node compareBullet = BulletQueue.Peek();

            if (!compareBullet.nodeBullet.activeSelf) return compareBullet;
            else return null;
        }


        public void DebugNode()
        {
            if (!log) return;

            string queueListData = "";
            Node currentNode = BulletQueue.Peek();

            while (true)
            {
                queueListData += $"{currentNode.Data}, ";
                if (currentNode.Next != null) currentNode = currentNode.Next;
                else break;
            }
            Debug.Log(queueListData);
        }
    }

    class Node
    {
        // ���� �Բ� ������ ������Ʈ ����
        public GameObject nodeBullet;

        public Node Next; // ���� ��� ����
        public int Data; // ������

        // ������ ������Ʈ, ������ ������ ������
        public Node(GameObject obj, int Num)
        {
            nodeBullet = obj;
            Next = null;
            Data = Num;
            obj.SetActive(false);
        }
    }

    // ���� �� BulletLinkedList ����
    void Start()
    {
        LinkedList = new BulletLinkedList(BulletPrefab, 10); // 10��
        dequeueLinkedList = new BulletLinkedList(); // 10��

        // �̰� ��� ���� ���θ� Ȯ���� ����� �α� ���� ����
        LinkedList.writeLog(true);
    }

    void Update()
    {
        // ���콺 Ŭ�� �� �� ����
        if (Input.GetMouseButtonDown(0))
        {
            // �߻��� ���� �ش��ϴ� ��� ��������
            Node dequeueNode;
            dequeueNode = LinkedList.Dequeue();

            // �ӽ÷� ������ ��� ����Ʈ�� �ֱ�
            if(dequeueNode != null)
            dequeueLinkedList.Enqueue(dequeueNode);
        }

        // �Ѿ� �΋H������ Ȯ��
        Node compareBullet = dequeueLinkedList.compare();
        if (compareBullet != null)
        {
            dequeueLinkedList.Dequeue(true);
            LinkedList.Enqueue(compareBullet);
        }
    }
}
