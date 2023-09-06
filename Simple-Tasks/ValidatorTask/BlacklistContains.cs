namespace ValidatorTask;

public sealed class BlacklistContains {
    public String Word { get; }
    public String Message { get; }

    public BlacklistContains(String word, String message) {
        if (String.IsNullOrEmpty(word))
            throw new ArgumentNullException(nameof(word));

        if (String.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        Word = word;
        Message = message;
    }
}
