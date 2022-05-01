using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{
    /// <summary>
    /// declare public button and gameobject variables
    /// to hold Lock and lockImg.
    /// </summary>
    public Button Level;
    public GameObject lockImg;

    //private bools for true or false.
    private bool On = true, Off = false;
    

    /// <summary>
    /// If level button is interactable then
    /// turn off lock image. If not then stay on.
    /// </summary>
    private void Update()
    {
        
        if(Level.interactable == true)
        {
            UnlockLevel();
        }
        
    }
    void UnlockLevel()
    {
        
        if (lockImg.activeInHierarchy == true)
        {
            lockImg.SetActive(Off);
            Debug.Log("Settings: OFF");
        }

       
    }
   
}
