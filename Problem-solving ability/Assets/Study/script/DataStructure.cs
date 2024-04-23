using System; 
using UnityEngine;

namespace DataStrucuture // DataStrucuture ���ӽ����̽��� �����մϴ�.
{
    public class LinkedListNode<T> // ���׸� ��ũ�� ����Ʈ ��� Ŭ������ �����մϴ�.
    {
        public T Data { get; set; } // ����� �����͸� �����ϴ� �Ӽ��Դϴ�.
        public LinkedListNode<T> Next { get; set; } // ���� ��带 ����Ű�� ��ũ�� �����ϴ� �Ӽ��Դϴ�.

        public LinkedListNode(T data) // ��ũ�� ����Ʈ ����� �������Դϴ�.
        {
            Data = data; // �����͸� �ʱ�ȭ�մϴ�.
            Next = null; // ���� ��带 null�� �ʱ�ȭ�մϴ�.
        }
    }

    public class LinkedList<T> // ���׸� ��ũ�� ����Ʈ Ŭ������ �����մϴ�.
    {
        public LinkedListNode<T> head; // ��ũ�� ����Ʈ�� ��� ��带 �����ϴ� �����Դϴ�.

        public LinkedList() // ��ũ�� ����Ʈ�� �������Դϴ�.
        {
            head = null; // ��� ��带 null�� �ʱ�ȭ�մϴ�.
        }

        public void Add(T data) // ��ũ�� ����Ʈ�� ��Ҹ� �߰��ϴ� �޼����Դϴ�.
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(data); // ���ο� ��带 �����մϴ�.
            if (head == null) // ��� ��尡 null�� ���
            {
                head = newNode; // ���ο� ��带 ��� ���� �����մϴ�.
            }
            else // ��� ��尡 null�� �ƴ� ���
            {
                LinkedListNode<T> current = head; // ���� ��带 ��� ���� �����մϴ�.
                while (current.Next != null) // ���� ����� ���� ��尡 null�� �ƴ� ����
                {
                    current = current.Next; // ���� ���� �̵��մϴ�.
                }
                current.Next = newNode; // ���� ����� ���� ��忡 ���ο� ��带 �����մϴ�.
            }
        }
    }

    public class Queue<T> // ���׸� ť Ŭ������ �����մϴ�.
    {
        private LinkedList<T> list; // ��ũ�� ����Ʈ�� �����ϴ� �����Դϴ�.

        public Queue() // ť�� �������Դϴ�.
        {
            list = new LinkedList<T>(); // ��ũ�� ����Ʈ�� �ʱ�ȭ�մϴ�.
        }

        // ť�� ��Ҹ� �߰��ϴ� �޼����Դϴ�.
        public void Enqueue(T data)
        {
            list.Add(data); // ��ũ�� ����Ʈ�� ��Ҹ� �߰��մϴ�.
        }

        // ť���� ��Ҹ� �����ϰ� ��ȯ�ϴ� �޼����Դϴ�.
        public T Dequeue()
        {
            T data = list.head.Data; // ��� ����� �����͸� �����մϴ�.
            list.head = list.head.Next; // ��� ��带 ���� ���� �����մϴ�.
            return data; // ������ �����͸� ��ȯ�մϴ�.
        }

        // ť�� ��� �ִ��� ���θ� ��ȯ�ϴ� �޼����Դϴ�.
        public bool IsEmpty()
        {
            return list.head == null; // ��� ��尡 null�̸� ť�� ��� �ֽ��ϴ�.
        }

    }

    public class Stack<T> // ���׸� ���� Ŭ������ �����մϴ�.
    {
        private Queue<T> queue; // ť�� �����ϴ� �����Դϴ�.
        private Queue<T> temp; // �ӽ� ť�� �����ϴ� �����Դϴ�.

        public Stack() // ������ �������Դϴ�.
        {
            queue = new Queue<T>(); // ť�� �ʱ�ȭ�մϴ�.
            temp = new Queue<T>(); // �ӽ� ť�� �ʱ�ȭ�մϴ�.
        }

        public void Push(T data) // ���ÿ� ��Ҹ� �߰��ϴ� �޼����Դϴ�.
        {
            // ť�� �ִ� ��� ��Ҹ� �ӽ� ť�� �ű�ϴ�.
            while (!queue.IsEmpty())
            {
                temp.Enqueue(queue.Dequeue()); // ť���� ��Ҹ� ���� �ӽ� ť�� �߰��մϴ�.
            }

            // ���ο� ��Ҹ� ť�� �߰��մϴ�.
            queue.Enqueue(data); // ���ο� ��Ҹ� ť�� �߰��մϴ�.

            // �ӽ� ť�� �ִ� ��� ��Ҹ� �ٽ� ť�� �Űܼ� ������ �����մϴ�.
            while (!temp.IsEmpty())
            {
                queue.Enqueue(temp.Dequeue()); // �ӽ� ť���� ��Ҹ� ���� ť�� �߰��մϴ�.
            }
        }

        public T Pop() // ���ÿ��� ��Ҹ� �����ϰ� ��ȯ�ϴ� �޼����Դϴ�.
        {
            if (queue.IsEmpty()) // ť�� ��� �ִ� ���
            {
                throw new InvalidOperationException("Stack is empty."); // ���ܸ� �߻���ŵ�ϴ�.
            }
            return queue.Dequeue(); // ť���� ��Ҹ� �����ϰ� ��ȯ�մϴ�.
        }
    }
}
