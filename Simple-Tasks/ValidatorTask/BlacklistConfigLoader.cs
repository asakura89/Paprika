using System.Xml;
using Eksmaru;
using AppSea;

namespace ValidatorTask;

public class BlacklistConfigLoader {
    readonly String configPath;

    //public BlacklistConfigLoader() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\blacklist.config.xml") { }

    public BlacklistConfigLoader() : this($"{AppDomain.CurrentDomain.BaseDirectory}\\paprika.config.xml") { }

    public BlacklistConfigLoader(String configPath) {
        if (String.IsNullOrEmpty(configPath))
            throw new ArgumentNullException(nameof(configPath));

        this.configPath = configPath;
    }

    BlacklistContains MapConfigToContains(XmlNode config) {
        String wordValue = config.GetAttributeValue("value");
        String messageValue = config.GetAttributeValue("message");

        if (String.IsNullOrEmpty(wordValue) || String.IsNullOrEmpty(messageValue))
            throw new BadConfigurationException("Blacklist's contains");

        return new BlacklistContains(wordValue, messageValue);
    }

    BlacklistFileExt MapConfigToFileExt(XmlNode config) {
        String extVale = config.GetAttributeValue("value");
        String messageValue = config.GetAttributeValue("message");

        if (String.IsNullOrEmpty(extVale) || String.IsNullOrEmpty(messageValue))
            throw new BadConfigurationException("Blacklist's file.ext");

        return new BlacklistFileExt(extVale, messageValue);
    }

    public BlacklistConfiguration Load() {
        XmlDocument config = XmlExt.LoadFromPath(configPath);
        String blacklistSelector = "configuration/blacklists";
        XmlNode blacklistNode = config.SelectSingleNode(blacklistSelector);
        if (blacklistNode == null)
            throw new BadConfigurationException($"{blacklistSelector}");

        IList<BlacklistContains> contains = blacklistNode
            .SelectNodes("contains")
            .Cast<XmlNode>()
            .Select(MapConfigToContains)
            .ToList();

        IList<BlacklistFileExt> fileExts = blacklistNode
            .SelectNodes("fileExt")
            .Cast<XmlNode>()
            .Select(MapConfigToFileExt)
            .ToList();

        return new BlacklistConfiguration(contains, fileExts);
    }
}
