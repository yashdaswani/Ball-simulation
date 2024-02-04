using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballSoundPlay : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "ball1" || collision.collider.tag == "ball2" || collision.collider.tag == "ball3" || collision.collider.tag == "ball4" || collision.collider.tag == "ball5" || collision.collider.tag == "ball6")
        {
            //gameManager.instance.SoundOnBall();
        }
    }
    private void Start()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }
}
