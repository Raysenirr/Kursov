using Education.Domain.ValueObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.ValueObjects.Exceptions;

namespace Education.Domain.ValueObjects.Validators
{
    public class PersonNameValidator : IValidator<string>
    {
        public static int MAX_LENGTH => 50;
        public static int MAX_WHITESPACES => 5;

        public static int MIN_LENGTH => 2;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new PersonNameNullException(nameof(value));

            if (value.Length > MAX_LENGTH)
                throw new PersonNameTooLongException(value, MAX_LENGTH);

            if (value.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
                throw new PersonNameInvalidCharactersException(value);

            if (value.Count(c => char.IsWhiteSpace(c)) > MAX_WHITESPACES)
                throw new PersonNameTooManyWhitespacesException(value, MAX_WHITESPACES);

            if (value.Contains("  "))
                throw new PersonNameDoubleWhitespaceException(value);

            if (value.Length < MIN_LENGTH)
                throw new PersonNameTooShortException(value, MIN_LENGTH);
        }
    }
}
