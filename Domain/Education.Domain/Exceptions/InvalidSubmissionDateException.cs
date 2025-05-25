using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение при попытке сдачи домашнего задания в будущем.
    /// </summary>
    public class InvalidSubmissionDateException(DateTime invalidDate)
        : ArgumentOutOfRangeException(nameof(invalidDate), invalidDate, $"Submission date cannot be in the future: {invalidDate}")
    {
        public DateTime InvalidDate => invalidDate;
    }
}
