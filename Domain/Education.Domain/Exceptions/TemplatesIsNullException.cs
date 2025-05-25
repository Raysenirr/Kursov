using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    internal class TemplatesIsNullException() : ArgumentNullException("Template", "Lesson cannot be null.")
    {
        public string Field => "Template";
    }
}
