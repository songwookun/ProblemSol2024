using UnityEngine;

public class SetCameraToCubeView : MonoBehaviour
{
    public Transform cube; // 큐브 오브젝트의 Transform 컴포넌트를 참조할 변수
    private Camera mainCamera; // 메인 카메라를 참조할 변수
    public float moveSpeed = 8f; // 플레이어 이동 속도
    public float sensitivity = 0.001f; // 마우스 감도
    private float rotationX = 0f;
    private float rotationY = 0f; // 마우스 좌우 회전값을 저장할 변수
    public GameObject Camera1;

    public GameObject redDotPrefab; // 빨간색 점 프리팹

    private GameObject redDot; // 생성된 빨간색 점 오브젝트를 참조할 변수

    private void Start()
    {
        mainCamera = Camera.main; // 게임 시작 시 메인 카메라를 찾아서 할당

        // 빨간색 점을 화면 정중앙에 생성
        redDot = Instantiate(redDotPrefab, mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f)), Quaternion.identity);
    }

    private void Update()
    {
        mainCamera.transform.position = cube.position;

        // 큐브가 바라보는 방향으로 카메라의 회전을 설정
        mainCamera.transform.rotation = Quaternion.LookRotation(cube.forward);

        // 마우스 입력을 감지하여 카메라의 회전값을 변경
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // 상하 회전 각도를 제한하여 오버헤드 방지

        rotationY += mouseX;

        // 플레이어의 회전값을 적용하여 시야를 움직임
        Camera1.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        // 플레이어 이동
        float horizontalInput = Input.GetAxis("Horizontal"); // 좌우 화살표 키 입력
        float verticalInput = Input.GetAxis("Vertical"); // 상하 화살표 키 입력
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized; // 이동 방향을 정규화
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 빨간색 점을 화면 정중앙에 고정
        redDot.transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 0.5f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("GreenBox"))
            {
                // 충돌한 객체가 "Wall" 태그를 가진 경우, 이동을 취소하여 벽을 통과하지 못하게 함
                transform.position -= moveDirection * moveSpeed * Time.deltaTime;
                break;
            }
        }
    }
}