using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Autos_Accounts
{
	// Token: 0x02000014 RID: 20
	public class clsComputerInfo
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x000083A4 File Offset: 0x000067A4
		internal string GetProcessorId()
		{
			string result = string.Empty;
			SelectQuery query = new SelectQuery("Win32_processor");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
			try
			{
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					result = managementObject["processorId"].ToString();
				}
			}
			finally
			{
				ManagementObjectCollection.ManagementObjectEnumerator enumerator;
				if (enumerator != null)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return result;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000842C File Offset: 0x0000682C
		internal string GetMACAddress()
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection instances = managementClass.GetInstances();
			string text = string.Empty;
			try
			{
				foreach (ManagementBaseObject managementBaseObject in instances)
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					bool flag = text.Equals(string.Empty);
					if (flag)
					{
						bool flag2 = Conversions.ToBoolean(managementObject["IPEnabled"]);
						if (flag2)
						{
							text = managementObject["MacAddress"].ToString();
						}
						managementObject.Dispose();
					}
					text = text.Replace(":", string.Empty);
				}
			}
			finally
			{
				ManagementObjectCollection.ManagementObjectEnumerator enumerator;
				if (enumerator != null)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return text;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000084F4 File Offset: 0x000068F4
		internal string GetVolumeSerial(string strDriveLetter = "C")
		{
			ManagementObject managementObject = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", strDriveLetter));
			managementObject.Get();
			return managementObject["VolumeSerialNumber"].ToString();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008530 File Offset: 0x00006930
		internal string GetMotherBoardID()
		{
			string result = string.Empty;
			SelectQuery query = new SelectQuery("Win32_BaseBoard");
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
			try
			{
				foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					result = managementObject["SerialNumber"].ToString();
				}
			}
			finally
			{
				ManagementObjectCollection.ManagementObjectEnumerator enumerator;
				if (enumerator != null)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			return result;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000085B8 File Offset: 0x000069B8
		internal string getMD5Hash(string strToHash)
		{
			MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = Encoding.ASCII.GetBytes(strToHash);
			array = md5CryptoServiceProvider.ComputeHash(array);
			string text = "";
			foreach (byte b in array)
			{
				text += b.ToString("x2");
			}
			return text;
		}
	}
}
