
using ExcelReader.Domain.Models;
using ExcelReader.Service.Repository;
using ExcelReader.Service.Services;
using ExcelReader.Utility.Helper;
using FastExcel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace ExcelReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _environment;
        private readonly IEmployeeService _employeeService;
        private readonly IOrganisationService _organisationService;
        private readonly IUploadExcelService _uploadExcelService;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, IEmployeeService employeeService, IOrganisationService organisationService, IUploadExcelService uploadExcelService)
        {
            _logger = logger;
            _environment = environment;
            _employeeService = employeeService;
            _organisationService = organisationService;
            _uploadExcelService = uploadExcelService;
        
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       

        [AllowAnonymous]
        [HttpPost]
        [Route("file/upload")]
        public async Task<IActionResult> UploadEntries()
        {
            if (!HttpContext.Request.Form.Files.Any()) return Json(new ResponseMessage
            {
                hasError = true,
                message = "You did not upload a valid file"
            });

            try
            {
                var file = HttpContext.Request.Form.Files[0];

                var allowedFiles = new string[] { ".xlsx", ".xls" };
                var fileExt = Path.GetExtension(file.FileName);

                if (!allowedFiles.Contains(fileExt)) return Json(new ResponseMessage
                {
                    hasError = true,
                    message = "You did not upload a valid file"
                });
                var inputFile = new FileInfo(FileUploadHelper.UploadFile(file, _environment.WebRootPath));

                var fileUpload = await _uploadExcelService.ReadExcelFile(inputFile);
                if (fileUpload)
                {
                    return Json(new ResponseMessage
                    {
                        hasError = false,
                        message = "File upladed successfully"
                    });
                }
                else
                {
                    return Json(new ResponseMessage
                    {
                        hasError = true,
                        message = "File upload failed"
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }




        }



       

        [HttpPost]
        [Route("employees")]
        public IActionResult ListAllEmployee()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var query = _employeeService.GetAllEmployees();

            recordsTotal = query.Count();

            var recordList = query.OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(pageSize);

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = recordList
            });
        }

        [HttpPost]
        [Route("org")]
        public IActionResult ListAllOrg()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var query = _organisationService.GetAllOrganisations();

            recordsTotal = query.Count();

            var recordList = query.OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(pageSize);

            return Json(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = recordList
            });
        }


    }
}