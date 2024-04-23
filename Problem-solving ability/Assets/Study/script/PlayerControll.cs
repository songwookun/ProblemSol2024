using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject BulletPrefab; //총알 프리팹
    public Transform reddot; //총알이 발사하는 위치 
    public Queue<GameObject> queue; //총알을 관리 큐

    // Start is called before the first frame update
    void Start()
    {
        queue = new Queue<GameObject>(); //큐 초기화
        for (int i = 0; i < 10; i++) //총알 10발을 미리 생성후 큐에 넣음
        {
            GameObject obj = Instantiate(BulletPrefab); // 총알 프리팹을 복제하여 생성
            obj.GetComponent<Bullet>().Init(reddot.position, queue, reddot.transform.forward);//Bullet스크립트 참조, Init 메서드 호출 위치 방향 설정
            queue.Enqueue(obj); //위에서 지정한 총알 객체를 큐에 추가
            obj.SetActive(false);// 큐에 들어간 객체는 비활성화
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽 클릭했을때
        {
            if (queue.Count > 0) //총알이 1개라도 남았는지 확인
            {
                GameObject bullet = queue.Dequeue(); // 큐에서 총알을 꺼냄
                bullet.GetComponent<Bullet>().Init(reddot.position, queue, reddot.transform.forward);//Bullet스크립트 참조, Init 메서드 호출 위치 방향 설정
                bullet.SetActive(true); //총알 활성화
            }
            else //총알이 없을 경우
            {
                Debug.Log("총알이 모두 소진되었습니다.");
            }
        }
    }
}
