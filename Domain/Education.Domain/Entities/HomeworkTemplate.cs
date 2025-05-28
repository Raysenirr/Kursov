using Education.Domain.Exceptions;
using Education.Domain.ValueObjects;

namespace Education.Domain.Entities
{
    /// <summary>
    /// Шаблон домашнего задания (Owned by HomeworkBank)
    /// </summary>
    public class HomeworkTemplate
    {
        #region Свойства
        /// <summary> Тема урока, к которой относится шаблон домашнего задания </summary>
        public LessonTopic Topic { get; private set; }

        /// <summary> Заголовок (название) шаблона домашнего задания </summary>
        public HomeworkTitle Title { get; private set; }
        #endregion

        #region Конструкторы

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
        #endregion

        #region Методы
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
        #endregion
    }
}
