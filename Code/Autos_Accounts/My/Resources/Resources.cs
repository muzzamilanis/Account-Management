using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts.My.Resources
{
	// Token: 0x02000006 RID: 6
	[HideModuleName]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[StandardModule]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal sealed class Resources
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00004464 File Offset: 0x00002864
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = object.ReferenceEquals(Resources.resourceMan, null);
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("Autos_Accounts.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000044AC File Offset: 0x000028AC
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002420 File Offset: 0x00000820
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000044C4 File Offset: 0x000028C4
		internal static string SimpleReport
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReport", Resources.resourceCulture);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000044EC File Offset: 0x000028EC
		internal static string SimpleReport_1
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReport_1", Resources.resourceCulture);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00004514 File Offset: 0x00002914
		internal static string SimpleReportAccounts
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportAccounts", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0000453C File Offset: 0x0000293C
		internal static string SimpleReportDutyExpenses
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportDutyExpenses", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00004564 File Offset: 0x00002964
		internal static string SimpleReportExpenses
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportExpenses", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0000458C File Offset: 0x0000298C
		internal static string SimpleReportMiscExpenses
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportMiscExpenses", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000045B4 File Offset: 0x000029B4
		internal static string SimpleReportPayments
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportPayments", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000045DC File Offset: 0x000029DC
		internal static string SimpleReportPaymentsPkr
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportPaymentsPkr", Resources.resourceCulture);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00004604 File Offset: 0x00002A04
		internal static string SimpleReportReceipts
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportReceipts", Resources.resourceCulture);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000462C File Offset: 0x00002A2C
		internal static string SimpleReportStocks
		{
			get
			{
				return Resources.ResourceManager.GetString("SimpleReportStocks", Resources.resourceCulture);
			}
		}

		// Token: 0x04000015 RID: 21
		private static ResourceManager resourceMan;

		// Token: 0x04000016 RID: 22
		private static CultureInfo resourceCulture;
	}
}
