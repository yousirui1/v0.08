using UnityEngine;
using System.Collections;
using tpgm;

/**************************************
*FileName: Skill.cs
*User: ysr 
*Data: 2017/12/12
*Describe: 技能实例化后脚本，包括普攻
**************************************/

public class Skill : MonoBehaviour {
    
    private SkillVal skillVal; //技能的所以数值

	
	void Start()
	{
          //音效
        //this.GetComponent<AudioSource>().PlayOneShot(throwsound);
    }
    
    
    public void init(SkillVal skillVal)
    {

        this.skillVal = skillVal;
        Destroy(gameObject, skillVal.duration);
    }


	void Update()
	{
        this.transform.position += new Vector3(skillVal.speed, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == Tags.player)
        {
            other.GetComponent<ATKAndDamage>().TakeDamage(skillVal.hurt);
            Destroy(gameObject);
        }


        /* if()
         {

          switch (skillVal.aim)
         {

             case 1：
             {
                 if (collider.gameObject.tag == Tags.player)
                 {
                     other.GetComponent<ATKAndDamage>().TakeDamage(attack);
                 }
             }
             break;    
             case 2:
             {

             }
             }
         } */

    }

    void OnTriggerStay2D(Collider2D collider)
    {
        /*
        Debug.Log("OnTriggerEnter");
        int time = 0;
        time += Time.fixedDeltaTime;
         if(!)
        {
          switch (skillVal.aim)
          {

            case 1：
            {
                if (collider.gameObject.tag == Tags.player)
                {
                    other.GetComponent<ATKAndDamage>().TakeDamage(attack);
                }
            }
            break;    
            case 2:
            {

            }
          }
        }
        */
    }
    void OnTriggerExit2D(Collider2D collider)
    {

       
    }

    
   

   

}
