using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    public class HomeworkTitleNullException(string paramName)
        : ArgumentNullException(paramName, "Homework title must not be null or whitespace.")
    {
        public string ParameterName => paramName;
    }

}
