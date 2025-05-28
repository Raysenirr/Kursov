using Education.Domain.Entities.Base;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Domain.Entities
{
    public class Student : Person
    {
        #region Свойства
        private readonly ICollection<Lesson> _lessons = [];
        private readonly ICollection<Grade> _grades = [];

        public IReadOnlyCollection<Lesson> AttendedLessons => [.. _lessons];
        public IReadOnlyCollection<Grade> RecievedGrades => [.. _grades];

        public Group Group { get; protected set; }
        #endregion

        #region Конструкторы

        protected Student(Guid id, PersonName name, Group group) : base(id, name)
        {
            Group = group ?? throw new GroupIsNullException();

            if (!group.Students.Contains(this))
                group.AddStudent(this);
        }

        public Student(PersonName name, Group group)
            : this(Guid.NewGuid(), name, group) { }

        protected Student(Guid id, PersonName name) : base(id, name) { }

        #endregion

        #region Методы

        public void AttendLesson(Lesson lesson)
        {
            if (lesson.State != LessonStatus.Teached)
                throw new LessonNotStartedException(lesson);

            if (lesson.Group != Group)
                throw new AnotherGroupLessonException(this, lesson.Group);

            if (_lessons.Contains(lesson))
                throw new DoubleVisitedLessonException(lesson, this);

            _lessons.Add(lesson);
        }

        public void SubmitHomework(Homework homework, DateTime submissionDate)
        {
            if (homework == null)
                throw new HomeworkNullException();

            if (homework.Lesson.Group != Group)
                throw new AnotherGroupLessonException(this, homework.Lesson.Group);

            if (!_lessons.Contains(homework.Lesson))
                throw new LessonNotVisitedException(homework.Lesson, this);

            if (homework.Submissions.Any(s => s.StudentId == Id))
                throw new HomeworkAlreadySubmittedException(homework);

            homework.SubmitBy(this, submissionDate);
        }

        public Grade GetGradeByLesson(Lesson lesson)
        {
            if (lesson == null)
                throw new LessonNullException();

            var grade = _grades.FirstOrDefault(g => g.Lesson == lesson);

            if (grade == null)
                throw new GradeNotFoundException(this, lesson);

            return grade;
        }

        internal void GetGrade(Grade grade)
        {
            if (grade.Student != this)
                throw new AnotherStudentGradeException(this, grade);

            if (!_lessons.Contains(grade.Lesson))
                throw new LessonNotVisitedException(grade.Lesson, this);

            if (_grades.Contains(grade))
                throw new DoubleGradeStudentLesson(grade.Lesson, this);

            _grades.Add(grade);
        }

        public IReadOnlyCollection<Homework> GetAssignedHomeworks()
        {
            var result = new List<Homework>();

            foreach (var lesson in _lessons)
            {
                result.AddRange(lesson.Homeworks);
            }

            return result.AsReadOnly();
        }

        ///<summary>Получить только те задания, которые были сданы студентом.</summary>
        public IReadOnlyCollection<Homework> GetSubmittedHomeworks()
        {
            var result = new List<Homework>();

            foreach (var lesson in _lessons)
            {
                foreach (var homework in lesson.Homeworks)
                {
                    if (homework.Submissions.Any(s => s.StudentId == Id))
                        result.Add(homework);
                }
            }

            return result.AsReadOnly();
        }

        ///<summary>Получить список всех заданий и их статуса: сдано или не сдано.</summary>
        public IReadOnlyCollection<(Homework Homework, bool IsSubmitted)> GetHomeworksWithStatus()
        {
            var result = new List<(Homework, bool)>();

            foreach (var lesson in _lessons)
            {
                foreach (var homework in lesson.Homeworks)
                {
                    bool isSubmitted = homework.Submissions.Any(s => s.StudentId == Id);
                    result.Add((homework, isSubmitted));
                }
            }

            return result.AsReadOnly();
        }

        ///<summary>Получить все оценки, полученные студентом.</summary>
        public IReadOnlyCollection<Grade> GetAllGrades()
        {
            return RecievedGrades;
        }


        #endregion
    }
}
