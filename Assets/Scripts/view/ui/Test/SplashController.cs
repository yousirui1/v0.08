using UnityEngine;
using System.Collections;
using System.Net;
using System;
using System.Threading;

using tpgm;
#if false
using tpgm.update;
using System.IO;

/**************************************
*FileName: SplashController.cs
*User: ysr 
*Data: 2018/1/24
*Describe: 启动检查版本更新逻辑
**************************************/

namespace tpgm
{

	public class SplashController : tpad.BaseController<SplashLayer>
	{
		public SplashController(SplashLayer ivew) :base(iview)
		{
			
		}

		//检查has是否更新
		public void checkHasNewApk()
		{
			Lod.d<>();
			int verCode = -1;
			
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJaveClass unityCallJava = new AndroidJaveClass("com.tpad.ttd.UnityCallJava");
			verCode = unityCallJave.CallStatic<int>("getVerCode");
			#endif

			if(-1 != verCode)
			{
				m_apkVerCode = verCode;
				ThreadPool.QueueUserWorkItem(checkHasNewApkAsync);
			}
		}

		//异步检查
		void checkHasNewApkAsync()
		{
			Lod.d<>();
			
			try
			{
				m_errCode = ErrCode.CHECK_APK_UPDATE_BAK_DIR;
			
				//检查目录
				checkApkUpdateBakDir();

				HttpWebRequest req = (HttpWebRequest)FileWebRequest.Creat("http://");

				HttpWebResponse resp = null;

				resp = (HttpWebResponse)req.GetResponse();

				switch(resp.StatusCode)
				{
					case HttpStatusCode.OK:
					{
						try
						{
							m_errCode = ErrCode.PARSE_SERVER_APK_HASH;
							Stream stream = resp.GetResponseStream();
							ApkHash serverApkHash;
							using()
							{
								serverApkHash = parseVersionApkHash(sr);

							}		
							Debug.Log("" + m_apkVerCode + "," + serverApkHash.m_verCode);
						
							if(m_apkVerCode != serverApkHash.m_verCode)
							{
								//通知页面需要更新
								MainLooper.instance().sendMessage(handleMessage, MSG_FIND_NEW_APK);

							}
							else
							{
								//不需要更新
								MainLooper.instance().sendMessage(handleMessage, MSG_FIND_NEW_APK);
							}
						
						}
						catch(Exception e)
						{
							Debug.Log(e.Message);
						}
						
						switch(m_errCode)
						{
							case ErrCode.INIT:
							{

							}
							break;

							default:
							{
								MainLooper.instance().sendMessage(handleMessage, MSG_ERR);
							}
							break;
						}
					}
					break;

					default:
					{

					}
					break;

				}
			}	
			catch(WebException ex)
			{
				Debug.Log(ex.Message);
				
				m_errCode = ErrCode.CHECK_NEW_APK_NET_ERR;
				
				MainLooper.instance().sendMessage(handlerMessage, MSG_ERR);
			}
		}

		
		//备份	
		void checkApkUpdateBakDir()
		{
			if(false)
			{
				throw new IOException();
			}

			string apkUpdateBakDirPath = SavedContext.s_externalPath +"apk_update_bak";

			if(Directory.Exists(apkUpdateBakDirPath))
			{
				string apkUpdateTmpDirPath = SavedContext.s_externalPath + "apk_update_tmp";
				
				if(Directory.Exists(apkUpdateTmpDirPath))
				{
					Directory.Delete(apkUpdateBakDirPath, true);
					
					string apkUpdateDirPath = SavedContext.s_externalPath + "apk_update";
					Directory.Move(apkUpdateTmpDirPath, apkUpdateDirPath);
				}
				else
				{

					Directory.Delete(apkUpdateBakDirPath, true);
				}

			}
		}

