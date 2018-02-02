using UnityEngine;
using System.Collections;
using SimpleJson;
public class JoyControl : MonoBehaviour {
    public RectTransform rect_Viewport;
    //public RectTransform rect_Joy;//将获取坐标作为摇杆键值
    public int iR;

    //虚拟摇杆的值-1 ~1
    public int vertical { set; get; }

    public int horizontal { set; get; }

    //技能和普攻
    private int skill = 0;

    private int fire = 0;
	
	private GameObject obj;

	public bool isMove = false;

    
	
    void Start()
    {
        iR = (int)rect_Viewport.sizeDelta.x / 2;
    }

	//手指结束滑动
	public void On_End()
	{
		isMove = false;
	}

	//手指开始滑动
	public void On_Begin()
	{
		isMove = true;
	}
	
	//计算滑动的方向
    public void On_Move(RectTransform rect)
    {

      	if (rect.anchoredPosition.magnitude > iR)
      	{//将摇杆限制在 半径 iR 以内
            rect.anchoredPosition = rect.anchoredPosition.normalized * iR;
      	}
		if(isMove)
		{
            if (rect.anchoredPosition.x > 3)
            {
                horizontal = 1;
            }
            else if(rect.anchoredPosition.x <-3)
            {
                horizontal = -1;
            }
            else{
                horizontal = 0;
            }

            if (rect.anchoredPosition.y > 3)
            {
                vertical = 1;
            }
            else if(rect.anchoredPosition.y <-3)
            {
                vertical = -1;
            }
            else
            {
                vertical = 0;
            }
		
		}		
		else
		{	
			vertical = 0;
			horizontal = 0;
            
        }
      //  Debug.Log("vertical"+vertical+"horizontal"+horizontal);
   }

    public void getPos()
    {

    }

    public JsonObject jsonPos;
    //事件发送给服务器
    void FixedUpdate()
    {
  
        
        
    }



#if false
    public void onClickButtonA()
    {
        Debug.Log("onClickButtonA"  );
    }

    public void onClickButtonB()
    {
        Debug.Log("onClickButtonB"  );
    }

    public void PickeUpAward(ItemType type)
    {//主角拾取奖励物品
        AudioSource.PlayClipAtPoint(pickeUpItem, this.transform.position, 1f);//播放捡到奖励物品的音效
        if (type == ItemType.DualSword)//捡到双刃剑切换的奖励物品，切换为双刃剑
        {
            ChangeToDualSword();
        }
        else if (type == ItemType.Gun)//捡到枪切换的奖励物品，切换为枪
        {
            ChangeToGun();
        }
    }


    void ChangeToDualSword()
    {//切换为双刃剑
        singleSwordGo.SetActive(false);
        dualSwordGo.SetActive(true);
        gunGo.SetActive(false);
        dualSwordTimer = exitTime;
        gunTimer = 0;
        UIAttackButtonChang._instance.ChangeToTwoAttackButton();//攻击按钮变成两个
    }
    void ChangeToGun()
    {//切换为枪
        singleSwordGo.SetActive(false);
        dualSwordGo.SetActive(false);
        gunGo.SetActive(true);
        gunTimer = exitTime;
        dualSwordTimer = 0;
        UIAttackButtonChang._instance.ChangeToOneAttackButton();//攻击按钮变成一个
    }
    void ChangeToSingleSword()
    { //切换为单刃剑
        singleSwordGo.SetActive(true);
        dualSwordGo.SetActive(false);
        gunGo.SetActive(false);
        gunTimer = 0;
        dualSwordTimer = 0;
        UIAttackButtonChang._instance.ChangeToTwoAttackButton(); //攻击按钮变成两个
    }

    // Update is called once per frame
    void Update()
    {
       // rectPlayer.anchoredPosition += (jrect_Joy.anchoredPosition / 10);//2D坐标 += 摇杆的坐标变化值/10
       
    }

#endif
}
