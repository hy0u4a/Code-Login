using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Code_Panel
{
	public class Main : Form
	{
		private string ftpUserID = "stitchmser"; // 무료서버만 사용가능

		private string ftpPassword = "qlrqod1539"; // 무료서버만 사용가능

        private string ftpServerIP = "112.175.184.84"; // 무료서버만 사용가능

        private static Random random;

		private string MODE = "";

		private IContainer components = null;

		private Button Enter;

		private TextBox intVal;

		private Button Plus;

		private Button Min;

		private ComboBox Day;

		private TextBox Code_Text;

		private Button ListView_Update;

		private Button ViewUnused;

		private Button ViewUsed;

		private Button Select_Remove;

		private TextBox Memo;

		private Button MEMO_SEARCH;
        private ColumnHeader Code;
        private ColumnHeader isUsed;
        private ColumnHeader isBanned;
        private ColumnHeader Expire_Time;
        private ColumnHeader Memo_;
        private ListView CodeList;
        private TextBox Memo_S;

		static Main()
		{
			DateTime now = DateTime.Now;
			Main.random = new Random((int)now.Ticks & 65535);
		}

		public Main()
		{
			this.InitializeComponent();
			this.Day.SelectedIndex = 0;
			this.CodeList.Items.Clear();
			this.MODE = "Update";
			(new Thread(new ThreadStart(this.S))).Start();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			StreamWriter streamWriter = new StreamWriter("Code.txt", false);
			for (int i = 0; i < Convert.ToInt32(this.intVal.Text); i++)
			{
				string str = string.Concat("PinkHyouka", Main.RandomString(24));
				string str1 = string.Concat(this.Day.Text, "|0|0|0|0|", this.Memo.Text);
				StreamWriter streamWriter1 = new StreamWriter(str, true);
				streamWriter1.WriteLine(str1);
				streamWriter1.Close();
				this.UploadFTPFile(str, "Code");
				streamWriter.WriteLine(str);
				File.Delete(str);
			}
			streamWriter.Close();
			this.MODE = "Update";
			(new Thread(new ThreadStart(this.S))).Start();
		}

		private void CodeList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				string str = null;
				for (int i = 0; i < this.CodeList.SelectedItems.Count; i++)
				{
					str = string.Concat(str, this.CodeList.SelectedItems[i].Text, "\n");
				}
				Clipboard.SetText(str);
			}
		}

		private void CodeList_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Clipboard.SetText(this.CodeList.SelectedItems[0].Text);
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public bool FtpFileDelete(string FileName, string dir)
		{
			bool flag = true;
			FtpWebRequest networkCredential = null;
			FtpWebResponse response = null;
			try
			{
				try
				{
					networkCredential = (FtpWebRequest)WebRequest.Create(string.Concat(new string[] { "ftp://", this.ftpServerIP, "/html/", dir, "/", FileName }));
					networkCredential.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
					networkCredential.Method = "DELE";
					response = (FtpWebResponse)networkCredential.GetResponse();
					networkCredential = null;
					response = null;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					flag = false;
					throw new Exception(exception.Message.ToString());
				}
			}
			finally
			{
				if (networkCredential != null)
				{
					networkCredential = null;
				}
				if (response != null)
				{
					response = null;
				}
			}
			return flag;
		}

		public string[] GetFileList(string subFolder)
		{
			string[] strArrays;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				FtpWebRequest networkCredential = (FtpWebRequest)WebRequest.Create(new Uri(string.Concat("ftp://", this.ftpServerIP, "/html/", subFolder)));
				networkCredential.UseBinary = true;
				networkCredential.UsePassive = false;
				networkCredential.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
				networkCredential.Method = "NLST";
				WebResponse response = networkCredential.GetResponse();
				StreamReader streamReader = new StreamReader(response.GetResponseStream());
				for (string i = streamReader.ReadLine(); i != null; i = streamReader.ReadLine())
				{
					stringBuilder.Append(i);
					stringBuilder.Append("\n");
				}
				stringBuilder.Remove(stringBuilder.ToString().LastIndexOf('\n'), 1);
				streamReader.Close();
				response.Close();
				strArrays = stringBuilder.ToString().Split(new char[] { '\n' });
			}
			catch
			{
				strArrays = null;
			}
			return strArrays;
		}

		public string GetFTPFileInfo(string fileName, string dir)
		{
			string end;
			string str;
			FtpWebRequest networkCredential = (FtpWebRequest)WebRequest.Create(string.Concat(new string[] { "ftp://", this.ftpServerIP, "/html/", dir, "/", fileName }));
			networkCredential.Method = "RETR";
			networkCredential.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
			using (FtpWebResponse response = (FtpWebResponse)networkCredential.GetResponse())
			{
				using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
				{
					end = streamReader.ReadToEnd();
				}
				str = end;
			}
			return str;
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Enter = new System.Windows.Forms.Button();
            this.intVal = new System.Windows.Forms.TextBox();
            this.Plus = new System.Windows.Forms.Button();
            this.Min = new System.Windows.Forms.Button();
            this.Day = new System.Windows.Forms.ComboBox();
            this.Code_Text = new System.Windows.Forms.TextBox();
            this.ListView_Update = new System.Windows.Forms.Button();
            this.ViewUnused = new System.Windows.Forms.Button();
            this.ViewUsed = new System.Windows.Forms.Button();
            this.Select_Remove = new System.Windows.Forms.Button();
            this.Memo = new System.Windows.Forms.TextBox();
            this.MEMO_SEARCH = new System.Windows.Forms.Button();
            this.Memo_S = new System.Windows.Forms.TextBox();
            this.Code = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.isUsed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.isBanned = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Expire_Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Memo_ = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CodeList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // Enter
            // 
            this.Enter.Location = new System.Drawing.Point(602, 350);
            this.Enter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Enter.Name = "Enter";
            this.Enter.Size = new System.Drawing.Size(260, 26);
            this.Enter.TabIndex = 0;
            this.Enter.TabStop = false;
            this.Enter.Text = "발급";
            this.Enter.UseVisualStyleBackColor = true;
            this.Enter.Click += new System.EventHandler(this.button1_Click);
            // 
            // intVal
            // 
            this.intVal.Location = new System.Drawing.Point(455, 351);
            this.intVal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.intVal.Name = "intVal";
            this.intVal.Size = new System.Drawing.Size(65, 23);
            this.intVal.TabIndex = 1;
            this.intVal.TabStop = false;
            this.intVal.Text = "1";
            this.intVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Plus
            // 
            this.Plus.Location = new System.Drawing.Point(526, 350);
            this.Plus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Plus.Name = "Plus";
            this.Plus.Size = new System.Drawing.Size(32, 26);
            this.Plus.TabIndex = 0;
            this.Plus.TabStop = false;
            this.Plus.Text = "+";
            this.Plus.UseVisualStyleBackColor = true;
            this.Plus.Click += new System.EventHandler(this.Plus_Click);
            // 
            // Min
            // 
            this.Min.Location = new System.Drawing.Point(564, 350);
            this.Min.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Min.Name = "Min";
            this.Min.Size = new System.Drawing.Size(32, 26);
            this.Min.TabIndex = 0;
            this.Min.TabStop = false;
            this.Min.Text = "-";
            this.Min.UseVisualStyleBackColor = true;
            this.Min.Click += new System.EventHandler(this.Min_Click);
            // 
            // Day
            // 
            this.Day.FormattingEnabled = true;
            this.Day.Items.AddRange(new object[] {
            "12 hours",
            "3 days",
            "15 days",
            "30 days",
            "Unlimited",
            "Test"});
            this.Day.Location = new System.Drawing.Point(244, 350);
            this.Day.Name = "Day";
            this.Day.Size = new System.Drawing.Size(205, 23);
            this.Day.TabIndex = 3;
            this.Day.TabStop = false;
            // 
            // Code_Text
            // 
            this.Code_Text.Enabled = false;
            this.Code_Text.Location = new System.Drawing.Point(12, 319);
            this.Code_Text.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Code_Text.Name = "Code_Text";
            this.Code_Text.Size = new System.Drawing.Size(850, 23);
            this.Code_Text.TabIndex = 1;
            this.Code_Text.TabStop = false;
            this.Code_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ListView_Update
            // 
            this.ListView_Update.Location = new System.Drawing.Point(868, 12);
            this.ListView_Update.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ListView_Update.Name = "ListView_Update";
            this.ListView_Update.Size = new System.Drawing.Size(60, 45);
            this.ListView_Update.TabIndex = 0;
            this.ListView_Update.TabStop = false;
            this.ListView_Update.Text = "Update";
            this.ListView_Update.UseVisualStyleBackColor = true;
            this.ListView_Update.Click += new System.EventHandler(this.ListView_Update_Click);
            // 
            // ViewUnused
            // 
            this.ViewUnused.Location = new System.Drawing.Point(868, 118);
            this.ViewUnused.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ViewUnused.Name = "ViewUnused";
            this.ViewUnused.Size = new System.Drawing.Size(60, 45);
            this.ViewUnused.TabIndex = 0;
            this.ViewUnused.TabStop = false;
            this.ViewUnused.Text = "Unused";
            this.ViewUnused.UseVisualStyleBackColor = true;
            this.ViewUnused.Click += new System.EventHandler(this.ViewUnused_Click);
            // 
            // ViewUsed
            // 
            this.ViewUsed.Location = new System.Drawing.Point(868, 65);
            this.ViewUsed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ViewUsed.Name = "ViewUsed";
            this.ViewUsed.Size = new System.Drawing.Size(60, 45);
            this.ViewUsed.TabIndex = 0;
            this.ViewUsed.TabStop = false;
            this.ViewUsed.Text = "Used";
            this.ViewUsed.UseVisualStyleBackColor = true;
            this.ViewUsed.Click += new System.EventHandler(this.ViewUsed_Click);
            // 
            // Select_Remove
            // 
            this.Select_Remove.Location = new System.Drawing.Point(868, 171);
            this.Select_Remove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Select_Remove.Name = "Select_Remove";
            this.Select_Remove.Size = new System.Drawing.Size(60, 45);
            this.Select_Remove.TabIndex = 0;
            this.Select_Remove.TabStop = false;
            this.Select_Remove.Text = "Remove";
            this.Select_Remove.UseVisualStyleBackColor = true;
            this.Select_Remove.Click += new System.EventHandler(this.Select_Remove_Click);
            // 
            // Memo
            // 
            this.Memo.Location = new System.Drawing.Point(12, 350);
            this.Memo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Memo.Name = "Memo";
            this.Memo.Size = new System.Drawing.Size(226, 23);
            this.Memo.TabIndex = 1;
            this.Memo.TabStop = false;
            this.Memo.Text = "메모내용";
            this.Memo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MEMO_SEARCH
            // 
            this.MEMO_SEARCH.Location = new System.Drawing.Point(868, 255);
            this.MEMO_SEARCH.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MEMO_SEARCH.Name = "MEMO_SEARCH";
            this.MEMO_SEARCH.Size = new System.Drawing.Size(60, 45);
            this.MEMO_SEARCH.TabIndex = 0;
            this.MEMO_SEARCH.TabStop = false;
            this.MEMO_SEARCH.Text = "Memo";
            this.MEMO_SEARCH.UseVisualStyleBackColor = true;
            this.MEMO_SEARCH.Click += new System.EventHandler(this.MEMO_SEARCH_Click);
            // 
            // Memo_S
            // 
            this.Memo_S.Location = new System.Drawing.Point(868, 224);
            this.Memo_S.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Memo_S.Name = "Memo_S";
            this.Memo_S.Size = new System.Drawing.Size(60, 23);
            this.Memo_S.TabIndex = 1;
            this.Memo_S.TabStop = false;
            this.Memo_S.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Code
            // 
            this.Code.Text = "Code";
            this.Code.Width = 430;
            // 
            // isUsed
            // 
            this.isUsed.Text = "Used";
            this.isUsed.Width = 42;
            // 
            // isBanned
            // 
            this.isBanned.Text = "Ban";
            this.isBanned.Width = 36;
            // 
            // Expire_Time
            // 
            this.Expire_Time.Text = "Expire Time";
            this.Expire_Time.Width = 120;
            // 
            // Memo_
            // 
            this.Memo_.Text = "Memo";
            this.Memo_.Width = 213;
            // 
            // CodeList
            // 
            this.CodeList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Code,
            this.isUsed,
            this.isBanned,
            this.Expire_Time,
            this.Memo_});
            this.CodeList.HideSelection = false;
            this.CodeList.Location = new System.Drawing.Point(12, 12);
            this.CodeList.Name = "CodeList";
            this.CodeList.Size = new System.Drawing.Size(850, 300);
            this.CodeList.TabIndex = 2;
            this.CodeList.UseCompatibleStateImageBehavior = false;
            this.CodeList.View = System.Windows.Forms.View.Details;
            this.CodeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CodeList_KeyDown);
            this.CodeList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.CodeList_MouseDoubleClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 389);
            this.Controls.Add(this.Day);
            this.Controls.Add(this.CodeList);
            this.Controls.Add(this.Memo_S);
            this.Controls.Add(this.Memo);
            this.Controls.Add(this.Code_Text);
            this.Controls.Add(this.intVal);
            this.Controls.Add(this.Min);
            this.Controls.Add(this.Plus);
            this.Controls.Add(this.MEMO_SEARCH);
            this.Controls.Add(this.ViewUsed);
            this.Controls.Add(this.ViewUnused);
            this.Controls.Add(this.Select_Remove);
            this.Controls.Add(this.ListView_Update);
            this.Controls.Add(this.Enter);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void ListView_Update_Click(object sender, EventArgs e)
		{
			this.MODE = "Update";
			(new Thread(new ThreadStart(this.S))).Start();
		}

		private void MEMO_SEARCH_Click(object sender, EventArgs e)
		{
			this.MODE = "MEMO";
			(new Thread(new ThreadStart(this.S))).Start();
		}

		private void Min_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(this.intVal.Text) - 1;
			this.intVal.Text = num.ToString();
		}

		private void Plus_Click(object sender, EventArgs e)
		{
			int num = Convert.ToInt32(this.intVal.Text) + 1;
			this.intVal.Text = num.ToString();
		}

		public static string RandomString(int _nLength)
		{
			char[] chrArray = new char[_nLength];
			for (int i = 0; i < _nLength; i++)
			{
				chrArray[i] = "abcdefghijklmnopqrstuvwxyz0123456789"[Main.random.Next("abcdefghijklmnopqrstuvwxyz0123456789".Length)];
			}
			return new string(chrArray);
		}

		private void S()
		{
			base.Invoke(new MethodInvoker(() => this.CodeList.Items.Clear()));
			string[] fileList = this.GetFileList("Code");
			if (fileList != null)
			{
				if (this.MODE == "MEMO")
				{
					for (int i = 0; i < (int)fileList.Length; i++)
					{
						string fTPFileInfo = this.GetFTPFileInfo(fileList[i].Substring(5, fileList[i].Length - 5), "Code");
						string[] strArrays = fTPFileInfo.Split(new char[] { '|' });
						if ((strArrays[5].Trim() == this.Memo_S.Text.Trim() ? true : fileList[i].Substring(5, fileList[i].Length - 5) == this.Memo_S.Text.Trim()))
						{
							ListViewItem listViewItem = new ListViewItem(fileList[i].Substring(5, fileList[i].Length - 5));
							listViewItem.SubItems.Add(strArrays[1]);
							listViewItem.SubItems.Add(strArrays[2]);
							listViewItem.SubItems.Add(strArrays[0]);
							listViewItem.SubItems.Add(strArrays[5]);
							base.Invoke(new MethodInvoker(() => this.CodeList.Items.Add(listViewItem)));
						}
					}
				}
				else if (this.MODE == "Used")
				{
					for (int j = 0; j < (int)fileList.Length; j++)
					{
						string str = this.GetFTPFileInfo(fileList[j].Substring(5, fileList[j].Length - 5), "Code");
						string[] strArrays1 = str.Split(new char[] { '|' });
						if (strArrays1[1] != "0")
						{
							ListViewItem listViewItem1 = new ListViewItem(fileList[j].Substring(5, fileList[j].Length - 5));
							listViewItem1.SubItems.Add(strArrays1[1]);
							listViewItem1.SubItems.Add(strArrays1[2]);
							listViewItem1.SubItems.Add(strArrays1[0]);
							listViewItem1.SubItems.Add(strArrays1[5]);
							base.Invoke(new MethodInvoker(() => this.CodeList.Items.Add(listViewItem1)));
						}
					}
				}
				else if (this.MODE == "Unused")
				{
					for (int k = 0; k < (int)fileList.Length; k++)
					{
						string fTPFileInfo1 = this.GetFTPFileInfo(fileList[k].Substring(5, fileList[k].Length - 5), "Code");
						string[] strArrays2 = fTPFileInfo1.Split(new char[] { '|' });
						if (strArrays2[1] != "1")
						{
							ListViewItem listViewItem2 = new ListViewItem(fileList[k].Substring(5, fileList[k].Length - 5));
							listViewItem2.SubItems.Add(strArrays2[1]);
							listViewItem2.SubItems.Add(strArrays2[2]);
							listViewItem2.SubItems.Add(strArrays2[0]);
							listViewItem2.SubItems.Add(strArrays2[5]);
							base.Invoke(new MethodInvoker(() => this.CodeList.Items.Add(listViewItem2)));
						}
					}
				}
				else if (this.MODE == "Update")
				{
					try
					{
						for (int l = 0; l < (int)fileList.Length; l++)
						{
							string str1 = this.GetFTPFileInfo(fileList[l].Substring(5, fileList[l].Length - 5), "Code");
							string[] strArrays3 = str1.Split(new char[] { '|' });
							ListViewItem listViewItem3 = new ListViewItem(fileList[l].Substring(5, fileList[l].Length - 5));
							listViewItem3.SubItems.Add(strArrays3[1]);
							listViewItem3.SubItems.Add(strArrays3[2]);
							listViewItem3.SubItems.Add(strArrays3[0]);
							listViewItem3.SubItems.Add(strArrays3[5]);
							base.Invoke(new MethodInvoker(() => this.CodeList.Items.Add(listViewItem3)));
						}
					}
					catch
					{
					}
				}
			}
		}

		private void Select_Remove_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.CodeList.SelectedItems.Count; i++)
			{
				this.FtpFileDelete(this.CodeList.SelectedItems[i].Text, "Code");
			}
			this.MODE = "Update";
			(new Thread(new ThreadStart(this.S))).Start();
		}

		public void UploadFTPFile(string FileName, string dir)
		{
			FtpWebRequest networkCredential = (FtpWebRequest)WebRequest.Create(string.Concat(new string[] { "ftp://", this.ftpServerIP, "/html/", dir, "/", FileName }));
			networkCredential.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
			networkCredential.Method = "STOR";
			FileStream fileStream = (new FileInfo(FileName)).OpenRead();
			int num = 2048;
			byte[] numArray = new byte[num];
			Stream requestStream = networkCredential.GetRequestStream();
			for (int i = fileStream.Read(numArray, 0, num); i != 0; i = fileStream.Read(numArray, 0, num))
			{
				requestStream.Write(numArray, 0, i);
			}
			requestStream.Close();
			fileStream.Close();
			networkCredential = null;
		}

		private void ViewUnused_Click(object sender, EventArgs e)
		{
			this.MODE = "Unused";
			(new Thread(new ThreadStart(this.S))).Start();
		}

		private void ViewUsed_Click(object sender, EventArgs e)
		{
			this.MODE = "Used";
			(new Thread(new ThreadStart(this.S))).Start();
		}
	}
}