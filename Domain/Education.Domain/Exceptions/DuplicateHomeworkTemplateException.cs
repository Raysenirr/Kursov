using Education.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при попытке добавить в банк шаблонов задание с уже существующей темой.
    /// </summary>
    public class DuplicateHomeworkTemplateException(LessonTopic topic)
        : InvalidOperationException($"A homework template for topic '{topic.Value}' already exists.")
    {
        public LessonTopic Topic => topic;
    }
}
