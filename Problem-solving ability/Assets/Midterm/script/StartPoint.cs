using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public GameObject playerPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void SpawnPlayer()
    {

        playerPrefab.SetActive(true);
    }
}
