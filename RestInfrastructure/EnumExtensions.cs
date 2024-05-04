namespace APITesting.RestInfrastructure
{
    public static class EnumExtensions
    {
        public static string StringValue(this Enum value)
        {
            return GetStringValue(value);
        }
        
        private static string GetStringValue(Enum value)
        {
            //Current value
            string output = null;
            //Get enum type
            var type = value.GetType();
            //Use reflection and get field info.
            var field = type.GetField(value.ToString());

            //Add values of the attributes

            //check...
            if (field.GetCustomAttributes(typeof(StringValueAttribute), false) is StringValueAttribute[] attrs && attrs.Length > 0)
            {
                //return value of the attribute.
                output = (attrs[0]).Value;
            }

            return output;
        }
        
        public static T GetEnumValueByStringValue<T>(this string value) where T : struct
        {
            try
            {
                return
                    Enum.GetValues(typeof(T))
                        .Cast<T>()
                        .First(enumMember => (enumMember as Enum).StringValue() == value);
            }

            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"{typeof(T).Name} enum does not contain '{value}' string value");
            }
        }

        public static T GetEnumValueByName<T>(this string name) where T : struct
        {
            var isParsed = Enum.TryParse<T>(name, out T result);

            if (isParsed)
            {
                return result;
            }

            throw new InvalidOperationException($"{typeof(T).Name} enum does not contain {name} element");
        }
    }   
}