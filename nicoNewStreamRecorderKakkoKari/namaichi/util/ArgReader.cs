﻿/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/09/29
 * Time: 20:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using namaichi.config;
using namaichi.info;

namespace namaichi.utility
{
	/// <summary>
	/// Description of ArgReader.
	/// </summary>
	public class ArgReader
	{
		private string[] args;
		private MainForm form;
		public bool isConcatMode = false;
		public string allPathStr;
		public Dictionary<string, string> argConfig = new Dictionary<string, string>();
		public string lvid = null;
		public config.config config;
		public TimeShiftConfig tsConfig;
		
		public ArgReader(string[] args, config.config config, MainForm form)
		{
			this.args = args;
			this.config = config;
			this.form = form;
		}
		public void read() {
			if (isAllPath()) {
				isConcatMode = true;
				return;
			}
			setArgConfig();
			util.debugWriteLine("args " + string.Join(" ", args));
			foreach(var a in argConfig) util.debugWriteLine(a.Key + " " + a.Value);
			
		}
		private bool isAllPath() {
			var isAllPath = true;
			foreach (var a in args) {
				try {
					if (!File.Exists(a) && !Directory.Exists(a)) isAllPath = false;
				} catch (Exception e) {
					isAllPath = false;
					util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
				}
			}
			if (isAllPath) allPathStr = string.Join("|", args);
			return isAllPath;
		}
		private void setArgConfig() {
			var lowKeys = new List<string>(config.defaultConfig.Keys.Select((x) => x.ToLower()));
			var values = config.defaultConfig.Values.ToList<string>();
			var keys = config.defaultConfig.Keys.ToList();
			lowKeys.AddRange(new string[] {"ts-start", "ts-end", "ts-list", "ts-list-update", "ts-list-command"});
			foreach (var a in args) {
				if (a.StartsWith("-")) {
					var name = util.getRegGroup(a, "-(.*)=");
					var val = util.getRegGroup(a, "=(.*)");
					if (name == null) continue;
					
					string setVal = null;
					string setName = null;
					if (!isValidConf(name, val, lowKeys, values, out setVal, out setName, keys)) continue;
					//argConfig.Add(setName, setVal);
					argConfig[setName] = setVal;
				} else {
					if (lvid == null) lvid = util.getRegGroup(a, "(lv\\d+)");
				}
			}
		}
		private bool isValidConf(string name, string val, List<string> lowKeys, List<string> defValues, out string setVal, out string setName, List<string> keys) {
			setVal = null;
			setName = null;
			for (var i = 0; i < lowKeys.Count; i++) {
				if (name.ToLower() != lowKeys[i]) continue;
				if (i < defValues.Count && (defValues[i] == "true" || defValues[i] == "false")) {
				    if (val.ToLower() == "true" || val.ToLower() == "false") {
						setVal = val.ToLower();
						setName = keys[i];
						return true;
				   	} else {
						form.addLogText(name + "の値が設定できませんでした(true or false) " + val, false);
						return false;;
					}
				}
				if (lowKeys[i] == "browsernum") {
					if (val == "1" || val == "2") {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(1 or 2) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "qualityrank") {
					if (isValidQualityRank(val)) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(例 「0,1,2,5,4,3」) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "segmentsavetype") {
					if (val == "0" || val == "1") {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(0 or 1) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "m3u8updateseconds") {
					double _s = 0;
					if (double.TryParse(val, out _s) && _s > 0) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(0以上の数値) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "subfoldernametype") {
					int _s = 0;
					if (int.TryParse(val, out _s) && _s >= 1 && _s <= 8) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(1から8の整数) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "filenametype") {
					int _s = 0;
					if (int.TryParse(val, out _s) && _s >= 1 && _s <= 10) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(1から10の整数) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "filenameformat") {
					if (val.IndexOf("{0}") > -1) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした({0}を含む文字列) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "volume") {
					int _s = 0;
					if (int.TryParse(val, out _s) && _s >= 0 && _s <= 100) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(0から100の整数) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "afterConvertMode") {
					int _s = 0;
					if (int.TryParse(val, out _s) && _s >= 0 && _s <= 12) {
						setVal = val;
						setName = keys[i];
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(0から12の整数) " + val, false);
						return false;
					}
				}
				//ts
				if (lowKeys[i] == "ts-start") {
					var _t = Regex.Match(val.ToLower(), "((\\d*)h)*((\\d*)m)*((\\d*)s)*");
					if (_t.Length != 0) {
						if (tsConfig == null) tsConfig = new TimeShiftConfig();
						
						tsConfig.timeType = 0;
						tsConfig.timeSeconds = 0;
						if (_t.Groups[2].Length != 0) tsConfig.timeSeconds += int.Parse(_t.Groups[2].Value) * 3600;
						if (_t.Groups[4].Length != 0) tsConfig.timeSeconds += int.Parse(_t.Groups[4].Value) * 60;
						if (_t.Groups[6].Length != 0) tsConfig.timeSeconds += int.Parse(_t.Groups[6].Value);
						return false;
					} else if (val.ToLower() == "continue" || 
				    		val.ToLower() == "continue-concat") {
						if (tsConfig == null) tsConfig = new TimeShiftConfig();
						
						tsConfig.timeType = 1;
						tsConfig.isContinueConcat = val.ToLower() == "continue-concat";  
						return false;
					} else {
						form.addLogText(name + "の値が設定できませんでした(0h0m0s形式の時間 or continue or continue-concat) " + val, false);
						return false;
					}
				}
				if (lowKeys[i] == "ts-end") {
					var _t = Regex.Match(val.ToLower(), "((\\d*)h)*((\\d*)m)*((\\d*)s)*");
					if (_t.Length != 0) {
						if (tsConfig == null) tsConfig = new TimeShiftConfig();
						
						tsConfig.endTimeSeconds = 0;
						if (_t.Groups[2].Length != 0) tsConfig.endTimeSeconds += int.Parse(_t.Groups[2].Value) * 3600;
						if (_t.Groups[4].Length != 0) tsConfig.endTimeSeconds += int.Parse(_t.Groups[4].Value) * 60;
						if (_t.Groups[6].Length != 0) tsConfig.endTimeSeconds += int.Parse(_t.Groups[6].Value);
						return false;
					} else {
						form.addLogText(name + "の値が設定できませんでした(0h0m0s形式の時間) " + val, false);
						return false;
					}
				}
				//ts list
				if (lowKeys[i] == "ts-list") {
					if (val.ToLower() == "true" || val.ToLower() == "false") {
						setName = "IsUrlList";
						setVal = val;
						return true;
					} else {
						form.addLogText(name + "の値が設定できませんでした(true or false) " + val, false);
						return false;;
					}
				}
				if (lowKeys[i] == "ts-list-open") {
					if (val.ToLower() == "true" || val.ToLower() == "false") {
						setName = "IsOpenUrlList";
						setVal = val;
						return true;
				   	} else {
						form.addLogText(name + "の値が設定できませんでした(true or false) " + val, false);
						return false;;
					}
				}
				if (lowKeys[i] == "ts-list-m3u8") {
					if (val.ToLower() == "true" || val.ToLower() == "false") {
						setName = "IsM3u8List";
						setVal = val;
						return true;
				   	} else {
						form.addLogText(name + "の値が設定できませんでした(true or false) " + val, false);
						return false;;
					}
				}
				if (lowKeys[i] == "ts-list-update") {
					double _s = 0;
					if (double.TryParse(val, out _s) && _s > 0) {
						setName = "M3u8UpdateSeconds";
						setVal = val;
						return true;
				   	} else {
						form.addLogText(name + "の値が設定できませんでした(0以上の数値) " + val, false);
						return false;;
					}
				}
				if (lowKeys[i] == "ts-list-command") {
//					if (val.ToLower() == "true" || val.ToLower() == "false") {
						setName = "openUrlListCommand";
						setVal = val;
						return true;
					/*
				   	} else {
						form.addLogText(name + "の値が設定できませんでした(0以上の数値) " + val, false);
						return false;;
					}
					*/
				}
				setName = keys[i];
				setVal = val;
				return true;
			}
			return false;
			    
		}
		private bool isValidQualityRank(string val) {
			try {
				var l = val.Split(',').Select((x) => int.Parse(x));
				if (l.Count() != 6) return false;
				var a = new List<int>{0,1,2,3,4,5};
				foreach (var _l in l) a.Remove(_l);
				return a.Count == 0;
			} catch (Exception e) {
				return false;
			}
		}

	}
}
