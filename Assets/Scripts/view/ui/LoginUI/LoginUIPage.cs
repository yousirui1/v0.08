using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using Pomelo.DotNetClient;
using tpgm;

public class LoginUIPage : UIPage
{
	private const string TAG = "LoginUIPage";
	//登录方式
	private const int LOGIN_TYPE_MAC = 1;
	private const int LOGIN_TYPE_UID = 2;

	//http控制
	Controller m_controller;

	GameObject toastObj = null;


	Coroutine coroutine;

	public LoginUIPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		//布局预制体
		uiPath = "Prefab/UI/LoginUI/LoginUIPage";

	}

	public override void Awake(GameObject go)
	{
		//Init(UIValue.login_btnID, UIValue.login_inputID, UIValue.login_txID, UIValue.login_imgID);
		toastObj = this.gameObject.transform.Find("tx_toast").gameObject;
		toastObj.SetActive (false);

		//定时器
		coroutine = UIRoot.Instance.StartCoroutine(Timer());



		m_controller = new Controller();

		this.gameObject.transform.Find("btn_register").GetComponent<Button>().onClick.AddListener(() =>
			{
				UIRoot.Instance.StopCoroutine(coroutine);
				// 注册
				UIPage.ShowPage<RegisterUIPage>();
			});

		this.gameObject.transform.Find("btn_login").GetComponent<Button>().onClick.AddListener(() =>
			{
				toastObj.SetActive (true);
				m_controller.reqThirdLogin (false,LOGIN_TYPE_UID);
			
			});

		this.gameObject.transform.Find("btn_maclogin").GetComponent<Button>().onClick.AddListener(() =>
			{
				toastObj.SetActive (true);
				m_controller.reqThirdLogin (false,LOGIN_TYPE_MAC);
			
			});



	}

	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	//秒定时器
	IEnumerator Timer() {
		while (true) {
			onPomeloEvent_UpState ();
			yield return new WaitForSeconds(1.0f);

		}
	}
	private int showEnd = 0;

	private string[] stringArr = {"", ".", "..", "..."};
	private int index = 0;
	void onPomeloEvent_UpState()
	{
		//等待长连接成功
		if (!SavedContext.s_gameServerConnected) {
			toastObj.GetComponent<Text> ().text = "连接中" + stringArr [index];
			index++;
			if (index >= stringArr.Length)
				index = 0;
		} else {
			toastObj.SetActive(false);
			UIPage.ShowPage<MainUIPage> ();
			UIRoot.Instance.StopCoroutine(coroutine);
		}

	}

	class Controller : NetHttp.INetCallback
	{
		NetHttp m_netHttp;

		PomeloClient m_pClient;

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
