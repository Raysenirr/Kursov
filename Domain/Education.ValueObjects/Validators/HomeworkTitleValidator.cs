using Education.Domain.ValueObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.ValueObjects.Exceptions;

namespace Education.Domain.ValueObjects.Validators
{
    public class HomeworkTitleValidator : IValidator<string>
    {
        public static int MAX_LENGTH => 100;
        public static int MIN_LENGTH => 3;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new HomeworkTitleNullException(nameof(value));

            if (value.Length < MIN_LENGTH)
                throw new HomeworkTitleTooShortException(value, MIN_LENGTH);

            if (value.Length > MAX_LENGTH)
                throw new HomeworkTitleTooLongException(value, MAX_LENGTH);
        }
    }
}