		//解析版本hash
		static ApkHash parseVersionApkHash(TextReader tr)
		{
			ApkHash apkHash = new ApkHash();
			
			int lineNum = 0;
			string line;
	
			while(null != (line = tr.ReadLine()))
			{
				switch(lineNum)
				{
					case 0:
					{
						try
						{
							apkHash.m_rVal = Convert.ToInt32(line);
						}
						catch(FormatException ex)
						{

							throw new IOException("invalid line 1:" + line);
						}
					}
					break;
					
					case 1:
					{
						try
						{
							apkHash.mVerCode = Convert.ToInt32(line);
						}
						catch(FormatException ex)
						{
							throw new IOException("invalid line 2: " +line);
						}

					}
					break;
			
					case 2:
					{
						int idx1 = line.IndexOf(" ; ");
						if(-1 == idx1)
						{
							throw new IOException("invalid line " + (lineNum + 1) + ":" +line);
						}
						String filename = line.SubString(0, idx1);

						int idx2 = line.IndexOf(" ; ", idx1 + 3);
						if(-1 == idx2)
						{
							throw new IOException("invalid line " + (lineNum + 1) + ":" +line);
						}
						String hash = line.Substring(idx1 + 3, idx2 -(idx1 +3 ));
					
						idx1 = idx2;
						idx2 = line.IndexOf(" ; ", idx1 + 3);
						if(-1 == idx2)
						{
							throw new IOException("invalid line " + (lineNum + 1) + ":" +line);
						}
						String bytesStr = line.Substring(idx1 + 3, idx2 -(idx1 + 3));
						long bytes;
						
						try
						{
							bytes = Convert.ToInt64(bytesStr);
						}
						catch(FormatException ex)
						{
							throw new IOException("invalid line " + (lineNum + 1) + ": "+line);
						}

						idx1 = idx2;
						String url = line.Substring(idx1 +3 );	

						apkHash.m_filename = filename;
						apkHash.m_hash = hash;
						apkHash.m_bytes = bytes;
						apkHash.m_url = url;
					}
					break;	
					default:
					{

						apkHash.m_log += line + "\n";
					}
					break;
				}
					lineNum++;


			}
			return apkHash;

		}


		public void downApk()
		{
			ThreadPool.QueueUserWorkItem(downApkAsync);
		}

		void downApkAsync(object state)
		{
			ApkHash apkHash = m_todownApkHash;
			Log.d<>("downApk");
			
			int errCode = ErrCode.INIT;
			do
			{
				try
				{
					errCode = ErrCode.DEL_OLD_UPDATE_APK_TMP;		
					string apkUpdateTmpDirPath = SavedContext.s_externalPath + "apk_update_tmp/";
					if(Directory.Exists(apkUpdateTmpDirPath))
					{
						Directory.Delete(apkUpdateTmpDirPath, true);
					}	

					HttpWebRequest req = (HttpWebRequest)FileWebRequest.Create(apkHash.m_url);


					if(false)
					{
						throw new WebException("mock: ");
					}

					HttpWebResponse response = (HttpWebResponse)req.GetResponse();

					FileInfo apkFile = new FileInfo(SavedContext.s_externalPath + "apk_update_tmp/" + m_todownApkHash.m_filename);
					
					errCode = ErrCode.DOWN_DIR_CREATE;
					if(false)
					{
						throw new IOException("mock: apk_update_tmp mkdir fail");

					}
					
					IoUtils.EnsureDir(apkFile.DirectoryName);
					
					errCode = ErrCode.DO_DOWN;
					
					doDownloadOrThrow(response);

					errCode = ErrCode.MD5_VERIFY;
				
					string hash = HashUtils.getFileHashOrThrow(apkFile);

					if(false)
					{
						hash = "1";
					}
					if(!hash.Equals(m_todownApkHash.m_hash))
					{
						break;
					}

					errCode = ErrCode.WRITE_OUT_SERVER_APK_HASH;
					writeOutServerApkHashOrThrow();
					
					errCode = ErrCode.FINISH_UPDATE;
					finishAllOrThrow();

					MainLooper.instance().sendMessage(handlerMessage, MSG_INSTALL_APK);
					errCode = ErrCode.INIT;
				}

				catch(IOException ex)
				{
					Log.w<>(ex.Message);
				}
				catch(WebException ex)
				{
					errCode = ErrCode.DOWN_APK_NET_ERR;
				}

			}while(false);
			
			switch(errCode)
			{
				case ErrCode.INIT:
				{

				}
				break;
				
				default:
				{
					m_errCode = errCode;
					MainLooper.instance().sendMessage(handlerMessage, MSG_ERR);

				}
				break;
			}	

		}

