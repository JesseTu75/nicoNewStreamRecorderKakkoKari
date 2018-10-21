<<<<<<< HEAD
﻿/*
 * Created by SharpDevelop.
 * User: user
 * Date: 2018/09/21
 * Time: 0:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Net;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using rokugaTouroku;
using rokugaTouroku.info;

namespace rokugaTouroku.rec
{
	/// <summary>
	/// Description of RecDataGetter.
	/// </summary>
	public class RecDataGetter
	{
		private RecListManager rlm;
		
		//public bool isStop = false;
		
		public RecDataGetter(RecListManager rlm)
		{
			this.rlm = rlm;
		}
		public void rec() {
			while (true) {
				try {
					var isAllEnd = true;
					
					var _count = rlm.form.getRecListCount();
					util.debugWriteLine("rlm.reclistdata.count " + _count + " reclist count " + rlm.form.recList.Rows.Count);
					for (var i = 0; i < _count; i++) {
						util.debugWriteLine("i " + i + " count " + _count);
						RecInfo ri = (RecInfo)rlm.recListData[i];
						util.debugWriteLine(i + " " + ri);
						
						if (ri == null) continue;
						if (ri.state == "待機中" || ri.state == "録画中") isAllEnd = false;
						if (ri.state != "待機中") continue;
						
						Task.Run(() => {recProcess(ri);});
					}
					util.debugWriteLine(isAllEnd);
					if (isAllEnd) break;
				} catch (Exception e) {
					util.debugWriteLine("rdg rec exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
				
				
				Thread.Sleep(1000);
			}
		}
		private void recProcess(RecInfo ri) {
			util.debugWriteLine("recProcess " + ri.id);
			ri.state = "録画中";
			var row = rlm.recListData.IndexOf(ri);
			if (row == -1) return;
			rlm.form.resetBindingList(row, "状態", "録画中");
			startRecProcess(ri);
			var r = ri.process.StandardOutput;
			var w = ri.process.StandardInput;
			while (!ri.process.HasExited && rlm.rdg == this) {
				var res = r.ReadLine();
				if (res == null) break;
				util.debugWriteLine("res " + res);
				
				readResProcess(res, w, ri);
			}
			ri.state = (ri.process.ExitCode == 5) ? "録画完了" : "録画失敗";
			rlm.form.resetBindingList(row);
		}
		private void startRecProcess(RecInfo ri) {
			util.debugWriteLine("startrecprocess " + ri);
			try {
				ri.process = new Process();
				var si = new ProcessStartInfo();
				si.FileName = "ニコ生新配信録画ツール（仮.exe";
				//si.FileName = "nicoNewStreamRecorderKakkoKari.exe";
				si.Arguments = "-nowindo -stdIO -IsmessageBox=false -IscloseExit=true " + ri.id + " -ts-start=" + ri.tsConfig.timeSeconds + "s -ts-end=" + ri.tsConfig.endTimeSeconds + "s -afterConvertMode=" + ri.getAfterConvertTypeNum() + " -qualityRank=" + ri.qualityRank + " -IsLogFile=false";
				util.debugWriteLine(si.Arguments);
				//si.CreateNoWindow = true;
				si.UseShellExecute = false;
				//si.WindowStyle = ProcessWindowStyle.Hidden;
				si.RedirectStandardInput = true;
				si.RedirectStandardOutput = true;
				si.RedirectStandardError = true;
				ri.process.StartInfo = si;
				ri.process.Start();
			} catch (Exception e) {
				rlm.form.addLogText("ニコ生新配信録画ツール（仮.exeを呼び出せませんでした");
				util.debugWriteLine("process start exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
		}
		private void readResProcess(string res, StreamWriter w, RecInfo ri) {
			if (res.StartsWith("info")) {
				setInfo(res, ri);
				return;
			}
			if (res.StartsWith("msgbox:")) {
				//showMsgBox(res);
			}
			
			
		}
		private void setInfo(string res, RecInfo ri) {
			if (res.StartsWith("info.title:")) 
				ri.title = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.host:")) 
				ri.host = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.communityName:")) 
				ri.communityName = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.url:")) 
				ri.url = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.communityUrl:")) 
				ri.communityUrl = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.description:")) 
				ri.description = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.startTime:")) 
				ri.startTime = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.endTime:")) 
				ri.endTime = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.programTime:")) 
				ri.programTime = util.getRegGroup(res, ":(.*)");
			if (res.StartsWith("info.keikaTime:")) {
				ri.keikaTimeStart = DateTime.Parse(util.getRegGroup(res, ":(.*)"));
			}
			if (res.StartsWith("info.samuneUrl:")) 
				ri.samune = getSamune(util.getRegGroup(res, ":(.*)"));
			if (res.StartsWith("info.log:")) {
				if (ri.log != "") ri.log += "\r\n";
				ri.log += util.getRegGroup(res, ":(.*)");
			}
			var ctrl = util.getRegGroup(res, "\\.(.+?):");
			var val = util.getRegGroup(res, ":(.+)");
			
			var row = rlm.recListData.IndexOf(ri);
			if (row == -1) return;
			rlm.form.resetBindingList(row);
			
			var _count = rlm.form.getRecListCount();
			util.debugWriteLine("setinfo c " + _count + " rowindex " + rlm.form.recList.SelectedCells[0].RowIndex);
			try {
				if (rlm.form.recList.SelectedCells.Count > 0 &&
				    rlm.recListData[rlm.form.recList.SelectedCells[0].RowIndex] == ri) 
					rlm.form.displayRiInfo(ri, ctrl, val);
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
			util.debugWriteLine("setinfo ok");
		}
		public void stopRecording() {
			foreach (RecInfo ri in rlm.recListData) {
				try {
					if (ri.process.HasExited) continue;
					ri.process.Kill();
				} catch (Exception e) {
					util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
			}
		}
		private Bitmap getSamune(string url) {
       		WebClient cl = new WebClient();
       		cl.Proxy = null;
			
       		System.Drawing.Icon icon =  null;
			try {
       			byte[] pic = cl.DownloadData(url);
				
				var  st = new System.IO.MemoryStream(pic);
				icon = Icon.FromHandle(new System.Drawing.Bitmap(st).GetHicon());
				st.Close();
				
			} catch (Exception e) {
				util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.Source + " " + e.TargetSite);
				return null;
			}
			return icon.ToBitmap();
		}
	}
}
=======
﻿/*
 * Created by SharpDevelop.
 * User: user
 * Date: 2018/09/21
 * Time: 0:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Net;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using rokugaTouroku;
using rokugaTouroku.info;

namespace rokugaTouroku.rec
{
	/// <summary>
	/// Description of RecDataGetter.
	/// </summary>
	public class RecDataGetter
	{
		private RecListManager rlm;
		
		//public bool isStop = false;
		
		public RecDataGetter(RecListManager rlm)
		{
			this.rlm = rlm;
		}
		public void rec() {
			while (true) {
				try {
					var isAllEnd = true;
					for (var i = 0; i < rlm.recListData.Count; i++) {
						RecInfo ri = (RecInfo)rlm.recListData[i];
						if (ri == null) continue;
						if (ri.state == "待機中" || ri.state == "録画中") isAllEnd = false;
						if (ri.state != "待機中") continue;
						
						Task.Run(() => {recProcess(ri);});
					}
					util.debugWriteLine(isAllEnd);
					if (isAllEnd) break;
				} catch (Exception e) {
					util.debugWriteLine("rdg rec exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
				
				
				Thread.Sleep(1000);
			}
		}
		private void recProcess(RecInfo ri) {
			ri.state = "録画中";
			rlm.form.resetBindingList();
			startRecProcess(ri);
			var r = ri.process.StandardOutput;
			var w = ri.process.StandardInput;
			while (!ri.process.HasExited) {
				var res = r.ReadLine();
				readResProcess(res, w, ri);
			}
			ri.state = (ri.process.ExitCode == 5) ? "録画完了" : "録画失敗";
			rlm.form.resetBindingList();
		}
		private void startRecProcess(RecInfo ri) {
			try {
				ri.process = new Process();
				var si = new ProcessStartInfo();
				si.FileName = "ニコ生新配信録画ツール（仮.exe";
				si.Arguments = "redist " + ri.id + " " + ri.afterFFmpegMode;
				si.CreateNoWindow = true;
				si.UseShellExecute = false;
				si.RedirectStandardInput = true;
				si.RedirectStandardOutput = true;
				si.RedirectStandardError = true;
				ri.process.Start(si);
			} catch (Exception e) {
				rlm.form.addLogText("ニコ生新配信録画ツール（仮.exeを呼び出せませんでした");
				util.debugWriteLine("process start exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
		}
		private void readResProcess(string res, StreamWriter w, RecInfo ri) {
			if (res.StartsWith("info:title")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			if (res.StartsWith("info:host")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			if (res.StartsWith("info:title")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			if (res.StartsWith("info:title")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			if (res.StartsWith("info:title")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			if (res.StartsWith("info:title")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			if (res.StartsWith("info:title")) 
				ri.title = util.getRegGroup(res, "/(.*)");
			
		}
	}
}
>>>>>>> 1faa06f1cca31cbe7e39015381b5150050941e1c
