namespace APITesting.RestInfrastructure
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