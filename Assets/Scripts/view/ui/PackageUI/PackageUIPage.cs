using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using tpgm;

public class PackageUIPage : UIPage
{
	private const string TAG = "PackageUIPage";

	private long AwadTime = 0;
	public PackageUIPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		//布局预制体
		uiPath = "Prefab/UI/PackageUI/PackageUIMain";
	}
	public override void Refresh()
	{
		/*this.gameObject.transform.Find("tx_username").GetComponent<Text>().text = "" + ConectData.Instance.userInfo.nickname;
		this.gameObject.transform.Find("tx_level").GetComponent<Text>().text = ConectData.Instance.userInfo.level + "级";

		AwadTime = ConectData.Instance.userInfo.boxData  - ConectData.Instance.NewTime;

		if (AwadTime < 0) {
			AwadTime = 0;
			this.gameObject.transform.Find ("btn_goldbox").GetComponent<Button> ().enabled = true;
		} else {
			this.gameObject.transform.Find ("btn_goldbox").GetComponent<Button> ().enabled = false; 
		}
		this.gameObject.transform.Find("btn_goldbox/Text").GetComponent<Text>().text = ""+ AwadTime;
		this.gameObject.transform.Find("btn_copperbox/Text").GetComponent<Text>().text ="" + ConectData.Instance.userInfo.gold;*/
	}


	//秒定时器
	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds(1.0f);
			Refresh ();
		}
	}



	public override void Awake(GameObject go)
	{
		this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
		{
			// 返回
			ClosePage();
		});
	}






	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	class Controller : NetHttp.INetCallback
	{
		NetHttp m_netHttp;


		public Controller()
		{
			m_netHttp = new NetHttp();
			m_netHttp.setPageNetCallback(this);

		}

		public void onDestroy()
		{
			m_netHttp.setPageNetCallback (null);

			//可能在login页面没有登录
			if (null != SavedContext.s_client) {
				SavedContext.s_client.NetWorkStateChangedEvent -= (state) => {
					Debug.Log (""+state);
				};
			}

		}

		//用于标识是那个接口用于处理接受函数
		private const int REQ_THIRD_LOGIN = 1;

		public void reqThirdLogin(bool isRetry,int type)
		{

			ReqThirdLogin paramsValObj;
			string checkID;

			string api = "/login";

			AppUtils.apiCheckID (api);

			if (isRetry) {
				paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdLogin> ();
				paramsValObj.m_isRetry = 1;

				checkID = paramsValObj.m_checkID;

			} else {
				checkID = AppUtils.apiCheckID(api);
				paramsValObj = new ReqThirdLogin();
				//重连
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_type = type;
				paramsValObj.m_mac = InfoUtil.getMac();
				paramsValObj.m_account = GameObject.Find("input_user").GetComponent<InputField>().text;
				paramsValObj.m_password = GameObject.Find ("input_passwd").GetComponent<InputField> ().text;

				paramsValObj.m_password = Md5Util.GetMd5FromStr(paramsValObj.m_password);

				//保存数据
				PlayerPrefs.SetString("account", paramsValObj.m_account);
				PlayerPrefs.SetString("password", paramsValObj.m_password);
			}

			string url = SavedContext.getApiUrl(api);
			Debug.Log (url);
			m_netHttp.postParamsValAsync(url,paramsValObj, REQ_THIRD_LOGIN, checkID);

		}





		public virtual void onHttpOk(DataNeedOnResponse data, ResponseData respData)
		{
			Debug.Log ("onHttpOk");
			switch (data.m_reqTag) {
			case REQ_THIRD_LOGIN:
				{
					RespThirdLogin resp = Utils.bytesToObject<RespThirdLogin> (respData.m_protobufBytes);
					switch (resp.m_code) {
					case 200:
						{
							if (null == SavedData.s_instance) {
								SavedData.s_instance = new SavedData ();

							}
							User user = SavedData.s_instance.m_user;
							user.m_uid = resp.m_uid; 
							user.m_token = resp.m_token; 


							LoginConect client = new GameObject ("Client").AddComponent<LoginConect> ();
							client.onLogin (SavedData.s_instance.s_clientUrl, SavedData.s_instance.s_clientPort);
						}
						break;

					default:
						{
							Debug.Log (resp.m_code);
						}
						break;



					}
				}
				break;
			}

		}

		public virtual void onHttpErr(DataNeedOnResponse data, int statusCode, string errMsg)
		{
			Debug.Log (TAG +":" +"onHttpErr");
		}

		public virtual void onOtherErr(DataNeedOnResponse data, int type)
		{
			Debug.Log (TAG +":" +"onOtherErr");

		}
	}

}

