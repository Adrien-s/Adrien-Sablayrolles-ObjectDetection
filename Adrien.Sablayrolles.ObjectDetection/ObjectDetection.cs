using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adrien.Sablayrolles.ObjectDetection;
using ObjectDetection;

namespace Adrien.Sablayrolles.ObjectDetection;
public class ObjectDetection
{
    public async Task<IList<ObjectDetectionResult>> DetectObjectInScenesAsync(IList<byte[]> imagesSceneData)
    {
        var results = new List<ObjectDetectionResult>();

        // Paralléliser le traitement des images
        var tasks = imagesSceneData.Select(imageData => Task.Run(() =>
        {
            var tinyYolo = new Yolo();
            var detectionResults = tinyYolo.Detect(imageData);

            // Transformer les résultats Yolo en ObjectDetectionResult
            return new ObjectDetectionResult
            {
                ImageData = detectionResults.ImageData,
                Box = detectionResults.Boxes.Select(detection => new BoundingBox
                {
                    Label = detection.Label,
                    Confidence = detection.Confidence,
                    Dimensions = new BoundingBoxDimensions
                    {
                        X = detection.Dimensions.X,
                        Y = detection.Dimensions.Y,
                        Height= detection.Dimensions.Height,
                        Width = detection.Dimensions.Width
                    }
                }).ToList()
            };
        }));

        // Attendre que toutes les tâches soient terminées
        var completedResults = await Task.WhenAll(tasks);
        results.AddRange(completedResults);

        return results;
    }
}