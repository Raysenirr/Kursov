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
        #region Свойства
        /// <summary> Группа, которой проводится урок </summary>
        public Group Group { get; }

        /// <summary> Преподаватель, ведущий урок </summary>
        public Teacher Teacher { get; }

        /// <summary> Дата и время проведения урока </summary>
        public DateTime ClassTime { get; private set; }

        /// <summary> Тема урока </summary>
        public LessonTopic Topic { get; }

        /// <summary> Текущее состояние урока </summary>
        public LessonStatus State { get; private set; }

        /// <summary> Идентификатор преподавателя </summary>
        public Guid TeacherId { get; private set; }

        /// <summary> Внутренний список оценок, выставленных за этот урок </summary>
        private readonly ICollection<Grade> _grades = [];

        /// <summary> Внутренний список домашних заданий, выданных на уроке </summary>
        private readonly ICollection<Homework> _homeworks = [];

        /// <summary> Оценки за урок (только для чтения, не отображаются в БД) </summary>
        [NotMapped]
        public IReadOnlyCollection<Grade> AssignedGrades =>[.. _grades];

        /// <summary> Домашние задания, выданные на уроке (только для чтения, не отображаются в БД) </summary>
        [NotMapped]
        public IReadOnlyCollection<Homework> AssignedHomeworks => [.. _homeworks];

        /// <summary> Домашние задания урока (только для чтения) </summary>
        public IReadOnlyCollection<Homework> Homeworks => [.. _homeworks];
        #endregion

        #region Конструкторы
        protected Lesson(Guid id, Group group, Teacher teacher, LessonTopic topic,
                 DateTime classTime, LessonStatus status) : base(id)
        {
            ValidateScheduleTime(classTime);
            Group = group ?? throw new GroupIsNullException();
            Teacher = teacher ?? throw new TeacherIsNullException();
            TeacherId = teacher.Id;
            Topic = topic ?? throw new LessonTopicIsNullsException();
            ClassTime = classTime;
            State = status;
            if (status == LessonStatus.New)
                teacher.ScheduleLesson(this);
            _grades = new List<Grade>();
            _homeworks = new List<Homework>();
        }

        public Lesson(Group group, Teacher teacher, DateTime classTime, LessonTopic topic)
            : this(Guid.NewGuid(), group, teacher, topic, classTime, LessonStatus.New) { }

        protected Lesson() : base(Guid.NewGuid())
        {
            _grades = new List<Grade>();
            _homeworks = new List<Homework>();
        }
        #endregion

        #region Методы
        /// <summary> Переводит урок в состояние "Проведён", если это допустимо </summary>
        public void Teach()
        {
            ValidateBeforeStateChange();
            State = LessonStatus.Teached;
        }
        /// <summary> Переносит урок на новую дату, если дата допустима </summary>
        public void Cancel()
        {
            ValidateBeforeStateChange();
            State = LessonStatus.Canselled;
        }

        /// <summary> Добавляет домашнее задание к уроку, если оно принадлежит этому уроку </summary>
        public void Reschedule(DateTime time)
        {
            ValidateBeforeStateChange();
            ValidateLessonSchedule(time);
            ClassTime = time;
        }
        /// <summary> Возвращает домашнее задание по его идентификатору </summary>
        public void AddHomework(Homework homework)
        {
            if (homework == null)
                throw new HomeworkIsNullException();

            if (homework.Lesson != this)
                throw new HomeworkLessonMismatchException(this, homework);

            _homeworks.Add(homework);
        }
        /// <summary> Возвращает домашнее задание по его идентификатору </summary>
        public Homework? GetHomeworkById(Guid id)
        {
            return _homeworks.FirstOrDefault(h => h.Id == id);
        }
        /// <summary> Возвращает все сдачи домашних заданий по всем заданиям урока </summary
        public IReadOnlyCollection<HomeworkSubmission> GetAllSubmissions()
        {
            return new ReadOnlyCollection<HomeworkSubmission>(
                _homeworks.SelectMany(h => h.Submissions).ToList());
        }
        /// <summary> Проверяет корректность запланированного времени урока </summary>
        private static void ValidateLessonSchedule(DateTime classTime)
        {
            if (classTime.ToUniversalTime() < DateTime.UtcNow.Date)
                throw new InvalidLessonScheduleTimeException(classTime);
        }
        /// <summary> Проверяет, можно ли изменить состояние урока </summary>
        private void ValidateBeforeStateChange()
        {
            if (State == LessonStatus.Canselled)
                throw new LessonCanselledException(this);

            if (State == LessonStatus.Teached)
                throw new LessonAlreadyTeachedException(this);
        }
        /// <summary> Проверка времени урока при создании (должно быть не в прошлом) </summary>
        public static void ValidateScheduleTime(DateTime classTime)
        {
            if (classTime.ToUniversalTime() < DateTime.Today.ToUniversalTime())
                throw new InvalidLessonScheduleTimeException(classTime);
        }
        /// <summary> Проверяет, можно ли провести урок </summary>
        public static void ValidateBeforeTeaching(Lesson lesson)
        {
            if (lesson.State == LessonStatus.Canselled)
                throw new LessonCanselledException(lesson);
            if (lesson.State == LessonStatus.Teached)
                throw new LessonAlreadyTeachedException(lesson);
        }
        /// <summary> Возвращает все домашние задания урока </summary>
        public IEnumerable<Homework> GetHomeworks() => _homeworks;

        #endregion
    }
}

