using Arvy;
using Ria;

namespace ValidatorTask;

public class BlacklistVerifier {
    public void RejectBlacklistedFiles(PipelineContext context) {
        BlacklistConfiguration config = new BlacklistConfigLoader().Load();
        var blacklistContext = (List<KeyValuePair<String, String>>) context.Data["NameAndPath"];
        foreach (KeyValuePair<String, String> nameAndPath in blacklistContext) {
            foreach (BlacklistFileExt ext in config.FileExts)
                if (Path.GetExtension(nameAndPath.Key).Equals(ext.Ext, StringComparison.OrdinalIgnoreCase))
                    context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Warning, $"[{nameAndPath.Value}], {ext.Message}"));

            foreach (BlacklistContains contain in config.Contains)
                if (Path.ChangeExtension(nameAndPath.Value, null).ToUpperInvariant().Contains(contain.Word.ToUpperInvariant()))
                    context.ActionMessages.Add(new ActionResponseViewModel(ActionResponseViewModel.Warning, $"[{nameAndPath.Value}], {contain.Message}"));
        }
    }
}
