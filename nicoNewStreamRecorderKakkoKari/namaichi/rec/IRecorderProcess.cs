﻿/*
 * Created by SharpDevelop.
 * User: zack
 * Date: 2018/08/31
 * Time: 18:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace namaichi.rec
{
	/// <summary>
	/// Description of IRecorderProcess1.
	/// </summary>
	abstract public class IRecorderProcess
	{
		public DateTime tsHlsRequestTime = DateTime.MinValue;
		public TimeSpan tsStartTime;
		public string msUri;
		public bool isTimeShift = false;
		public string[] msReq;
		public long openTime;
		public bool isJikken;
		public string[] gotTsCommentList;
<<<<<<< HEAD
		public double firstSegmentSecond = -1;
=======
>>>>>>> b77d287f700e628ca0b621134ab8ddd993dbb4fc
		
		public IRecorderProcess()
		{
		}
		abstract public void reConnect();
		abstract public string[] getRecFilePath(long _openTime);
		abstract public void sendComment(string s, bool is184);
	}
}
