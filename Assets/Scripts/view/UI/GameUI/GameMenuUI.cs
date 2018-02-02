using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**************************************
*FileName: GameMenuUI.cs
*User:ysr
*Data: 2017/11/30
*Describe: 菜单界面及状态机
**************************************/
public class GameMenuUI : MonoBehaviour
{
  
    public float fpsMeasuringDelta = 2.0f;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;

    //主游戏界面
    public const int STATE_MAINGAME = 0;
    //菜单界面
    public const int STATE_MAINMENU = 1;

    //游戏设置界面
    public const int STATE_OPTION = 2;

    //游戏设置界面
    public const int STATE_HLEP = 3;
    //游戏退出界面
    public const int STATE_EXIT = 4;

    //游戏退出界面
    public const int STATE_TIME = 5;

    private int height;
    private int width;

    private int gameState = 0;
	private bool isShowFPS = true;
    private void Start()
    {
        timePassed = 0.0f;
        gameState = STATE_MAINGAME;

        height = Screen.height;
        width = Screen.width;
    }

    private float time;
    void Update()
    {
        /*
        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;
        }

        time += Time.deltaTime;
        */
    }


    void OnGUI()
    {
        switch (gameState)
        {
            case STATE_MAINMENU:
                RenderMenu();
                //绘制主菜单界面
                break;

            case STATE_OPTION:
                //绘制游戏开始界面
                RenderOption();
                break;
            case STATE_HLEP:
                //绘制游戏开始界面
                RenderHelp();
                break;

            case STATE_TIME:
                //绘制游戏开始界面
                RenderTime();
                break;

            case STATE_EXIT:

                break;
        }
		if(isShowFPS)
		{
        	ShowFPS();
		}
    }

    void ShowFPS()
    {
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = new Color(1.0f, 0.5f, 0.0f);   //设置字体颜色的
        bb.fontSize = 40;       //当然，这是字体大小

        //居中显示FPS
        GUI.Label(new Rect((Screen.width / 2) - 40, 0, 200, 200), "FPS: " + m_FPS, bb);
    }



    //绘制主菜单界面
    void RenderMenu()
    {

        //游戏开始按钮
        if (GUI.Button(new Rect(width * 0.4f, height * 0.3f, width * 0.1f, height * 0.05f), "返回"))
        {
            //开始游戏状态
            gameState = STATE_MAINGAME;

        }
        //游戏设置按钮
        if (GUI.Button(new Rect(width * 0.4f, height * 0.4f, width * 0.1f, height * 0.05f), "设置"))
        {
            gameState = STATE_OPTION;
            Debug.Log("onButton");

        }

        //游戏帮助按钮
        if (GUI.Button(new Rect(width * 0.4f, height * 0.5f, width * 0.1f, height * 0.05f), "帮助"))
        {
            //开始游戏状态
            gameState = STATE_HLEP;

            //conect.onRecv()
        }

        //游戏退出按钮
        if (GUI.Button(new Rect(width * 0.4f, height * 0.6f, width * 0.1f, height * 0.05f), "退出"))
        {
            //开始游戏状态
            gameState = STATE_EXIT;
            Application.LoadLevel("menu");
            //ChatUIManager.Instance.OnLeav();
        }
    }

    //绘制游戏设置界面
    void RenderOption()
    {
        //GUI.skin = mySkin;
      //  GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), textureBG);

        if (GUI.Button(new Rect(width * 0.4f, height * 0.3f, width * 0.1f, height * 0.05f), "music开关"))
        {
            Debug.Log("height" + height + "width" + width);
        }

        if (GUI.Button(new Rect(width * 0.4f, height * 0.4f, width * 0.1f, height * 0.05f), "fps显示"))
        {
			
			isShowFPS = !isShowFPS;
        }

        if (GUI.Button(new Rect(width * 0.4f, height * 0.5f, width * 0.1f, height * 0.05f), "back"))
        {
            gameState = STATE_MAINMENU;
        }
    }

    void RenderTime()
    {
        GUI.Window(0, window0, onTimeWindow, "wait");
    }


    //绘制游戏帮助界面
    void RenderHelp()
    {

    }
    private Rect window0 = new Rect(Screen.width*0.4f, Screen.height*0.3f, Screen.width * 0.3f, Screen.height * 0.3f);
   
    void onTimeWindow(int windowID)
    {
       /* GUI.Box(new Rect(new Rect(120, 50, 150, 50)), "游戏开始还有"+ (int)(PlayerData.Instance.Time - time) + "秒");
        if (GUI.Button(new Rect(new Rect(10, 120, 150, 50)), "确定"))
        {
             gameState = STATE_MAINGAME;
        }
        if (GUI.Button(new Rect(new Rect(200, 120, 150, 50)), "取消"))
        {
            gameState = STATE_MAINGAME;
        }*/

    }

    public void onClickSet()
    {
        if(gameState == STATE_MAINMENU)
        {
            gameState = STATE_MAINGAME;
        }
        else
        {
            gameState = STATE_MAINMENU;
        }
        
    }


    public void onClickStart()
    {
       
        gameState = STATE_TIME;

    }
  

}



