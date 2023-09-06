using Arvy;
using System.Text.RegularExpressions;
using Ria;

namespace ValidatorTask;

public class FilenameVerifier {
    public void VerifyMinifiedFiles(PipelineContext context) {
        var filenameContext = (List<KeyValuePair<String, String>>) context.Data["NameAndPath"];
        foreach (KeyValuePair<String, String> nameAndPath in filenameContext) {
            if (!new[] { ".js", ".css" }.Contains(Path.GetExtension(nameAndPath.Key), StringComparer.OrdinalIgnoreCase))
                continue;

            Boolean notMinJs =
                Path.GetExtension(nameAndPath.Key).Equals(".js", StringComparison.OrdinalIgnoreCase) &&
                !(
                    nameAndPath.Key.EndsWith(".min.js", StringComparison.OrdinalIgnoreCase) ||
                    nameAndPath.Key.EndsWith(".bundle.js", StringComparison.OrdinalIgnoreCase)
                );
            if (notMinJs) {
                context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Warning, $"[{nameAndPath.Value}], Not a minified file."));

                continue;
            }

            Boolean notMinCss =
                Path.GetExtension(nameAndPath.Key).Equals(".css", StringComparison.OrdinalIgnoreCase) &&
                !(
                    nameAndPath.Key.EndsWith(".min.css", StringComparison.OrdinalIgnoreCase) ||
                    nameAndPath.Key.EndsWith(".bundle.css", StringComparison.OrdinalIgnoreCase)
                );
            if (notMinCss)
                context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Warning, $"[{nameAndPath.Value}], Not a minified file."));
        }
    }

    public void VerifyVersionInFilename(PipelineContext context) {
        var filenameContext = (List<KeyValuePair<String, String>>) context.Data["NameAndPath"];
        foreach (KeyValuePair<String, String> nameAndPath in filenameContext) {
            Boolean containsVersion = Regex.IsMatch(nameAndPath.Key, @"(?:\d{1,}\.){2}\d{1,}(?:.+)?", RegexOptions.Compiled);
            if (containsVersion)
                context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Warning, $"[{nameAndPath.Value}], File name contains version."));
        }
    }
}