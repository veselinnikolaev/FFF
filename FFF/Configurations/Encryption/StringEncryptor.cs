﻿using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Configurations
{
	public class StringEncryptor
	{
		private static IKernel _kernel;

		static StringEncryptor()
		{
			_kernel = new StandardKernel(new EncryptionModule());
		}

		public static string EncryptString(string plainText)
		{
			return _kernel.Get<IStringEncryptor>().EncryptString(plainText);
		}

		public static string DecryptString(string encryptedText)
		{
			return _kernel.Get<IStringEncryptor>().DecryptString(encryptedText);
		}
	}
}
