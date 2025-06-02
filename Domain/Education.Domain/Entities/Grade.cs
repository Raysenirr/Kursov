using Education.Domain.Entities.Base;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;


namespace Education.Domain.Entities
{
    /// <summary>
    /// Оценка студента за урок
    /// </summary>
    public class Grade : Entity<Guid>
    {
        #region Свойства

        /// <summary> Преподаватель, который поставил оценку </summary>
        public Teacher Teacher { get; private set; }

        /// <summary> Студент, которому поставлена оценка </summary>
        public Student Student { get; private set; }

        /// <summary> Урок, за который поставлена оценка </summary>
        public Lesson Lesson { get; private set; }

        /// <summary> Время, когда была выставлена оценка </summary>
        public DateTime GradedTime { get; private set; }

        /// <summary> Значение оценки (оценка как таковая) </summary>
        public Mark Mark { get; private set; }

        /// <summary> Идентификатор студента, получившего оценку </summary>
        public Guid StudentId { get; private set; }

        /// <summary> Идентификатор урока, за который поставлена оценка </summary>
        public Guid LessonId { get; private set; }

        /// <summary> Идентификатор преподавателя, поставившего оценку </summary>
        public Guid TeacherId { get; private set; }

        #endregion

        #region Конструкторы

        public Grade(Teacher teacher, Student student, Lesson lesson, DateTime gradeTime, Mark mark)
    : this(Guid.NewGuid(), teacher, student, lesson, gradeTime, mark)
        {
        }

        protected Grade(Guid id, Teacher teacher, Student student, Lesson lesson, DateTime gradeTime, Mark mark)
            : base(id)
        {
            ValidateGrade(teacher, student, lesson, gradeTime, mark);

            Teacher = teacher;
            TeacherId = teacher.Id;

            Student = student;
            StudentId = student.Id;

            Lesson = lesson;
            LessonId = lesson.Id;

            GradedTime = gradeTime;
            Mark = mark;
        }

        /// <summary>
        /// Конструктор для EF
        /// </summary>
        protected Grade() : base(Guid.NewGuid()) { }
        #endregion

        #region Валидация


        /// <summary> Корректность создаваемой оценки </summary>
        private static void ValidateGrade(Teacher teacher, Student student, Lesson lesson,
                                          DateTime gradeTime, Mark mark)
        {
            if (teacher == null) throw new TeacherIsNullException();
            if (student == null) throw new StudentIsNullException();
            if (lesson == null) throw new LessonIsNullException();

            if (lesson.State != LessonStatus.Teached)
                throw new LessonNotStartedException(lesson);

            if (lesson.Teacher != teacher)
                throw new AnotherTeacherLessonGradedException(lesson, teacher);

            if (gradeTime.ToUniversalTime() < lesson.ClassTime.ToUniversalTime())
                throw new LessonNotStartedException(lesson);

            if (!student.AttendedLessons.Any(l => l != null && l.Id == lesson.Id))
                throw new LessonNotVisitedException(lesson, student);

        }

        #endregion
    }
}
