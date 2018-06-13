﻿/*
 * Created by SharpDevelop.
 * User: pc
 * Date: 2018/05/06
 * Time: 20:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using namaichi.config;
using SunokoLibrary.Application;

namespace namaichi
{
	/// <summary>
	/// Description of optionForm.
	/// </summary>
	public partial class optionForm : Form
	{
		private config.config cfg;
		
		static readonly Uri TargetUrl = new Uri("http://live.nicovideo.jp/");
		private string fileNameFormat;
//		private string 
		
		public optionForm(config.config cfg)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.StartPosition = FormStartPosition.CenterParent;
			//System.Diagnostics.Debug.WriteLine(p.X + " " + p.Y);
			InitializeComponent();
			//this.Location = p;
			this.cfg = cfg;
			
			nicoSessionComboBox1.Selector.PropertyChanged += Selector_PropertyChanged;
			
			setFormFromConfig();
		}
		
		void hozonFolderSanshouBtn_Click(object sender, EventArgs e)
		{
			var f = new FolderBrowserDialog();
			DialogResult r = f.ShowDialog();
			System.Diagnostics.Debug.WriteLine(f.SelectedPath);

		}
		
		void fileNameOptionBtn(object sender, EventArgs e)
		{
			
		}
		void FileNameDokujiSetteiBtn_Click(object sender, EventArgs e)
		{
			var a = new fileNameOptionForm(fileNameFormat);
			var res = a.ShowDialog();
			if (res != DialogResult.OK) return;
			fileNameTypeDokujiSetteiBtn.Text = util.getFileNameTypeSample(a.ret);
			fileNameFormat = a.ret;
		}
		
		void optionOk_Click(object sender, EventArgs e)
		{
			var formData = getFormData();
			cfg.saveFromForm(formData);
			Close();
			
			var importer = nicoSessionComboBox1.Selector.SelectedImporter;
			if (importer == null || importer.SourceInfo == null) return;
			var si = importer.SourceInfo;
			
			if (isCookieFileSiteiChkBox.Checked)
				SourceInfoSerialize.save(si.GenerateCopy(si.BrowserName, si.ProfileName, cookieFileText.Text));
			else SourceInfoSerialize.save(si); 
			
		}

		private Dictionary<string, string> getFormData() {
			var selectedImporter = nicoSessionComboBox1.Selector.SelectedImporter;
//			var browserName = (selectedImporter != null) ? selectedImporter.SourceInfo.BrowserName : "";
			var browserNum = (useCookieRadioBtn.Checked) ? "2" : "1";
			return new Dictionary<string, string>(){
				{"accountId",mailText.Text},
				{"accountPass",passText.Text},
				//{"user_session",passText.Text},
				{"browserNum",browserNum},
//				{"isAllBrowserMode",checkBoxShowAll.Checked.ToString().ToLower()},
				{"issecondlogin",useSecondLoginChkBox.Checked.ToString().ToLower()},
				{"IsdefaultBrowserPath",isDefaultBrowserPathChkBox.Checked.ToString().ToLower()},
				{"browserPath",browserPathText.Text},
				{"Isminimized",isMinimizedChkBox.Checked.ToString().ToLower()},
				{"IscloseExit",isCloseExitChkBox.Checked.ToString().ToLower()},
				{"IsfailExit",isFailExit.Checked.ToString().ToLower()},
				{"IsgetComment",isGetCommentChkBox.Checked.ToString().ToLower()},
				{"IsmessageBox",isMessageBoxChkBox.Checked.ToString().ToLower()},
//				{"IshosoInfo",isHosoInfoChkBox.Checked.ToString().ToLower()},
//				{"Islog",isLogChkBox.Checked.ToString().ToLower()},
				{"IstitlebarInfo",isTitleBarInfoChkBox.Checked.ToString().ToLower()},
//				{"Islimitpopup",isLimitPopupChkBox.Checked.ToString().ToLower()},
				{"Isretry",isRetryChkBox.Checked.ToString().ToLower()},
				{"IsdeleteExit",isDeleteExitChkBox.Checked.ToString().ToLower()},
				{"IsgetcommentXml",isCommentXML.Checked.ToString().ToLower()},
				{"IstitlebarSamune",isTitleBarSamune.Checked.ToString().ToLower()},
				{"IsautoFollowComgen",isAutoFollowComGen.Checked.ToString().ToLower()},
				{"qualityRank",getQualityRank()},
				{"IsLogFile",isLogFileChkBox.Checked.ToString().ToLower()},
				
				{"cookieFile",cookieFileText.Text},
				{"iscookie",isCookieFileSiteiChkBox.Checked.ToString().ToLower()},
				{"recordDir",recordDirectoryText.Text},
				{"IsdefaultRecordDir",useDefaultRecFolderChk.Checked.ToString().ToLower()},
				{"IscreateSubfolder",useSubFolderChk.Checked.ToString().ToLower()},
				{"subFolderNameType",getSubFolderNameType() + ""},
				{"fileNameType",getFileNameType() + ""},
				{"filenameformat",fileNameFormat},
				{"ffmpegopt",ffmpegoptText.Text},
				{"user_session",""},
				{"user_session_secure",""},
			};
			
		}
		async void Selector_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
			//if (isInitRun) initRec();
			
	
			
            switch(e.PropertyName)
            {
                case "SelectedIndex":
                    var cookieContainer = new CookieContainer();
                    var currentGetter = nicoSessionComboBox1.Selector.SelectedImporter;
                    if (currentGetter != null)
                    {
                        var result = await currentGetter.GetCookiesAsync(TargetUrl);
                        
                        var cookie = result.Status == CookieImportState.Success ? result.Cookies["user_session"] : null;
//                        foreach (var c in result.Cookies)
//                        	System.Diagnostics.Debug.WriteLine(c);
                        //logText.Text += cookie.Name + cookie.Value+ cookie.Expires;
                        
                        //UI更新
//                        txtCookiePath.Text = currentGetter.SourceInfo.CookiePath;
//                        btnOpenCookieFileDialog.Enabled = true;
//                        txtUserSession.Text = cookie != null ? cookie.Value : null;
//                        txtUserSession.Enabled = result.Status == CookieImportState.Success;
                        //Properties.Settings.Default.SelectedSourceInfo = currentGetter.SourceInfo;
                        //Properties.Settings.Default.Save();
                        //cfg.set("browserNum", nicoSessionComboBox1.Selector.SelectedIndex.ToString());
                        //if (cookie != null) cfg.set("user_session", cookie.Value);
                        //cfg.set("isAllBrowserMode", nicoSessionComboBox1.Selector.IsAllBrowserMode.ToString());
                    }
                    else
                    {
//                        txtCookiePath.Text = null;
//                        txtUserSession.Text = null;
//                        txtUserSession.Enabled = false;
//                        btnOpenCookieFileDialog.Enabled = false;
                    }
                    break;
            }
        }

		void btnReload_Click(object sender, EventArgs e)
        { 
			//var si = nicoSessionComboBox1.Selector.SelectedImporter.SourceInfo;
			//System.Diagnostics.Debug.WriteLine(si.EngineId + " " + si.BrowserName + " " + si.ProfileName);
//			var a = new SunokoLibrary.Application.Browsers.FirefoxImporterFactory();
//			foreach (var b in a.GetCookieImporters()) {
//				var c = b.GetCookiesAsync(TargetUrl);
//				c.ConfigureAwait(false);
				
//				System.Diagnostics.Debug.WriteLine(c.Result.Cookies["user_session"]);
//			}
				
//			a.GetCookieImporter(new CookieSourceInfo("
			var tsk = nicoSessionComboBox1.Selector.UpdateAsync(); 
		}
        void btnOpenCookieFileDialog_Click(object sender, EventArgs e)
        { var tsk = nicoSessionComboBox1.ShowCookieDialogAsync(); }
        void checkBoxShowAll_CheckedChanged(object sender, EventArgs e)
        { nicoSessionComboBox1.Selector.IsAllBrowserMode = checkBoxShowAll.Checked;
//        	cfg.set("isAllBrowserMode", nicoSessionComboBox1.Selector.IsAllBrowserMode.ToString());
        }
        int getSubFolderNameType() {
        	if (housoushaRadioBtn.Checked) return 1;
        	if (userIDRadioBtn.Checked) return 2;
        	if (userIDHousoushaRadioBtn.Checked) return 3;
        	if (comNameRadioBtn.Checked) return 4;
        	if (comIDRadioBtn.Checked) return 5;
        	if (ComIDComNameRadioBtn.Checked) return 6;
        	if (comIDHousoushaRadioBtn.Checked) return 7;
        	if (housoushaComIDRadioBtn.Checked) return 8;
        	return 1;
        }
        int getFileNameType() {
        	if (fileNameTypeRadioBtn0.Checked) return 1;
        	if (fileNameTypeRadioBtn1.Checked) return 2;
        	if (fileNameTypeRadioBtn2.Checked) return 3;
        	if (fileNameTypeRadioBtn3.Checked) return 4;
        	if (fileNameTypeRadioBtn4.Checked) return 5;
        	if (fileNameTypeRadioBtn5.Checked) return 6;
        	if (fileNameTypeRadioBtn6.Checked) return 7;
        	if (fileNameTypeRadioBtn7.Checked) return 8;
        	if (fileNameTypeRadioBtn8.Checked) return 9;
        	if (fileNameTypeRadioBtn9.Checked) return 10;
        	return 1;
        }
        private void setFormFromConfig() {
        	mailText.Text = cfg.get("accountId");
        	passText.Text = cfg.get("accountPass");
        	
        	if (cfg.get("browserNum") == "1") useAccountLoginRadioBtn.Checked = true;
        	else useCookieRadioBtn.Checked = true; 
        	useSecondLoginChkBox.Checked = bool.Parse(cfg.get("issecondlogin"));
        	isDefaultBrowserPathChkBox.Checked = bool.Parse(cfg.get("IsdefaultBrowserPath"));
        	isDefaultBrowserPathChkBox_UpdateAction();
        	browserPathText.Text = cfg.get("browserPath");
        	isMinimizedChkBox.Checked = bool.Parse(cfg.get("Isminimized"));
        	isCloseExitChkBox.Checked = bool.Parse(cfg.get("IscloseExit"));
        	isFailExit.Checked = bool.Parse(cfg.get("IsfailExit"));
        	isGetCommentChkBox.Checked = bool.Parse(cfg.get("IsgetComment"));
        	isMessageBoxChkBox.Checked = bool.Parse(cfg.get("IsmessageBox"));
//        	isHosoInfoChkBox.Checked = bool.Parse(cfg.get("IshosoInfo"));
//        	isLogChkBox.Checked = bool.Parse(cfg.get("Islog"));
        	isTitleBarInfoChkBox.Checked = bool.Parse(cfg.get("IstitlebarInfo"));
//        	isLimitPopupChkBox.Checked = bool.Parse(cfg.get("Islimitpopup"));
        	isRetryChkBox.Checked = bool.Parse(cfg.get("Isretry"));
        	isDeleteExitChkBox.Checked = bool.Parse(cfg.get("IsdeleteExit"));
        	isCommentXML.Checked = bool.Parse(cfg.get("IsgetcommentXml"));
        	isCommentJson.Checked = !bool.Parse(cfg.get("IsgetcommentXml"));
        	isGetCommentChkBox_UpdateAction();
        	isTitleBarSamune.Checked = bool.Parse(cfg.get("IstitlebarSamune"));
        	isAutoFollowComGen.Checked = bool.Parse(cfg.get("IsautoFollowComgen"));
        	setInitQualityRankList(cfg.get("qualityRank"));
        	isLogFileChkBox.Checked = bool.Parse(cfg.get("IsLogFile"));
        	
        	isCookieFileSiteiChkBox.Checked = bool.Parse(cfg.get("iscookie"));
        	isCookieFileSiteiChkBox_UpdateAction();
        	cookieFileText.Text = cfg.get("cookieFile");
        	recordDirectoryText.Text = cfg.get("recordDir");
        	useDefaultRecFolderChk.Checked = bool.Parse(cfg.get("IsdefaultRecordDir"));
        	useDefaultRecFolderChkBox_UpdateAction();
        	useSubFolderChk.Checked = bool.Parse(cfg.get("IscreateSubfolder"));
        	useSubFolderChk_UpdateAction();
        	setSubFolderNameType(int.Parse(cfg.get("subFolderNameType")));
        	setFileNameType(int.Parse(cfg.get("fileNameType")));
        	fileNameFormat = cfg.get("filenameformat");
        	fileNameTypeDokujiSetteiBtn.Text = util.getFileNameTypeSample(fileNameFormat);
        	ffmpegoptText.Text = cfg.get("ffmpegopt");
        	
        	var si = SourceInfoSerialize.load();
        	nicoSessionComboBox1.Selector.SetInfoAsync(si);
//			!bool.Parse(cfg.get("defaultBrowserPath"))
        }
        private void setSubFolderNameType(int subFolderNameType) {
        	if (subFolderNameType == 1) housoushaRadioBtn.Checked = true;
			else if (subFolderNameType == 2) userIDRadioBtn.Checked = true;
			else if (subFolderNameType == 3) userIDHousoushaRadioBtn.Checked = true;
			else if (subFolderNameType == 4) comNameRadioBtn.Checked = true;
			else if (subFolderNameType == 5) comIDRadioBtn.Checked = true;
			else if (subFolderNameType == 6) ComIDComNameRadioBtn.Checked = true;
			else if (subFolderNameType == 7) comIDHousoushaRadioBtn.Checked = true;
			else if (subFolderNameType == 8) housoushaComIDRadioBtn.Checked = true;
			else housoushaRadioBtn.Checked = true;
        }
        private void setFileNameType(int nameType) {
        	if (nameType == 1) fileNameTypeRadioBtn0.Checked = true;
			else if (nameType == 2) fileNameTypeRadioBtn1.Checked = true;
			else if (nameType == 3) fileNameTypeRadioBtn2.Checked = true;
			else if (nameType == 4) fileNameTypeRadioBtn3.Checked = true;
			else if (nameType == 5) fileNameTypeRadioBtn4.Checked = true;
			else if (nameType == 6) fileNameTypeRadioBtn5.Checked = true;
			else if (nameType == 7) fileNameTypeRadioBtn6.Checked = true;
			else if (nameType == 8) fileNameTypeRadioBtn7.Checked = true;
			else if (nameType == 9) fileNameTypeRadioBtn8.Checked = true;
			else if (nameType == 10) fileNameTypeRadioBtn9.Checked = true;
			else fileNameTypeRadioBtn0.Checked = true;
        }
		
		void optionCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
		
		void cookieFileSiteiSanshouBtn_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			var result = dialog.ShowDialog();
			if (result != DialogResult.OK) return;
			
			cookieFileText.Text = dialog.FileName;
		}
		
		void isCookieFileSiteiChkBox_CheckedChanged(object sender, EventArgs e)
		{
			isCookieFileSiteiChkBox_UpdateAction();
		}
		void isCookieFileSiteiChkBox_UpdateAction() {
//			cookieFileText.Enabled = isCookieFileSiteiChkBox.Checked;
//			cookieFileSanshouBtn.Enabled = isCookieFileSiteiChkBox.Checked;
		}
		
		void useDefaultRecFolderChkBox_CheckedChanged(object sender, EventArgs e)
		{
			useDefaultRecFolderChkBox_UpdateAction();
		}
		void useDefaultRecFolderChkBox_UpdateAction()
		{
			recordDirectoryText.Enabled = !useDefaultRecFolderChk.Checked;
			recFolderSanshouBtn.Enabled = !useDefaultRecFolderChk.Checked;
		}
		
		void useSubFolderChk_CheckedChanged(object sender, EventArgs e)
		{
			useSubFolderChk_UpdateAction();
		}
		void useSubFolderChk_UpdateAction()
		{
			/*
			housoushaRadioBtn.Enabled = useSubFolderChk.Checked;
			userIDRadioBtn.Enabled = useSubFolderChk.Checked;
			userIDHousoushaRadioBtn.Enabled = useSubFolderChk.Checked;
			comNameRadioBtn.Enabled = useSubFolderChk.Checked;
			comIDRadioBtn.Enabled = useSubFolderChk.Checked;
			ComIDComNameRadioBtn.Enabled = useSubFolderChk.Checked;
			comIDHousoushaRadioBtn.Enabled = useSubFolderChk.Checked;
			housoushaComIDRadioBtn.Enabled = useSubFolderChk.Checked;
			*/
		}
		
		async void loginBtn_Click(object sender, EventArgs e)
		{
			
			var cg = new rec.CookieGetter(cfg);
			var cc = await cg.getAccountCookie(mailText.Text, passText.Text);
			if (cc == null) {
				MessageBox.Show("error", "aaa", MessageBoxButtons.OK);
				return;
			}
			if (cc.GetCookies(TargetUrl)["user_session"] == null &&
				                   cc.GetCookies(TargetUrl)["user_session_secure"] == null)
				MessageBox.Show("no login", "aaa", MessageBoxButtons.OK);
			else MessageBox.Show("login ok", "aaa", MessageBoxButtons.OK);
			
			//MessageBox.Show("aa");
		}
		
		
		void isDefaultBrowserPathChkBox_CheckedChanged(object sender, EventArgs e)
		{
			isDefaultBrowserPathChkBox_UpdateAction();
		}
		void isDefaultBrowserPathChkBox_UpdateAction()
		{
			browserPathText.Enabled = !isDefaultBrowserPathChkBox.Checked;
			browserPathSanshouBtn.Enabled = !isDefaultBrowserPathChkBox.Checked;
		}
		
		
		void browserPathSanshouBtn_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.Multiselect = false;
			var result = dialog.ShowDialog();
			if (result != DialogResult.OK) return;
			
			browserPathText.Text = dialog.FileName;
		}
		
		void isGetCommentChkBox_CheckedChanged(object sender, EventArgs e)
		{
			isGetCommentChkBox_UpdateAction();
		}
		void isGetCommentChkBox_UpdateAction()
		{
			isCommentXML.Enabled = isGetCommentChkBox.Checked;
			isCommentJson.Enabled = isGetCommentChkBox.Checked;
		}
		
		void highRankBtn_Click(object sender, EventArgs e)
		{
			int[] ranks = {1,2,3,4,5,0};
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks, qualityListBox));
		}
		void lowRankBtn_Click(object sender, EventArgs e)
		{
			int[] ranks = {5, 4, 3, 2, 1, 0};
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks, qualityListBox));
		}
		public object[] getRanksToItems(int[] ranks, ListBox owner) {
			var items = new Dictionary<int, string> {
				{0, "自動(abr)"}, {1, "3Mbps(super_high)"},
				{2, "2Mbps(high・高画質)"}, {3, "1Mbps(normal・低画質)"},
				{4, "384kbps(low)"}, {5, "192kbps(super_low)"},
			};
//			var ret = new ListBox.ObjectCollection(owner);
			var ret = new List<object>();
			for (int i = 0; i < ranks.Length; i++) {
				ret.Add((i + 1) + ". " + items[ranks[i]]);
			}
			return ret.ToArray();
		}
		void UpBtn_Click(object sender, EventArgs e)
		{
			var selectedIndex = qualityListBox.SelectedIndex;
			if (selectedIndex < 1) return;
			
			var ranks = getItemsToRanks(qualityListBox.Items);
			var selectedVal = ranks[selectedIndex + 0];
			ranks.RemoveAt(selectedIndex);
			var addIndex = (selectedIndex == 0) ? 0 : (selectedIndex - 1);
			ranks.Insert(addIndex, selectedVal);
			
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks.ToArray(), qualityListBox));
			qualityListBox.SetSelected(addIndex, true);
		}
		void DownBtn_Click(object sender, EventArgs e)
		{
			var selectedIndex = qualityListBox.SelectedIndex;
			if (selectedIndex > 4) return;
			
			var ranks = getItemsToRanks(qualityListBox.Items);
			var selectedVal = ranks[selectedIndex + 0];
			ranks.RemoveAt(selectedIndex);
			var addIndex = (selectedIndex == 5) ? 5 : (selectedIndex + 1);
			ranks.Insert(addIndex, selectedVal);
			
			qualityListBox.Items.Clear();
			qualityListBox.Items.AddRange(getRanksToItems(ranks.ToArray(), qualityListBox));
			qualityListBox.SetSelected(addIndex, true);
		}
		List<int> getItemsToRanks(ListBox.ObjectCollection items) {
			var itemsDic = new Dictionary<int, string> {
				{0, "自動(abr)"}, {1, "3Mbps(super_high)"},
				{2, "2Mbps(high・高画質)"}, {3, "1Mbps(normal・低画質)"},
				{4, "384kbps(low)"}, {5, "192kbps(super_low)"},
			};
			var ret = new List<int>();
			for (int i = 0; i < items.Count; i++) {
				foreach (KeyValuePair <int, string> p in itemsDic)
					if (p.Value.IndexOf(((string)items[i]).Substring(3)) > -1) ret.Add(p.Key);
			}
			return ret;
		}
		string getQualityRank() {
			var buf = getItemsToRanks(qualityListBox.Items);
			var ret = "";
			foreach (var r in buf) {
				if (ret != "") ret += ",";
				ret += r;
			}
			return ret;
		}
		void setInitQualityRankList(string qualityRank) {
			var ranks = new List<int>();
			foreach (var r in qualityRank.Split(','))
				ranks.Add(int.Parse(r));
//			ranks.AddRange(qualityRank.Split(','));
			
			qualityListBox.Items.Clear();
			var items = getRanksToItems(ranks.ToArray(), qualityListBox);
			qualityListBox.Items.AddRange(items);
		}
	}
}
