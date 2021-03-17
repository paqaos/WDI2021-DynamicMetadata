using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using MovieDatabase.Constants;
using MovieDatabase.Model;
using MovieDatabase.Model.PropertyConfiguration;

namespace MovieDatabase.DataBuilder
{
    public abstract class PropertyBuilder
    {
        public string Id { get; set; }
     
        public abstract DynamicMetadataProperty Build();
    }

    public class StringPropertyBuilder : PropertyBuilder
    {

        private string _default = null;
        /// <inheritdoc />
   
        public StringPropertyBuilder DefaultValue(string value = null)
        {
            _default = value;

            return this;
        }

        /// <inheritdoc />
        public override DynamicMetadataProperty Build()
        {
            return new StringProperty
            {
                Id = Id,
                HasDefaultValue = true,
                State = PropertyState.Active,
                WrappedValue = _default
            };
        }
    }

    public class NumberPropertyBuilder : PropertyBuilder
    {
        private int _default = 0;
        /// <inheritdoc />
        /// 
        public NumberPropertyBuilder DefaultValue(int defaultValue = 0)
        {
            _default = defaultValue;

            return this;
        }

        /// <inheritdoc />
        public override DynamicMetadataProperty Build()
        {
            return new NumericProperty
            {
                Id = Id,
                State = PropertyState.Active,
                HasDefaultValue = true,
                WrappedValue = _default
            };
        }
    }

    public class PersonPropertyBuilder : PropertyBuilder
    {
        private Person _default;

        /// <inheritdoc />
    
        public PersonPropertyBuilder DefaultValue(Person person)
        {
            _default = person;

            return this;
        }

        /// <inheritdoc />
        public override DynamicMetadataProperty Build()
        {
            return new PersonProperty()
            {
                Id = Id,
                State = PropertyState.Active,
                HasDefaultValue = true,
                WrappedValue = _default
            };
        }
    }
}
