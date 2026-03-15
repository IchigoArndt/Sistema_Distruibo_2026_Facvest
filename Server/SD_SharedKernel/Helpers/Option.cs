using System;
using System.Collections.Generic;
using System.Text;

namespace SD_SharedKernel.Helpers
{
    public struct Option<T>
    {
        public static readonly Option<T> None;

        public T Value { get; internal set; }

        public bool IsSome { get; }

        public bool IsNone => !IsSome;

        internal Option(T value, bool isSome)
        {
            Value = value;
            IsSome = isSome;
        }

        public TR Match<TR>(Func<T, TR> some, Func<TR> none)
        {
            if (!IsSome)
            {
                return none();
            }

            return some(Value);
        }

        public static implicit operator Option<T>(T value)
        {
            return Helper.Some(value);
        }

        public static implicit operator Option<T>(NoneType _)
        {
            return None;
        }

        public static Option<T> Of<T>(T value)
        {
            return new Option<T>(value, value != null);
        }
    }
}
