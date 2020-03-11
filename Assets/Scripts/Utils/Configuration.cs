using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.IO;

/**
 * Helper class with methods to serialize and deserialize json and xml files
 */
public static class Configuration
{
    static public T DeserializeFromFile<T> (string configPath)
    {
        if (BetterStreamingAssets.FileExists(configPath))
        {
            string text = BetterStreamingAssets.ReadAllText(configPath);
            // extension without the dot
            return Deserialize<T>(text, Path.GetExtension(configPath).Replace(".", ""));
        }
        else
        {
            throw new FileNotFoundException($"configuration file {configPath} not found");
        }
    }

    static public T Deserialize<T> (string content, string format)
    {
        switch (format)
        {
            case "json":
                try
                {
                    return JsonUtility.FromJson<T>(content);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    throw new FileLoadException();
                }
            case "xml":
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    using (TextReader reader = new StringReader(content))
                    {
                        return (T)serializer.Deserialize(reader);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    throw new FileLoadException();
                }
            default:
                throw new ArgumentException($"the format {format} is not supported");
        }
    }

    static public string Serialize<T> (T config, string format)
    {
        switch (format)
        {
            case "json":
                return JsonUtility.ToJson(config, true);
            case "xml":
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();

                // pretty printing
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlserializer.Serialize(writer, config);
                    return stringWriter.ToString();
                }
            default:
                throw new ArgumentException($"the format {format} is not supported");
        }
    }

    static public void SerializeToFile<T> (T config, string configPath)
    {
        File.WriteAllText(configPath, Serialize(config, Path.GetExtension(configPath).Replace(".", "")));
        Debug.Log($"saved the config to {configPath}");
    }
}
