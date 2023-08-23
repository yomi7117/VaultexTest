using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Service.Services
{
    public interface IUploadExcelService
    {
        Task<bool> ReadExcelFile(FileInfo fileStream);
    }
}
