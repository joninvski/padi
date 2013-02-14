using System;

namespace MMG.Config
{
	public class ParseErrorException : Exception
	{
		public ParseErrorException(string msg) : base(msg) {}
	}
}
