using Education.Domain.ValueObjects.Base;
using Education.Domain.ValueObjects.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.ValueObjects
{
    namespace Education.Domain.ValueObjects
    {
        /// <summary>
        /// Группа
        /// </summary>
        /// <param name="name">Обязательное значение названия группы.</param>
        public class GroupName(string name)
            : ValueObject<string>(new GroupNameValidator(), name)
        {
        }
    }
}
