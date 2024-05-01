using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuadTreeDrawer : MonoBehaviour
{
    public int depth = 3; // ���� Ʈ���� ���� ����
    public GameObject TargetObject;
    public GameObject GameObjectContainer; // GameObject�� ���� �θ� ��ü
    public QuadTreeNode root; // ���� Ʈ�� ��Ʈ ���
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
        // ������ �ִ� ������Ʈ�� ����
        ClearObjects();

        // GameObjectContainer ����
        GameObjectContainer = new GameObject("GameObject");

        // ��Ʈ ��� ����
        root = new QuadTreeNode(new Bounds(transform.position,
                                     new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)));
        root.Split(); // ���� Ʈ�� ����

        // ����� ���� �� ���� Ʈ���� �߰�
        Vector3[] objectPositions = new Vector3[ObjectNumber]; // ��ġ �迭 ����

        // ���� ��� ��ġ�� ���
        for (int i = 0; i < ObjectNumber; i++)
        {
            objectPositions[i] = new Vector3(
                Random.Range(root.boundary.min.x, root.boundary.max.x),
                Random.Range(root.boundary.min.y, root.boundary.max.y),
                Random.Range(root.boundary.min.z, root.boundary.max.z)
            );
        }

        // ���� ��ġ�� ����Ͽ� ������Ʈ�� �����ϰ� ���� Ʈ���� �߰�
        for (int i = 0; i < ObjectNumber; i++)
        {
            GameObject newObject = Instantiate(TargetObject, objectPositions[i], Quaternion.identity, GameObjectContainer.transform); // GameObjectContainer�� �ڽ����� ����
            newObject.tag = "RedCube";
            newObject.AddComponent<enemy>();

            // ������ ������Ʈ�� ��ġ�� ���� Ʈ�� ���� ���� �ִ��� Ȯ���ϰ� ���� ���� ���� ������ �߰����� ����
            if (root.boundary.Contains(newObject.transform.position))
            {
                // ������ ���� ���� Ʈ���� �߰�
                root.Insert(newObject);
            }
            else
            {
                DestroyImmediate(newObject);  // ���� �ۿ� ������ ������Ʈ�� ����
            }
        }
    }

    // ���� Ʈ������ ������ ��� ������Ʈ ����
    void ClearObjects()
    {
        if (root != null)
        {
            foreach (var obj in root.objects)
            {
                Destroy(obj);
            }
        }

        // GameObjectContainer ����
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

        // �ڽ� ��忡 ���� ��������� �׸��ϴ�.
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

    // ���� Ʈ�� ��� Ŭ���� ����
    public class QuadTreeNode
    {
        public Bounds boundary; // ����� ����
        public GameObject[] objects; // �ش� ������ ���� ��ü��
        public QuadTreeNode[] children; // �ڽ� ����

        public QuadTreeNode(Bounds boundary)
        {
            this.boundary = boundary;
            objects = new GameObject[0];
            children = null;
        }

        // ��ü�� ��忡 �߰��ϴ� �޼���
        public void Insert(GameObject obj)
        {
            // ��ü�� ������ ������ ������ ����
            if (!boundary.Contains(obj.transform.position))
                return;

            // �ڽ� ��尡 ���ų� �ּ� ũ�� ���ϸ� ���� ��忡 �߰�
            if (children == null || boundary.size.x / 2f < 1)
            {
                ArrayUtility.Add(ref objects, obj);
            }
            else
            {
                // �ڽ� ��忡 ����
                foreach (var child in children)
                {
                    child.Insert(obj);
                }
            }
        }

        // ������ �����ϴ� �޼���
        public void Split()
        {
            if (children != null)
                return; // �̹� ���ҵ� ��� ����

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
