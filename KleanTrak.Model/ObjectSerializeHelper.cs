using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KleanTrak.Model
{
    public class ObjectSerializeHelper
    {
        protected ObjectSerializeHelper()
        {
        }

        public static ObjectSerializeHelper ReadObjectFromXml(string input)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(input)))
                {
                    reader.MoveToContent(); // get the root-element
                    Type type = Type.GetType(typeof(ObjectSerializeHelper).Namespace + "." + reader.Name);
                    if (type == null)
                        return null;

                    XmlSerializer ser = new XmlSerializer(type);
                    return (ObjectSerializeHelper)ser.Deserialize(reader);
                }
            }
            catch (Exception)
            {
				throw;
            }
        }

        public static ObjectSerializeHelper ReadObjectFromJson(string input)
        {
            try
            {
                JsonType jtype = new JsonType();
                var otype = JsonConvert.DeserializeAnonymousType(input, jtype);

                Type type = Type.GetType(otype.MethodName);
                if (type == null)
                    return null;
                
                return (ObjectSerializeHelper)ReadJson(input, type);
            }
            catch (Exception)
            {
            }

            return null;
        }        

        public string SaveObjectToXml()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8
            };

            MemoryStream memoryStream = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);

            XmlSerializer s = new XmlSerializer(GetType());
            s.Serialize(xmlWriter, this);

            // we just output back to the console for this demo.
            memoryStream.Position = 0; // rewind the stream before reading back.
            StreamReader sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }

        public string SaveObjectToJson() => JsonConvert.SerializeObject(this);

        private static ObjectSerializeHelper ReadJson(string input, Type objectType)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(input));

            object instance = Activator.CreateInstance(objectType);
            var props = objectType.GetProperties().ToList();

            JObject jo = JObject.Load(reader);
            foreach (JProperty jp in jo.Properties())
            {
                var prop = props.FirstOrDefault(pi => pi.CanWrite && pi.Name == jp.Name);

                if (prop != null)
                    prop.SetValue(instance, jp.Value.ToObject(prop.PropertyType, new JsonSerializer()), null);
            }

            reader.Close();

            return (ObjectSerializeHelper)instance;
        }
    }
}
