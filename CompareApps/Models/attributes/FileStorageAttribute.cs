
namespace CompareApps.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.ObjectModel;
    public class FileStorageAttribute : Attribute
    {
        public string StorageName { get; private set; }
        public FileStorageAttribute(string storageName)
        {
            StorageName = storageName;
        }
    }

}
