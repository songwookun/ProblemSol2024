using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuadTreeDrawer : MonoBehaviour
{
    public int depth = 3;
    public GameObject TargetObject;
    public GameObject GameObjectContainer;
    public QuadTreeNode root;
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
        ClearObjects();

        GameObjectContainer = new GameObject("GameObject");

        root = new QuadTreeNode(new Bounds(transform.position,
                                     new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)));
        root.Split();

        Vector3[] objectPositions = new Vector3[ObjectNumber];

        for (int i = 0; i < ObjectNumber; i++)
        {
            objectPositions[i] = new Vector3(
                Random.Range(root.boundary.min.x, root.boundary.max.x),
                Random.Range(root.boundary.min.y, root.boundary.max.y),
                Random.Range(root.boundary.min.z, root.boundary.max.z)
            );
        }

        for (int i = 0; i < ObjectNumber; i++)
        {
            GameObject newObject = Instantiate(TargetObject, objectPositions[i], Quaternion.identity, GameObjectContainer.transform);

            if (root.boundary.Contains(newObject.transform.position))
            {
                root.Insert(newObject);
            }
            else
            {
                DestroyImmediate(newObject);
            }
        }
    }

    void ClearObjects()
    {
        if (root != null)
        {
            foreach (var obj in root.objects)
            {
                Destroy(obj);
            }
        }

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

    public class QuadTreeNode
    {
        public Bounds boundary;
        public GameObject[] objects;
        public QuadTreeNode[] children;

        public QuadTreeNode(Bounds boundary)
        {
            this.boundary = boundary;
            objects = new GameObject[0];
            children = null;
        }

        public void Insert(GameObject obj)
        {
            if (!boundary.Contains(obj.transform.position))
                return;

            if (children == null || boundary.size.x / 2f < 1)
            {
                ArrayUtility.Add(ref objects, obj);
            }
            else
            {
                foreach (var child in children)
                {
                    child.Insert(obj);
                }
            }
        }

        public void Split()
        {
            if (children != null)
                return;

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