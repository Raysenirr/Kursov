using Education.Domain.ValueObjects.Base;
using Education.Domain.ValueObjects.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.ValueObjects
{
    /// <summary>
    /// Имя человека.
    /// </summary>
    /// <param name="name">Обязательное строковое значение имени.</param>
    public class PersonName(string name)
        : ValueObject<string>(new PersonNameValidator(), name)
    {
    }
}
