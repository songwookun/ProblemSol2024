using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Key // enum ��� �̸��� �빮�ڷ� �����ϴ� ���� �����Դϴ�.
}

public class Item : MonoBehaviour
{
    public ItemType type;
    public int count = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == ItemType.Key) // ��� �̸� ����
            {
                ItemKeeper.haskeys += 1;
            }

            gameObject.GetComponent<Collider>().enabled = false;
            Rigidbody itemBody = GetComponent<Rigidbody>();
            itemBody.useGravity = true; // Rigidbody�� gravity ����
            itemBody.AddForce(new Vector3(0, 6, 0), ForceMode.Impulse);
            Destroy(gameObject, 0.5f);
        }
    }
}
