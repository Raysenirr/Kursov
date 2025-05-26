
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

        private readonly ICollection<Lesson> _lessons = new List<Lesson>();
        private readonly ICollection<Grade> _grades = new List<Grade>();
        private readonly HomeworkBank _homeworkBank;

        public HomeworkBank HomeworkBank => _homeworkBank;

        ///<summary>Навигационное свойство для EF — доступ к урокам</summary>
        //public ICollection<Lesson> Lessons => (ICollection<Lesson>)_lessons;

        ///<summary>Навигационное свойство для EF — доступ к оценкам</summary>
        //public ICollection<Grade> Grades => (ICollection<Grade>)_grades;

        ///<summary>Проведённые уроки</summary>
        public IReadOnlyCollection<Lesson> TeachedLessons =>
            _lessons.Where(l => l.State == LessonStatus.Teached).ToList().AsReadOnly();

        ///<summary>Запланированные уроки</summary>
        public IReadOnlyCollection<Lesson> ScheduledLessons =>
            _lessons.Where(l => l.State == LessonStatus.New).ToList().AsReadOnly();

        ///<summary>Оценки, выставленные преподавателем</summary>
        public IReadOnlyCollection<Grade> AssignedGrades =>
            _grades.ToList().AsReadOnly();


        #region Constructors

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

        #region Поведение

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

        internal void ScheduleLesson(Lesson lesson)
        {
            if (lesson.State != LessonStatus.New)
                throw new LessonAlreadyTeachedException(lesson);

            if (!_lessons.Contains(lesson))
                _lessons.Add(lesson);
        }

        public Homework AssignHomeworkFromBank(Lesson lesson)
        {
            var template = _homeworkBank.FindTemplate(lesson.Topic);

            if (template == null)
                throw new HomeworkTemplateNotFoundException(lesson.Topic, this);

            var homework = new Homework(lesson, template.Title);
            lesson.AddHomework(homework);
            return homework;
        }

        public IReadOnlyCollection<Group> GetTeachedGroups()
        {
            return _lessons.Select(l => l.Group).Distinct().ToList().AsReadOnly();
        }

        public IReadOnlyCollection<Student> GetTeachedStudents()
        {
            return _lessons.SelectMany(l => l.Group.Students).Distinct().ToList().AsReadOnly();
        }

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
