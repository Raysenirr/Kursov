
using Education.Domain.Entities.Base;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using System.Collections.ObjectModel;


namespace Education.Domain.Entities
{
    /// <summary>
    /// Представляет преподавателя в системе.
    /// </summary>
    public class Teacher : Person
    {
        #region Свойства
        /// <summary> Внутренний список всех уроков преподавателя </summary>
        private readonly ICollection<Lesson> _lessons = [];

        /// <summary> Внутренний список всех выставленных оценок </summary>
        private readonly ICollection<Grade> _grades = [];

        /// <summary> Банк шаблонов домашних заданий, доступный преподавателю </summary>
        private readonly HomeworkBank _homeworkBank;

        /// <summary> Доступ к банку домашних заданий (только для чтения) </summary>
        public HomeworkBank HomeworkBank => _homeworkBank;

        /// <summary> Список уроков, которые преподаватель уже провёл </summary>
        public IReadOnlyCollection<Lesson> TeachedLessons => [.. _lessons.Where(l => l.State == LessonStatus.Teached)];

        /// <summary> Список запланированных уроков (ещё не проведены) </summary>
        public IReadOnlyCollection<Lesson> ScheduledLessons => [.. _lessons.Where(l => l.State == LessonStatus.New)];

        /// <summary> Все оценки, выставленные преподавателем </summary>
        public IReadOnlyCollection<Grade> AssignedGrades =>[.. _grades];
        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор для восстановления из БД (Entity Framework).
        /// </summary>
        /// <summary>
        /// Полный защищённый конструктор (используется в ручной инициализации, в т.ч. публичной версии ниже).
        /// </summary>
        protected Teacher(Guid id, PersonName name, ICollection<Lesson> lessons, ICollection<Grade> grades, HomeworkBank homeworkBank)
            : base(id, name)
        {
            _lessons = lessons ?? new Collection<Lesson>();
            _grades = grades ?? new Collection<Grade>();
            _homeworkBank = homeworkBank ?? new HomeworkBank();
        }

        /// <summary>
        /// Публичный ручной конструктор.
        /// </summary>
        public Teacher(PersonName name)
            : this(Guid.NewGuid(), name, new List<Lesson>(), new List<Grade>(), new HomeworkBank())
        {
        }

        /// <summary>
        /// EF-конструктор — должен корректно инициализировать навигации и value-объекты.
        /// </summary>
        protected Teacher(Guid id, PersonName name)
            : base(id, name)
        {
            _lessons = new Collection<Lesson>();
            _grades = new Collection<Grade>();
            _homeworkBank = new HomeworkBank();
        }


        #endregion

        #region Методы
        /// <summary> Помечает урок как проведённый и добавляет его в список, если это новый урок </summary>
        public void TeachLesson(Lesson lesson)
        {
            if (lesson.State == LessonStatus.Teached)
                throw new LessonAlreadyTeachedException(lesson);

            if (TeachedLessons.Contains(lesson))
                throw new DoubleTeachedLessonException(lesson, this);

            lesson.Teach();

            if (!_lessons.Contains(lesson))
                _lessons.Add(lesson);
        }
        /// <summary> Добавляет запланированный урок в список преподавателя </summary>
        public void GradeStudent(Student student, Mark mark, Lesson lesson, Homework homework)
        {
            if (lesson.State != LessonStatus.Teached)
                throw new LessonNotStartedException(lesson);

            if (!_lessons.Contains(lesson))
                throw new AnotherTeacherLessonGradedException(lesson, this);

            if (_grades.Any(g => g.Student == student && g.Lesson == lesson))
                throw new DoubleGradeStudentLesson(lesson, student);

            if (homework.IsLate(student) && mark > Mark.Satisfactorily)
                mark = Mark.Satisfactorily;

            var grade = new Grade(this, student, lesson, DateTime.UtcNow, mark);
            student.GetGrade(grade);
            _grades.Add(grade);
        }
        /// <summary> Назначает домашнюю работу на урок из шаблона банка заданий по теме урока </summary>
        internal void ScheduleLesson(Lesson lesson)
        {
            if (lesson.State != LessonStatus.New)
                throw new LessonAlreadyTeachedException(lesson);

            if (!_lessons.Contains(lesson))
                _lessons.Add(lesson);
        }
        /// <summary> Назначает домашнюю работу на урок из шаблона банка заданий по теме урока </summary>
        public Homework AssignHomeworkFromBank(Lesson lesson)
        {
            var template = _homeworkBank.FindTemplate(lesson.Topic);

            if (template == null)
                throw new HomeworkTemplateNotFoundException(lesson.Topic, this);

            var homework = new Homework(lesson, template.Title);
            lesson.AddHomework(homework);
            return homework;
        }
        /// <summary> Возвращает список всех групп, которым преподаватель вёл уроки </summary>
        public IReadOnlyCollection<Group> GetTeachedGroups()
        {
            return _lessons.Select(l => l.Group).Distinct().ToList().AsReadOnly();
        }
        /// <summary> Возвращает список всех студентов, которых преподаватель обучал </summary>
        public IReadOnlyCollection<Student> GetTeachedStudents()
        {
            return _lessons.SelectMany(l => l.Group.Students).Distinct().ToList().AsReadOnly();
        }
        /// <summary> Возвращает все сдачи домашних заданий по проведённым урокам </summary>
        public IReadOnlyCollection<SubmittedHomeworkInfo> GetSubmittedHomeworks()
        {
            var result = new List<SubmittedHomeworkInfo>();

            foreach (var lesson in TeachedLessons)
            {
                foreach (var homework in lesson.Homeworks)
                {
                    foreach (var submission in homework.Submissions)
                    {
                        result.Add(new SubmittedHomeworkInfo(homework, submission.Student, submission.SubmissionDate));
                    }
                }
            }

            return result.AsReadOnly();
        }

        #endregion
    }
}
