using UnityEngine;
using System.Collections;
using System;
using tpgm;

/**************************************
*FileName: ATKAndDamage.cs
*User: ysr 
*Data: 2017/12/12
*Describe: 收到伤害的公共接口
**************************************/

public class ATKAndDamage : MonoBehaviour{
    public float hp  = 100;
    public float mp = 100;
    public float exp = 0;
    public int level = 1;

    public float score = 0;
    //普通攻击伤害值
    public float normalAttack = 50;

    //攻击距离
    public float attackDistance = 1;
    //动画控制器    
    private Animator animator;
    //死亡音效
    public AudioClip death;


    //申请为受保护的方便子类调用
    protected void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    //捡到资源物体
    public virtual void TakeItem(ItemVal item)
    {
        //造成伤害前判断人物是否死亡
        if(this.hp > 0)
        {
            this.hp += item.rehp;
            this.score += item.score;
            this.exp += item.exp;
        }
   }

     //收到伤害
    public virtual void TakeDamage(float hp)
    {
        //造成伤害前判断人物是否死亡
        if(this.hp > 0)
        {
            this.hp -= hp;
        }
        //造成伤害后是否死亡
        if(this.hp > 0)
        {
            //判断是其他
            if(this.tag == Tags.otherplayer)
            {
                animator.SetTrigger("Damage");
            }
            //还是主角
            else if(this.tag == Tags.player)
            {
                animator.SetTrigger("");
            }

        }
        else
        {
            animator.SetTrigger("Dead");
            
            if(this.tag == Tags.player)
            {
                //显示结束界面   
                //死亡后爆装备
                //SpawnAwardItem();

                //Remove移除角色
                Destroy(this.gameObject, 1f);
            }

            if(this.tag == Tags.otherplayer)
            {
            }
        }

        //实例化攻击特效
        if(this.tag == Tags.bullet)
        {
            GameObject.Instantiate(Resources.Load("HitBoss"), this.transform.position + Vector3.up, this.transform.rotation);
        }
    }


    //死亡后爆装备
    void SpawnAwardItem()
    {
        //随机生成物品
        double score  = Math.Floor(this.score * 0.2);
        double max = Math.Floor(score / 50);
        double mid = Math.Floor ((score %50)/10);
        double min = Math.Floor ((score %50)%10);

        for(int i =0; i<max;i++)
        {
            int rid = UnityEngine.Random.Range(0,90);
            GameObject.Instantiate(Resources.Load("Item_DualSword"), this.transform.position + Vector3.up, Quaternion.identity);
        }

        for(int i =0; i< mid; i++ )
        {
            int rid = UnityEngine.Random.Range(0,90);
            GameObject.Instantiate(Resources.Load("Item_DualSword"), this.transform.position + Vector3.up, Quaternion.identity);
        }

         for(int i =0; i< min;i++)
        {
            int rid = UnityEngine.Random.Range(0,90);
            GameObject.Instantiate(Resources.Load("Item_DualSword"), this.transform.position + Vector3.up, Quaternion.identity);
        }

    }

}
