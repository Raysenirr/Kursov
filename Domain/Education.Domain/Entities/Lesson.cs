using Education.Domain.Entities.Base;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Domain.Entities
{
    public class Lesson : Entity<Guid>
    {
        public Group Group { get; }
        public Teacher Teacher { get; }
        public DateTime ClassTime { get; private set; }
        public LessonTopic Topic { get; }
        public LessonStatus State { get; private set; }
        public Guid TeacherId { get; private set; }

        /// <summary>
        /// Навигация к домашним заданиям (используется EF)
        /// </summary>
        private readonly ICollection<Grade> _grades = new List<Grade>();
        private readonly ICollection<Homework> _homeworks = new List<Homework>();

        [NotMapped]
        public IReadOnlyCollection<Grade> AssignedGrades => _grades.ToList().AsReadOnly();

        [NotMapped]
        public IReadOnlyCollection<Homework> AssignedHomeworks => _homeworks.ToList().AsReadOnly();

        public IReadOnlyCollection<Homework> Homeworks => _homeworks.ToList().AsReadOnly();

        protected Lesson(Guid id, Group group, Teacher teacher, LessonTopic topic,
                 DateTime classTime, LessonStatus status) : base(id)
        {
            ValidateScheduleTime(classTime);
            Group = group ?? throw new GroupIsNullException();
            Teacher = teacher ?? throw new TeacherIsNullException();
            TeacherId = teacher.Id; // добавлено
            Topic = topic ?? throw new LessonTopicIsNullsException();
            ClassTime = classTime;
            State = status;

            if (status == LessonStatus.New)
                teacher.ScheduleLesson(this);
        }

        public Lesson(Group group, Teacher teacher, DateTime classTime, LessonTopic topic)
            : this(Guid.NewGuid(), group, teacher, topic, classTime, LessonStatus.New) { }

        protected Lesson() : base(Guid.NewGuid()) { }
        public void Teach()
        {
            ValidateBeforeStateChange();
            State = LessonStatus.Teached;
        }

        public void Cancel()
        {
            ValidateBeforeStateChange();
            State = LessonStatus.Canselled;
        }

        public void Reschedule(DateTime time)
        {
            ValidateBeforeStateChange();
            ValidateLessonSchedule(time);
            ClassTime = time;
        }

        public void AddHomework(Homework homework)
        {
            if (homework == null)
                throw new HomeworkIsNullException();

            if (homework.Lesson != this)
                throw new HomeworkLessonMismatchException(this, homework);

            _homeworks.Add(homework);
        }

        public Homework? GetHomeworkById(Guid id)
        {
            return _homeworks.FirstOrDefault(h => h.Id == id);
        }

        public IReadOnlyCollection<HomeworkSubmission> GetAllSubmissions()
        {
            return new ReadOnlyCollection<HomeworkSubmission>(
                _homeworks.SelectMany(h => h.Submissions).ToList());
        }

        private static void ValidateLessonSchedule(DateTime classTime)
        {
            if (classTime.ToUniversalTime() < DateTime.UtcNow.Date)
                throw new InvalidLessonScheduleTimeException(classTime);
        }

        private void ValidateBeforeStateChange()
        {
            if (State == LessonStatus.Canselled)
                throw new LessonCanselledException(this);

            if (State == LessonStatus.Teached)
                throw new LessonAlreadyTeachedException(this);
        }

        public static void ValidateScheduleTime(DateTime classTime)
        {
            if (classTime.ToUniversalTime() < DateTime.Today.ToUniversalTime())
                throw new InvalidLessonScheduleTimeException(classTime);
        }

        public static void ValidateBeforeTeaching(Lesson lesson)
        {
            if (lesson.State == LessonStatus.Canselled)
                throw new LessonCanselledException(lesson);
            if (lesson.State == LessonStatus.Teached)
                throw new LessonAlreadyTeachedException(lesson);
        }
        public IEnumerable<Homework> GetHomeworks() => _homeworks;

    }
}

