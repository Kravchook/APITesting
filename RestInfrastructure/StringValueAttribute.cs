using System;

namespace Task10.RestInfrastructure
{
    public class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; private set; }
    }   
}