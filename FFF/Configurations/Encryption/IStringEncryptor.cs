using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Configurations
{
	public interface IStringEncryptor
	{
		string EncryptString(string plainText);
		string DecryptString(string encryptedText);
	}
}
