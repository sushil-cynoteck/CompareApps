using System.Collections.Generic;
namespace CompareApps.Models
{
    public class ImagesListModel
    {
        public string Id { get; set; }
        public string StorageName { get; set; }
        public string Directory { get; set; }
        public List<FileEntry> Files { get; set; }
    }
    public class FileEntry
    {
        public enum FileKind
        {
            GenericFile,
            Image
        }
        public string Name { get; set; }
        public string Path { get; set; }
        public FileKind Kind { get; set; }
        public long Size { get; set; }
    }
}