using System;

namespace osu_mapping.util
{
    public class Optional<T>
    {
        private readonly T _t;

        private Optional() => _t = default(T);

        private Optional(T t) => _t = t;

        public bool IsPresent() => _t != null;

        public OptionalElse IfPresent(Action<T> action)
        {
            if (IsPresent())
                action.Invoke(_t);
            
            return new OptionalElse(IsPresent());
        }

        public void IfNotPresent(Action action)
        {
            if (!IsPresent())
                action.Invoke();
        }

        public T OrElse(T other) => IsPresent() ? _t : other;

        public T Get() => _t;

        public static Optional<T> Empty() => new Optional<T>();

        public static Optional<T> Of(T t) =>
            t == null ? throw new NullReferenceException("Optional was null!") : new Optional<T>(t);

        public static Optional<T> OfNullable(T t) => t == null ? Empty() : new Optional<T>(t);

        public class OptionalElse
        {
            private readonly bool _isPresent;
            
            public OptionalElse(bool present) => _isPresent = present;
            
            public void Otherwise(Action action)
            {
                if (!_isPresent)
                    action.Invoke();
            }
        }
    }
}