﻿/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/05/29
 * Time: 14:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using namaichi.config;

namespace namaichi.rec
{
	/// <summary>
	/// Description of FollowCommunity.
	/// </summary>
	public class FollowCommunity
	{
<<<<<<< HEAD
		private bool isSub;
		public FollowCommunity(bool isSub)
		{
			this.isSub = isSub;
		}
		public bool followCommunity(string res, CookieContainer cc, MainForm form, config.config cfg) {
			var isJikken = res.IndexOf("siteId&quot;:&quot;nicocas") > -1;
			var comId = (isJikken) ? util.getRegGroup(res, "&quot;followPageUrl&quot;\\:&quot;.+?motion/(.+?)&quot;") :
					util.getRegGroup(res, "Nicolive_JS_Conf\\.Recommend = \\{type\\: 'community', community_id\\: '(co\\d+)'\\};");
=======
		public FollowCommunity()
		{
		}
		public bool followCommunity(string res, CookieContainer cc, MainForm form, config.config cfg) {
			var comId = util.getRegGroup(res, "Nicolive_JS_Conf\\.Recommend = \\{type\\: 'community', community_id\\: '(co\\d+)'\\};");
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
			if (comId == null) {
				form.addLogText("この放送はフォローできませんでした。");
				return false;
			}
			
<<<<<<< HEAD
			var isJoinedTask = join(comId, cc, form, cfg, isSub);
=======
			var isJoinedTask = join(comId, cc, form, cfg);
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
//			isJoinedTask.Wait();
			return isJoinedTask;
//			return false;
		}
<<<<<<< HEAD
		private bool join(string comId, CookieContainer cc, MainForm form, config.config cfg, bool isSub) {
=======
		private bool join(string comId, CookieContainer cc, MainForm form, config.config cfg) {
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
			for (int i = 0; i < 5; i++) {
				var myPageUrl = "http://www.nicovideo.jp/my";
				var comUrl = "https://com.nicovideo.jp/community/" + comId; 
				var url = "https://com.nicovideo.jp/motion/" + comId;
				var headers = new WebHeaderCollection();
				headers.Add("Upgrade-Insecure-Requests", "1");
				headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.87 Safari/537.36");
				try {
					var cg = new CookieGetter(cfg);
					var cgret = cg.getHtml5RecordCookie(url);
					cgret.Wait();
					                                  
					
		//			cgret.ConfigureAwait(false);
					if (cgret == null || cgret.Result == null) {
						System.Threading.Thread.Sleep(3000);
						continue;
					}
<<<<<<< HEAD
					var _cc = cgret.Result[(isSub) ? 1 : 0];
//					util.debugWriteLine(cg.pageSource);
					
					var isJidouShounin = util.getPageSource(url, ref headers, _cc, comUrl).IndexOf("自動承認されます") > -1;
=======
					cc = cgret.Result;
//					util.debugWriteLine(cg.pageSource);
					
					var isJidouShounin = util.getPageSource(url, ref headers, cc, comUrl).IndexOf("自動承認されます") > -1;
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
	//				var _compage = util.getPageSource(url, ref headers, cc);
	//				var gateurl = "http://live.nicovideo.jp/gate/lv313793991";
	//				var __gatePage = util.getPageSource(gateurl, ref headers, cc);
	//				var _compage2 = util.getPageSource(url, ref headers, cc);
					util.debugWriteLine(cc.GetCookieHeader(new Uri(url)));
					form.addLogText(isJidouShounin ? "フォローを試みます。" : "自動承認ではありませんでした。");
					
					if (!isJidouShounin) return false;
				} catch (Exception e) {
					return false;
				}
				
				
				try {
					var handler = new System.Net.Http.HttpClientHandler();
					handler.UseCookies = true;
					handler.CookieContainer = cc;
					handler.Proxy = null;
					
					
					var http = new System.Net.Http.HttpClient(handler);
					http.DefaultRequestHeaders.Referrer = new Uri(url);
					
					var content = new System.Net.Http.FormUrlEncodedContent(new Dictionary<string, string>
					{
						{"mode", "commit"}, {"title", "フォローリクエスト"}
					});
					
					var enc = Encoding.GetEncoding("UTF-8");
					string data =
					    "mode=commit&title=" + System.Web.HttpUtility.UrlEncode("フォローリクエスト", enc);
					byte[] postDataBytes = Encoding.ASCII.GetBytes(data);
	
					
					var req = (HttpWebRequest)WebRequest.Create(url);
					req.Method = "POST";
					req.Proxy = null;
					req.CookieContainer = cc;
					req.Referer = url;
					req.ContentLength = postDataBytes.Length;
					req.ContentType = "application/x-www-form-urlencoded";
	//				req.Headers.Add("Referer", url);
					using (var stream = req.GetRequestStream()) {
						try {
							stream.Write(postDataBytes, 0, postDataBytes.Length);
						} catch (Exception e) {
				       		util.showException(e);
				       	}
					}
	//					stream.Close();
					
	
					var res = req.GetResponse();
					
					var resStream = new System.IO.StreamReader(res.GetResponseStream());
					var resStr = resStream.ReadToEnd();
	
					var isSuccess = resStr.IndexOf("フォローしました") > -1;
					form.addLogText(isSuccess ?
		                	"フォローしました。録画開始までしばらくお待ちください。" : "フォローに失敗しました。");
					return isSuccess;
					
	//				resStream.Close();
					
					
	//				Task<HttpResponseMessage> _resTask = http.PostAsync(url, content);
					
	//				_resTask.Wait();
	//				var _res = _resTask.Result;
					
	//				var resTask = _res.Content.ReadAsStringAsync();
	//				resTask.Wait();
	//				var res = resTask.Result;
		//			var a = _res.Headers;
					
		//			if (res.IndexOf("login_status = 'login'") < 0) return null;
					
	//				var cc = handler.CookieContainer;
					
				} catch (Exception e) {
					form.addLogText("フォローに失敗しました。");
					util.debugWriteLine(e.Message+e.StackTrace);
					continue;
//					return false;
				}
			}
			form.addLogText("フォローに失敗しました。");
			util.debugWriteLine("フォロー失敗");
			return false;
		}
	}
}
