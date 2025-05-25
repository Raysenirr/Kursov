using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.ValueObjects.Exceptions;

namespace Education.Domain.ValueObjects.Base
{
    public abstract class ValueObject<T> : IEquatable<ValueObject<T>>
    {
        public T Value { get; }

        protected ValueObject(IValidator<T> validator, T value)
        {
            if (validator == null)
                throw new NullException(GetType().FullName ?? String.Empty,"Validator must be specified");
            validator.Validate(value);
            Value = value;
        }

        public override string ToString()
            => Value!.ToString() ?? GetType().ToString();

        public override int GetHashCode()
            => Value!.GetHashCode();

        public override bool Equals(object? other)
            => Equals(other as ValueObject<T>);

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

        public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
            => Equals(left, right);

        public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
            => !(left == right);
    }
}
