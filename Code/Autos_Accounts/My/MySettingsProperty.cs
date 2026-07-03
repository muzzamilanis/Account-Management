using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts.My
{
	// Token: 0x02000007 RID: 7
	[StandardModule]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[HideModuleName]
	internal sealed class MySettingsProperty
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00004654 File Offset: 0x00002A54
		[HelpKeyword("My.Settings")]
		internal static MySettings Settings
		{
			get
			{
				return MySettings.Default;
			}
		}
	}
}
