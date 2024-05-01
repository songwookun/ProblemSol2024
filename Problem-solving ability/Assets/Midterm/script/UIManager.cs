using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int haskeys = 0;
    public Text keyText; 
   
    void Start()
    {
        UpdateItemCount();
    }

    void Update()
    {
        UpdateItemCount();
    }

    void UpdateItemCount()
    {
        if (haskeys != ItemKeeper.haskeys) 
        {
            keyText.text = ItemKeeper.haskeys.ToString(); 
            haskeys = ItemKeeper.haskeys; 
        }
    }
}
