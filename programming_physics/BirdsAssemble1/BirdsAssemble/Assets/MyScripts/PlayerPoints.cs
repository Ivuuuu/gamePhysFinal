using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    /// <summary>
    /// If the bird touches any object
    /// call AddPoints() and add 200 to score.
    /// </summary>
    /// <param name="bird"></param>
    private void OnCollisionEnter2D(Collision2D bird)
    {
        if(bird.gameObject.tag == "bird")
        {
            Debug.Log(bird.gameObject.tag + "_hit_" + gameObject);
            AddPoints();
        }
    }
    void AddPoints()
    {
        Score.score += 200;
    }
}
