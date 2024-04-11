using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ninject.Modules;

namespace FFF.Configurations
{
	public class EncryptionModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IStringEncryptor>().To<TripleDESStringEncryptor>();
		}
	}
}
