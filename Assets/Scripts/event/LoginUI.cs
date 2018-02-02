using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

/**************************************
*FileName: LoginUI.cs
*User:ysr
*Data: 2017/11/30
*Describe: 登录UI
**************************************/

public class LoginUI : MonoBehaviour{
	#if false
	private int height;
    private int width;
    private string editUsername = "1";
    private string editPassword = "";

    private LoginConect conect = null;

    void Start()
    {
        // gameState = STATE_LOGIN;

        //添加网络脚步
        conect = GetComponent<LoginConect>();
        if (conect == null)
            conect = gameObject.AddComponent<LoginConect>();

        //获取屏幕大小
        height = Screen.height;
        width = Screen.width;

    }
    void OnGUI()
    {
        //绘制登录界面
   
            GUI.Label(new Rect(width * 0.4f, height * 0.3f, width * 0.1f, height * 0.05f), "用户名");
            GUI.Label(new Rect(width * 0.4f, height * 0.35f, width * 0.1f, height * 0.05f), "密码");
            editUsername = GUI.TextField(new Rect(width * 0.45f, height * 0.3f, width * 0.1f, height * 0.05f), editUsername, 15);
            editPassword = GUI.PasswordField(new Rect(width * 0.45f, height * 0.35f, width * 0.1f, height * 0.05f), editPassword, "*"[0], 15);

            //GUI.Label(new Rect(width * 0.4f, height * 0.3f, width * 0.1f, height * 0.05f), editShow);

            if (GUI.Button(new Rect(width * 0.42f, height * 0.45f, width * 0.1f, height * 0.05f), "登录"))
            {
                 //gameState = STATE_MAINMENU;
                conect.onLogin(editUsername, editPassword);

             }


            if (GUI.Button(new Rect(width * 0.42f, height * 0.55f, width * 0.1f, height * 0.05f), "快速登录"))
            {
                //gameState = STATE_MAINMENU;
                string mac = new InfoUtil().getMac();
                conect.onLogin(mac, editPassword);

            }
    }
	#endif

    /*
	private string editUsername = "1";
    private string editPassword = "";
    private string editShow = "";
	
	public const int STATE_LOGIN = 0;
    //主菜单界面
    public const int STATE_MAINMENU = 1;
    //开始游戏界面
    public const int STATE_STARTGAME = 2;
    //游戏设置界面
    public const int STATE_OPTION = 3;
    //游戏退出界面
    public const int STATE_EXIT = 4;

    public Texture2D textureBG;

    public GUISkin mySkin;

    private int height;
    private int width;

    //游戏状态
    private int gameState ;

	private LoginConect login;

    void Start()
    {
        gameState = STATE_LOGIN;

        height = Screen.height;
        width = Screen.width;
		login = GameObject.Find("NetScript").GetComponent<LoginUIManager>();

    }


    void Update()
    {
       switch(gameState)
       {
         case STATE_MAINMENU :
            //绘制主菜单界面
            break;

            case STATE_STARTGAME:
            //绘制游戏开始界面
            break;

            case STATE_OPTION:
            //绘制游戏设置界面
            break;

            case STATE_EXIT:

            break;
       }
    }

    void OnGUI()
    {
        switch(gameState)
        {
			case STATE_LOGIN:
			RenderLogin();
			break;
			 case STATE_MAINMENU :
            //绘制主菜单界面
            RenderMainMenu();
            break;

            case STATE_STARTGAME:
                //绘制游戏开始界面
                Application.LoadLevel("game");
                break;

            case STATE_OPTION:
            //绘制游戏设置界面
            RenderOption();
            break;

            case STATE_EXIT:

            break;
        }
    }

    //绘制登录界面
    void RenderLogin()
    {

        GUI.Label(new Rect(width * 0.4f, height * 0.3f, width * 0.1f, height * 0.05f), "用户名");
        GUI.Label(new Rect(width * 0.4f, height * 0.35f, width * 0.1f, height * 0.05f), "密码");
        editUsername = GUI.TextField(new Rect(width * 0.45f, height * 0.3f, width * 0.1f, height * 0.05f), editUsername, 15);
        editPassword = GUI.PasswordField(new Rect(width * 0.45f, height * 0.35f, width * 0.1f, height * 0.05f), editPassword, "*"[0], 15);

        //GUI.Label(new Rect(width * 0.4f, height * 0.3f, width * 0.1f, height * 0.05f), editShow);

        if (GUI.Button(new Rect(width * 0.42f, height * 0.45f, width * 0.1f, height * 0.05f), "登录"))
        {
            //gameState = STATE_MAINMENU;
            login.Login(editUsername, editPassword);

        }


        if (GUI.Button(new Rect(width * 0.42f, height * 0.55f, width * 0.1f, height * 0.05f), "快速登录"))
        {
            //gameState = STATE_MAINMENU;
            string mac = new DataMath().GetMac();
            login.Login(mac, editPassword);

        }

       

       


    }


    //绘制主菜单界面
    void RenderMainMenu()
    {
        //设置界面皮肤
        //GUI.skin = mySkin;
        //绘制游戏背景图
        // GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),textureBG);



        //游戏开始按钮
        if(GUI.Button(new Rect(height/2,200,70,30), "开始"))
        {
            //开始游戏状态
            gameState =STATE_STARTGAME;
           
        }
        //游戏设置按钮
        if(GUI.Button(new Rect(height / 2, 240,70,30), "设置"))
        {
            gameState = STATE_MAINMENU;
            Debug.Log("onButton");
            
        }

        //游戏设置按钮
        if(GUI.Button(new Rect(height / 2, 280,70,30), "帮助"))
        {
            //开始游戏状态
            gameState = STATE_STARTGAME;
           
            //conect.onRecv()
        }

        //游戏退出按钮
        if(GUI.Button(new Rect(height / 2, 320,70,30), "退出"))
        {
            //开始游戏状态
            gameState =STATE_STARTGAME;
        }
    }

    

    //绘制游戏开始界面
    void RenderStart()
    {


    }
    
    //绘制游戏设置界面
    void RenderOption()
    {
        //GUI.skin = mySkin;
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height),textureBG);

        if(GUI.Button(new Rect(0,0,403,75),"","music_on"))
        {
        }

        if(GUI.Button(new Rect(0,200,403,75),"","music_off"))
        {
            
        }

        if(GUI.Button(new Rect(0,500,403,78),"","back"))
        {
            gameState = STATE_MAINMENU;
        }
    }

    //绘制游戏帮助界面
    void RenderHelp()
    {

    }


    */
}
