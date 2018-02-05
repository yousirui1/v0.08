using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpgm;

namespace tpgm
{
    #region http数据类
    public class LoginSendBuf
    {
        public int isRetry;      //是否重连
        public string checkID;   //重连之后有用校验id
        public int type;         //登录方式：1：快捷登录，2：账号密码登录
        public string mac;
        public string account;   //账号
        public string password;  //密码
    }


  

    public class LoginRecvBuf
    {
        public int code;
        public string token;
        public int utcMs;
		public string uid;
    }




    public class InfoSendBuf
    {
        public int isRetry;
        public string checkID;
        public int user;
        public int box;
        public string token;

        public float Lng;   //经度
        public float Lat;   //维度
        
    }


    public class UserInfoBuf
    {
        public int code;
        public int utcMs;                  //当前时间

        public int boxData;             //宝箱开始时间
        public string head;             //图像
        public string nickname;         //昵称
        public int level;               //等级    
        public int section;             //段位    
        public string mac;              //mac地址
        public int gold;                //金钱
        public int diamond;             //钻石
        public int prestige;            //声望
        public int fragment;            //碎片
    }


 



    public class UpdateBuf
    {
        public int isRetry;
        public string checkID;
        public string token;
    }



    public class BoxRewardBuf
    {
        public int code;
		public int boxTime;                 //宝箱开始时间 
		public int utcMs;                   //当前时间
        public BoxReward reward;
    }

    public class BoxReward
    {
        public int gold;
    }



#endregion

    #region  pomelo client 数据类
    //传输帧
    public class FrameBuf
    {
        public PlayerVal[] data;
        public int frame;  //逻辑帧
        public long time;   //服务器发送的时间

    }

    //玩家类用于服务器传输数据
    public class PlayerVal
    {
        public string uid;
        public int x;
        public int y;
        public int d;     //方向
        public int v;     //速度
        public int hp;     //区域编号
        public int skill;  //技能 0,1,2 对应3种状态
        public int score;  //玩家所获得积分
        public long utcMs; //服务器接收到的请求值


        public int sx;
        public int sy;
        public int sd;
        public int sv;
    }

	//房间数据
	public class RoomBuf
	{
		public int code ;
		public string roomNum;   //房间号
		public FriendArr[] friendArr;  //好友列表
		public string utcMs;   
	}

	//好友数组
	public class FriendArr
	{
		public string nickname;   //昵称
		public string uid;		  //uid
		public int gender;		  //性别
		public int head;		 //头像
		public int levle;		  //段位
		public int status;		  //状态 1：在线，2：离线，3：组队中，4：匹配中，5：战斗中
	}

    #endregion

    #region 系统信息

    public class GPSVal
    {
        public bool isCould;  //是否开启定位服务，即开启GPS定位  
        public float altitude; //海拔高度  
        public float horizontalAccuracy; //水平精度  
        public float verticalAccuracy;  //垂直精度  
        public float latitude;       //纬度  
        public float longitude;      //经度  
        public double timestamp;     //最近一次定位的时间戳，从 1970年开始  
    }

    #endregion

    #region 游戏数据类
    //资源物品
    public class ItemVal
    {
        public int score = 0;
        public int rehp = 0;
        public int exp = 0;

        //1 min 2 mid 3 max 水晶
        /*public ItemVal(int type)
        {
            switch (type)
            {
                case 1:
                    {
                        score = 1;
                        rehp = 1;
                        exp = 1;
                    }
                    break;
                case 2:
                    {
                        score = 10;
                        rehp = 10;
                        exp = 10;
                    }
                    break;
                case 3:
                    {
                        score = 50;
                        rehp = 50;
                        exp = 50;
                    }
                    break;
                default:
                    break;
            }
        }*/
    }

    public class ItemAwad
    {
        public int id;
        public int num;
    }

    public class ItemJs
    {
        public int id;
        public int x;
        public int y;

    }

    public class SkillVal
    {
        public int id;
        public string name;
        public int grade;   //等级
        public int aim;     //技能生效对象1、敌方2、友方 3、仅自己4、敌我双方效果不同5、敌我双方相同
        public int radius;  //效果半径
        public float speed;
        public int dist;    //魔法攻击距离
        public int hurt;    //伤害
        public int duration; //技能持续时间
        public int cd;      //
        public int count;   //有效个数
        public int bookSid;
        
    }
    #endregion

	public class TabIndex
	{
		public int id;
		public string tabname;
		public string panelPath;

		public TabIndex(int id, string tabname, string panelPath)
		{
			this.id = id;
			this.tabname = tabname;
			this.panelPath = panelPath;
		}
	}

	public class UDFriend 
	{
		public class Friend
		{
			public string nickname;
			public string uid;
			public int gender; 	//性别
			public int head;     //头像
			public int level;		//段位
			public int status;		//状态 1：在线，2：离线，3：组队中，4：匹配中，5：战斗中


		}
		public class FriendBuf
		{
			public int code;
			public string roomNum;
			public Friend[] friendArr;
			public string utcMs;
		}

		public List<Friend> friends;
	}


	public class UDStore 
	{
		public class Materials
		{
			public string nickname;
			public string uid;
			public int gender; 	//性别
			public int head;     //头像
			public int level;		//段位
			public int status;		//状态 1：在线，2：离线，3：组队中，4：匹配中，5：战斗中


		}

	}



	public class UDActivity
	{
		public class Activity
		{
			public string image;
			public string item_tx;
			public string url;

		}
		/*public class FriendBuf
		{
			public int code;
			public string roomNum;
			public Friend[] friendArr;
		}*/

		public List<Activity> activitys;
	}


	public class UDGroup
	{
		public class Group
		{
			public string uid;            //
			public string isReady;		  //
			public string nickname;		  //

			public string group;		  //
			public int head;
			public int level;			
			public int mmr;   //隐藏分

		}
		public class PreparePlayerBuf
		{
			public Group[] message;
			public string type;
		}



		public class StartPlayerBuf
		{
			public Group[] newUser;
		}

		public List<Group> groups;


	}
		

	public enum RecvType
	{
		roomMember = 0,
		playerInfo,
		magicStage,
		first,
		attackInfo,
		attackNum,
		dead,
		sceneInfo,
		attackData,
	};

}
