using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigT
{

    //Net
    public const string host = "121.40.149.87";
    //public const string host = "192.168.52.1";
    //public const string host = "120.26.200.203";
    public const int port = 7014;

	public const int playerMax = 25;



    public enum ui{
     	//摇杆
        battery_img = 0,

        ListView =1,

		set_btn,
        voice_switch,
        chat_btn,
        msg_switch,

        die_img,
        kill_img,
		assist_img,

        wifi_img,

        Viewport ,
        Joy ,
    
        flash_btn,
        fire_btn,
        skill_btn,


        info_img,
    }

    #region  UI

    //摇杆
    public const string joy_control = "JoyControl";
    public const string viewport = "Viewport";
    public const string joy = "Joy";

    //排行榜
    public const string listview = "ListView";


    //按钮
    public const string flash_btn = "flash_btn";
    public const string fire_btn = "fire_btn";
    public const string chat_btn = "chat_btn";
    public const string set_btn = "set_btn";
    public const string skill_btn = "skill_btn";
    public const string msg_switch = "msg_switch";
    public const string voice_switch = "voice_switch";

    //贴图
    public const string kill_img = "kill_img";
    public const string kill_tx = "kill_img";

    public const string die_img = "die_img";
    public const string die_tx = "kill_img";

    public const string assist_img = "assist_img";
    public const string assist_tx = "kill_img";


    public const string info_img = "info_img";
    public const string battery_img = "battery_img";
    public const string wifi_img = "wifi_img";


    #endregion

}
