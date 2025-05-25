using Education.Domain.Exceptions;
using Education.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Entities.Base
{
    /// <summary>
    /// Базовая сущность, представляющая человека.
    /// </summary>
    /// <param name="id">Уникальный идентификатор.</param>
    /// <param name="name">Имя человека.</param>
    public class Person(Guid id, PersonName name) : Entity<Guid>(id)
    {
        // Имя человека
        public PersonName Name { get; private set; } = name;

        /// <summary>
        /// Изменить имя человека.
        /// </summary>
        /// <param name="newName">Новое имя.</param>
        public void ChangeName(PersonName newName)
        {
            if (newName == null)
                throw new PersonNameNullsException(nameof(newName));

            if (newName == Name)
                return; // имя не изменилось

            Name = newName;
        }
    }
}

