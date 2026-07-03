using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.Logging;

namespace Autos_Accounts.My
{
	// Token: 0x02000004 RID: 4
	[StandardModule]
	[HideModuleName]
	internal sealed class MyWpfExtension
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000042EC File Offset: 0x000026EC
		internal static Application Application
		{
			get
			{
				return (Application)System.Windows.Application.Current;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00004308 File Offset: 0x00002708
		internal static Computer Computer
		{
			get
			{
				return MyWpfExtension.s_Computer.GetInstance;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00004324 File Offset: 0x00002724
		internal static User User
		{
			get
			{
				return MyWpfExtension.s_User.GetInstance;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00004340 File Offset: 0x00002740
		internal static Log Log
		{
			get
			{
				return MyWpfExtension.s_Log.GetInstance;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000435C File Offset: 0x0000275C
		internal static MyWpfExtension.MyWindows Windows
		{
			[DebuggerHidden]
			get
			{
				return MyWpfExtension.s_Windows.GetInstance;
			}
		}

		// Token: 0x04000002 RID: 2
		private static MyProject.ThreadSafeObjectProvider<Computer> s_Computer = new MyProject.ThreadSafeObjectProvider<Computer>();

		// Token: 0x04000003 RID: 3
		private static MyProject.ThreadSafeObjectProvider<User> s_User = new MyProject.ThreadSafeObjectProvider<User>();

		// Token: 0x04000004 RID: 4
		private static MyProject.ThreadSafeObjectProvider<MyWpfExtension.MyWindows> s_Windows = new MyProject.ThreadSafeObjectProvider<MyWpfExtension.MyWindows>();

		// Token: 0x04000005 RID: 5
		private static MyProject.ThreadSafeObjectProvider<Log> s_Log = new MyProject.ThreadSafeObjectProvider<Log>();

		// Token: 0x02000005 RID: 5
		[MyGroupCollection("System.Windows.Window", "Create__Instance__", "Dispose__Instance__", "My.MyWpfExtenstionModule.Windows")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyWindows
		{
			// Token: 0x0600000C RID: 12 RVA: 0x00004378 File Offset: 0x00002778
			[DebuggerHidden]
			private static T Create__Instance__<T>(T Instance) where T : Window, new()
			{
				bool flag = Instance == null;
				T result;
				if (flag)
				{
					bool flag2 = MyWpfExtension.MyWindows.s_WindowBeingCreated != null;
					if (flag2)
					{
						bool flag3 = MyWpfExtension.MyWindows.s_WindowBeingCreated.ContainsKey(typeof(!!0));
						if (flag3)
						{
							throw new InvalidOperationException("The window cannot be accessed via My.Windows from the Window constructor.");
						}
					}
					else
					{
						MyWpfExtension.MyWindows.s_WindowBeingCreated = new Hashtable();
					}
					MyWpfExtension.MyWindows.s_WindowBeingCreated.Add(typeof(!!0), null);
					result = Activator.CreateInstance<T>();
				}
				else
				{
					result = Instance;
				}
				return result;
			}

			// Token: 0x0600000D RID: 13 RVA: 0x00002034 File Offset: 0x00000434
			[DebuggerHidden]
			private void Dispose__Instance__<T>(ref T instance) where T : Window
			{
				instance = default(!!0);
			}

			// Token: 0x0600000E RID: 14 RVA: 0x00002000 File Offset: 0x00000400
			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public MyWindows()
			{
			}

			// Token: 0x0600000F RID: 15 RVA: 0x000043F8 File Offset: 0x000027F8
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}

			// Token: 0x06000010 RID: 16 RVA: 0x00004418 File Offset: 0x00002818
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x06000011 RID: 17 RVA: 0x00004430 File Offset: 0x00002830
			[EditorBrowsable(EditorBrowsableState.Never)]
			internal new Type GetType()
			{
				return typeof(MyWpfExtension.MyWindows);
			}

			// Token: 0x06000012 RID: 18 RVA: 0x0000444C File Offset: 0x0000284C
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override string ToString()
			{
				return base.ToString();
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x06000013 RID: 19 RVA: 0x0000203E File Offset: 0x0000043E
			// (set) Token: 0x06000021 RID: 33 RVA: 0x000021B8 File Offset: 0x000005B8
			public AccounttoAccount AccounttoAccount
			{
				[DebuggerHidden]
				get
				{
					this.m_AccounttoAccount = MyWpfExtension.MyWindows.Create__Instance__<AccounttoAccount>(this.m_AccounttoAccount);
					return this.m_AccounttoAccount;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_AccounttoAccount)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<AccounttoAccount>(ref this.m_AccounttoAccount);
					}
				}
			}

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000014 RID: 20 RVA: 0x00002059 File Offset: 0x00000459
			// (set) Token: 0x06000022 RID: 34 RVA: 0x000021E4 File Offset: 0x000005E4
			public AddAccount AddAccount
			{
				[DebuggerHidden]
				get
				{
					this.m_AddAccount = MyWpfExtension.MyWindows.Create__Instance__<AddAccount>(this.m_AddAccount);
					return this.m_AddAccount;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_AddAccount)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<AddAccount>(ref this.m_AddAccount);
					}
				}
			}

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000015 RID: 21 RVA: 0x00002074 File Offset: 0x00000474
			// (set) Token: 0x06000023 RID: 35 RVA: 0x00002210 File Offset: 0x00000610
			public AddAgentForm AddAgentForm
			{
				[DebuggerHidden]
				get
				{
					this.m_AddAgentForm = MyWpfExtension.MyWindows.Create__Instance__<AddAgentForm>(this.m_AddAgentForm);
					return this.m_AddAgentForm;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_AddAgentForm)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<AddAgentForm>(ref this.m_AddAgentForm);
					}
				}
			}

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000016 RID: 22 RVA: 0x0000208F File Offset: 0x0000048F
			// (set) Token: 0x06000024 RID: 36 RVA: 0x0000223C File Offset: 0x0000063C
			public AddCustomer AddCustomer
			{
				[DebuggerHidden]
				get
				{
					this.m_AddCustomer = MyWpfExtension.MyWindows.Create__Instance__<AddCustomer>(this.m_AddCustomer);
					return this.m_AddCustomer;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_AddCustomer)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<AddCustomer>(ref this.m_AddCustomer);
					}
				}
			}

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000017 RID: 23 RVA: 0x000020AA File Offset: 0x000004AA
			// (set) Token: 0x06000025 RID: 37 RVA: 0x00002268 File Offset: 0x00000668
			public DutyExpEntry DutyExpEntry
			{
				[DebuggerHidden]
				get
				{
					this.m_DutyExpEntry = MyWpfExtension.MyWindows.Create__Instance__<DutyExpEntry>(this.m_DutyExpEntry);
					return this.m_DutyExpEntry;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_DutyExpEntry)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<DutyExpEntry>(ref this.m_DutyExpEntry);
					}
				}
			}

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000018 RID: 24 RVA: 0x000020C5 File Offset: 0x000004C5
			// (set) Token: 0x06000026 RID: 38 RVA: 0x00002294 File Offset: 0x00000694
			public MainWindow MainWindow
			{
				[DebuggerHidden]
				get
				{
					this.m_MainWindow = MyWpfExtension.MyWindows.Create__Instance__<MainWindow>(this.m_MainWindow);
					return this.m_MainWindow;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_MainWindow)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<MainWindow>(ref this.m_MainWindow);
					}
				}
			}

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000019 RID: 25 RVA: 0x000020E0 File Offset: 0x000004E0
			// (set) Token: 0x06000027 RID: 39 RVA: 0x000022C0 File Offset: 0x000006C0
			public MiscExpEntry MiscExpEntry
			{
				[DebuggerHidden]
				get
				{
					this.m_MiscExpEntry = MyWpfExtension.MyWindows.Create__Instance__<MiscExpEntry>(this.m_MiscExpEntry);
					return this.m_MiscExpEntry;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_MiscExpEntry)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<MiscExpEntry>(ref this.m_MiscExpEntry);
					}
				}
			}

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x0600001A RID: 26 RVA: 0x000020FB File Offset: 0x000004FB
			// (set) Token: 0x06000028 RID: 40 RVA: 0x000022EC File Offset: 0x000006EC
			public OfficeExpEntry OfficeExpEntry
			{
				[DebuggerHidden]
				get
				{
					this.m_OfficeExpEntry = MyWpfExtension.MyWindows.Create__Instance__<OfficeExpEntry>(this.m_OfficeExpEntry);
					return this.m_OfficeExpEntry;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_OfficeExpEntry)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<OfficeExpEntry>(ref this.m_OfficeExpEntry);
					}
				}
			}

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x0600001B RID: 27 RVA: 0x00002116 File Offset: 0x00000516
			// (set) Token: 0x06000029 RID: 41 RVA: 0x00002318 File Offset: 0x00000718
			public PaymentAgentEntry PaymentAgentEntry
			{
				[DebuggerHidden]
				get
				{
					this.m_PaymentAgentEntry = MyWpfExtension.MyWindows.Create__Instance__<PaymentAgentEntry>(this.m_PaymentAgentEntry);
					return this.m_PaymentAgentEntry;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_PaymentAgentEntry)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<PaymentAgentEntry>(ref this.m_PaymentAgentEntry);
					}
				}
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x0600001C RID: 28 RVA: 0x00002131 File Offset: 0x00000531
			// (set) Token: 0x0600002A RID: 42 RVA: 0x00002344 File Offset: 0x00000744
			public PaymentPkrEntry PaymentPkrEntry
			{
				[DebuggerHidden]
				get
				{
					this.m_PaymentPkrEntry = MyWpfExtension.MyWindows.Create__Instance__<PaymentPkrEntry>(this.m_PaymentPkrEntry);
					return this.m_PaymentPkrEntry;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_PaymentPkrEntry)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<PaymentPkrEntry>(ref this.m_PaymentPkrEntry);
					}
				}
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600001D RID: 29 RVA: 0x0000214C File Offset: 0x0000054C
			// (set) Token: 0x0600002B RID: 43 RVA: 0x00002370 File Offset: 0x00000770
			public PurchaseAuto PurchaseAuto
			{
				[DebuggerHidden]
				get
				{
					this.m_PurchaseAuto = MyWpfExtension.MyWindows.Create__Instance__<PurchaseAuto>(this.m_PurchaseAuto);
					return this.m_PurchaseAuto;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_PurchaseAuto)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<PurchaseAuto>(ref this.m_PurchaseAuto);
					}
				}
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600001E RID: 30 RVA: 0x00002167 File Offset: 0x00000567
			// (set) Token: 0x0600002C RID: 44 RVA: 0x0000239C File Offset: 0x0000079C
			public ReceiptEntry ReceiptEntry
			{
				[DebuggerHidden]
				get
				{
					this.m_ReceiptEntry = MyWpfExtension.MyWindows.Create__Instance__<ReceiptEntry>(this.m_ReceiptEntry);
					return this.m_ReceiptEntry;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_ReceiptEntry)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<ReceiptEntry>(ref this.m_ReceiptEntry);
					}
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600001F RID: 31 RVA: 0x00002182 File Offset: 0x00000582
			// (set) Token: 0x0600002D RID: 45 RVA: 0x000023C8 File Offset: 0x000007C8
			public SaleAuto SaleAuto
			{
				[DebuggerHidden]
				get
				{
					this.m_SaleAuto = MyWpfExtension.MyWindows.Create__Instance__<SaleAuto>(this.m_SaleAuto);
					return this.m_SaleAuto;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_SaleAuto)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<SaleAuto>(ref this.m_SaleAuto);
					}
				}
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x06000020 RID: 32 RVA: 0x0000219D File Offset: 0x0000059D
			// (set) Token: 0x0600002E RID: 46 RVA: 0x000023F4 File Offset: 0x000007F4
			public UserLogin UserLogin
			{
				[DebuggerHidden]
				get
				{
					this.m_UserLogin = MyWpfExtension.MyWindows.Create__Instance__<UserLogin>(this.m_UserLogin);
					return this.m_UserLogin;
				}
				[DebuggerHidden]
				set
				{
					if (value != this.m_UserLogin)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						this.Dispose__Instance__<UserLogin>(ref this.m_UserLogin);
					}
				}
			}

			// Token: 0x04000006 RID: 6
			[ThreadStatic]
			private static Hashtable s_WindowBeingCreated;

			// Token: 0x04000007 RID: 7
			[EditorBrowsable(EditorBrowsableState.Never)]
			public AccounttoAccount m_AccounttoAccount;

			// Token: 0x04000008 RID: 8
			[EditorBrowsable(EditorBrowsableState.Never)]
			public AddAccount m_AddAccount;

			// Token: 0x04000009 RID: 9
			[EditorBrowsable(EditorBrowsableState.Never)]
			public AddAgentForm m_AddAgentForm;

			// Token: 0x0400000A RID: 10
			[EditorBrowsable(EditorBrowsableState.Never)]
			public AddCustomer m_AddCustomer;

			// Token: 0x0400000B RID: 11
			[EditorBrowsable(EditorBrowsableState.Never)]
			public DutyExpEntry m_DutyExpEntry;

			// Token: 0x0400000C RID: 12
			[EditorBrowsable(EditorBrowsableState.Never)]
			public MainWindow m_MainWindow;

			// Token: 0x0400000D RID: 13
			[EditorBrowsable(EditorBrowsableState.Never)]
			public MiscExpEntry m_MiscExpEntry;

			// Token: 0x0400000E RID: 14
			[EditorBrowsable(EditorBrowsableState.Never)]
			public OfficeExpEntry m_OfficeExpEntry;

			// Token: 0x0400000F RID: 15
			[EditorBrowsable(EditorBrowsableState.Never)]
			public PaymentAgentEntry m_PaymentAgentEntry;

			// Token: 0x04000010 RID: 16
			[EditorBrowsable(EditorBrowsableState.Never)]
			public PaymentPkrEntry m_PaymentPkrEntry;

			// Token: 0x04000011 RID: 17
			[EditorBrowsable(EditorBrowsableState.Never)]
			public PurchaseAuto m_PurchaseAuto;

			// Token: 0x04000012 RID: 18
			[EditorBrowsable(EditorBrowsableState.Never)]
			public ReceiptEntry m_ReceiptEntry;

			// Token: 0x04000013 RID: 19
			[EditorBrowsable(EditorBrowsableState.Never)]
			public SaleAuto m_SaleAuto;

			// Token: 0x04000014 RID: 20
			[EditorBrowsable(EditorBrowsableState.Never)]
			public UserLogin m_UserLogin;
		}
	}
}
