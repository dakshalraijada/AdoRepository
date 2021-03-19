using System;

namespace AdoRepository.Attributes
{
    public sealed class DefaultIfNullAttribute : Attribute
    {
        public DefaultIfNullAttribute()
        {
        }

        public DefaultIfNullAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object DefaultValue { get; }
    }
}