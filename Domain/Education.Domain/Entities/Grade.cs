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

        /// <summary> Преподаватель, поставивший оценку </summary>
        public Teacher Teacher { get; private set; }

        /// <summary> Студент, получивший оценку </summary>
        public Student Student { get; private set; }

        /// <summary> Урок, за который поставлена оценка </summary>

        public Lesson Lesson { get; private set; }

        /// <summary> Время выставления оценки </summary>
        public DateTime GradedTime { get; private set; }

        /// <summary> Значение оценки </summary>
        public Mark Mark { get; private set; }

        /// <summary> Внешний ключ для студента </summary>
        public Guid StudentId { get; private set; }

        /// <summary> Внешний ключ для урока </summary>
        public Guid LessonId { get; private set; }

        public Guid TeacherId { get; private set; } // добавлено

        #endregion

        #region Конструкторы

        public Grade(Teacher teacher, Student student, Lesson lesson, DateTime gradeTime, Mark mark)
    : this(Guid.NewGuid(), teacher, student, lesson, gradeTime, mark)
        {
        }

        /// <summary>
        /// Конструктор для восстановления из БД
        /// </summary>
        protected Grade(Guid id, Teacher teacher, Student student, Lesson lesson, DateTime gradeTime, Mark mark)
            : base(id)
        {
            ValidateGrade(teacher, student, lesson, gradeTime, mark);

            Teacher = teacher;
            TeacherId = teacher.Id;

            Student = student;
            StudentId = student.Id;

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

            if (!student.AttendedLessons.Contains(lesson))
                throw new LessonNotVisitedException(lesson, student);
        }

        #endregion
    }
}
