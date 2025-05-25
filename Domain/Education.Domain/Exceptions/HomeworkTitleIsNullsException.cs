using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class HomeworkTitleIsNullsException()
        : ArgumentNullException("title", "Homework title cannot be null.")
    {
        public string Field => "title";
    }
}
