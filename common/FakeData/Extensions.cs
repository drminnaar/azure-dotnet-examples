using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FakeData
{
    public static class Extensions
    {
        public static string ToJson<T>(this IEnumerable<FakeEntityBase<T>> fakeEntities) where T : class, new()
        {
            return JsonSerializer.Serialize(
                fakeEntities,
                typeof(IEnumerable<T>),
                new JsonSerializerOptions { WriteIndented = true });
        }

        public static (string FileName, string FileContent) ToRandomFile<T>(this IEnumerable<FakeEntityBase<T>> fakeEntities) where T : class, new()
        {
            var fileContent = fakeEntities.ToJson();
            var fileName = $"{Path.GetRandomFileName()}.json";
            return (fileName, fileContent);
        }
    }
}
