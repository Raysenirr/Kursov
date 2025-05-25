using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Domain.ValueObjects.Base;
using Education.Domain.ValueObjects.Validators;

namespace Education.Domain.ValueObjects
{
    /// <summary>
    /// ДЗ
    /// </summary>
    /// <param name="value">Обязательное строковое значение заголовка.</param>
    public class HomeworkTitle(string value)
        : ValueObject<string>(new HomeworkTitleValidator(), value)
    {
    }
}
