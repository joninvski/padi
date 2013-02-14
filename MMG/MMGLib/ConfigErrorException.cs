using System;

namespace MMG.Config
{
	public class ConfigErrorException : Exception
	{
		public ConfigErrorException(string msg) : base(msg) {}
	}
}
