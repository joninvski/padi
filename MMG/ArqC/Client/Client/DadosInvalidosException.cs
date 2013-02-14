using System;
using System.Collections;
using System.Text;

namespace MMG.Exec
{
    public class DadosInvalidosException : Exception
    {
        public DadosInvalidosException(string msg) : base(msg) { }
    }
}
