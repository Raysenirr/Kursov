using Education.Domain.Exceptions;
using Education.Domain.ValueObjects;
using System;


namespace Education.Domain.Entities
{
    /// <summary>
    /// Шаблон домашнего задания (Owned by HomeworkBank)
    /// </summary>
    public class HomeworkTemplate
    {
        public LessonTopic Topic { get; private set; }
        public HomeworkTitle Title { get; private set; }

        /// <summary>
        /// Конструктор для доменной логики.
        /// </summary>
        public HomeworkTemplate(LessonTopic topic, HomeworkTitle title)
        {
            Topic = topic ?? throw new LessonTopicIsNullsException();
            Title = title ?? throw new HomeworkTitleIsNullsException();
        }

        /// <summary>
        /// EF 
        /// </summary>
        protected HomeworkTemplate()
        {
            Topic = null!;
            Title = null!;
        }

        /// <summary>
        /// Обновление темы.
        /// </summary>
        public void UpdateTopic(LessonTopic newTopic)
        {
            Topic = newTopic ?? throw new LessonTopicIsNullsException();
        }

        /// <summary>
        /// Обновление заголовка.
        /// </summary>
        public void UpdateTitle(HomeworkTitle newTitle)
        {
            Title = newTitle ?? throw new HomeworkTitleIsNullsException();
        }
    }
}
