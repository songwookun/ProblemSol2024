using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader : MonoBehaviour
{
    private Renderer rend;

    private void Reset()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial.color = Color.yellow;
    }
}