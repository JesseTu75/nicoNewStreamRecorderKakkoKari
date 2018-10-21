﻿/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/05/03
 * Time: 20:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using namaichi.rec;
using namaichi;



namespace namaichi.play
{
	/// <summary>
	/// Description of Player.
	/// </summary>
	public class Player
	{
		private MainForm form;
		private config.config config;
		private Process process = null;
		private Process commentProcess = null;
		string lastPlayUrl = null;
		private defaultFFplayController ctrl = null;
		private commentForm commentForm = null;
		private bool isDefaultPlayer = false;
		private bool isDefaultCommentPlayer = false;
		
		private bool isRecording = false;
		public bool isReconnect = false;
			
		public Player(MainForm form, config.config config)
		{
			this.form = form;
			this.config = config;
		}
		public void play() {
			util.debugWriteLine("play");
<<<<<<< HEAD
			
=======
			if (form.rec.rfu == null) return;
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
			
			if (form.playerBtn.Text == "視聴") {
				form.playerBtn.Text = "視聴停止";
				lastPlayUrl = null;
<<<<<<< HEAD
				
				Task.Run(() => {
		         	if (!getHlsUrl()) {
		         		//end();
		         		form.Invoke((MethodInvoker)delegate() {
		         			setPlayerBtnText("視聴");
		         			form.recBtn.Enabled = true;
						});
		         		form.rec.isPlayOnlyMode = false;
		         		return;
		         	}
					
					videoPlay(true);
					commentPlay(true);
				});
			} else {
				end();
				
			}
		}
		private void end() {
			Task.Run(() => {
			    setPlayerBtnText("視聴");
				stopPlaying(true, true);
				if (isDefaultPlayer) ctrlFormClose();
				if (isDefaultCommentPlayer) defaultCommentFormClose();
				
				form.Invoke((MethodInvoker)delegate() {
					form.rec.isPlayOnlyMode = false;
					
					if (!form.recBtn.Enabled) {
						form.recBtn.Enabled = true;
						form.rec.rec();
					}
				});
			});
		}
		private void videoPlay(bool isStart) {
			isRecording = true;
=======
				videoPlay(true);
				commentPlay(true);
			} else {
				Task.Run(() => {
				    setPlayerBtnText("視聴");
					stopPlaying(true, true);
					if (isDefaultPlayer) ctrlFormClose();
					if (isDefaultCommentPlayer) defaultCommentFormClose();
				});
				
			}
		}
		private void videoPlay(bool isStart) {
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
			isDefaultPlayer = bool.Parse(config.get("IsDefaultPlayer"));
			if (isStart) {
				Task.Run(() => {
					var isStarted = false;
					while (form.playerBtn.Text == "視聴停止") {
		         		if (form.rec.hlsUrl == "end") {
		         			form.rec.hlsUrl = null;
		         			Thread.Sleep(15000);
		         			stopPlaying(true, false);
		         			if (isDefaultPlayer) ctrlFormClose();
		         			break;
				        }
						if (!isPlayable() && !isStarted) {
							Thread.Sleep(300);
							continue;
						} else isStarted = true;
						
						lastPlayUrl = form.rec.hlsUrl;
						
<<<<<<< HEAD
						//isRecording = true;
=======
						isRecording = true;
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
						sendPlayCommand(isDefaultPlayer);
						
						while (true) {
							if ((form.rec.hlsUrl == "end" ||
							     form.rec.hlsUrl == null) && 
						        process.HasExited) break;
							if (form.playerBtn.Text != "視聴停止") break;
							if (form.rec.rfu == null) break;
						    
							Thread.Sleep(300);
							if ((form.rec.hlsUrl != lastPlayUrl 
								&& form.rec.hlsUrl != null    
								&& form.rec.hlsUrl.StartsWith("http")) || isReconnect) {
								isReconnect = false;
								
								stopPlaying(true, false);

								lastPlayUrl = form.rec.hlsUrl;
								sendPlayCommand(isDefaultPlayer);
//								if (isDefaultPlayer) {
//									ctrlFormClose();
//								}
								var aaa = process.HasExited;
							}
						}
						stopPlaying(true, false);
				    	if (isDefaultPlayer) ctrlFormClose();
<<<<<<< HEAD
				    	//isRecording = false;
						break;
					}
				    isRecording = false;
=======
				    	isRecording = false;
						break;
					}
				    
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
				    setPlayerBtnText("視聴");
				    
				});
				
			} else {
				
			}
			
		}
		private void sendPlayCommand(bool isDefaultPlayer) {
			if (isDefaultPlayer) {
				Environment.SetEnvironmentVariable("SDL_AUDIODRIVER", "directsound", EnvironmentVariableTarget.Process);
				var volume = (ctrl != null) ? ((ctrl.volume == -10) ? 0 : ctrl.volume) : int.Parse(config.get("volume"));
				util.debugWriteLine("kia 00 " + form.rec.hlsUrl);
				playCommand("ffplay", form.rec.hlsUrl + " -autoexit -volume " + volume);
				util.debugWriteLine("kia 0 " + ctrl);
				form.Invoke((MethodInvoker)delegate() {
					if (ctrl == null) {
						ctrl = new defaultFFplayController(config, process, this);
						ctrl.Show();
	            	} else {
	            		ctrl.process = process;
	            		ctrl.reset();
	            	}
				});
				util.debugWriteLine("kia 1 " + ctrl);
			} else {
				playCommand(config.get("anotherPlayerPath"), form.rec.hlsUrl);
			}
		}
		
		private void commentPlay(bool isStart) {
			isDefaultCommentPlayer = bool.Parse(config.get("IsDefaultCommentViewer"));
			if (isStart) {
				Task.Run(() => {
					if (isDefaultCommentPlayer) {
				        //while (form.rec.wscUrl == null && form.rec.hlsUrl != null) {
						//	Thread.Sleep(300);
						//	continue;
				        //}
				        form.Invoke((MethodInvoker)delegate() {
							commentForm = new commentForm(config, form);
							//commentForm.Show(form);
							commentForm.Show();
				        });
				        while (isRecording) {
							Thread.Sleep(300);
						}
						defaultCommentFormClose();
						
					} else {
				        commentCommand(config.get("anotherCommentViewerPath"), form.rec.recordingUrl);
						
				        /*
 						while (true) {
							Thread.Sleep(300);
							try {
								if (commentProcess.HasExited) break;
							} catch (Exception e) {
								util.debugWriteLine("comment hasexited exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
							}
						}
						*/
					}
				});
			} else {
				
			}
		}
		bool isPlayable() {
			//return form.rec.hlsUrl != null &&
			//		form.rec.hlsUrl != lastPlayUrl;
			return form.rec.hlsUrl != null;
		}
		private void playCommand(string exe, string args) {
			process = new System.Diagnostics.Process();
			process.StartInfo.FileName = exe;
//			process.StartInfo.RedirectStandardOutput = true;
//			process.StartInfo.RedirectStandardError = true;
			if (isDefaultPlayer) {
				process.StartInfo.RedirectStandardInput = true;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
			}

			util.debugWriteLine(args);
			process.StartInfo.Arguments = args;
			
			try {
				process.Start();
				
			} catch (Exception ee) {
				util.debugWriteLine(ee.Message + ee.StackTrace);
				form.addLogText("プレイヤーを開始できませんでした " + exe + " " + args);
			}
		}
		private void commentCommand(string exe, string args) {
			commentProcess = new System.Diagnostics.Process();
			commentProcess.StartInfo.FileName = exe;
//			process.StartInfo.RedirectStandardOutput = true;
//			process.StartInfo.RedirectStandardError = true;
//			process.StartInfo.RedirectStandardInput = true;
//			process.StartInfo.UseShellExecute = false;
//			process.StartInfo.CreateNoWindow = true;
			util.debugWriteLine(args);
			commentProcess.StartInfo.Arguments = args;
			
			try {
				commentProcess.Start();
				
			} catch (Exception ee) {
				util.debugWriteLine("comment exception " + ee.Message + ee.StackTrace);
				form.addLogText("コメントビューワーを開始できませんでした " + exe + " " + args);
			}
		}
		public void stopPlaying(bool isVideoStop, bool isCommentStop) {
			
			try {
				if (process != null && !process.HasExited && isVideoStop) 
					process.Kill();
				
			} catch (Exception e) {
				util.debugWriteLine("play stop " + e.Message + e.Source + e.StackTrace + e.TargetSite);
			}
			try {
				if (commentProcess != null && !commentProcess.HasExited && isCommentStop) 
					commentProcess.Kill();
				
			} catch (Exception ee) {
				util.debugWriteLine("play stop " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
				
		}
		private void setPlayerBtnText(string s) {
			form.Invoke((MethodInvoker)delegate() {
				form.playerBtn.Text = s; 
			});
		}
		private void ctrlFormClose() {
			if (ctrl == null || ctrl.IsDisposed) return;
			try {
				ctrl.Invoke((MethodInvoker)delegate() {
					try {
				    	ctrl.Close();
				    } catch (Exception e) {
				    	util.debugWriteLine("ctrl close exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
	           		}
				});
			} catch (Exception ee) {
				util.debugWriteLine("ctrl close2 exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
			ctrl = null;
		}
		private void defaultCommentFormClose() {
			if (commentForm == null || commentForm.IsDisposed) return;
			try {
				commentForm.Invoke((MethodInvoker)delegate() {
					try {
				    	commentForm.Close();
				    } catch (Exception e) {
				    	util.debugWriteLine("comment form close exception " + e.Message + e.Source + e.StackTrace + e.TargetSite);
	           		}
				});
			} catch (Exception ee) {
				util.debugWriteLine("comment form close2 exception " + ee.Message + ee.Source + ee.StackTrace + ee.TargetSite);
			}
		}
		public void setCtrlFormKeikaJikan(string s) {
			if (ctrl != null) ctrl.setTimeLabel(s);
			
			if (util.getRegGroup(s, "(\\d+:\\d+)/") != null) {
				var __m = util.getRegGroup(s, "(\\d+):\\d+");
				var __s = util.getRegGroup(s, "\\d+:(\\d+)");
				if (__m == null || __s == null) return;
				if (commentForm != null) commentForm.setTime(0, int.Parse(__m), int.Parse(__s));
			} else {
				var __h = util.getRegGroup(s, "(\\d+):");
				var __m = util.getRegGroup(s, "\\d+:(\\d+):");
				var __s = util.getRegGroup(s, "\\d+:\\d+:(\\d+)");
				if (__h == null || __m == null || __s == null) return;
				if (commentForm != null) commentForm.setTime(int.Parse(__h), int.Parse(__m), int.Parse(__s));
			}
		}
		public void addComment(string time, string contents, string userId, string score, string color) {
			if (commentForm != null) commentForm.addComment(time, contents, userId, score, color);
		}
		public void setStatistics(string visit, string comment) {
			if (commentForm != null) commentForm.setStatistics(visit, comment);
		}
<<<<<<< HEAD
		private bool getHlsUrl() {
			if (form.rec.rfu == null) {
				form.rec.hlsUrl = null;
				form.rec.isPlayOnlyMode = true;
				form.rec.rec();
				form.Invoke((MethodInvoker)delegate() {
					form.recBtn.Enabled = false;
				});
				if (form.rec.rfu == null) return false;
				while(form.rec.rfu != null) {
					if (form.rec.hlsUrl == "end") return false;
					if (form.rec.hlsUrl != null) {
						
						return true;
					}
					Thread.Sleep(300);
				}
				return false;
			}
			return true;
		}
=======
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
	}
}
