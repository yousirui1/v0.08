using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassshake : MonoBehaviour {
   Animator ani;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        ani.speed = 0;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
           
            ani.speed = 1;
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.43f);
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ani.speed=0;

            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        }
    }
}
