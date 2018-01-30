using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using tpgm;

/**************************************
*FileName: LoadingLabel.cs
*User: ysr 
*Data: 2018/1/29
*Describe: 邮件页面
**************************************/

public class PublicUIEmailPage : UIPage
{
	private const string TAG = "ChatUIPage";


	Controller m_controller;
	public PublicUIEmailPage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.WithBg)
	{
		//布局预制体
		uiPath = "Prefab/UI/PublicUI/PublicUIEmail";

	}

	public override void Awake(GameObject go)
	{
		/*ItemAwad awad = (ItemAwad)this.data;

		this.gameObject.transform.Find("content/img_awad0").GetComponent<Image>().sprite = ResourceMgr.Instance().Load<Sprite>("Public/Item/Awad/awad" + awad.id, false);

		this.gameObject.transform.Find("content/tx_awad0").GetComponent<Text>().text = ""+awad.num;

		ConectData.Instance.userInfo.gold += awad.num;
		//child.GetComponent<SpriteRenderer>().sprite =
		*/
		m_controller = new Controller (this);

		m_controller.reqThirdEmails (false);

		this.gameObject.transform.Find("content/btn_back").GetComponent<Button>().onClick.AddListener(() =>
		{
			// 隐藏
			Hide();
		});
		ValTableCache valCache = getValTableCache ();

		Dictionary<int, ValFish> valDict = valCache.getValDictInPageScopeOrThrow<ValFish>(m_pageID, ConstsVal.val_fish);
		ValFish gval = ValUtils.getValByKeyOrThrow(valDict,100001);
		Debug.Log ("asdasd" + gval.name);
	}





	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
		
		valCache.markPageUseOrThrow<ValFish> (m_pageID, ConstsVal.val_fish);
		//valCache.markPageUseOrThrow<ValFish> (m_pageID, ConstsVal.val_fish);

		//valCache.markPageUseOrThrow<ValSignIn7> (m_pageID, ConstsVal.val_signIn7);

		//valCache.markPageUseOrThrow<ValSignInAdd> (m_pageID, ConstsVal.val_signInAdd);

		//valCache.markPageUseOrThrow<ValGlobal> (m_pageID, ConstsVal.val_global);
		/*valCache.markPageUseOrThrow<ValFish>(m_pageID, ConstsVal.val_fish);
		Dictionary<int, ValFish> valDict = valCache.getValDictInPageScopeOrThrow<ValFish>(m_pageID, ConstsVal.val_fish);
		ValFish gval = ValUtils.getValByKeyOrThrow(valDict,100001);
		Debug.Log ("asdasd" + gval.name);*/


	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	class Controller : BaseController<RegisterUIPage>, NetHttp.INetCallback
	{
		NetHttp m_netHttp;
		PublicUIEmailPage m_email;
		public Controller(PublicUIEmailPage iview):base(null)
		{
			m_netHttp = new NetHttp();
			m_netHttp.setPageNetCallback(this);
			m_email = iview;
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
		private const int REQ_THIRD_EMAILS = 9;

		public void reqThirdEmails(bool isRetry)
		{

			ReqThirdEmails paramsValObj;
			string checkID;

			string api = "/emails";

			AppUtils.apiCheckID (api);

			if (isRetry) {
				paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdEmails> ();
				paramsValObj.m_isRetry = 1;

				checkID = paramsValObj.m_checkID;

			} else {
				checkID = AppUtils.apiCheckID(api);
				paramsValObj = new ReqThirdEmails();
				//重连
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_token = SavedData.s_instance.m_user.m_token;

			}

			string url = SavedContext.getApiUrl(api);
			Debug.Log (url);
			m_netHttp.postParamsValAsync(url,paramsValObj, REQ_THIRD_EMAILS, checkID);

		}





		public virtual void onHttpOk(DataNeedOnResponse data, ResponseData respData)
		{
			Debug.Log ("onHttpOk");
			switch (data.m_reqTag) {
			case REQ_THIRD_EMAILS:
				{
					RespThirdEmails resp = Utils.bytesToObject<RespThirdEmails> (respData.m_protobufBytes);
					switch (resp.m_code) {
					case 200:
						{
							Debug.Log (resp.m_emailList);
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
			Debug.Log (TAG +":" +"onOtherErr"+data.m_url);

		}
	}

}
