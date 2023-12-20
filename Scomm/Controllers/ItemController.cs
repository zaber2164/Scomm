﻿using DAL;
using DAL.EntityModel;
using DAL.UnitOfWork;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Configuration;

namespace Scomm.Controllers
{
    public class ItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;
        public ItemController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadItemExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadItemExcel(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //Upload the excel file
            if (file != null && file.Length > 0)
            {
                var uploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }
                var filePath = Path.Combine(uploadDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //Read file
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    var excelData = new List<List<object>>();
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                //** code for showing in the grid **//
                                //// reader.GetDouble(0);
                                //var rowData = new List<object>();
                                //for (int column = 0; column < reader.FieldCount; column++)
                                //{
                                //    rowData.Add(reader.GetValue(column));
                                //    // insert into DB
                                //    var dbItem = new Item
                                //    {
                                //        ItemName = reader.GetValue(column).ToString()
                                //    };
                                //    _unitOfWork.Items.Add(dbItem);
                                //    _unitOfWork.Complete();
                                //}
                                //excelData.Add(rowData);

                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                // Build the item object
                                Item item = new Item();
                                item.ItemName = reader.GetValue(1).ToString();
                                item.ItemUnit = reader.GetValue(2).ToString();
                                item.ItemQty = Convert.ToInt32(reader.GetValue(3).ToString());
                                item.CategoryID = Convert.ToInt32(reader.GetValue(4).ToString());

                                // save to DB using entity framework
                                _unitOfWork.Items.Add(item);
                                _unitOfWork.Complete();
                            }
                        } while (reader.NextResult());
                        ViewBag.excelData = excelData;
                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
    }
}
