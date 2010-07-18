namespace SquishIt.Framework.Files
{
    public interface IPackagedFileReaderFactory
    {
        string PackagedFileName(string file);
        bool PackagedFileNameExists(string file);
    }
}