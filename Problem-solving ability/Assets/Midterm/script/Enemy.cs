using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 patrolRange; // 각 Enemy 오브젝트의 고유한 돌아다닐 범위의 크기
    Transform player; // 플레이어
    Camera enemyCamera;

    public Vector3 targetPosition; // 목표 위치

    float moveSpeed = 3f;
    float elapsedTime = 0f;
    float turnDuration = 3f;
    bool isTurning = false;

    void Start()
    {
        enemyCamera = GetComponentInChildren<Camera>();
        player = GameObject.FindWithTag("Player").transform;
        SetNewTargetPosition(); // 처음 시작할 때 목표 위치를 설정
    }

    void Update()
    {
        // 플레이어가 시야에 있다면
        if (IsPlayerInCameraView())
        {
            // 플레이어를 쫒아가기
            ChasePlayer();

            // 만약 시야 안에 플레이어가 없다면
            if (!IsPlayerInCameraView())
            {
                isTurning = true;
            }
        }
        else // 플레이어가 시야에 없다면
        {
            if (isTurning)
            {
                TurnAround();
            }
            else
            {
                // 랜덤한 위치로 이동
                Patrol();
            }
        }
    }

    // enemyCamera의 프러스텀 내에 플레이어가 있는지 확인하는 메서드
    bool IsPlayerInCameraView()
    {
        Bounds playerBounds = player.GetComponent<Collider>().bounds;
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(enemyCamera);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, playerBounds);
    }

    // 새로운 목표 위치 설정

    void SetNewTargetPosition()
    {
        float x = Random.Range(-patrolRange.x / 2, patrolRange.x / 2);
        float z = Random.Range(-patrolRange.z / 2, patrolRange.z / 2);
        targetPosition = transform.position + new Vector3(x, 0, z);
    }

    // 특정 범위 내를 돌아다니는 메서드
    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
        transform.LookAt(targetPosition);
    }

    // 플레이어를 쫒아가는 메서드
    void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * moveSpeed);
        transform.LookAt(player.position);
    }

    // 일정 시간 동안 좌회전, 우회전하는 메서드
    void TurnAround()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime <= turnDuration)
        {
            transform.Rotate(0f, -90f / turnDuration * Time.deltaTime, 0f);
        }
        else if (elapsedTime > turnDuration && elapsedTime <= turnDuration * 2)
        {
            transform.Rotate(0f, 180f / turnDuration * Time.deltaTime, 0f);
        }

        if (elapsedTime >= turnDuration * 2)
        {
            isTurning = false;
            elapsedTime = 0f;
        }
    }

    // wall 태그에 닿았을 때 반대 방향으로 돌아서 이동
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (gameObject.CompareTag("Enemyeye"))
            {
                // Enemyeye 태그를 가진 오브젝트의 경우
                transform.Rotate(0f, 70f, 0f); // Enemy의 방향을 45도 회전
            }
           

            // 새로운 목표 위치 설정
            SetNewTargetPosition();
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, patrolRange);
    }

}
