using UnityEngine;

public class SetCameraToCubeView : MonoBehaviour
{
    public Transform cube; // ť�� ������Ʈ�� Transform ������Ʈ�� ������ ����
    private Camera mainCamera; // ���� ī�޶� ������ ����
    public float moveSpeed = 8f; // �÷��̾� �̵� �ӵ�
    public float sensitivity = 0.001f; // ���콺 ����
    private float rotationX = 0f;
    private float rotationY = 0f; // ���콺 �¿� ȸ������ ������ ����
    public GameObject Camera1;

    public GameObject redDotPrefab; // ������ �� ������

    private GameObject redDot; // ������ ������ �� ������Ʈ�� ������ ����

    private void Start()
    {
        mainCamera = Camera.main; // ���� ���� �� ���� ī�޶� ã�Ƽ� �Ҵ�

        // ������ ���� ȭ�� ���߾ӿ� ����
        redDot = Instantiate(redDotPrefab, mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f)), Quaternion.identity);
    }

    private void Update()
    {
        mainCamera.transform.position = cube.position;

        // ť�갡 �ٶ󺸴� �������� ī�޶��� ȸ���� ����
        mainCamera.transform.rotation = Quaternion.LookRotation(cube.forward);

        // ���콺 �Է��� �����Ͽ� ī�޶��� ȸ������ ����
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // ���� ȸ�� ������ �����Ͽ� ������� ����

        rotationY += mouseX;

        // �÷��̾��� ȸ������ �����Ͽ� �þ߸� ������
        Camera1.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        // �÷��̾� �̵�
        float horizontalInput = Input.GetAxis("Horizontal"); // �¿� ȭ��ǥ Ű �Է�
        float verticalInput = Input.GetAxis("Vertical"); // ���� ȭ��ǥ Ű �Է�
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized; // �̵� ������ ����ȭ
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // ������ ���� ȭ�� ���߾ӿ� ����
        redDot.transform.position = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 0.5f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("GreenBox"))
            {
                // �浹�� ��ü�� "Wall" �±׸� ���� ���, �̵��� ����Ͽ� ���� ������� ���ϰ� ��
                transform.position -= moveDirection * moveSpeed * Time.deltaTime;
                break;
            }
        }
    }
}