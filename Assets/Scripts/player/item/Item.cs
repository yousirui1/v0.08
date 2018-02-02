using UnityEngine;
using System.Collections;
using tpgm;

public class Item : MonoBehaviour
{
   
    private ItemVal item;
    public void init(ItemVal item)
    {
        this.item = item;
    }



    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == Tags.player)
        {
            other.GetComponent<ATKAndDamage>().TakeItem(item);
            Destroy(gameObject);
        }
    }



  }