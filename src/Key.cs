using System;

namespace Laconic
{
    public class Key : IEquatable<Key>
    {
        readonly object _value;

        // TODO: overloads for allowed types
        public Key(object value) => _value = value;

        public bool Equals(Key other) => _value.Equals(other._value);

        public override bool Equals(object other) => other is Key key && this.Equals(key);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => $"Key: {_value}";

        public static bool operator ==(Key lhs, Key rhs) => lhs._value.Equals(rhs._value);
        public static bool operator !=(Key lhs, Key rhs) => !lhs._value.Equals(rhs._value);

        // Must provide implicit conversions for all primitive types allowed as keys (string, int, long, guid)
        // later: DateTime, DateTimeOffset

        public static bool operator ==(Key lhs, string rhs) => lhs._value is string && lhs._value.Equals(rhs);
        public static bool operator !=(Key lhs, string rhs) => !(lhs._value is string && lhs._value.Equals(rhs));
        public static implicit operator Key(string value) => new Key(value);

        public static bool operator ==(Key lhs, int rhs) => lhs._value is int && lhs._value.Equals(rhs);
        public static bool operator !=(Key lhs, int rhs) => !(lhs._value is int && lhs._value.Equals(rhs));
        public static implicit operator Key(int value) => new Key(value);

        public static bool operator ==(Key lhs, long rhs) => lhs._value.Equals(rhs);
        public static bool operator !=(Key lhs, long rhs) => !lhs._value.Equals(rhs);
        public static implicit operator Key(long value) => new Key(value);
    }
}