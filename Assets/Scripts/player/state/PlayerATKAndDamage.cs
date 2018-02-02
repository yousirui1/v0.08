using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**************************************
*FileName: PlayerATKAndDamage.cs
*User: ysr 
*Data: 2017/12/12
*Describe: 玩家攻击和伤害计算脚步
**************************************/

public class PlayerATKAndDamage : ATKAndDamage{

    //GameObject singleSwordGo;

    //攻击距离
    public float  attackRange = 0;

    //攻击力
    public float attack = 10;

    //捡到奖励物品音效
    public AudioClip pickeUpItem;

   void Update()
   {
       //if()

   } 

    //换技能
    void ChangeToSkill()
    {
       // singleSwordGo.SetActive(true);
        //dualSwordGo.SetActive(false);
       // gunGo.SetActive(false);
    }

}
