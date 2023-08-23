using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Utility.Helper
{
    public static class FileUploadHelper
    {
      
       
        public static string  UploadFile(IFormFile formFile, string uploadPath  )
        {

            string finalPath = Path.Combine(uploadPath, "uploaded_files");

            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }
            string sourcefile = Path.GetFileName(formFile.FileName);
            string path = Path.Combine(finalPath, sourcefile);

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                formFile.CopyTo(filestream);
            }
            return path;
        }
    }
}
