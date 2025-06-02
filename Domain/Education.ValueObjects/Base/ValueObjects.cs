using Education.ValueObjects.Exceptions;

namespace Education.Domain.ValueObjects.Base
{
    public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        /// <summary> Значение объекта-значения (value object) </summary>
        public T Value { get; }

        protected ValueObject(IValidator<T> validator, T value)
        {
            if (validator == null)
                throw new NullException(GetType().FullName ?? String.Empty,"Validator must be specified");
            validator.Validate(value);
            Value = value;
        }
        /// <summary> Преобразует значение в строку </summary>
        public override string ToString()
            => Value!.ToString() ?? GetType().ToString();

        /// <summary> Получает хеш-код текущего значения </summary>
        public override int GetHashCode()
            => Value!.GetHashCode();
        /// <summary> Проверяет равенство текущего объекта с другим объектом </summary>
        public override bool Equals(object? other)
            => Equals(other as ValueObject<T>);
        /// <summary> Проверяет равенство двух объектов-значений по значению и типу </summary>
        public bool Equals(ValueObject<T>? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            return other.Value!.Equals(Value);
        }
        /// <summary> Оператор сравнения на равенство для объектов-значений </summary>
        public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
            => Equals(left, right);
        /// <summary> Оператор сравнения на неравенство для объектов-значений </summary>
        public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
            => !(left == right);
    }
}
