using System; 
using UnityEngine;

namespace DataStrucuture // DataStrucuture 네임스페이스를 정의합니다.
{
    public class LinkedListNode<T> // 제네릭 링크드 리스트 노드 클래스를 정의합니다.
    {
        public T Data { get; set; } // 노드의 데이터를 저장하는 속성입니다.
        public LinkedListNode<T> Next { get; set; } // 다음 노드를 가리키는 링크를 저장하는 속성입니다.

        public LinkedListNode(T data) // 링크드 리스트 노드의 생성자입니다.
        {
            Data = data; // 데이터를 초기화합니다.
            Next = null; // 다음 노드를 null로 초기화합니다.
        }
    }

    public class LinkedList<T> // 제네릭 링크드 리스트 클래스를 정의합니다.
    {
        public LinkedListNode<T> head; // 링크드 리스트의 헤드 노드를 저장하는 변수입니다.

        public LinkedList() // 링크드 리스트의 생성자입니다.
        {
            head = null; // 헤드 노드를 null로 초기화합니다.
        }

        public void Add(T data) // 링크드 리스트에 요소를 추가하는 메서드입니다.
        {
            LinkedListNode<T> newNode = new LinkedListNode<T>(data); // 새로운 노드를 생성합니다.
            if (head == null) // 헤드 노드가 null인 경우
            {
                head = newNode; // 새로운 노드를 헤드 노드로 지정합니다.
            }
            else // 헤드 노드가 null이 아닌 경우
            {
                LinkedListNode<T> current = head; // 현재 노드를 헤드 노드로 설정합니다.
                while (current.Next != null) // 현재 노드의 다음 노드가 null이 아닌 동안
                {
                    current = current.Next; // 다음 노드로 이동합니다.
                }
                current.Next = newNode; // 현재 노드의 다음 노드에 새로운 노드를 연결합니다.
            }
        }
    }

    public class Queue<T> // 제네릭 큐 클래스를 정의합니다.
    {
        private LinkedList<T> list; // 링크드 리스트를 저장하는 변수입니다.

        public Queue() // 큐의 생성자입니다.
        {
            list = new LinkedList<T>(); // 링크드 리스트를 초기화합니다.
        }

        // 큐에 요소를 추가하는 메서드입니다.
        public void Enqueue(T data)
        {
            list.Add(data); // 링크드 리스트에 요소를 추가합니다.
        }

        // 큐에서 요소를 제거하고 반환하는 메서드입니다.
        public T Dequeue()
        {
            T data = list.head.Data; // 헤드 노드의 데이터를 저장합니다.
            list.head = list.head.Next; // 헤드 노드를 다음 노드로 변경합니다.
            return data; // 저장한 데이터를 반환합니다.
        }

        // 큐가 비어 있는지 여부를 반환하는 메서드입니다.
        public bool IsEmpty()
        {
            return list.head == null; // 헤드 노드가 null이면 큐가 비어 있습니다.
        }

    }

    public class Stack<T> // 제네릭 스택 클래스를 정의합니다.
    {
        private Queue<T> queue; // 큐를 저장하는 변수입니다.
        private Queue<T> temp; // 임시 큐를 저장하는 변수입니다.

        public Stack() // 스택의 생성자입니다.
        {
            queue = new Queue<T>(); // 큐를 초기화합니다.
            temp = new Queue<T>(); // 임시 큐를 초기화합니다.
        }

        public void Push(T data) // 스택에 요소를 추가하는 메서드입니다.
        {
            // 큐에 있는 모든 요소를 임시 큐로 옮깁니다.
            while (!queue.IsEmpty())
            {
                temp.Enqueue(queue.Dequeue()); // 큐에서 요소를 꺼내 임시 큐에 추가합니다.
            }

            // 새로운 요소를 큐에 추가합니다.
            queue.Enqueue(data); // 새로운 요소를 큐에 추가합니다.

            // 임시 큐에 있는 모든 요소를 다시 큐로 옮겨서 순서를 복원합니다.
            while (!temp.IsEmpty())
            {
                queue.Enqueue(temp.Dequeue()); // 임시 큐에서 요소를 꺼내 큐에 추가합니다.
            }
        }

        public T Pop() // 스택에서 요소를 제거하고 반환하는 메서드입니다.
        {
            if (queue.IsEmpty()) // 큐가 비어 있는 경우
            {
                throw new InvalidOperationException("Stack is empty."); // 예외를 발생시킵니다.
            }
            return queue.Dequeue(); // 큐에서 요소를 제거하고 반환합니다.
        }
    }
}
