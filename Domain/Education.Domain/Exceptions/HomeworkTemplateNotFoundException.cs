using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при отсутствии шаблона домашнего задания по указанной теме в банке учителя.
    /// </summary>
    public class HomeworkTemplateNotFoundException(LessonTopic topic, Teacher teacher)
        : InvalidOperationException($"Teacher {teacher.Name} has no homework template for topic '{topic.Value}'.")
    {
        public LessonTopic Topic => topic;
        public Teacher Teacher => teacher;
    }
}
