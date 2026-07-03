using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using AES256;
using Autos_Accounts.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts
{
	// Token: 0x02000013 RID: 19
	[DesignerGenerated]
	public partial class UserLogin : Window
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00002E77 File Offset: 0x00001277
		public UserLogin()
		{
			base.Closing += this.UserLogin_Closing;
			base.Loaded += this.UserLogin_Loaded;
			this.InitializeComponent();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00002EA9 File Offset: 0x000012A9
		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
			Application.Current.Shutdown();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007AD4 File Offset: 0x00005ED4
		private void OK_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(this.txtPass.Password, string.Empty, false) != 0;
			if (flag)
			{
				bool flag2 = Operators.CompareString(this.txtPass.Password, MySettingsProperty.Settings.strPassword, false) == 0;
				if (flag2)
				{
					base.DialogResult = new bool?(true);
					base.Close();
				}
				else
				{
					Interaction.MsgBox("Password Not Match", MsgBoxStyle.OkOnly, null);
				}
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007B4C File Offset: 0x00005F4C
		private void UserLogin_Closing(object sender, CancelEventArgs e)
		{
			bool? dialogResult = base.DialogResult;
			bool valueOrDefault = ((dialogResult != null) ? new bool?(dialogResult.GetValueOrDefault()) : null).GetValueOrDefault();
			if (!valueOrDefault)
			{
				e.Cancel = true;
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007B9C File Offset: 0x00005F9C
		private void UserLogin_Loaded(object sender, RoutedEventArgs e)
		{
			bool flag = Operators.CompareString(MySettingsProperty.Settings.strID, string.Empty, false) == 0 | MySettingsProperty.Settings.intCheck == 0;
			if (flag)
			{
				bool flag2 = Operators.CompareString(MySettingsProperty.Settings.strKey, string.Empty, false) != 0;
				if (flag2)
				{
					bool flag3 = Operators.CompareString(_Default.Decrypt(MySettingsProperty.Settings.strKey, "MultyinfoHostineasy", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).Split(new char[]
					{
						'|'
					}).Last<string>().Replace("\0", ""), "Allowed", false) == 0;
					if (flag3)
					{
						MySettingsProperty.Settings.intCheck = 21;
						MySettingsProperty.Settings.Save();
					}
				}
				clsComputerInfo clsComputerInfo = new clsComputerInfo();
				string processorId = clsComputerInfo.GetProcessorId();
				string volumeSerial = clsComputerInfo.GetVolumeSerial("C");
				string motherBoardID = clsComputerInfo.GetMotherBoardID();
				string macaddress = clsComputerInfo.GetMACAddress();
				MySettingsProperty.Settings.strID = string.Concat(new string[]
				{
					processorId,
					"|",
					volumeSerial,
					"|",
					motherBoardID,
					"|",
					macaddress
				});
				MySettingsProperty.Settings.Save();
				string str = _Default.Encrypt(MySettingsProperty.Settings.strID + "|Jaini Autos", "MultyinfoHostineasy", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256);
				string text = Conversions.ToString(this.GethtmlSourcecode("http://hostineasy.com/license.php?key=" + HttpUtility.UrlEncode(str)));
				bool flag4 = text.Contains("Key:");
				if (flag4)
				{
					MySettingsProperty.Settings.strKey = text.Replace("Key:", "").Trim();
					MySettingsProperty.Settings.Save();
					bool flag5 = Operators.CompareString(_Default.Decrypt(MySettingsProperty.Settings.strKey, "MultyinfoHostineasy", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).Split(new char[]
					{
						'|'
					}).Last<string>().Replace("\0", ""), "Allowed", false) != 0;
					if (flag5)
					{
						bool flag6 = MySettingsProperty.Settings.intCheck == 0;
						if (flag6)
						{
							Interaction.MsgBox("Please Register to Continue!", MsgBoxStyle.OkOnly, null);
							Application.Current.Shutdown();
						}
						else
						{
							Interaction.MsgBox("Please Contact Developer to Register Before " + Conversions.ToString(MySettingsProperty.Settings.intCheck) + " Appears to 0!", MsgBoxStyle.OkOnly, null);
						}
					}
					else
					{
						MySettingsProperty.Settings.intCheck = 21;
						MySettingsProperty.Settings.Save();
					}
				}
				else
				{
					MainWindow.errorlog("Registration Attempt: " + text);
				}
			}
			else
			{
				MySettings settings;
				(settings = MySettingsProperty.Settings).intCheck = checked(settings.intCheck - 1);
				MySettingsProperty.Settings.Save();
				bool flag7 = Operators.CompareString(_Default.Decrypt(MySettingsProperty.Settings.strKey, "MultyinfoHostineasy", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).Split(new char[]
				{
					'|'
				}).Last<string>().Replace("\0", ""), "Allowed", false) != 0;
				if (flag7)
				{
					this.Register.Visibility = Visibility.Visible;
					Interaction.MsgBox("Please Contact Developer to Register Before " + Conversions.ToString(MySettingsProperty.Settings.intCheck) + " Appears to 0!", MsgBoxStyle.OkOnly, null);
				}
			}
			this.txtPass.Focus();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007F44 File Offset: 0x00006344
		public object GethtmlSourcecode(string site)
		{
			CookieContainer cookieContainer = new CookieContainer();
			object result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(site);
				HttpWebRequest httpWebRequest2 = httpWebRequest;
				httpWebRequest2.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest2.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
				httpWebRequest2.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
				httpWebRequest2.Accept = "*/*";
				httpWebRequest2.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729)";
				httpWebRequest.KeepAlive = true;
				httpWebRequest.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
				httpWebRequest.ServicePoint.Expect100Continue = false;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				cookieContainer.Add(httpWebResponse.Cookies);
				StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
				string text = streamReader.ReadToEnd();
				httpWebResponse.Close();
				streamReader.Close();
				result = text;
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008050 File Offset: 0x00006450
		private void Register_Click(object sender, RoutedEventArgs e)
		{
			clsComputerInfo clsComputerInfo = new clsComputerInfo();
			string processorId = clsComputerInfo.GetProcessorId();
			string volumeSerial = clsComputerInfo.GetVolumeSerial("C");
			string motherBoardID = clsComputerInfo.GetMotherBoardID();
			string macaddress = clsComputerInfo.GetMACAddress();
			MySettingsProperty.Settings.strID = string.Concat(new string[]
			{
				processorId,
				"|",
				volumeSerial,
				"|",
				motherBoardID,
				"|",
				macaddress
			});
			MySettingsProperty.Settings.Save();
			string str = _Default.Encrypt(MySettingsProperty.Settings.strID + "|Jaini Autos", "MultyinfoHostineasy", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256);
			string text = Conversions.ToString(this.GethtmlSourcecode("http://hostineasy.com/license.php?key=" + HttpUtility.UrlEncode(str)));
			bool flag = text.Contains("Key:");
			if (flag)
			{
				MySettingsProperty.Settings.strKey = text.Replace("Key:", "").Trim();
				MySettingsProperty.Settings.Save();
				bool flag2 = Operators.CompareString(_Default.Decrypt(MySettingsProperty.Settings.strKey, "MultyinfoHostineasy", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).Split(new char[]
				{
					'|'
				}).Last<string>().Replace("\0", ""), "Allowed", false) != 0;
				if (flag2)
				{
					bool flag3 = MySettingsProperty.Settings.intCheck == 0;
					if (flag3)
					{
						Interaction.MsgBox("Please Register to Continue!", MsgBoxStyle.OkOnly, null);
						Application.Current.Shutdown();
					}
					else
					{
						Interaction.MsgBox("Unable to Register!\r\nPlease Contact Developer to Register Before " + Conversions.ToString(MySettingsProperty.Settings.intCheck) + " Appears to 0!", MsgBoxStyle.OkOnly, null);
					}
				}
				else
				{
					this.Register.Visibility = Visibility.Collapsed;
					Interaction.MsgBox("Registered :)", MsgBoxStyle.OkOnly, null);
					MySettingsProperty.Settings.intCheck = 21;
					MySettingsProperty.Settings.Save();
				}
			}
			else
			{
				MainWindow.errorlog("Registration Attempt: " + text);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00002EC4 File Offset: 0x000012C4
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00002ECE File Offset: 0x000012CE
		internal virtual DockPanel dockAddAccounts { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00002ED7 File Offset: 0x000012D7
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00002EE1 File Offset: 0x000012E1
		internal virtual Button Register { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00002EEA File Offset: 0x000012EA
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00002EF4 File Offset: 0x000012F4
		internal virtual Button OK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00002EFD File Offset: 0x000012FD
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00002F07 File Offset: 0x00001307
		internal virtual Button Cancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00002F10 File Offset: 0x00001310
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00002F1A File Offset: 0x0000131A
		internal virtual TextBlock AccInfoText { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00002F23 File Offset: 0x00001323
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00002F2D File Offset: 0x0000132D
		internal virtual PasswordBox txtPass { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }
	}
}
