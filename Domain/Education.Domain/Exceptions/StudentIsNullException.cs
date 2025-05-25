using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class StudentIsNullException()
        : ArgumentNullException("student", "Student cannot be null.")
    {
        public string Field => "student";
    }
}