using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Domain.ValueObjects.Base;
using Education.ValueObjects.Exceptions;

namespace Education.Domain.ValueObjects.Validators
{
    public class LessonTopicValidator : IValidator<string>
    {
        public static int MAX_LENGTH => 100;
        public static int MAX_WHITESPACES => 10;

        public void Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new LessonTopicNullOrWhiteSpaceException(nameof(value));

            if (value.Length > MAX_LENGTH)
                throw new LessonTopicTooLongException(value, MAX_LENGTH);

            if (value.Any(c => !(char.IsLetter(c) || char.IsWhiteSpace(c))))
                throw new LessonTopicInvalidCharactersException(value);

            if (value.Count(c => char.IsWhiteSpace(c)) > MAX_WHITESPACES)
                throw new LessonTopicTooManyWhitespacesException(value, MAX_WHITESPACES);

            if (value.Contains("  "))
                throw new LessonTopicDoubleWhitespaceException(value);
        }
    }
}