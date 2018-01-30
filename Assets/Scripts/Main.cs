using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpgm.UI;
using tpgm;
using Pomelo.DotNetClient;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//主线程检查并启动
		MainLooper.checkSetup ();
		//设置数据存储目录
		SavedContext.setup ("game_Data");

		//远程打印
		NetLog.Instance();


		UIPage.ShowPage<LoginUIPage> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