		void doDownloadOrThrow(HttpWebResponse response)
		{
			long totalBytes = response.ContentLength;
			m_totalBytes = totalBytes;

			FileInfo apkFile = new FileInfo(SavedContext.s_externalPath + "apk_update_tmp/" + m_todownApkHash.m_filename);

			MainLooper looper = MainLooper.instance();
			
			using(Stream s = response.GetResponseStream())
			using(FileStream fs = apkFile.OpenWrite())
			{
				long nowBytes = 0;
				m_nowBytes = 0;
				m_prg = 0;
				m_bytesPerSec = 0;
				byte[] buffer = new byte[1024];

				int len;
				long t1 = Utils.utcNowMs();
				looper.sendMessage(handlerMessage, MSG_DOWN_BEGIN);

				while(true)
				{
					if((len = s.Read(buffer, 0 ,buffer.Length)) <= 0)
					{
						break;
					}
					nowBytes += len;
					m_nowBytes = nowBytes;
				

					long t2 = Utils.utcNowMs();
					long diff = t2 - t1;
					if(diff > 0)
					{
						m_bytesPerSec = (int)(nowBytes * 1000 /diff);
					}

					fs.Write(buffer, 0, len);
				
					if(totalBytes > 0)
					{
						int prg2 = (int)(nowBytes * 100/ totalBytes);
						int diffPrg = prg2 - m_prg;
						if(diffPrg >= 1)
						{
							m_prg = prg2;
							
							looper.sendMessage(handlerMessage, MSG_DOWN_PRG);
							
							if(false)
							{
								if(prg2 >= 30)
								{
									throw new IOException("mock down err!");
								}
							}
						}

					}
					else
					{

					}

				}

				fs.Flush();
			}

		}


		public void retryWriteOutServerApkHash()
		{
			ThreadPool.QueueUserWorkItem(writeOutServerApkHashAsync);

		}

		void writeOutServerApkHashAsyc(object state)
		{

			int errCode = ErrCode.INIT;
			try
			{
				errCode = ErrCode.WRITE_OUT_SERVER_APK_HASH;
				writeOutServerApkHashOrThrow();
				
				errCode = ErrCode.FINISH_UPDATE;
				finishAllOrThrow();

				MainLooper.instance().sendMessage(handlerMessage, MSG_INSTALL_APK);
				
				errCode = ErrCode.INIT;
			}
			catch(IOException ex)
			{
				Debug.Log();
			}

			switch(errCode)
			{
				case ErrCode.INIT:
				{
					
				}
				break;

				default:
				{
					m_errCode = errCode;
					MainLooper.instance().sendMessage(handlerMessage, MSG_ERR);
				}

				break;
			}
		}

		void writeOutServerApkHashOrThrow()
		{

			Log.d<SplashController>("writeOutServerApkHash");
			
			if(false)
			{
				throw new IOException("mock: fail!");
			}
	
			string apkHashFilePath = SavedContext.s_externalPath + "apk_update_tmp/" + SavedContext.APK_HASH_NAME+ ".txt";

			using(FileStream fs = File.OpenWrite(apkHashFilePath))
			{
				StreamWrite sw = new StreamWrite(fs);

				ApkHash apkHash = m_todownApkHash;

				sw.Write("" + apkHash.m_rVal);
				sw.WriteLine();

				sw.Write(apkHash.m_verCode);
				sw.WriteLine();

				sw.Write(apkHash.m_filename);

				sw.Write(" ; ");
				sw.WriteLine(apkHash.m_hash);


				sw.Write(" ; ");
				sw.WriteLine(apkHash.m_url);

				sw.Flush();
			}
		}


		void finishAllOrThrow()
		{
			if(false)
			{
				throw new IOE;
			}

			string apkUpdateBakDirPath = SavedContext.s_externalPath + "apk_update_bak";
				
			string apkUpdateDirPath = SavedContext.s_externalPath + "apk_update";
			
			bool apkUpdateDirExists = Directroy.Exists(apkUpdateDirPath);
			
			if(apkUpdateDirExists)
			{
				Directory.Move(apkUpdateDirPath, apkUpdateBakDirPath);
			}

			string apkUpdateTmpDirPath = SavedContext.s_externalPath + "apk_update_tmp";
			Directory.Move(apkUpdateTmpDirPath, apkUpdateDirPath);

			if(apkUpdateDirExists)
			{
				Directory.Delete(apkUpdateBakDirPath, true);
			}
		}


		public void installApk()
		{
			Log.d<>();
			
			#if UNITY_ANDROID && !UNITY_EDITOR
			FileInfo apkFile = new FileInfo(SavedContext.s_externalPath + "apk_update/" + m_todownApkHash.m_filename);
			AndroidJaveClass unityCallJava = new AndroidJavaClass("com.tpad.ttd.UnityCallJava");
			bool ok = unityCallJave.CallStatic<bool>("installApk", apkFile.FullName);
			if(!ok)
			{
				m_errCode = ErrCode.INSTALL_APK;
			}
			#endif

			m_todownApkHash = null;	
		}
				}
			
				

			}

		}

	}
}
#endif