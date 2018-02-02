using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/**************************************
*FileName: ATKAndDamage.cs
*User: ysr 
*Data: 2017/12/12
*Describe: 收到伤害的公共接口
**************************************/

public class HeadState : MonoBehaviour
{
    private Image sliderHpBar;
    private Image sliderMpBar;

    private Image sliderExp;


    private Text txLevel;

    private PlayerATKAndDamage playerATK;//主角的攻击和伤害脚本s

    void Awake()
    {
        //获取血条

        sliderHpBar = transform.GetChild(2).GetComponent<Image>();
        sliderMpBar = transform.GetChild(4).GetComponent<Image>();
        sliderExp = transform.GetChild(6).GetComponent<Image>();

        txLevel = transform.GetChild(7).GetComponent<Text>();

        playerATK = gameObject.transform.parent.GetComponent<PlayerATKAndDamage>();
        //GameObject.FindWithTag(Tags.player).GetComponent<PlayerATKAndDamage>();
    }


    private void Update()
    {
        //设置状态数值

        sliderHpBar.fillAmount = playerATK.hp / 100;
        sliderMpBar.fillAmount = playerATK.mp / 100;
        sliderExp.fillAmount = playerATK.exp / (100* playerATK.level);
        if(sliderExp.fillAmount == 1.0f)
        {
            playerATK.level++;
            playerATK.exp = 0;
        }

        txLevel.text = ""+ playerATK.level;
       
        //levelnum.text = list[0].id.ToString();
        //level.fillAmount = (float)curlevel / list[1].res;
    }


  
   

   
}