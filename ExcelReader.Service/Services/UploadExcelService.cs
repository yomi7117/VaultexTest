using ExcelReader.Domain.Data;
using ExcelReader.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReader.Service.Services
{
    public class UploadExcelService : IUploadExcelService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public UploadExcelService(AppDbContext context, ILogger<UploadExcelService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> ReadExcelFile(FileInfo fileStream)
        {
            var result = false;

            try
            {
                var sheetName = string.Empty;
                using (FastExcel.FastExcel fastExcel = new FastExcel.FastExcel(fileStream, true))
                {
                    foreach (var worksheet in fastExcel.Worksheets)
                    {

                        worksheet.Read();
                        sheetName = worksheet.Name;
                        var rows = worksheet.Rows.ToArray();
                        //Do something with rows


                        if (sheetName.ToLower() == "employee")
                        {
                            var empDt = new DataTable();
                            var empRowNum = 1;
                            foreach (var row in rows)
                            {
                                if (empRowNum == 1)
                                {
                                    foreach (var cell in row.Cells)
                                    {
                                        var columnName = cell.Value.ToString();
                                        empDt.Columns.Add(columnName);
                                    }
                                    empRowNum++;
                                }
                                else
                                {

                                    int l = 0;
                                    DataRow dr = empDt.NewRow();
                                    foreach (var cell in row.Cells)
                                    {
                                        dr[l++] = cell.Value;
                                        //dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    }
                                    empDt.Rows.Add(dr);
                                }

                            }

                            var employees = new List<Employee>();
                            foreach (DataRow row in empDt.Rows)
                            {
                                employees.Add(new Employee { OrgNumber = row[0].ToString(), FirstName = row[1].ToString(), LastName = row[2].ToString() });
                            }
                            await _context.Employees.AddRangeAsync(employees);

                        }
                        else if (sheetName.ToLower() == "organisation")
                        {
                            var orgDt = new DataTable();
                            var orgRowNum = 1;
                            foreach (var row in rows)
                            {
                                if (orgRowNum == 1)
                                {
                                    foreach (var cell in row.Cells)
                                    {
                                        var columnName = cell.Value.ToString();
                                        orgDt.Columns.Add(columnName);
                                    }
                                    orgRowNum++;
                                }
                                else
                                {

                                    int m = 0;
                                    DataRow dr = orgDt.NewRow();
                                    foreach (var cell in row.Cells)
                                    {
                                        dr[m++] = cell.Value;
                                        //dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    }
                                    orgDt.Rows.Add(dr);
                                }

                            }

                            var organisations = new List<Organisation>();
                            foreach (DataRow row in orgDt.Rows)
                            {
                                organisations.Add(new Organisation
                                {
                                    Name = row[0].ToString(),
                                    OrgNumber = row[1].ToString(),
                                    Address1 = row[2].ToString(),
                                    Address2 = row[3].ToString(),
                                    Address3 = row[4].ToString(),
                                    Address4 = row[5].ToString(),
                                    Town =  row[6].ToString(),
                                    PostCode = row[7].ToString(),
                                    Unknown = row[8].ToString(),
                                });

                            }
                            await _context.Organisations.AddRangeAsync(organisations);

                        }

                    }
                    await _context.SaveChangesAsync();
                }
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return result;
        }
    }
}
