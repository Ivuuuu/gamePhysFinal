using UnityEngine;
using System.Collections;

public class Pig : MonoBehaviour
{

    public float Health = 150f;
    public Sprite SpriteShownWhenHurt;
    private float ChangeSpriteHealth;
    
    void Start()
    {
        ChangeSpriteHealth = Health - 30f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;

        
        if (col.gameObject.tag == "Bird")
        {
            GetComponent<AudioSource>().volume = SoundController.Singleton.EffectVolume.Value * SoundController.Singleton.MasterVolume.Value;
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
        else 
        {
            float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
            Health -= damage;
            if (damage >= 10)
                GetComponent<AudioSource>().volume = SoundController.Singleton.EffectVolume.Value * SoundController.Singleton.MasterVolume.Value;
                GetComponent<AudioSource>().Play();

            if (Health < ChangeSpriteHealth)
            {
                GetComponent<SpriteRenderer>().sprite = SpriteShownWhenHurt;
            }
            if (Health <= 0) Destroy(this.gameObject);
        }
    }
}
