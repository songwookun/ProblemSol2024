using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialInFrustum : MonoBehaviour
{
    private GameObject root;
    private Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
        root = GameObject.Find("GameObject");
        AdjustFrustumSize();

    }

    private void Update()
    {
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

        foreach (Transform child in root.transform)
        {
            bool insideFrustum = IsObjectInFrustum(child.gameObject);
            child.gameObject.SetActive(insideFrustum);
        }
    }

    bool IsObjectInFrustum(GameObject obj)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer == null)
        {
            // Renderer ������Ʈ�� ������ false ��ȯ
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

    List<Vector3> GetObjectVertices(Renderer renderer)
    {
        List<Vector3> vertices = new List<Vector3>();
        Bounds bounds = renderer.bounds;
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
