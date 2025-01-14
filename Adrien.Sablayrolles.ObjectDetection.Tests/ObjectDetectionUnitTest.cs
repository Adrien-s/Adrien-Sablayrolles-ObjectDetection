using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Adrien.Sablayrolles.ObjectDetection.Tests;
public class ObjectDetectionUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }
        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenesAsync(imageScenesData);

        Assert.Equal(
            "[{\"Dimensions\":{\"X\":37.332184,\"Y\":86.2762,\"Height\":313.85245,\"Width\":158.66014},\"Label\":\"person\",\"Confidence\":0.6521324},{\"Dimensions\":{\"X\":263.70755,\"Y\":208.16797,\"Height\":201.84424,\"Width\":147.65645},\"Label\":\"person\",\"Confidence\":0.5487216},{\"Dimensions\":{\"X\":165.62262,\"Y\":88.66367,\"Height\":239.7965,\"Width\":93.99473},\"Label\":\"person\",\"Confidence\":0.44741338}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Box)
        );

        Assert.Equal(
            "[{\"Dimensions\":{\"X\":179.30159,\"Y\":34.73529,\"Height\":283.06897,\"Width\":62.186066},\"Label\":\"bottle\",\"Confidence\":0.5455813},{\"Dimensions\":{\"X\":-74.60907,\"Y\":135.58258,\"Height\":267.4231,\"Width\":429.4089},\"Label\":\"diningtable\",\"Confidence\":0.3448307},{\"Dimensions\":{\"X\":186.38177,\"Y\":130.47598,\"Height\":159.47896,\"Width\":52.504757},\"Label\":\"bottle\",\"Confidence\":0.33118075}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[1].Box)
        );
    }
    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}