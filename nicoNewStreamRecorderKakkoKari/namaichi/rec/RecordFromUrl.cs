﻿/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/04/14
 * Time: 0:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Threading.Tasks;
using namaichi;
using System.Windows.Forms;
using System.IO;

namespace namaichi.rec
{
	/// <summary>
	/// Description of RecordFromUrl.
	/// </summary>
	public class RecordFromUrl
	{
		private CookieContainer container;
		private RecordingManager rm;
		private string res;
		
		public RecordFromUrl(RecordingManager rm)
		{
			this.rm = rm;
			//CookieContainer container = new CookieContainer();
	        //container.Add(cookie);
			//this.container = container;
		}
		public int rec(string url, string lvid) {
			//endcode 0-その他の理由 1-stop 2-最初に終了 3-始まった後に番組終了
			util.debugWriteLine("RecordFromUrl rec");
			util.debugWriteLine(url + " " + lvid);
			var pageType = this.getPageType(url, true);
			util.debugWriteLine("pagetype " + pageType + " container " + container);
			if (container == null) {
				rm.form.addLogText("ログインに失敗しました。" + lvid);
				if (bool.Parse(rm.cfg.get("IsmessageBox"))) {
					if (rm.form.IsDisposed) return 2;
					try {
			        	rm.form.Invoke((MethodInvoker)delegate() {
			       			MessageBox.Show("ログインに失敗しました。\n" + lvid, "", MessageBoxButtons.OK, MessageBoxIcon.None);
						});
					} catch (Exception e) {
			       		util.showException(e);
			       	}
				}
				if (bool.Parse(rm.cfg.get("IsfailExit"))) {
					rm.rfu = null;
					try {
						rm.form.Invoke((MethodInvoker)delegate() {
			       			rm.form.Close();
						});
					} catch (Exception e) {
			       		util.showException(e);
			       	}
					
				}
				
				return 2;
			}
			
			util.debugWriteLine("pagetype " + pageType);
			
			while (true && this == rm.rfu) {
				util.debugWriteLine("pagetype " + pageType);
				if (pageType == 0) {
					var h5r = new Html5Recorder(url, container, lvid, rm, this);
					var recResult = h5r.record(res);
					util.debugWriteLine("recresult " + recResult);
					return recResult;					
				} else if (pageType == 1) {
					rm.form.addLogText("満員です。");
					if (bool.Parse(rm.cfg.get("Isretry"))) {
						System.Threading.Thread.Sleep(10000);
						
						while(this == rm.rfu) {
							try {
								var wc = new WebHeaderCollection();
								res = util.getPageSource(url, ref wc, container);
								pageType = util.getPageType(res);
							} catch (Exception e) {
								util.debugWriteLine(e.Message + " " + e.StackTrace + " ");
							}
						}
							
						continue;
					} else {
						
						return 2;
					}
					
				} else if (pageType == 5) {
					if (bool.Parse(rm.cfg.get("Isretry"))) {
						rm.form.addLogText("接続エラー。10秒後リトライします。");
						System.Threading.Thread.Sleep(10000);
						
						try {
//							var wc = new WebHeaderCollection();
//							res = util.getPageSource(url, ref wc, container);
//							pageType = util.getPageType(res);
							pageType = getPageType(url);
							util.debugWriteLine("pagetype_ " + pageType);
						} catch (Exception e) {
							util.debugWriteLine(e.Message + " " + e.StackTrace + " ");
//							rm.form.addLogText(e.Message + " " + e.StackTrace + " ");
						}
						continue;
					} else {
						rm.form.addLogText("接続エラー");
						return 2;
					}
					
				} else if (pageType == 6) {
					util.debugWriteLine("pagetype6process");
					System.Threading.Thread.Sleep(3000);
					try {
						pageType = getPageType(url);
						util.debugWriteLine("pagetype_ " + pageType);
					} catch (Exception e) {
						util.debugWriteLine(e.Message + " " + e.StackTrace + " ");
						rm.form.addLogText(e.Message + " " + e.StackTrace + " ");
					}
					continue;

					
				} else if (pageType == 4) {
					rm.form.addLogText("require_community_menber");
//					rm.form.addLogText(res);
					
					util.debugWriteLine(rm.cfg.get("IsautoFollowComgen"));
					if (bool.Parse(rm.cfg.get("IsautoFollowComgen"))) {
						 
						var isFollow = new FollowCommunity().followCommunity(res, container, rm.form, rm.cfg);
						util.debugWriteLine("isfollow " + isFollow);
						if (isFollow) {
//							var wc = new WebHeaderCollection();
//							var referer = "http://live.nicovideo.jp/gate/" + lvid;
							pageType = getPageAfterFollow(url, lvid);
							util.debugWriteLine("pagetype_ " + pageType);
							continue;
						}
					}
					if (bool.Parse(rm.cfg.get("IsmessageBox"))) {
						if (rm.form.IsDisposed) return 2;
						try {
				        	rm.form.Invoke((MethodInvoker)delegate() {
				       			MessageBox.Show("コミュニティに入る必要があります：\nrequire_community_menber/" + lvid, "", MessageBoxButtons.OK, MessageBoxIcon.None);
							});
						} catch (Exception e) {
				       		util.showException(e);
				       	}
					}
					if (bool.Parse(rm.cfg.get("IsfailExit"))) {
						rm.rfu = null;
						try {
							rm.form.Invoke((MethodInvoker)delegate() {
				       			rm.form.Close();
							});
						} catch (Exception e) {
				       		util.showException(e);
				       	}
						
					}
					return 2;
					
				} else {
					var mes = "";
					if (pageType == 2) mes = "この放送は終了しています。";
					if (pageType == 3) mes = "この放送は終了しています。";
					rm.form.addLogText(mes);
					util.debugWriteLine("pagetype " + pageType + " 終了");
					
					if (bool.Parse(rm.cfg.get("IsdeleteExit"))) {
						rm.rfu = null;
						try {
							rm.form.Invoke((MethodInvoker)delegate() {
				       			rm.form.Close();
							});
						} catch (Exception e) {
				       		util.showException(e);
				       	}
						
					}
					return 2;
					//var nh5r = new NotHtml5Recorder(url, container, lvid, rm, this);
					//nh5r.record(res);
				}
			}
			return 2;
            

		}
		public int getPageType(string url, bool isLogin = false) {
			while (this == rm.rfu) {
				try {
					
					var cg = new CookieGetter(rm.cfg);
					var cgret = cg.getHtml5RecordCookie(url);
					cgret.Wait();
					                                  
					
		//			cgret.ConfigureAwait(false);
					if (cgret == null || cgret.Result == null) {
						util.debugWriteLine("cgret " + cgret);
						if (isLogin) {
							rm.form.addLogText("ログインに失敗しました。");
							isLogin = false;
						}
						System.Threading.Thread.Sleep(3000);
						continue;
					}
		//			if (cgret == null) return true;
					container = cgret.Result;
					util.debugWriteLine("container " + container);
					

	
					res = cg.pageSource;
					
	//				Uri TargetUrl = new Uri("http://live.nicovideo.jp/");
	//				util.debugWriteLine("1 " + container.GetCookieHeader(TargetUrl));
	//				TargetUrl = new Uri("http://live2.nicovideo.jp/");
	//				util.debugWriteLine("2 " + container.GetCookieHeader(TargetUrl));
					var _pageType = util.getPageType(res);
					util.debugWriteLine(_pageType);
					
					return _pageType;
				} catch (Exception e) {
					util.debugWriteLine(e.Message + " " + e.StackTrace);
					System.Threading.Thread.Sleep(3000);
					if (isLogin) {	
						rm.form.addLogText("ログインに失敗しました。");
						isLogin = false;
					}
				}
			}
			return 5;
			
			/*
			var req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Timeout = 15000;
            req.CookieContainer = this.container;
            req.AllowAutoRedirect  = false;
            
            var res = (HttpWebResponse)req.GetResponse();
            return (res.Headers.Get("Location") == null) ? false : true;
            */
		}
		private int getPageAfterFollow(string url, string lvid) {
			Uri TargetUrl = new Uri("http://live.nicovideo.jp");
			Uri TargetUrl2 = new Uri("http://live2.nicovideo.jp");
			for (int i = 0; this == rm.rfu; i++) {
				try {
					var cg = new CookieGetter(rm.cfg);
					var cgret = cg.getHtml5RecordCookie(url);
					cgret.Wait();
					
					if (cgret == null || cgret.Result == null) {
						System.Threading.Thread.Sleep(1000);
						continue;
					}
					container = cgret.Result;
					/*
					var wc = new WebHeaderCollection();
					var referer =  "http://live.nicovideo.jp/gate/" + lvid;
					container.Add(TargetUrl, new Cookie("_gali", "jsFollowingAdMain"));
					container.Add(TargetUrl2, new Cookie("_gali", "jsFollowingAdMain"));
	//				container.Add(TargetUrl, new Cookie("_gali", "all"));
	//				container.Add(TargetUrl2, new Cookie("_gali", "all"));
					
					res = util.getPageSource(url + "?ref=grel", ref wc, container, "");
					
					var pagetype = util.getPageType(res);
					*/
					
	//				var pagetype = getPageType(url + "?ref=grel");
	//				if (pagetype != 5) return pagetype;
	//				if (res.IndexOf("会場のご案内") < 0) break;
					var _url = "http://live2.nicovideo.jp/watch/" + lvid;                              
					var req = (HttpWebRequest)WebRequest.Create(_url + "?ref=grel");
					req.Proxy = null;
					req.AllowAutoRedirect = true;
		//			req.Headers = getheaders;
					req.Referer = "http://live.nicovideo.jp/gate/" + lvid;
					container.Add(TargetUrl, new Cookie("_gali", "box" + lvid));
					if (container != null) req.CookieContainer = container;
					var _res = (HttpWebResponse)req.GetResponse();
					var dataStream = _res.GetResponseStream();
					var reader = new StreamReader(dataStream);
					res = reader.ReadToEnd();
					var getheaders = _res.Headers;
					var resCookie = _res.Cookies;
					
	//				if (res.IndexOf("会場のご案内") < 0) break;
					var pagetype = util.getPageType(res);
					if (pagetype != 5) return pagetype;
					util.debugWriteLine(i);
				} catch (Exception e) {
					util.debugWriteLine(e.Message + " " + e.StackTrace);
				}
				System.Threading.Thread.Sleep(1000);
			}
			return -1;
		}
	}
}
