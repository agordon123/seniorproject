using Microsoft.AspNetCore.Http;
using PanOpticon.UserRoles;
using System;

namespace PanOpticon.Models
{
    public class FileUpload
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string FileName { get; set; }
        public Guid Guid { get; set; }
        public PanopticonUser PanopticonUser { get; set; } 
        public KanboardNote KanboardNote { get; set; }
        
    }

    public class FileUploadVM
    {
        public string Name { get; set; }
        public FileUpload FileUpload { get; set; }
        public IFormFile File { get; set; }
    }

    public class FileVM
    {
        public String Name { get; set; }
        public String Path { get; set; }
    }
}
