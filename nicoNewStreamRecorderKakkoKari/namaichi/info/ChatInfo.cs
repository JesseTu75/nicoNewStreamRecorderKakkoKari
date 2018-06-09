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
		public string contents;
		public string premium;
		public string root;
		public long serverTime;
		public int date;
		public long date_usec;
		public long vpos;
		
		public ChatInfo(XDocument xml)
		{
			this.xml = xml;
		}
		public XDocument getFormatXml(long serverTime) {
			this.serverTime = serverTime;
			//xml.Root
//			System.Diagnostics.Debug.WriteLine(xml.Root);
			var _xml = new XDocument();
			_xml.Add(new XElement(xml.Root.Name));
			root = xml.Root.Name.ToString();
//			System.Diagnostics.Debug.WriteLine(xml.Root.Name);
			
//			var atts = _xml.Root.Attributes();
			Object[] o = new Object[20];
			
			date = 0;
			foreach (XElement e in xml.Root.Elements()) {
//				System.Diagnostics.Debug.WriteLine(xml.Root);
//				o[0] = new XAttribute(e.Name, e.Value);
//				_xml.Root.SetAttributeValue(e.Name, e.Value);
				if (e.Name == "content") {
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
			}
			
			if (root == "chat") {
				vpos = (date - serverTime) * 100;
				if (vpos < 0) vpos = 0;
				_xml.Root.SetAttributeValue("vpos", vpos);
			}
//			_xml.Add(new XElement("ele", o));
//			http://live2.nicovideo.jp/watch/lv312502201?ref=top&zroute=index&kind=top_onair&row=3
//			System.Diagnostics.Debug.WriteLine(_xml);
			
			return _xml;
		}
	}
}
