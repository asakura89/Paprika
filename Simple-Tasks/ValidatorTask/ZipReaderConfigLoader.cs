using System.Xml;
using Eksmaru;
using AppSea;

namespace ValidatorTask;

public class ZipReaderConfigLoader {
    readonly String configPath;

    public ZipReaderConfigLoader() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\paprika.config.xml") { }

    public ZipReaderConfigLoader(String configPath) {
        if (String.IsNullOrEmpty(configPath))
            throw new ArgumentNullException(nameof(configPath));

        this.configPath = configPath;
    }

    ZipReaderPath MapConfigToPaths(XmlNode config) {
        String pathValue = config.GetAttributeValue("path");

        if (String.IsNullOrEmpty(pathValue))
            throw new BadConfigurationException("ZipReader's path");

        return new ZipReaderPath(pathValue);
    }

    public ZipReaderConfiguration Load() {
        XmlDocument config = XmlExt.LoadFromPath(configPath);
        String configSelector = "configuration/zippackages";
        XmlNode configNode = config.SelectSingleNode(configSelector);
        if (configNode == null)
            throw new BadConfigurationException($"{configSelector}");

        IList<ZipReaderPath> paths = configNode
            .SelectNodes("item")
            .Cast<XmlNode>()
            .Select(MapConfigToPaths)
            .ToList();

        return new ZipReaderConfiguration(paths);
    }
}
