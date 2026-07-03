using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts.My
{
	// Token: 0x02000002 RID: 2
	[StandardModule]
	[GeneratedCode("MyTemplate", "11.0.0.0")]
	[HideModuleName]
	internal sealed class MyProject
	{
		// Token: 0x02000003 RID: 3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[ComVisible(false)]
		internal sealed class ThreadSafeObjectProvider<T> where T : new()
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000004 RID: 4 RVA: 0x000042B8 File Offset: 0x000026B8
			internal T GetInstance
			{
				[DebuggerHidden]
				get
				{
					bool flag = MyProject.ThreadSafeObjectProvider<!0>.m_ThreadStaticValue == null;
					if (flag)
					{
						MyProject.ThreadSafeObjectProvider<!0>.m_ThreadStaticValue = Activator.CreateInstance<T>();
					}
					return MyProject.ThreadSafeObjectProvider<!0>.m_ThreadStaticValue;
				}
			}

			// Token: 0x06000005 RID: 5 RVA: 0x00002000 File Offset: 0x00000400
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public ThreadSafeObjectProvider()
			{
			}

			// Token: 0x04000001 RID: 1
			[ThreadStatic]
			[CompilerGenerated]
			private static T m_ThreadStaticValue;
		}
	}
}
