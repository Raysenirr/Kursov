using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.ValueObjects.Base
{
    /// <summary>
    /// Defines a method that implements the validation of the object.
    /// </summary>
    /// <typeparam name="T">Type of validation object.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates data.
        /// </summary>
        /// <param name="value">The verified value</param>
        void Validate(T value);
    }
}
