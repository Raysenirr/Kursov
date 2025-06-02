using Education.Domain.Entities.Base;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Domain.Entities
{
    /// <summary>
    /// Домашнее задание, выданное по теме урока
    /// </summary>
    public class Homework : Entity<Guid>
    {
        #region Свойства
        /// <summary> Урок, к которому относится домашнее задание </summary>
        public Lesson Lesson { get; private set; }

        /// <summary> Идентификатор урока </summary>
        public Guid LessonId { get; private set; }

        /// <summary> Заголовок домашнего задания </summary>
        public HomeworkTitle Title { get; private set; }

        /// <summary> Внутренний список сданных домашних заданий </summary>
        private readonly ICollection<HomeworkSubmission> _submissions = [];

        /// <summary> Список сданных домашних заданий (только для чтения, не отображается в БД) </summary>
        [NotMapped]
        public IReadOnlyCollection<HomeworkSubmission> Submissions => [.. _submissions];
        #endregion
        #region Конструкторы
        /// <summary>
        /// Конструктор для EF
        /// </summary>
        protected Homework() : base(Guid.NewGuid())
        {
            _submissions = new List<HomeworkSubmission>();
        }


        public Homework(Lesson lesson, HomeworkTitle title)
            : this(Guid.NewGuid(), lesson, title)
        {
        }

        protected Homework(Guid id, Lesson lesson, HomeworkTitle title)
            : base(id)
        {
            Lesson = lesson ?? throw new LessonIsNullException();
            Title = title ?? throw new HomeworkTitleIsNullsException();
            _submissions = new List<HomeworkSubmission>();
        }

        #endregion
        #region Методы
        /// <summary> Принимает домашнее задание от студента и сохраняет его с датой сдачи </summary>
        public void SubmitBy(Student student, DateTime submissionDate)
        {
            if (student == null)
                throw new StudentIsNullException();

            ValidateSubmission(student, submissionDate);
            _submissions.Add(new HomeworkSubmission(student, this, submissionDate));
        }
        /// <summary> Проверяет, сдал ли студент задание позже урока </summary>
        public bool IsLate(Student student)
        {
            var submission = _submissions.FirstOrDefault(s => s.StudentId == student.Id);
            return submission != null && submission.SubmissionDate > Lesson.ClassTime.AddSeconds(1);
        }
        /// <summary> Выполняет проверку перед приёмом домашнего задания </summary>
        private void ValidateSubmission(Student student, DateTime submissionDate)
        {
            if (!student.AttendedLessons.Any(l => l != null && l.Id == Lesson.Id))
                throw new LessonNotVisitedException(Lesson, student);

            if (_submissions.Any(s => s.StudentId == student.Id))
                throw new HomeworkAlreadySubmittedException(this);

            if (submissionDate > DateTime.UtcNow.AddMinutes(1))
                throw new InvalidSubmissionDateException(submissionDate);
        }
        #endregion
    }
}


