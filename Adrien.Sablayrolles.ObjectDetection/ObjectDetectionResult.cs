using ObjectDetection;

namespace Adrien.Sablayrolles.ObjectDetection;

using Adrien.Sablayrolles.ObjectDetection;
public record ObjectDetectionResult
{
    public byte[] ImageData { get; set; }
    public List<BoundingBox> Box { get; set; }
} 