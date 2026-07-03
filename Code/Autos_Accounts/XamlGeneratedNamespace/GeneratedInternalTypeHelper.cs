using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

namespace Autos_Accounts.XamlGeneratedNamespace
{
	// Token: 0x0200001C RID: 28
	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class GeneratedInternalTypeHelper : InternalTypeHelper
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x0001E3A8 File Offset: 0x0001C7A8
		protected override object CreateInstance(Type type, CultureInfo culture)
		{
			return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, culture);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001E3C8 File Offset: 0x0001C7C8
		protected override object GetPropertyValue(PropertyInfo propertyInfo, object target, CultureInfo culture)
		{
			return propertyInfo.GetValue(RuntimeHelpers.GetObjectValue(target), BindingFlags.Default, null, null, culture);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00004284 File Offset: 0x00002684
		protected override void SetPropertyValue(PropertyInfo propertyInfo, object target, object value, CultureInfo culture)
		{
			propertyInfo.SetValue(RuntimeHelpers.GetObjectValue(target), RuntimeHelpers.GetObjectValue(value), BindingFlags.Default, null, null, culture);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001E3EC File Offset: 0x0001C7EC
		protected override Delegate CreateDelegate(Type delegateType, object target, string handler)
		{
			return (Delegate)target.GetType().InvokeMember("_CreateDelegate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, RuntimeHelpers.GetObjectValue(target), new object[]
			{
				delegateType,
				handler
			}, null);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000429F File Offset: 0x0000269F
		protected override void AddEventHandler(EventInfo eventInfo, object target, Delegate handler)
		{
			eventInfo.AddEventHandler(RuntimeHelpers.GetObjectValue(target), handler);
		}
	}
}
