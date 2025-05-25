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
    /// Тема урока.
    /// </summary>
    /// <param name="topic">Обязательное строковое значение темы.</param>
    public class LessonTopic(string topic)
        : ValueObject<string>(new LessonTopicValidator(), topic)
    {
    }
}