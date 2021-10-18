using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace FakeData
{
    public abstract record FakeEntityBase<T> where T : class, new()
    {
        protected FakeEntityBase()
        {
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(ToBytes());
        }

        public byte[] ToBytes() => Encoding.UTF8.GetBytes(ToJson());

        public abstract int GetConsistentHashCode();

        public ConsoleColor ToColor()
        {
            var color = Math.Abs(GetConsistentHashCode());
            return (ConsoleColor)((color % 14) + 1);
        }

        public string ToJson(bool writeIndented = false)
        {
            return JsonSerializer.Serialize(
                value: this,
                inputType: GetType(),
                options: new JsonSerializerOptions { WriteIndented = writeIndented });
        }

        public string ToXml()
        {
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
                CheckCharacters = false
            };

            var xml = new StringBuilder();
            using var stream = new StringWriter(xml);
            using var writer = XmlWriter.Create(stream, settings);
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(writer, this, namespaces);

            return xml.ToString();
        }

        public static T FromBytes(byte[] value)
        {
            var entity = Encoding.UTF8.GetString(value);
            return JsonSerializer.Deserialize<T>(entity) ?? throw NewDeserializationException(entity);
        }

        private static SerializationException NewDeserializationException(string value) =>
            new($"Deserialization from value '{value}' to type '{typeof(T).Name}' failed.");
    }
}
