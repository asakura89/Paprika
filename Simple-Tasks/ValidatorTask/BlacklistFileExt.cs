namespace ValidatorTask;

public sealed class BlacklistFileExt {
    public String Ext { get; }
    public String Message { get; }

    public BlacklistFileExt(String ext, String message) {
        if (String.IsNullOrEmpty(ext))
            throw new ArgumentNullException(nameof(ext));

        if (String.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        Ext = ext;
        Message = message;
    }
}
