using System.IO;

namespace CommonWPF;

public class TxtFile : ViewModelBase
{
    public string FilePath
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    public string DisplayName
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public TxtFile(string filepath)
    {
        FilePath = filepath;
        DisplayName = Path.GetFileNameWithoutExtension(FilePath);
    }
}
