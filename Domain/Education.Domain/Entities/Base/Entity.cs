using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Domain.Entities.Base
{
    /// <summary>
    /// Представляет сущность в системе.
    /// </summary>
    /// <typeparam name="TId">Тип идентификатора сущности.</typeparam>
    /// <param name="id">Обязательный идентификатор сущности.</param>
    public abstract class Entity<TId>(TId id) where TId : struct
    {
        /// <summary>
        /// Идентификатор сущности.
        /// </summary>
        public TId Id { get; } = id;

        /// <summary>
        /// Защищённый конструктор без параметров — используется Entity Framework.
        /// </summary>
        protected Entity() : this(default!)
        {
        }
    }
}
