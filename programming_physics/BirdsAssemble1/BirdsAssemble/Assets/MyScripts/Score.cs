using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    /// <summary>
    /// Make a text object and static int variable to hold 
    /// string and value.
    /// </summary>
   
    public Text scoreTxt;
    public static int score = 0;

    void Awake()
    {
        //convert score to string and store it in scoreTxt.text
        //upon wake...
        scoreTxt.text = ("Score: ") + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //and update the score everytime it changes.
        scoreTxt.text = ("Score: ") + score.ToString();

    }

   
}
