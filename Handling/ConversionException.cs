using System;
using System.Collections.Generic;
using System.Text;

namespace LinqImplementation.Handling
{
    public class ConversionException : Exception
    {
        public ConversionException() { }
        public ConversionException(string execType) : base(String.Format("Conversion failed for {0}. Please try again.", execType)) { }
    }
}
