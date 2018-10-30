using System.Collections;

namespace System
{
    public static class ObjectExtensions
    {
        public static Object AsJson(this Object model)
        {
            Hashtable json = new Hashtable();

            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                var key = property.Name;
                var value = property.GetValue(model);

                json.Add(key, value == null ? string.Empty : value.ToString());
                //if (value != null)
                //{
                //    json.Add(key,value);
                //}
            }

            return json;
        }
    }
}