
namespace lsprotocol_tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LSPTests
{
    public static IEnumerable<object[]> JsonTestData()
    {
        string folderPath = Environment.GetEnvironmentVariable("JSON_FOLDER_PATH");
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
        foreach (string filePath in jsonFiles)
        {
            yield return new object[] { filePath };
        }
    }

    [Theory]
    [MemberData(nameof(JsonTestData))]
    public void ValidateLSPTypes(string filePath)
    {
        string original = File.ReadAllText(filePath);

        // Get the class name from the file name
        // format: <class-name>_<test-id>.json
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string[] nameParts = fileName.Split('_');
        string className = nameParts[0];

        Type type = Type.GetType(className) ?? throw new Exception($"Type {className} not found");
        object? deserializedObject = JsonConvert.DeserializeObject(original, type);
        string newJson = JsonConvert.SerializeObject(deserializedObject);

        JToken token1 = JToken.Parse(original);
        JToken token2 = JToken.Parse(newJson);
        Assert.True(JToken.DeepEquals(token1, token2));
    }
}