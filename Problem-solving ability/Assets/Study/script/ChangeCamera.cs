using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    private GameObject root; // 루트 오브젝트
    private Camera thisCamera; // 현재 카메라

    private void Start()
    {
        thisCamera = GetComponent<Camera>(); // 현재 스크립트가 부착된 카메라를 가져옵니다.
        root = GameObject.Find("GameObject"); // 루트 오브젝트를 찾아서 할당합니다.
        AdjustFrustumSize(); // 카메라 시야를 조정하는 함수를 호출합니다.
    }

    private void Update()
    {
        // 카메라나 루트 오브젝트가 없는 경우 오류를 표시하고 함수를 종료합니다.
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

        // 루트 오브젝트의 각 자식에 대해 처리합니다.
        foreach (Transform child in root.transform)
        {
            // 오브젝트가 카메라 시야 안에 있는지 확인합니다.
            bool insideFrustum = IsObjectInFrustum(child.gameObject);
            // 오브젝트의 활성화 상태를 카메라 시야 안에 있는지에 따라 설정합니다.
            child.gameObject.SetActive(insideFrustum);
        }
    }

    // 오브젝트가 카메라 시야 안에 있는지 확인하는 함수
    bool IsObjectInFrustum(GameObject obj)
    {
        // 카메라 시야를 정의하는 평면 배열을 가져옵니다.
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
        Renderer renderer = obj.GetComponent<Renderer>();

        // Renderer 컴포넌트가 없으면 false 반환
        if (renderer == null)
        {
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

    // 오브젝트의 꼭짓점을 가져오는 함수
    List<Vector3> GetObjectVertices(Renderer renderer)
    {
        List<Vector3> vertices = new List<Vector3>();
        Bounds bounds = renderer.bounds;
        // 오브젝트의 꼭짓점을 계산하여 리스트에 추가합니다.
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

    // 카메라 시야를 조정하는 함수
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
