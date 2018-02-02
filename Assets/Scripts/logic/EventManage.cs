using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using tpgm;

/**************************************
*FileName: EventManage.cs 
*User: ysr 
*Data: 2017/12/20
*Describe: 对应从网络接收到的数据进行处理
			并返回给网络。逻辑数值计算
**************************************/


public class EventManage : MonoBehaviour
{

	private Map mapObj;				//地图用于实例化物体
	private JoyControl joyObj;		//摇杆输入
	private GameMenuUI menuObj;		//摇杆输入
	
	private PlayerVal []players = new PlayerVal[ConfigT.playerMax]; //在线玩家数据

    private Shadow shadow;

	private int id =0;	//主角对应的玩家id

	// 0站立 1行走 2攻击1 3 攻击2 
	private int iAnimationSate = 0;

	private int iFire = 0;		//普攻 
	private int iSkill = 0;		//技能
	private int iFlash = 0;     //闪现

    //移动方向
    private int[,] direction = new int[9, 2] { {0,0 },{0, -1},{1, -1},{1, 0},{1, 1},
    {0, 1},{-1, 1},{-1, 0},{-1, -1}};


    private float UserListScrollHeight = 800;
    private float TabItemWidth = 0;
    private float UserItemHeight = 0;

    //当前帧
    private double frame;

    private bool isFrame = false;

    void Start()
    {
        UserItemGroup = GameObject.Find("Content") as GameObject;
        UserListScrollHeight = UserItemGroup.GetComponent<RectTransform>().rect.height;
        UserItemPre = (GameObject)Resources.Load("UI/Prefabs/UserNameItem");
        UserItemHeight = UserItemPre.GetComponent<RectTransform>().rect.height;

        joyObj = GameObject.Find("JoyControl").GetComponent<JoyControl>() as JoyControl;
        mapObj = GameObject.Find("map").GetComponent<Map>() as Map;
        menuObj = GameObject.Find("UI").GetComponent<GameMenuUI>() as GameMenuUI;
        shadow = new Shadow();

        frame = 0;
    }


    #region player event

    //主角输入数据
    public PlayerVal onInput()
    //void FixedUpdate()
    {
        float vertical;
        float horizontal;

        frame++;

        //获取摇杆数据
        if (joyObj.isMove)
        {
            vertical = joyObj.vertical;
            horizontal = joyObj.horizontal;
        }
        //获取键盘数据
        else
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        if (null != players[id])
        {

            //判断移动方向
            for (int i = 0; i < 9; i++)
            {
                if (direction[i, 0] == vertical && direction[i, 1] == horizontal)
                {
                    players[id].d = i;
                    break;
                }
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                players[id].skill = 1;

            }


            if (Input.GetKeyDown(KeyCode.K))
            {
                players[id].skill = 2;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                players[id].skill = 3;
            }


            //计算移动位置
            shadow.craft_move(players[id], 1);
           
            //Event移动实体
            mapObj.onEvent(id, players[id]);

            //返回给Net发送出去
            return players[id];
        }
       return null;
	
	}


	//网络传输过来的数据处理
    public void onMove(FrameBuf buf)
	{
        bool isFind = false;
        for (int i =0; i< buf.data.Length; i++)
		{

            if(!isFrame)
            {
                frame = buf.frame;
                isFrame = true;
            }
            //在用户表里查找
            foreach (string name in ConectData.Instance.UserNameList)
            {
                isFind = false;
                if (buf.data[i].uid == name)
                {
                    isFind = true;

                    //更新战斗机影子
                    //shadow.shadow_refresh(buf.data[i], frame,buf.frame);

                    //判断是否为主角用户
					if (buf.data[i].uid == ConectData.Instance.Uid)
						id = i;
                    else
                    {
                        //数值计算
                        //更新非主角影子位置 
                        shadow.shadow_move(buf.data[i], 1);
                        
                        //让非主角实体朝自己的影子飞过去1 同步跟随 2相位跟随
                       
                        shadow.trace(1, buf.data[i], 1);
                     
                        //在场景里移动实例化物体
                        mapObj.onEvent(i, buf.data[i]);
                    }
                    break;
                }
             
            }
            //在用户表里没有查找到为新加入的用户
            if (!isFind)
            {
                addPlayer(buf.data[i], i);
				mapObj.addGameObj(i, buf.data[i].uid, buf.data[i].x, buf.data[i].y, ObjType.player);
                //用户表
                ConectData.Instance.UserNameList.Add(buf.data[i].uid);
                if (count < 6)
                {
                    AddNewUserItem(buf.data[i].uid, buf.data[i].score);
                    count++;
                }
            }
           
		}

	}

    int count = 0;

