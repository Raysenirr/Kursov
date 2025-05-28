using Education.Domain.Exceptions;

namespace Education.Domain.Entities
{
    public class HomeworkSubmission
    {
        #region Свойства
        /// <summary> Идентификатор студента, сдавшего домашнее задание </summary>
        public Guid StudentId { get; private set; }

        /// <summary> Студент, сдавший домашнее задание </summary>
        public Student Student { get; private set; }

        /// <summary> Идентификатор домашнего задания, которое было сдано </summary>
        public Guid HomeworkId { get; private set; }

        /// <summary> Домашнее задание, к которому относится сдача </summary>
        public Homework Homework { get; private set; }

        /// <summary> Дата и время сдачи домашнего задания </summary>
        public DateTime SubmissionDate { get; private set; }
        #endregion

        #region Конструкторы
        // Конструктор для EF  
        private HomeworkSubmission() { }

        public HomeworkSubmission(Student student, Homework homework, DateTime submissionDate)
        {
            Student = student ?? throw new InvalidHomeworkSubmissionException(
                nameof(student));

            Homework = homework ?? throw new InvalidHomeworkSubmissionException(
                nameof(homework));

            if (submissionDate > DateTime.UtcNow.AddMinutes(1))
                throw new InvalidSubmissionDateException(submissionDate);

            if (submissionDate < homework.Lesson.ClassTime.AddMonths(-1))
                throw new InvalidHomeworkSubmissionException(
                    nameof(submissionDate));

            StudentId = student.Id;
            HomeworkId = homework.Id;
            SubmissionDate = submissionDate;
        }
        protected HomeworkSubmission(Guid studentId, Guid homeworkId, DateTime submissionDate)
        {
        }
        #endregion
    }
}