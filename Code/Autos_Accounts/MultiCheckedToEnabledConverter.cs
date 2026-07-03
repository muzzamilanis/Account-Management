using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Autos_Accounts
{
	// Token: 0x02000019 RID: 25
	public class MultiCheckedToEnabledConverter : IMultiValueConverter
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x0001E15C File Offset: 0x0001C55C
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = values != null;
			object result;
			if (flag)
			{
				result = values.OfType<bool>().Any((MultiCheckedToEnabledConverter._Closure$__.$I1-0 == null) ? (MultiCheckedToEnabledConverter._Closure$__.$I1-0 = ((bool b) => b)) : MultiCheckedToEnabledConverter._Closure$__.$I1-0);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001E1B4 File Offset: 0x0001C5B4
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[0];
		}
	}
}
