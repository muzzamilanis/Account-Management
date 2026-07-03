using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Autos_Accounts.My;
using Microsoft.VisualBasic.ApplicationServices;

namespace Autos_Accounts
{
	// Token: 0x02000016 RID: 22
	public partial class Application : Application
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00008B08 File Offset: 0x00006F08
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Exception ex = e.Exception;
			string text = MyWpfExtension.Application.Info.DirectoryPath + "\\GeneratorTestbedError.txt";
			using (TextWriter textWriter = new StreamWriter(text, true))
			{
				DateTime now = DateTime.Now;
				textWriter.WriteLine("The error time: " + now.ToShortDateString() + " " + now.ToShortTimeString());
				while (ex != null)
				{
					textWriter.WriteLine("Exception: " + ex.ToString());
					ex = ex.InnerException;
				}
			}
			MessageBox.Show(Convert.ToString("The program crashed.  A stack trace can be found at:\n") + text);
			e.Handled = true;
			Application.Current.Shutdown();
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008BDC File Offset: 0x00006FDC
		internal AssemblyInfo Info
		{
			[DebuggerHidden]
			get
			{
				return new AssemblyInfo(Assembly.GetExecutingAssembly());
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008C54 File Offset: 0x00007054
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[STAThread]
		public static void Main()
		{
			Application application = new Application();
			application.InitializeComponent();
			application.Run();
		}
	}
}
