using Education.Domain.Entities.Base;
using Education.Domain.Exceptions;
using System.Collections.ObjectModel;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

namespace Education.Domain.Entities
{
    /// <summary>
    /// Учебная группа студентов
    /// </summary>
    public class Group : Entity<Guid>
    {
        private readonly ICollection<Student> _students = new Collection<Student>();

        /// <summary>
        /// Список студентов в группе (только для чтения)
        /// </summary>
        public IReadOnlyCollection<Student> Students => new ReadOnlyCollection<Student>(_students.ToList());

        /// <summary>
        /// Название группы
        /// </summary>
        public GroupName Name { get; private set; }

        #region Constructors

        /// <summary>
        /// Основной конструктор
        /// </summary>
        public Group(GroupName name) : this(Guid.NewGuid(), name) { }

        /// <summary>
        /// Конструктор для восстановления из БД
        /// </summary>
        protected Group(Guid id, GroupName name) : base(id)
        {
            Name = name ?? throw new GroupIsNullException();
        }

        /// <summary>
        /// Конструктор для EF 
        /// </summary>
        protected Group() : base(Guid.NewGuid()) { }

        #endregion

        /// <summary>
        /// Добавить студента в группу
        /// </summary>
        /// <param name="student">Студент для добавления</param>
        /// <exception cref="ArgumentNullException">Если студент равен null</exception>
        /// <exception cref="DoubleEnrollmentException">Если студент уже в группе</exception>
        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (_students.Any(s => s.Id == student.Id))
                throw new DoubleEnrollmentException(student, this);

            _students.Add(student);
        }

        /// <summary>
        /// Удалить студента из группы
        /// </summary>
        public bool RemoveStudent(Student student)
        {
            return _students.Remove(student);
        }
    }
}