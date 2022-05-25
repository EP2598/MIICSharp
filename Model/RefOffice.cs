using System;
using System.Collections.Generic;
using System.Text;

namespace LinqImplementation.Model
{
    public class RefOffice
    {
        public long RefOfficeId { get; set; }
        public string OfficeName { get; set; }
        public string Address { get; set; }
        public char IsActive { get; set; }
    }
}