	public void addPlayer(PlayerVal player, int i)
	{
		PlayerVal newplayer = new PlayerVal();
		newplayer.uid = player.uid;	
		newplayer.x = player.x;	
		newplayer.y = player.y;
        newplayer.d = 0;
        newplayer.v = 30;	
		newplayer.sx = newplayer.x;	
		newplayer.sy = newplayer.y;
        newplayer.sd = newplayer.d;
        newplayer.sv = newplayer.v;

        players[i] = newplayer;	
	}


	public void delPlayer(int i)
	{
		//删除用户表对应的用户
		players[i] = null;
		//删除实力物体
		mapObj.delGameObj(i);
    }
    #endregion


 #region UI Event

GameObject UserItemGroup;
GameObject UserItemPre;
private UserNameItem CurUserNameItem;

private float flastDown;

    
                
public void OnClick(GameObject btn)
{
	iFire = 1;
    //flastDown = Time.time;
	switch(btn.name)
	{
        case ConfigT.fire_btn:
        {
			players[id].skill = 1;
			//btn.GetComponent<Button>().enabled = false;

        }        
		break;

		case ConfigT.skill_btn:
        {
			players[id].skill = 2;
			//btn.GetComponent<Button>().enabled = false;
        }        
		break;

		case ConfigT.flash_btn:
        {
			players[id].skill = 3;
			//cd 
			//btn.GetComponent<Button>().enabled = false;
        }        
		break;
		case ConfigT.chat_btn:
        {
         	Debug.Log(""+ btn.name);
          	//SetkillData(2, 4, 6);
         }        
		break;
           
		case ConfigT.set_btn:
        {
           Debug.Log(""+ btn.name);
		   menuObj.onClickSet();
        }        
		break;

		case ConfigT.msg_switch:
        {
           Debug.Log(""+ btn.name);
        }        
		break;

		case ConfigT.voice_switch:
        {
           Debug.Log(""+ btn.name);
        }        
		break;
     }
    }


	//排行列表增加item
	UserNameItem AddNewUserItem(string name, int score)
	{
		//获取当前人数
		int itemCount = UserItemGroup.transform.childCount;

		
		GameObject itemObj = Instantiate(UserItemPre) as GameObject;

		itemObj.transform.SetParent(UserItemGroup.transform, false);
        itemObj.transform.localPosition += new Vector3(0, -itemCount * UserItemHeight, 0);
		
		//计算位置
		float cutTotalHeight = (itemCount + 1) * UserItemHeight;
		if(cutTotalHeight > UserListScrollHeight)
		{
			UserItemGroup.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, -UserItemHeight);
			Vector3 oriPos = UserItemGroup.GetComponent<RectTransform>().position;
			UserItemGroup.GetComponent<RectTransform>().position -= new Vector3(0, UserItemHeight, 0);
		}

		
		itemObj.GetComponent<Toggle>().group = UserItemGroup.GetComponent<ToggleGroup>();
		itemObj.GetComponent<Toggle>().isOn = true;
		
		//user name item
		UserNameItem uni = itemObj.GetComponent<UserNameItem>();	
		uni.SetName(name);
		//
        //设置颜色
		uni.SetColor(1);
		itemObj.GetComponent<Toggle>().onValueChanged.AddListener(uni.OnToggleItem);	

		return uni;
	}

    void RemoveUserFromUserWindow(string name)
    {
        UserNameItem uni = FindUserNameItemByName(name);
        //掉线变灰
        //uni.SetStateDeActive();
    }

    //更新显示排行窗口
    public void UpdateUserListWindow()
	{
		UserNameItem uni = null;
		foreach(string name in ConectData.Instance.UserNameList)
		{
			bool isFind = false;
			for(int i =0 ;i<UserItemGroup.transform.childCount; i++)
			{
				uni = UserItemGroup.transform.GetChild(i).GetComponent<UserNameItem>();
				if(uni.Name.Equals(name))
				{
					uni.SetColor(1);
					//Vector3 pos = new Vector3(2, 1, 1);
					//newObj = (GameObject)Instantiate (OtherObj, pos, transform.position);
					//userObj = (GameObject)Instantiate (UserObj, pos, transform.position);
					isFind = true;	
					break;
				}
			}
			if(!isFind)
			{
				AddNewUserItem(name,1);
			}
		}
	}

	//查找对应的item
	UserNameItem FindUserNameItemByName(string name)
	{
		UserNameItem uni = null;

		for(int i =0; i< UserItemGroup.transform.childCount; i++)
		{
				uni = UserItemGroup.transform.GetChild(i).GetComponent<UserNameItem>();	
				if(uni.Name.Equals(name))
				{
					break;
				}
		}
		return uni;
	}


	

	public void OnUserItemSelect(UserNameItem uni)
	{
		CurUserNameItem = uni;
	}


   

  
    #endregion


}
