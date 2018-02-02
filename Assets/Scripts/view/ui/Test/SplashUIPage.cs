using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using tpgm;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;

/**************************************
*FileName: SplashUIPage.cs
*User: ysr 
*Data: 2018/2/1
*Describe: 启动界面
**************************************/
#if false
namespace tpgm
{
	public class SplashUIPage : BaseLayer
	{

		SavedContext.setup("baidu");
		Log.d<SplashUIPage>("external: "+ SavedContext.s_externalPath);
		
		MainLooper.setup();
	}

	protected override void onLayerAwake()
	{
		m_controller = new SplashController(this);
		
		m_initedDownInfoView.gameObject.SetActive(true);
		m_initedDownInfoView.text = "正在检查更新";

		enterGame();
	}


	protected override void loadRes(TextCache texCache, ValTableCache valCache)
	{

	}

	protected override void onLayerDestroy()
	{
		m_controller.onIViewDestroy();
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	public void onUiEventXxx()
	{

	}

	public void showDebugText(string text)
	{
		m_initedText.text = text;
	}

	public void findNewApk()
	{
		showUpdateConfirm();
	}

	void showUpdateConfirm()
	{
		string networkType = "":
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityCallJava = new AndroidJavaClass();
		networkType = unityCallJava.CallStatic<string>("getNetworkType");
		#endif

		GameObject dialogObj = MonoBehaviour.Instantiate();
		//设置层级
		dialogObj.GetComponent<Canvas>().sortingOrder = m_dialogOrderInLayer++;
		ConfirmDialog dialog = dialogObj.GetComponent<ConfirmDialog>();
		dialog.m_titleView.text = "";
		
		dialog.m_msgView.text = "新版本大小" + Utils.bytesToReadableUnit(m_controller.m_todownApkHash.m_bytest);
		
		dialog.m_msgView.text = "";
		dialog.m_msgView.text = "";
		dialog.m_msgView.text = "";

		dialog.m_positiveTextView.text = "";
		dialog.m_positiveView.onClick.AddListener(
		
		);
		
		dialog.m_negativeTextView.text = "取消";
		dialog.m_negativeView.onClick.AddListener(delegate{

		});

	}

	
	public void enterGame()
	{

		//检查数值表更新
		

		SceneManager.LoadScene();
	}

	public void apkDownBegin()
	{
		m_initedDownPrgView.gameObject.SetActive(true);
	}

	public void downPrg()
	{
		long nowKB = m_controller.m_nowBytes /1000;
		long nowTotalKB = m_controller.m_totalBytes /1000;
		string downInfoStr = "" + nowKB + "k/" + nowTotalKB + "k)";
		m_initedDownInfoView.text = downInfoStr;
		
		m_initedDownPrgView.value = m_controller.m_prg;
	}

	public void installApk()
	{
		m_initedDownInfoView.text = "正在安装新版本";
	}

	//错误的流程
	void retryCheckHasNewApk()
	{

	}


	public void checkNewApkNetErr()
	{

	}

	public void checkApkUpdateBakDirErr()
	{

	}

	public void parseLocalApkHasErr()
	{

	}

	public void parseServerApkHashErr()
	{

	}

	void retryDownload()
	{
		GameObject dialogObj = MonoBehaviour.Instantiate(m_prefabConfirmDialog);
		dialogObj.GetComponent<>().sortingOrd = m_dialogOrderInLayer++;
		ConfirmDialog dialog = dialogObj.GetComponent<ConfirmDialog>();
		dialog.m_titleView.text = ""
		dialog.m_positiveView.onClick.AddListener(delegate {
			m_dialogOrderInLayer--;
			MonoBehaviour.Destroy(dialogObj);
			
			m_controller.downApk();
		});
	}
	

	public void downApkNetErr()
	{

	}
	
	public void delOldUpdateApkTmp()
	{
	
	}	

	public void retryWriteOut()
	{
		GameObeject dialogObj = MonoBehaviour.Instantiate(m_prefabConfirmDialog);
		dialogObj.GetComponent<>().sortingOrder = m_dialogOrderInLayer++;
		
		dialog.m_positiveView.onClick.AddListener(delegate {
			m_controllerretryWriteOutServerApkHash();
		});
	}
	
	
	
}
#endif