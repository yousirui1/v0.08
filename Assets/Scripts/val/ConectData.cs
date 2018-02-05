using System;
using SimpleJson;
using Pomelo.DotNetClient;
using System.Collections.Generic;
using tpgm;

/**************************************/
//FileName: ConectData.cs
//Author: Star
//Data: 2017/12/19
//Describe: 登录连接数据全局类
/**************************************/
public class ConectData
{
    private static ConectData instance = null;
    public static ConectData Instance
    {
        get
        {
            if (instance == null)
                instance = new ConectData();

            return instance;
        }
    }

    

    public string Channel { get; set; }
    public JsonObject LoginUserData { get; set; }
    public PomeloClient pClient { get; set; }

    public long Time { get; set; }

    public List<string> UserNameList;

    public string Token { get; set; }

	public string Uid { get; set; }

    //http获取的登录信息
    public UserInfoBuf userInfo { get; set; }

    public int NewTime { get; set; }

	//好友数据
	public  List<UDFriend.Friend> friedns;

	//好友数据
	public  List<UDGroup.Group> repare_groups;

	public List<UDGroup.Group> start_groups;

	public JsonObject playerInfo { get ; set; }

	//房间号
	public string roomNum { get ; set ; } 

    public ConectData()
    {
    }
}


