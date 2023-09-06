namespace ValidatorTask;

public sealed class BlacklistConfiguration {
    public IList<BlacklistContains> Contains { get; }
    public IList<BlacklistFileExt> FileExts { get; }

    public BlacklistConfiguration(IList<BlacklistContains> contains, IList<BlacklistFileExt> fileExts) {
        Contains = contains ?? throw new ArgumentNullException(nameof(contains));
        FileExts = fileExts ?? throw new ArgumentNullException(nameof(fileExts));
    }
}
