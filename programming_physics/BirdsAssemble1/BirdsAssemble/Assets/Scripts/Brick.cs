using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{

    public float Health = 70f;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;
        float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10; 
        if (damage >= 10)
            GetComponent<AudioSource>().Play();
        Health -= damage;
        if (Health <= 0) Destroy(this.gameObject);
    }
}
