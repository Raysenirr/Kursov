using Education.Domain.ValueObjects.Base;
using Education.Domain.ValueObjects.Validators;

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
