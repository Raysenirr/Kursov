using Education.Domain.ValueObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.ValueObjects.Exceptions;


namespace Education.Domain.ValueObjects.Validators
{

    public class GroupNameValidator : IValidator<string>
    {
        public static int MAX_LENGTH => 10;
        public static int MIN_LENGTH => 2;
        public static int REQUIRED_MINUS_COUNT => 2;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new GroupNameNullException(nameof(value));

            if (value.Length < MIN_LENGTH)
                throw new GroupNameTooShortException(value, MIN_LENGTH);

            if (value.Length > MAX_LENGTH)
                throw new GroupNameTooLongException(value, MAX_LENGTH);

            if (value.Any(c => !(char.IsLetterOrDigit(c) || c == '-')))
                throw new GroupNameInvalidCharactersException(value);

            if (value.Count(c => c == '-') != REQUIRED_MINUS_COUNT)
                throw new GroupNameMinusCountException(value, REQUIRED_MINUS_COUNT);

            if (value.Contains("--"))
                throw new GroupNameDoubleMinusException(value);
        }
    }
}