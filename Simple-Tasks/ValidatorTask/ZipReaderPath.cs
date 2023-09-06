namespace ValidatorTask;

public sealed class ZipReaderPath {
    public String Path { get; }

    public ZipReaderPath(String path) {
        if (String.IsNullOrEmpty(path))
            throw new ArgumentNullException(nameof(path));

        Path = path;
    }
}
