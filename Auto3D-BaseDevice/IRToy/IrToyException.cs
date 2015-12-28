using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrToyLibrary {
    public class IrToyException : ApplicationException {

        public IrToyException() { } 
        public IrToyException(string message):base(message) { }
        public IrToyException(string message, Exception inner) : base(message, inner) { } 
    }
}
