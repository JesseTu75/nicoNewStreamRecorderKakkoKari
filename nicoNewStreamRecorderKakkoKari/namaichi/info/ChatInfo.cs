﻿/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/04/21
 * Time: 21:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Xml.Linq;
using System.Xml;
	
namespace namaichi.info
{
	/// <summary>
	/// Description of ChatInfo.
	/// </summary>
	public class ChatInfo
	{
		private XDocument xml;
<<<<<<< HEAD
		public string json;
=======
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
		public string contents;
		public string premium;
		public string root;
		public long serverTime;
		public int date;
		public long date_usec;
		public long vpos;
		public string userId;
		public string score = "0";
		public string ticket;
<<<<<<< HEAD
		public string lastRes;
		public bool isPingRf;
		public long vposOriginal;
		
		public ChatInfo(XDocument xml, string json)
		{
			this.xml = xml;
			this.json =json;
		}
=======
		
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
		public ChatInfo(XDocument xml)
		{
			this.xml = xml;
		}
		public XDocument getFormatXml(long serverTime) {
			this.serverTime = serverTime;
			//xml.Root
//			util.debugWriteLine(xml.Root);
			var _xml = new XDocument();
			_xml.Add(new XElement(xml.Root.Name));
			root = xml.Root.Name.ToString();
//			util.debugWriteLine(xml.Root.Name);
			
//			var atts = _xml.Root.Attributes();
			Object[] o = new Object[20];
			
			date = 0;
<<<<<<< HEAD
			
//			foreach(var a in xml.Root.Elements()) {
//				util.debugWriteLine(a.Name + a.Value);
//			}
			
=======
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
			foreach (XElement e in xml.Root.Elements()) {
//				util.debugWriteLine(xml.Root);
//				o[0] = new XAttribute(e.Name, e.Value);
//				_xml.Root.SetAttributeValue(e.Name, e.Value);
<<<<<<< HEAD
				if ((root == "chat" && e.Name == "content") ||
				    (root == "control" && e.Name == "text") ||
				    (root == "ping" && e.Name == "content")) {
=======
				if (e.Name == "content") {
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
					_xml.Root.Add(e.Value);
					contents = e.Value;
				} else _xml.Root.SetAttributeValue(e.Name, e.Value);
				if (e.Name == "premium") premium = e.Value;
				if (e.Name == "server_time") 
					this.serverTime = int.Parse(e.Value);
				if (e.Name == "date") date = int.Parse(e.Value);
//				_xml.Root.Add(new XAttribute(e.Name, e.Value));
				if (e.Name == "date_usec") date_usec = int.Parse(e.Value);
//				if (e.Name == "vpos") vpos = long.Parse(e.Value);
				if (e.Name == "user_id") userId = e.Value;
				if (e.Name == "score") score = e.Value;
				if (e.Name == "ticket") ticket = e.Value;
<<<<<<< HEAD
				if (e.Name == "last_res") lastRes = e.Value;
			}
			
			if (root == "chat" || root == "control") {
<<<<<<< HEAD
				vposOriginal = vpos = (date - serverTime) * 100;
=======
=======
			}
			
			if (root == "chat") {
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
				vpos = (date - serverTime) * 100;
>>>>>>> b77d287f700e628ca0b621134ab8ddd993dbb4fc
				if (vpos < 0) vpos = 0;
				_xml.Root.SetAttributeValue("vpos", vpos);
			}
<<<<<<< HEAD
			
			//コメント再生用
			if (root == "control") _xml.Root.Name = "chat";
			var thread = _xml.Root.Attribute("thread");
			if (thread != null) 
				_xml.Root.SetAttributeValue("thread", util.getRegGroup(thread.Value, "(\\d+)"));
				
=======
>>>>>>> 41df14c80172b3ccda9b7c5de41ef417f8572ea0
//			_xml.Add(new XElement("ele", o));
//			http://live2.nicovideo.jp/watch/lv312502201?ref=top&zroute=index&kind=top_onair&row=3
//			util.debugWriteLine(_xml);
			
			return _xml;
		}
	}
}
