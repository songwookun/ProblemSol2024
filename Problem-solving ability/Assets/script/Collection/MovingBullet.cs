using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBullet : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.right * 5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedBox"))
        {
            gameObject.SetActive(false);
        }
    }
}
