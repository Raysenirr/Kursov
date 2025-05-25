using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.ValueObjects.Exceptions
{
    public class HomeworkTitleIsNullException()
        : ArgumentNullException("title", "Homework title cannot be null.")
    {
        public string Field => "title";
    }
}
