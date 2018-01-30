using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace tpgm
{
	public class UiControl : MonoBehaviour
	{

		#if false
		ValTableCache m_initedValTableCache;
		public List<ValUserExp> m_valUserExpList = new List<ValUserExp>();

		Image m_userHead;
		Slider m_userExpProgress;
		Slider m_gunExpProgress;
		Text m_textGoldNum;
		Text m_textPearlNum;
		Text m_textUserLevel;
		Text m_gunNeedGold;
		GameObject m_gunPinZhi;

		public Sprite m_spritePinZhi1;
		public Sprite m_spritePinZhi2;
		public Sprite m_spritePinZhi3;
		public Sprite m_spritePinZhi4;
		public Sprite m_spritePinZhi5;

		int m_userLevel = 1;
		public ValUserExp m_curExpValData;
	

		void Awake()
		{
			m_initedValTableCache = SavedContext.s_valTableCache;

			ReadJsonOfUserExp();
		
			m_userHead = GameObject.Find("Image_myTouxiang").GetComponent<Image>();
	
			initUi();
		}


		public void initUi()
		{
			setUserLeave();
			
			setUserExpProgress();

		}

		//设置头像
		public void setUserHead(string headPath)
		{
			m_userHead.overrideSprite = Resources.Load(headPath, typeof(Sprite)) as Sprite;
		}

		//设置人物经验
		public void setUserExp(string exp)
		{
			SavedData.s_instance.m_userdata.exp = exp;
				
			setUserLevel();	
			setUserExpProgress();
		}

		
		//设置人物等级
		public void setUserLevel()
		{
			int myExp = SavedData.s_instance.m_userdata.exp;
		
			for(int i = 0; i<m_valUserExpList.Count; i++)
			{
				if(myExp >= m_valUserExpList[i].exp)
				{
					m_userLevel = m_valUserExpList[i].lev;
					m_curExpValData = m_valUserExpList[i];

				}
				else
				{
					break;
				}
			}	

			SavedData.s_instance.m_userdata.lev = m_userLevel;
			m_textUserLevel.text =  m_userLevel.ToString();
		}		

		//设置人物经验进度条
		public void setUserExpProgress()
		{
			for(int i = 0; i < m_valUserExpList.Count; i++)
			{
				if(m_userLevel == 1)
				{
					float exp1 = SavedData.s_instance.m_userdata.exp;
					float exp2 = m_valUserExpList[1].exp;

					int percent = (int)(exp1 / exp2 * 100.0f);
					m_userExpProgress.value = percent;

					return ;
				}
				
				if(m_userLevel < m_valUserExpList[i].lev)
				{

				}
				
			}

		}


		public void setGunNeedGold()
		{


		}

		public void setGunPinZhi(int pinzhi)
		{
			switch(pinzhi)
			{
				case 1:
				{
					m_gunPinZhi.GetComponent<SpriteRenderer>().sprite = m_spritePinZhi1;
				}
				break;


			}

		}


		void ReadJsonOfUserExp()
		{
			m_valUserExpList = m_initedValTableCache.getValListOutPageScoperOrThrow<ValUserExp>();
			
			foreach(ValUserExp item in m_valUserExpList)
			{
				item.getRewards();
			}

		}


		public class ValUserExp : BaseVal
		{
			public int lev = 0;
			public int exp = 0;
			public string reward = "";
			public int goold = 0;  //你真棒条件
			public int nice = 0;   //你真牛条件

			private List<GsidNumPair> rewardList;
			
			public List<GsidNumPair> getRewards()
			{
				if(!reward.Equals(""))
				{
					if(null == rewardList)
					{
						rewardList = ValUtils.parseGsidNumPairsOrThrow(reward);

					}

				}
				return rewardList;

			}
			
			
		}
		#endif
		
	}

}
