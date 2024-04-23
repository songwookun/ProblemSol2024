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
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
            return;
        }
        if (root == null)
        {
            Debug.LogError("루트 오브젝트를 찾을 수 없습니다.");
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
            // Renderer 컴포넌트가 없으면 false 반환
            return false;
        }

        // 오브젝트의 모든 꼭짓점이 카메라 시야 안에 있으면 true 반환, 하나라도 밖에 있으면 false 반환
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
        float targetAspect = 1920f / 1080f; // 유니티 게임창의 가로 및 세로 비율
        float currentAspect = (float)Screen.width / Screen.height;

        // 만약 현재 가로 및 세로 비율이 타겟과 다르다면 가로 또는 세로 중 작은 쪽을 기준으로 프러스텀을 조정
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
