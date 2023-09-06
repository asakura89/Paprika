namespace ValidatorTask;

public sealed class ZipReaderConfiguration {
    public IList<ZipReaderPath> Paths { get; }

    public ZipReaderConfiguration(IList<ZipReaderPath> paths) {
        Paths = paths ?? throw new ArgumentNullException(nameof(paths));
    }
}