#if false


		//定时器
		//UIRoot.Instance.StartCoroutine(Timer());

		//初始化
		Init();

		GameObject rankObj = ResourceMgr.Instance().CreateGameObject("Prefab/UI/PublicUI/PublicUIRank",false);
		rankObj.SetActive (false);

		//隐藏按钮
		this.gameObject.transform.Find("btn_hid").GetComponent<Button>().onClick.AddListener(() =>
		{
		//isActive = !isActive;
		// 隐藏设置按钮
		//Active_btn(isActive);
		//				
		isActive = !isActive;
		Active_btn(isActive);
		int iActive = isActive == false ? 0 : 1;
		PlayerPrefs.SetInt("hid", iActive);
		});

		

		this.gameObject.transform.Find("btn_rank").GetComponent<Button>().onClick.AddListener(() =>
		{

		rankObj.SetActive (true);

		Debug.Log("obj null");

		rankObj.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
		{
		rankObj.SetActive (false);
		});
		});

		//领取宝箱
		this.gameObject.transform.Find("btn_goldbox").GetComponent<Button>().onClick.AddListener(() =>
		{
		UpdateBuf update = new UpdateBuf();
		update.isRetry = 0;
		update.checkID = "1";
		update.token = ConectData.Instance.Token;

		BoxRewardBuf rewardBuf = new BoxRewardBuf();
		BoxReward reward = new BoxReward();

		rewardBuf.reward = reward;
		//设置

		NetHttp.HttpSend<UpdateBuf, BoxRewardBuf>("http://121.40.149.87:3000/boxreward", update, rewardBuf);

		if(NetHttp.HttpResult(rewardBuf.code))
		{
		if (rewardBuf.reward.gold != null)
		{
		Debug.Log(rewardBuf.reward.gold);

		ItemAwad awad = new ItemAwad();
		awad.id = 0;
		awad.num = 100;

		UIPage.ShowPage<PublicUIAwadPage>(awad);
		ConectData.Instance.userInfo.boxData = rewardBuf.boxTime +30000;
		ConectData.Instance.NewTime =rewardBuf.utcMs;

		}
		else
		{
		Debug.LogError("Json null");
		}
		}
		else if(rewardBuf.code == 1006)
		{
		//ConectData.Instance.NetTime = 0;
		//校正时间
		ConectData.Instance.NewTime= rewardBuf.utcMs;
		}

		Refresh();


		});


		this.gameObject.transform.Find("btn_mode1").GetComponent<Button>().onClick.AddListener(() =>
		{

		//向服务器创建房间
		ClientMgr.Instance().CreatRoom();

		//荣耀对决
		UIPage.ShowPage<RoomUIMainPage>();
		});



		}

		private void Init()
		{
		//初始化GameObject
		btn_set = GameObject.Find("btn_set") as GameObject;
		btn_email = GameObject.Find("btn_email") as GameObject;
		btn_activity = GameObject.Find("btn_activity") as GameObject;
		btn_check = GameObject.Find("btn_check") as GameObject;
		btn_rank = GameObject.Find("btn_rank") as GameObject;

		//http获取玩家数据
		gps = new GPSVal();
		InfoUtil.Instance().getGPS(gps);

		InfoSendBuf sendbuf = new InfoSendBuf();
		sendbuf.isRetry = 0;
		sendbuf.checkID = "1";
		sendbuf.user = 1;
		sendbuf.box = 1;
		sendbuf.token = ConectData.Instance.Token;
		sendbuf.Lng = gps.longitude;
		sendbuf.Lat = gps.latitude;

		UserInfoBuf userinfo = new UserInfoBuf(); 
		NetHttp.HttpSend<InfoSendBuf, UserInfoBuf>("http://121.40.149.87:3000/getdata", sendbuf, userinfo);

		ConectData.Instance.userInfo = userinfo;

		ConectData.Instance.userInfo.boxData += 30000;

		ConectData.Instance.NewTime = userinfo.utcMs;


		//载入本地数据
		int iActive = PlayerPrefs.GetInt("hid");
		isActive = iActive == 0 ? false : true;
		Active_btn (isActive);
		//显示数据

		//刷新显示
		Refresh();
		if (ConectData.Instance.Uid != "") {
		//登录服务器
		ClientMgr.Instance ().SetUrl (Config.host, Config.port);
		}

		}

		private void Active_btn(bool isActive)
		{

		btn_set.SetActive(isActive);
		btn_email.SetActive(isActive);
		btn_activity.SetActive(isActive);
		btn_check.SetActive(isActive);
		btn_rank.SetActive(isActive);
		}
		}


//Init(UIValue.main_btnID, UIValue.main_inputID, UIValue.main_txID, UIValue.main_imgID);
this.gameObject.transform.Find("btn_bigpack").GetComponent<Button>().onClick.AddListener(() =>
{
// 大礼包
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");


});

this.gameObject.transform.Find("btn_firstpay").GetComponent<Button>().onClick.AddListener(() =>
{
// 首充
UIPage.ShowPage<PublicUINotice>("首充包未完成，敬请期待");

});

this.gameObject.transform.Find("btn_info").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");

});

this.gameObject.transform.Find("btn_task").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");

});

this.gameObject.transform.Find("btn_achievement").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");
});


this.gameObject.transform.Find("btn_draw").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");

});


this.gameObject.transform.Find("btn_friends").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");
});


this.gameObject.transform.Find("btn_store").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");
});





this.gameObject.transform.Find("btn_email").GetComponent<Button>().onClick.AddListener(() =>
{
// 开箱
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");
});


this.gameObject.transform.Find("btn_activity").GetComponent<Button>().onClick.AddListener(() =>
{
//设置
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");
});

this.gameObject.transform.Find("btn_check").GetComponent<Button>().onClick.AddListener(() =>
{
//设置
UIPage.ShowPage<PublicUINotice>("大礼包未完成，敬请期待");
});



this.gameObject.transform.Find("btn_copperbox").GetComponent<Button>().onClick.AddListener(() =>
{



});


this.gameObject.transform.Find("btn_mode1").GetComponent<Button>().onClick.AddListener(() =>
{
//荣耀对决
UIPage.ShowPage<RoomUIMainPage>();
});
this.gameObject.transform.Find("btn_mode2").GetComponent<Button>().onClick.AddListener(() =>
{
//魔法联赛
UIPage.ShowPage<RoomUIMainPage>();
});
this.gameObject.transform.Find("btn_modequick").GetComponent<Button>().onClick.AddListener(() =>
{
//设置
UIPage.ShowPage<RoomUIMainPage>();
});
#endif
