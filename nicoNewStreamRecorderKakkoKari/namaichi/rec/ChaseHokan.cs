﻿/*
 * Created by SharpDevelop.
 * User: user
 * Date: 2019/06/17
 * Time: 0:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using System.Net;
using namaichi.info;

namespace namaichi.rec
{
	/// <summary>
	/// Description of ChaseHokan.
	/// </summary>
	public class ChaseHokan
	{
		private numTaskInfo nti;
		private int lastSegmentNo;
		private string[] name;
		private string lvid;
		private RecordingManager rm;
		private CookieContainer container = null;
		private Html5Recorder h5r;
		public WebSocketRecorder wr = null;
		public ChaseHokan(numTaskInfo nti, int lastSegmentNo, string[] name,
				string lvid, RecordingManager rm, Html5Recorder h5r)
		{
			this.nti = nti;
			this.lastSegmentNo = lastSegmentNo;
			this.name = name;
			this.lvid = lvid;
			this.rm = rm;
			this.h5r = h5r;
		}
		public void start() {
			container = getCookie();
			if (container == null) {
				rm.form.addLogText("補完用アカウントにログインできませんでした");
				return;
			}
			var res = getRes();
			if (res == null) {
				rm.form.addLogText("補完用アカウントでページを取得できませんでした");
				return;
			}
			var isChasable = util.getRegGroup(res, "&quot;permissions&quot;:\\[[^\\]]*(CHASE_PLAY)") != null &&
					res.IndexOf("isChasePlayEnabled&quot;:true") > -1;
			if (!isChasable) {
				rm.form.addLogText("補完用アカウントで追っかけ再生を取得できませんでした");
				return;
			}
			
			wr = getWebsocketRecorder(res);
			if (wr == null) {
				rm.form.addLogText("補完の設定に失敗しました");
			}
			try {
				wr.start();
				if (wr.isEndProgram) {
					//rm.form.addLogText("補完を完了しました " + util.getRegGroup(name, ".+_(.*)"));
					var _name = wr.rec.recFolderFile != null ? wr.rec.recFolderFile : name[0];
					rm.form.addLogText("補完を完了しました " + _name);
					return;
				}
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
			rm.form.addLogText("補完に失敗しました");
		}
		private CookieContainer getCookie() {
			try {
				var url = "https://live2.nicovideo.jp/watch/" + lvid;
				for (var i = 0; i < 3; i++) {
					var cg = new CookieGetter(rm.cfg);
					var cgret = cg.getHtml5RecordCookie(url, true);
					cgret.Wait();
					
					if (cgret == null || cgret.Result == null
					    	|| cgret.Result[0] == null) {
						System.Threading.Thread.Sleep(1000);
						continue;
					}
					return cgret.Result[0];
				}
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
			return null;
		}
		string getRes() {
			for (var i = 0; i < 3; i++) {
				try {
					var _res = util.getPageSource("https://live2.nicovideo.jp/watch/" + lvid, container);
					if (_res == null) continue;
					
					var pageType = util.getPageType(_res); 
					if (pageType == 0) {
						return _res;
					} else if (pageType == 2 || pageType == 3
							|| pageType == 7) return null;
				} catch (Exception e) {
					util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
				Thread.Sleep(3000);
				
			}
			return null;
		}
		WebSocketRecorder getWebsocketRecorder(string res) {
			try {
				var data = util.getRegGroup(res, "<script id=\"embedded-data\" data-props=\"([\\d\\D]+?)</script>");
				if (data == null) return null;
				data = System.Web.HttpUtility.HtmlDecode(data);
				var type = util.getRegGroup(res, "\"content_type\":\"(.+?)\"");
				var webSocketRecInfo = Html5Recorder.getWebSocketInfo(data, false, true, true);
				if (webSocketRecInfo == null) return null;
				
				
				//var a = recFolderFileInfo;
				//var segmentSaveType = rm.cfg.get("segmentSaveType");
				//var lastFile = util.getLastTimeshiftFileName(
				//	recFolderFileInfo[0], recFolderFileInfo[1], recFolderFileInfo[2], recFolderFileInfo[3], recFolderFileInfo[4], recFolderFileInfo[5], rm.cfg, openTime);
				//util.debugWriteLine("timeshift lastfile " + lastFile);
				//string[] lastFileTime = util.getLastTimeShiftFileTime(lastFile, segmentSaveType);
				//if (lastFileTime == null)
				//	util.debugWriteLine("timeshift lastfiletime " + 
				//	                    ((lastFileTime == null) ? "null" : string.Join(" ", lastFileTime)));
				var n = nti;
				var lastWroteSecondsAgo = (int)(((TimeSpan)(DateTime.Now - nti.dt)).TotalSeconds + (int)((nti.no - lastSegmentNo) * nti.second) + 25) * -1;
				var endSecondsAgo = (int)(((TimeSpan)(DateTime.Now - nti.dt)).TotalSeconds - 15) * -1;
				var tsConfig = new TimeShiftConfig(0, 0, 0, lastWroteSecondsAgo, 0, 0, endSecondsAgo, false, false, "", false, 0, false, false, 1, 1);
				var recFolderFile = new string[] {h5r.recFolderFile[0], name[1], null};
				/*
				var	recFolderFile = util.getRecFolderFilePath(recFolderFileInfo[0], recFolderFileInfo[1], recFolderFileInfo[2], recFolderFileInfo[3], recFolderFileInfo[4], recFolderFileInfo[5], rm.cfg, true, tsConfig, openTime, false);
				if (recFolderFile == null || recFolderFile[0] == null) {
					//パスが長すぎ
					rm.form.addLogText("パスに問題があります。 " + recFolderFile[1]);
					util.debugWriteLine("too long path? " + recFolderFile[1]);
					return null;
				}
				*/
				
				var userId = util.getRegGroup(res, "\"user\"\\:\\{\"user_id\"\\:(.+?),");
				var isPremium = res.IndexOf("\"member_status\":\"premium\"") > -1;
				return new WebSocketRecorder(webSocketRecInfo, container, recFolderFile, rm, rm.rfu, h5r, 0, true, lvid, tsConfig, userId, isPremium, TimeSpan.MaxValue, type, 0, false, false, true, false, false, null);
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				return null;
			}
		}
	}
}
