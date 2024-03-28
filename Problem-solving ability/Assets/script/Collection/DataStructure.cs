using System;
using UnityEngine;

namespace DataStrucuture
{
    public class LinkedListNode<T>
    {
        public T Data { get; set; }
        public LinkedListNode<T> Next { get; set; }

        public LinkedListNode(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class LinkedList<T>
    {
        public LinkedListNode<T> head;

        public LinkedList()
        {
            head = null;
        }

        public void Add(T data)
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                LinkedListNode<T> current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }
    }

    public class Queue<T>
    {
        private LinkedList<T> list;

        public Queue()
        {
            list = new LinkedList<T>();
        }

        // 큐에 요소를 추가합니다.
        public void Enqueue(T data)
        {
            list.Add(data);
        }

        // 큐에서 요소를 제거하고 반환합니다.
        public T Dequeue()
        {

            T data = list.head.Data;
            list.head = list.head.Next;
            return data;
        }

        public bool IsEmpty()
        {
            return list.head == null;
        }

    }

    public class Stack<T>
    {
        private Queue<T> queue;
        private Queue<T> temp;

        public Stack()
        {
            queue = new Queue<T>();
            temp = new Queue<T>();
        }

        public void Push(T data)
        {
            // queue에 있는 모든 요소를 temp로 옮깁니다.
            while (!queue.IsEmpty())
            {
                temp.Enqueue(queue.Dequeue());
            }

            // 새로운 요소를 queue에 추가합니다.
            queue.Enqueue(data);

            // temp에 있는 모든 요소를 다시 queue로 옮겨서 순서를 복원합니다.
            while (!temp.IsEmpty())
            {
                queue.Enqueue(temp.Dequeue());
            }
        }

        public T Pop()
        {
            if (queue.IsEmpty())
            {
                throw new InvalidOperationException("Stack is empty.");
            }
            return queue.Dequeue();
        }
    }
}