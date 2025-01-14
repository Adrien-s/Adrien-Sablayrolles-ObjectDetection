using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Adrien.Sablayrolles.ObjectDetection;

class Program
{
    static async Task Main(string[] args)
    {
        // Vérifier qu'un argument (chemin des images) a été fourni
        if (args.Length < 1)
        {
            Console.WriteLine("Veuillez fournir le chemin complet du répertoire contenant les images de scènes.");
            return;
        }

        string scenesDirectory = args[0];

        // Vérifier si le répertoire existe
        if (!Directory.Exists(scenesDirectory))
        {
            Console.WriteLine($"Le répertoire spécifié n'existe pas : {scenesDirectory}");
            return;
        }

        // Charger les images du répertoire
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.GetFiles(scenesDirectory))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        // Utiliser la librairie de détection d'objets
        var objectDetection = new Adrien.Sablayrolles.ObjectDetection.ObjectDetection();
        var detectionResults = await objectDetection.DetectObjectInScenesAsync(imageScenesData);

        // Afficher les résultats au format JSON
        foreach (var result in detectionResults)
        {
            Console.WriteLine($"Box: {JsonSerializer.Serialize(result.Box)}");
        }
    }
}

