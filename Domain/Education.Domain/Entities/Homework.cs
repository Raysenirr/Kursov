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
        public Lesson Lesson { get; private set; }
        public Guid LessonId { get; private set; }
        public HomeworkTitle Title { get; private set; }

        private readonly ICollection<HomeworkSubmission> _submissions = new List<HomeworkSubmission>();

        [NotMapped]
        public IReadOnlyCollection<HomeworkSubmission> Submissions => new ReadOnlyCollection<HomeworkSubmission>(_submissions.ToList());

        /// <summary>
        /// Конструктор для EF Core
        /// </summary>
        protected Homework() : base(Guid.NewGuid())
        {
            _submissions = new List<HomeworkSubmission>();
        }

        /// <summary>
        /// Публичный конструктор для создания домашнего задания вручную
        /// </summary>
        public Homework(Lesson lesson, HomeworkTitle title)
            : this(Guid.NewGuid(), lesson, title)
        {
        }

        /// <summary>
        /// Защищённый конструктор с полным контролем (для наследников, тестов или маппинга)
        /// </summary>
        protected Homework(Guid id, Lesson lesson, HomeworkTitle title)
            : base(id)
        {
            Lesson = lesson ?? throw new LessonIsNullException();
            Title = title ?? throw new HomeworkTitleIsNullsException();
            _submissions = new List<HomeworkSubmission>();
        }


        public void SubmitBy(Student student, DateTime submissionDate)
        {
            if (student == null)
                throw new StudentIsNullException();

            ValidateSubmission(student, submissionDate);
            _submissions.Add(new HomeworkSubmission(student, this, submissionDate));
        }

        public bool IsLate(Student student)
        {
            var submission = _submissions.FirstOrDefault(s => s.StudentId == student.Id);
            return submission != null && submission.SubmissionDate > Lesson.ClassTime.AddSeconds(1);
        }

        private void ValidateSubmission(Student student, DateTime submissionDate)
        {
            if (!student.AttendedLessons.Contains(Lesson))
                throw new LessonNotVisitedException(Lesson, student);

            if (_submissions.Any(s => s.StudentId == student.Id))
                throw new HomeworkAlreadySubmittedException(this);

            if (submissionDate > DateTime.UtcNow.AddMinutes(1))
                throw new InvalidSubmissionDateException(submissionDate);
        }
    }
}


