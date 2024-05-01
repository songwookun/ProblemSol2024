using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuadTreeDrawer : MonoBehaviour
{
    public int depth = 3; // 쿼드 트리의 깊이 설정
    public GameObject TargetObject;
    public GameObject GameObjectContainer; // GameObject를 담을 부모 객체
    public QuadTreeNode root; // 쿼드 트리 루트 노드
    public int ObjectNumber;

#if UNITY_EDITOR
    [CustomEditor(typeof(QuadTreeDrawer))]
    public class QuadTreeDrawerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            QuadTreeDrawer drawer = (QuadTreeDrawer)target;
            if (GUILayout.Button("Generate Objects"))
            {
                drawer.GenerateObjects();
            }
        }
    }
#endif


    public void GenerateObjects()
    {
        // 기존에 있는 오브젝트들 제거
        ClearObjects();

        // GameObjectContainer 생성
        GameObjectContainer = new GameObject("GameObject");

        // 루트 노드 생성
        root = new QuadTreeNode(new Bounds(transform.position,
                                     new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)));
        root.Split(); // 쿼드 트리 분할

        // 스페어 생성 및 쿼드 트리에 추가
        Vector3[] objectPositions = new Vector3[ObjectNumber]; // 위치 배열 생성

        // 먼저 모든 위치를 계산
        for (int i = 0; i < ObjectNumber; i++)
        {
            objectPositions[i] = new Vector3(
                Random.Range(root.boundary.min.x, root.boundary.max.x),
                Random.Range(root.boundary.min.y, root.boundary.max.y),
                Random.Range(root.boundary.min.z, root.boundary.max.z)
            );
        }

        // 계산된 위치를 사용하여 오브젝트를 생성하고 쿼드 트리에 추가
        for (int i = 0; i < ObjectNumber; i++)
        {
            GameObject newObject = Instantiate(TargetObject, objectPositions[i], Quaternion.identity, GameObjectContainer.transform); // GameObjectContainer의 자식으로 생성
            newObject.tag = "RedCube";
            newObject.AddComponent<enemy>();

            // 생성된 오브젝트의 위치가 쿼드 트리 영역 내에 있는지 확인하고 영역 내에 있지 않으면 추가하지 않음
            if (root.boundary.Contains(newObject.transform.position))
            {
                // 생성된 스페어를 쿼드 트리에 추가
                root.Insert(newObject);
            }
            else
            {
                DestroyImmediate(newObject);  // 영역 밖에 생성된 오브젝트는 제거
            }
        }
    }

    // 쿼드 트리에서 생성된 모든 오브젝트 제거
    void ClearObjects()
    {
        if (root != null)
        {
            foreach (var obj in root.objects)
            {
                Destroy(obj);
            }
        }

        // GameObjectContainer 제거
        if (GameObjectContainer != null)
        {
            DestroyImmediate(GameObjectContainer);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (root != null)
            DrawQuadTree(root);
    }

    private void DrawQuadTree(QuadTreeNode node)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(node.boundary.center, node.boundary.size);

        // 자식 노드에 대해 재귀적으로 그립니다.
        if (node.children != null)
        {
            foreach (var child in node.children)
            {
                if (child != null)
                {
                    DrawQuadTree(child);
                }
            }
        }
    }
#endif

    // 쿼드 트리 노드 클래스 정의
    public class QuadTreeNode
    {
        public Bounds boundary; // 노드의 영역
        public GameObject[] objects; // 해당 영역에 속한 객체들
        public QuadTreeNode[] children; // 자식 노드들

        public QuadTreeNode(Bounds boundary)
        {
            this.boundary = boundary;
            objects = new GameObject[0];
            children = null;
        }

        // 객체를 노드에 추가하는 메서드
        public void Insert(GameObject obj)
        {
            // 객체가 영역에 속하지 않으면 종료
            if (!boundary.Contains(obj.transform.position))
                return;

            // 자식 노드가 없거나 최소 크기 이하면 현재 노드에 추가
            if (children == null || boundary.size.x / 2f < 1)
            {
                ArrayUtility.Add(ref objects, obj);
            }
            else
            {
                // 자식 노드에 삽입
                foreach (var child in children)
                {
                    child.Insert(obj);
                }
            }
        }

        // 영역을 분할하는 메서드
        public void Split()
        {
            if (children != null)
                return; // 이미 분할된 경우 종료

            float subWidth = boundary.size.x / 2f;
            float subHeight = boundary.size.z / 2f;
            float x = boundary.center.x;
            float z = boundary.center.z;

            children = new QuadTreeNode[4];
            children[0] = new QuadTreeNode(new Bounds(new Vector3(x - subWidth / 2, boundary.center.y, z - subHeight / 2), new Vector3(subWidth, boundary.size.y, subHeight)));
            children[1] = new QuadTreeNode(new Bounds(new Vector3(x + subWidth / 2, boundary.center.y, z - subHeight / 2), new Vector3(subWidth, boundary.size.y, subHeight)));
            children[2] = new QuadTreeNode(new Bounds(new Vector3(x - subWidth / 2, boundary.center.y, z + subHeight / 2), new Vector3(subWidth, boundary.size.y, subHeight)));
            children[3] = new QuadTreeNode(new Bounds(new Vector3(x + subWidth / 2, boundary.center.y, z + subHeight / 2), new Vector3(subWidth, boundary.size.y, subHeight)));
        }
    }
}
