using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieDatabase.Constants;

namespace MovieDatabase.Model
{
    public abstract class DynamicMetadataProperty<T> : DynamicMetadataProperty
    {
        public T WrappedValue { get; set; }

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return WrappedValue;
            }
            set => WrappedValue = (T) value;
        }
    }

    public abstract class DynamicMetadataProperty
    {
        public          string Id   { get; set; }
        public abstract string Type { get; }

        public abstract object Value { get; set;  }
        public PropertyState State { get; set; }
        public bool HasDefaultValue { get; set; }
    }
}
