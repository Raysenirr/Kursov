using Education.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке проверить ДЗ в недопустимом состоянии.
    /// </summary>
    public class InvalidHomeworkCheckException(Homework homework)
        : InvalidOperationException($"Homework {homework.Id} cannot be checked — only submitted homework can be checked.")
    {
        public Homework Homework => homework;
    }
}
