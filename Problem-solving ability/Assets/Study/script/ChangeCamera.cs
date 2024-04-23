using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    private GameObject root; // ��Ʈ ������Ʈ
    private Camera thisCamera; // ���� ī�޶�

    private void Start()
    {
        thisCamera = GetComponent<Camera>(); // ���� ��ũ��Ʈ�� ������ ī�޶� �����ɴϴ�.
        root = GameObject.Find("GameObject"); // ��Ʈ ������Ʈ�� ã�Ƽ� �Ҵ��մϴ�.
        AdjustFrustumSize(); // ī�޶� �þ߸� �����ϴ� �Լ��� ȣ���մϴ�.
    }

    private void Update()
    {
        // ī�޶� ��Ʈ ������Ʈ�� ���� ��� ������ ǥ���ϰ� �Լ��� �����մϴ�.
        if (thisCamera == null)
        {
            Debug.LogError("ī�޶� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }
        if (root == null)
        {
            Debug.LogError("��Ʈ ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // ��Ʈ ������Ʈ�� �� �ڽĿ� ���� ó���մϴ�.
        foreach (Transform child in root.transform)
        {
            // ������Ʈ�� ī�޶� �þ� �ȿ� �ִ��� Ȯ���մϴ�.
            bool insideFrustum = IsObjectInFrustum(child.gameObject);
            // ������Ʈ�� Ȱ��ȭ ���¸� ī�޶� �þ� �ȿ� �ִ����� ���� �����մϴ�.
            child.gameObject.SetActive(insideFrustum);
        }
    }

    // ������Ʈ�� ī�޶� �þ� �ȿ� �ִ��� Ȯ���ϴ� �Լ�
    bool IsObjectInFrustum(GameObject obj)
    {
        // ī�޶� �þ߸� �����ϴ� ��� �迭�� �����ɴϴ�.
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
        Renderer renderer = obj.GetComponent<Renderer>();

        // Renderer ������Ʈ�� ������ false ��ȯ
        if (renderer == null)
        {
            return false;
        }

        // ������Ʈ�� ��� �������� ī�޶� �þ� �ȿ� ������ true ��ȯ, �ϳ��� �ۿ� ������ false ��ȯ
        foreach (Vector3 vertex in GetObjectVertices(renderer))
        {
            bool insideFrustum = true;
            foreach (Plane plane in planes)
            {
                if (plane.GetDistanceToPoint(vertex) < 0)
                {
                    insideFrustum = false;
                    break;
                }
            }
            if (insideFrustum)
            {
                return true;
            }
        }

        return false;
    }

    // ������Ʈ�� �������� �������� �Լ�
    List<Vector3> GetObjectVertices(Renderer renderer)
    {
        List<Vector3> vertices = new List<Vector3>();
        Bounds bounds = renderer.bounds;
        // ������Ʈ�� �������� ����Ͽ� ����Ʈ�� �߰��մϴ�.
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z));
        vertices.Add(bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z));
        return vertices;
    }

    // ī�޶� �þ߸� �����ϴ� �Լ�
    void AdjustFrustumSize()
    {
        float targetAspect = 1920f / 1080f; // ����Ƽ ����â�� ���� �� ���� ����
        float currentAspect = (float)Screen.width / Screen.height;

        // ���� ���� ���� �� ���� ������ Ÿ�ٰ� �ٸ��ٸ� ���� �Ǵ� ���� �� ���� ���� �������� ���������� ����
        if (currentAspect < targetAspect)
        {
            float targetWidth = thisCamera.orthographicSize * 2 * targetAspect;
            thisCamera.aspect = targetAspect;
            thisCamera.orthographicSize = targetWidth / 4;
        }
        else
        {
            thisCamera.aspect = targetAspect;
        }
    }
}
