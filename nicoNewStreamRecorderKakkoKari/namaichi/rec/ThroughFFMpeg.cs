﻿/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/07/27
 * Time: 17:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.IO;

namespace namaichi.rec
{
	/// <summary>
	/// Description of ThroughFFMpeg.
	/// </summary>
	public class ThroughFFMpeg
	{
		private System.Diagnostics.Process process;
		private RecordingManager rm;
		
		public ThroughFFMpeg(RecordingManager rm)
		{
			this.rm = rm;
		}
		public void start(string path, bool isConvert) {
			util.debugWriteLine("through ffmpeg path " + path);
			rm.form.addLogText("FFmpeg処理を開始します");
			var fName = util.getRegGroup(path, ".+(\\\\|/)(.+)", 2);
			var dir = util.getRegGroup(path, "(.+(\\\\|/)).+");
			string tmp = dir + "_" + fName;
			util.debugWriteLine("through ffmpeg tmp " + tmp);
			
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> b77d287f700e628ca0b621134ab8ddd993dbb4fc
			var afterConvertMode = int.Parse(rm.cfg.get("afterConvertMode"));
			string outPath = path;
			if (isConvert) 
				getConvertPaths(path, ref tmp, ref outPath, afterConvertMode);
			string _command;
			if (afterConvertMode == 8 || afterConvertMode == 6 || afterConvertMode == 9)
				_command = ("-i \"" + path + "\" \"" + tmp + "\"");
			else if (afterConvertMode == 10)
				_command =  ("-i \"" + path + "\" -vn -c copy \"" + tmp + "\"");
			else if (afterConvertMode == 12)
				_command =  ("-i \"" + path + "\" -vn \"" + tmp + "\"");
			else if (afterConvertMode == 3)
				_command = ("-i \"" + path + "\" -c copy -bsf:a aac_adtstoasc \"" + tmp + "\"");
			else _command = ("-i \"" + path + "\" -c copy \"" + tmp + "\"");
<<<<<<< HEAD
=======
=======
<<<<<<< HEAD
			var _command = getCommand(path, tmp);
			
>>>>>>> 1faa06f1cca31cbe7e39015381b5150050941e1c
>>>>>>> b77d287f700e628ca0b621134ab8ddd993dbb4fc
			
=======
			string _command = "-i \"" + path + "\" -c copy \"" + tmp + "\"";
//			string _command = "-i \"" + path + "\" -movflags faststart -c copy \"" + tmp + "\"";
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
			
			util.debugWriteLine("through command " + _command);
			
			var e = new EventHandler(appExitHandler);
			Application.ApplicationExit += e;
			
			process = new System.Diagnostics.Process();
			process.StartInfo.FileName = "" + util.getJarPath()[0] + 
				("\\ffmpeg") + "";
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.Arguments = _command;
		
			try {
				process.Start();
				
//				Task.Run(() => {
//				         	getStandardOutput();
//				});
				
				displayRecordStatus();
//				util.debugWriteLine("stop record");
//				stopRecording();
				Application.ApplicationExit -= e;
				
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + ee.StackTrace);
			}
			
			
			try {
				if (!File.Exists(tmp)) {
					util.debugWriteLine("through ffmpeg not exist tmp " + tmp);
					return;
				}
				File.Delete(path);
				File.Move(tmp, outPath);
			} catch (Exception eee) {
				util.debugWriteLine("through ffmpeg delete move exception");
				util.debugWriteLine(path + " " + tmp);
				util.debugWriteLine(eee.Message + eee.Source + eee.StackTrace + eee.TargetSite);
			}
			
			
//			while(true)
//				System.Threading.Thread.Sleep(3000);
			rm.form.addLogText("FFmpeg処理を完了しました");
			util.debugWriteLine("rec end through ffmpeg");
			
		}
		private void appExitHandler(object sender, EventArgs e) {
//			stopRecording();
		}
<<<<<<< HEAD
		private void getConvertPaths(string path, ref string tmp, ref string outPath, int afterConvertMode) {
			var ext = "";
//			if (afterConvertMode == 0 &&
//			    rm.cfg.get("IsRenketuAfter") == "true") ext = "ts";
			if (afterConvertMode == 0) ext = "ts";
=======
<<<<<<< HEAD
		private void getConvertPaths(string path, ref string tmp, ref string outPath, int afterConvertMode) {
			var ext = "";
<<<<<<< HEAD
//			if (afterConvertMode == 0 &&
//			    rm.cfg.get("IsRenketuAfter") == "true") ext = "ts";
			if (afterConvertMode == 0) ext = "ts";
=======
			if (afterConvertMode == 0 &&
			    rm.cfg.get("IsRenketuAfter") == "true") ext = "ts";
>>>>>>> b77d287f700e628ca0b621134ab8ddd993dbb4fc
>>>>>>> 7626697f2a2b755bc0d503452dc12ba5710be7f0
			if (afterConvertMode == 1) ext = "avi";
			if (afterConvertMode == 2) ext = "mp4";
			if (afterConvertMode == 3) ext = "flv";
			if (afterConvertMode == 4) ext = "mov";
			if (afterConvertMode == 5) ext = "wmv";
			if (afterConvertMode == 6) ext = "vob";
			if (afterConvertMode == 7) ext = "mkv";
			if (afterConvertMode == 8) ext = "mp3";
			if (afterConvertMode == 9) ext = "wav";
			if (afterConvertMode == 10) ext = "wma";
			if (afterConvertMode == 11) ext = "aac";
			if (afterConvertMode == 12) ext = "ogg";
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 7626697f2a2b755bc0d503452dc12ba5710be7f0
			var originalExtLen = tmp.EndsWith("ts") ? 2 : 3;
			tmp = tmp.Substring(0, tmp.Length - originalExtLen) + ext;
//			tmp = tmp.Substring(0, tmp.Length - 2) + ext;
			outPath = path.Substring(0, path.Length - originalExtLen) + ext;
<<<<<<< HEAD
=======
=======
			tmp = tmp.Substring(0, tmp.Length - 2) + ext;
			outPath = path.Substring(0, path.Length - 2) + ext;
=======
<<<<<<< HEAD
		private string getCommand(string path, string tmp) {
			var def = "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			if (rm.ri == null)
				return def;
//			string _command = "-i \"" + path + "\" -movflags faststart -c copy \"" + tmp + "\"";
			if (rm.ri.afterFFmpegMode == 0)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			else if (rm.ri.afterFFmpegMode == 1)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			else if (rm.ri.afterFFmpegMode == 1)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			else if (rm.ri.afterFFmpegMode == 1)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			else if (rm.ri.afterFFmpegMode == 1)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			else if (rm.ri.afterFFmpegMode == 1)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			else if (rm.ri.afterFFmpegMode == 1)
				return "-i \"" + path + "\" -c copy \"" + tmp + "\"";
			return def;
>>>>>>> 1faa06f1cca31cbe7e39015381b5150050941e1c
>>>>>>> b77d287f700e628ca0b621134ab8ddd993dbb4fc
>>>>>>> 7626697f2a2b755bc0d503452dc12ba5710be7f0
		}
=======
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
		/*
		public void stopRecording() {
			util.debugWriteLine("stop recording through ffmpeg");
	
			if (process == null || process.HasExited) return;
			
			
			try {
				var i = process.StandardInput;
				i.WriteLine("q");
				i.Flush();
				i.Close();
			} catch (Exception ee) {
				if (!(ee is  System.ObjectDisposedException))
					util.debugWriteLine(ee.Message + ee.StackTrace);
				try {
					process.Kill();
				} catch (Exception eee) {
					util.debugWriteLine(eee.Message + eee.StackTrace);
				}
			};
			
			while(!process.HasExited) {
				System.Threading.Thread.Sleep(200);
			}
			
			util.debugWriteLine("destroy " + process.ExitCode);
		}
		*/
		public void displayRecordStatus() {
			var es = process.StandardError;
			
			
			while (!process.HasExited) {
				try {
					var line = es.ReadLine();
//					lastReadTime = DateTime.UtcNow;
					
					if (line == null) break;
					
					util.debugWriteLine("error " + line);
					displayStateGui(line);
					
				} catch (Exception e) {
					util.debugWriteLine(e.Message + " " + e.StackTrace + " " + e.TargetSite);
				};
				
//				if (rm.rfu != rfu) stopRecording();
			}
			try {
				
				es.Close();
			} catch (Exception e) {
				util.debugWriteLine(e.Message + e.StackTrace);
			}

		}
		private void displayStateGui(string line) {
			if (line.StartsWith("[hls")) return;
			if (line.IndexOf("Cannot reuse HTTP") != -1) return;
			if (line.IndexOf("Opening") != -1) return;
			if (line.IndexOf("Last message") != -1) return;
			if (line.IndexOf("skipping") != -1) return;
			if (line.IndexOf("expire") != -1) return;
			if (line.IndexOf("retrying with new") != -1) return;
			if (line.IndexOf("different host") != -1) return;
             
			if (line.IndexOf("http @ ") != -1) return;
			if (line.IndexOf("for reading") != -1) return;
			if (line.IndexOf("may result in incorrect") != -1) return;
			
			if (line.StartsWith("frame=")) rm.form.setRecordState(line);
			
//				util.getShiftJisToUni
//			else rm.form.addLogText(line);
			
		}
	}
}
