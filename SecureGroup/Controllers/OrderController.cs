using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using SecureGroup.ViewModel;
using Microsoft.Extensions.Logging;
using SecureGroup.DBContexts;
using SecureGroup.Models;
using SecureGroup.Library;
using System.Linq;
using Azure;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using Microsoft.AspNetCore.Http;
using iTextSharp.text.html.simpleparser;
using NuGet.Protocol;
using System.Net.Http;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf.draw;
using System.Data;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Metadata;
using Org.BouncyCastle.Ocsp;
using SecureGroup.ViewModel.Models;
using System.Collections;

namespace SecureGroup.Controllers
{
    [ValidateSession]
    public class OrderController : BaseController
    {

        private readonly ILogger<OrderController> _logger;
        private MsDBContext myDbContext;
        DataAccessLayer _dataAccessLayer = null;
        DataAccessLayerLinq _dataAccessLayerLinq = null;

        public OrderController(ILogger<OrderController> logger, MsDBContext context)
        {

            _logger = logger;
            myDbContext = context;
            _dataAccessLayer = new DataAccessLayer();
            _dataAccessLayerLinq = new DataAccessLayerLinq(context);
        }
        public IActionResult PurchaseOrder()
        {

            return View();
        }

        public IActionResult CreatePurchaseOrder()
        {

            PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel();
            viewModel.PurchaseOrderDetails = new List<PurchaseOrderDetailsViewModel>();
            viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("SubProduct", 0);
            viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
            //for a while we are generating rows from server side but good practice //is to genrate it from client side(JQuery/JavaScript)
            PurchaseOrderDetailsViewModel row1 = new PurchaseOrderDetailsViewModel();

            viewModel.PurchaseDate = DateTime.Now;
            viewModel.PurchaseOrderDetails.Add(row1);
            return View(viewModel);

        }

        public JsonResult GetUserDetailsJsonData(int Id)
        {
            UserViewModel _userViewModel = new UserViewModel();
            if (Id > 0)
            {
                _userViewModel = _dataAccessLayer.GetAllUser(4, Id, 0).FirstOrDefault();
            }
            return Json(_userViewModel);
        }

        [HttpPost]
        public IActionResult CreatePurchaseOrder(PurchaseOrderViewModel viewModel)
        {
            int PurchaseOrderId = 0;
            try
            {
                PurchaseOrderId = _dataAccessLayer.AddUpdatePurchaseOrderData(viewModel, 1);
                if (PurchaseOrderId > 0)
                {
                    foreach (var item in viewModel.PurchaseOrderDetails)
                    {
                        _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(2, PurchaseOrderId, item);
                    }

                    if (PurchaseOrderId > 0)
                    {
                        TempData["successmessage"] = "Your data has been saved successfully";
                        return RedirectToAction(nameof(PurchaseOrderList));
                    }
                    else
                    {
                        TempData["errormessage"] = "Something went wrong!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }

            return RedirectToAction(nameof(PurchaseOrderList));
        }

        public IActionResult PurchaseOrderList()
        {
            int UserId = 0;
           int RoleId = GetUserSession().RoleId;
            if(RoleId==5|| RoleId == 2)
            {
                UserId = GetUserSession().UserId;
            }           
            List<PurchaseOrderViewModel> _purchaseOrderList = new List<PurchaseOrderViewModel>();
            _purchaseOrderList = _dataAccessLayer.GetPurchaseOrderListData(12, 0, UserId).ToList();

            return View(_purchaseOrderList);

        }

        public IActionResult EditPurchaseOrder(int Id)
        {

            PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel();
            viewModel.PurchaseOrderDetails = new List<PurchaseOrderDetailsViewModel>();
            viewModel = _dataAccessLayer.GetPurchaseOrderListData(8, Id, 0).FirstOrDefault();

            viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
            viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);

            //PurchaseOrderDetailsViewModel row1 = new PurchaseOrderDetailsViewModel();
            //viewModel.PurchaseDate = DateTime.Now;
            //viewModel.PurchaseOrderDetails.Add(row1);

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult EditPurchaseOrder(PurchaseOrderViewModel viewModel)
        {
            int PurchaseOrderId = 0;
            try
            {
                PurchaseOrderId = _dataAccessLayer.AddUpdatePurchaseOrderData(viewModel, 9);

                if (PurchaseOrderId > 0)
                {
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction(nameof(PurchaseOrderList));
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }

            return RedirectToAction(nameof(PurchaseOrderList));
        }

        public IActionResult _PurchaseItemPartial(int PurchaseOrderDetailsId, int PurchaseOrderId)
        {

            PurchaseOrderDetailsViewModel viewModel = new PurchaseOrderDetailsViewModel();
            if (PurchaseOrderDetailsId > 0)
            {
                viewModel = _dataAccessLayer.GetPurchaseOrderItemListData(5, 0, PurchaseOrderDetailsId, 0).FirstOrDefault();
                viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
                viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
                viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
                viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
            }
            else
            {
                viewModel.PurchaseOrderId = PurchaseOrderId;
                viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
                viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
                viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
                viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
            }


            return PartialView(viewModel);

        }

        [HttpPost]
        public IActionResult _PurchaseItemPartial(PurchaseOrderDetailsViewModel viewModel)
        {
            int Response = 0;
            int PurchaseOrderId = viewModel.PurchaseOrderId;
            int PurchaseOrderDetailsId = viewModel.PurchaseOrderDetailsId;
            if (PurchaseOrderDetailsId > 0)
            {
                Response = _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(6, PurchaseOrderId, viewModel);
            }
            else
            {
                Response = _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(2, PurchaseOrderId, viewModel);
            }

            if (Response > 0)
            {
                TempData["successmessage"] = "Your data has been saved successfully";
                return RedirectToAction(nameof(EditPurchaseOrder), new { id = PurchaseOrderId });
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction(nameof(EditPurchaseOrder), new { id = PurchaseOrderId });

        }

        public IActionResult DeletePurchaseItem(int PurchaseOrderDetailsId, int PurchaseOrderId)
        {
            int Response = 0;
            PurchaseOrderDetailsViewModel viewModel = new PurchaseOrderDetailsViewModel();
            viewModel.PurchaseOrderDetailsId = PurchaseOrderDetailsId;

            if (PurchaseOrderDetailsId > 0)
            {
                Response = _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(7, PurchaseOrderId, viewModel);
            }
            if (Response > 0)
            {
                TempData["successmessage"] = "Your data has been remove successfully";
                return RedirectToAction(nameof(EditPurchaseOrder), new { id = PurchaseOrderId });
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction(nameof(EditPurchaseOrder), new { id = PurchaseOrderId });

        }

        public IActionResult DeletePurchaseOrder(int PurchaseOrderId)
        {
            int Response = 0;
            PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel();
            viewModel.PurchaseOrderId = PurchaseOrderId;
            Response = _dataAccessLayer.DeletePurchaseOrderData(viewModel, 10);


            if (Response > 0)
            {
                TempData["successmessage"] = "Your data has been remove successfully";
                return RedirectToAction(nameof(PurchaseOrderList));
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction(nameof(PurchaseOrderList));

        }


        public DataTable GetPurchaseOrderItemList(int PurchaseOrderId)
        {
            DataTable result = new DataTable();
            List<PurchaseOrderDetailsViewModel> _purchaseOrderItemList = new List<PurchaseOrderDetailsViewModel>();            
            _purchaseOrderItemList = _dataAccessLayer.GetPurchaseOrderItemListData(4, PurchaseOrderId, 0, 0).ToList();
            result = ToDataTable(_purchaseOrderItemList);
            //decimal total = _purchaseOrderItemList.Sum(item => item.Amount);
            return result;
        }

        public decimal GetPurchaseOrderItemTotalAmountList(int PurchaseOrderId)
        {
            decimal total = 0;
            List <PurchaseOrderDetailsViewModel> _purchaseOrderItemList = new List<PurchaseOrderDetailsViewModel>();
            _purchaseOrderItemList = _dataAccessLayer.GetPurchaseOrderItemListData(4, PurchaseOrderId, 0, 0).ToList(); 
            if(_purchaseOrderItemList.Count > 0)
            {
                total = _purchaseOrderItemList.Sum(item => item.TotalAmount);
            }           
            return total;
        }
        public DataTable GetPurchaseOrderDetailsData(int PurchaseOrderId)
        {
            DataTable result = new DataTable();
            List<PurchaseOrderViewModel> _purchaseOrderList = new List<PurchaseOrderViewModel>();
            
            _purchaseOrderList = _dataAccessLayer.GetPurchaseOrderListData(8, PurchaseOrderId, 0).ToList();
            result = ToDataTable(_purchaseOrderList);
            return result;
        }

        public DataTable GetInvoiceDetailsData(int Invoice)
        {
            DataTable result = new DataTable();
            List<PaymentInvoiceViewModel> _InvoiceList = new List<PaymentInvoiceViewModel>();
            _InvoiceList = _dataAccessLayer.GetInvoiceListData(8, Invoice, 0).ToList();
            result = ToDataTable(_InvoiceList);
            return result;
        }

        public DataTable GetInvoiceItemList(int Invoice)
        {
            DataTable result = new DataTable();
            List<InvoiceItemDetailsViewModel> _invoiceItemList = new List<InvoiceItemDetailsViewModel>();
            _invoiceItemList = _dataAccessLayer.GetInvoiceItemListData(4, Invoice, 0, 0).ToList();
            result = ToDataTable(_invoiceItemList);
            //decimal total = _purchaseOrderItemList.Sum(item => item.Amount);
            return result;
        }
        public InvoiceDataForPdfViewModel GetInvoiceItemTotalAmountList(int Invoice)
        {
            decimal total = 0;
            InvoiceDataForPdfViewModel _invoiceDataForPdfViewModel = new InvoiceDataForPdfViewModel();
            PaymentInvoiceViewModel _InvoicDetails = new PaymentInvoiceViewModel();
            _InvoicDetails = _dataAccessLayer.GetInvoiceListData(8, Invoice, 0).FirstOrDefault();
            if(_InvoicDetails!=null)
            {
                _invoiceDataForPdfViewModel.AdvancePaymentPercentage = _InvoicDetails.AdvancePaymentPercentage;
            }

            List<InvoiceItemDetailsViewModel> _invoiceItemList = new List<InvoiceItemDetailsViewModel>();
            _invoiceItemList = _dataAccessLayer.GetInvoiceItemListData(4, Invoice, 0, 0).ToList();
            if (_invoiceItemList.Count > 0)
            {
                _invoiceDataForPdfViewModel.TotalAmount = _invoiceItemList.Sum(item => item.TotalAmount);
                _invoiceDataForPdfViewModel.Amount = _invoiceItemList.Sum(item => item.Amount);
            }

            {
                _invoiceDataForPdfViewModel.AdvancePaymentAmount =Convert.ToDecimal(string.Format("{0:F2}", ((_invoiceDataForPdfViewModel.AdvancePaymentPercentage / 100) * _invoiceDataForPdfViewModel.Amount)));
                _invoiceDataForPdfViewModel.TotalAmount = (_invoiceDataForPdfViewModel.TotalAmount - _invoiceDataForPdfViewModel.AdvancePaymentAmount);
            }

            return _invoiceDataForPdfViewModel;
        }

        public FileResult GeneratePurchaseOrderPdf(int PurchaseOrderId)
        {
            decimal totalAmount = 0;
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

            //**************************
            DataTable PODetailsDT = new DataTable();
            PODetailsDT = GetPurchaseOrderDetailsData(PurchaseOrderId);
            DataTable OrderItemDT = new DataTable();
            OrderItemDT = GetPurchaseOrderItemList(PurchaseOrderId);
            totalAmount=GetPurchaseOrderItemTotalAmountList(PurchaseOrderId);
            //************************

            BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#3a4e86");
            BaseColor headerBackColor = WebColors.GetRGBColor("#3a4e86");
            //BaseColor footerFontColor = WebColors.GetRGBColor("#86899f");
            BaseColor footerFontColor = WebColors.GetRGBColor("#66677a");
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, true);

            var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            var titleFontWhite = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.WHITE);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            var footerFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, footerFontColor);
            var footerFont2 = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            var poFont = FontFactory.GetFont("Arial", 9, Font.BOLD | Font.UNDERLINE);
            var poFont2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
            var poFont3 = FontFactory.GetFont("Arial", 8, Font.BOLD | Font.UNDERLINE);
            // BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");


            var EmailFont1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            //*************** Add Header *************
            AddHeader(writer, pdfDoc);

            #region Top table



            PdfPTable Invoicetable = new PdfPTable(3);
            Invoicetable.HorizontalAlignment = 0;
            Invoicetable.WidthPercentage = 100;
            Invoicetable.SetWidths(new float[] { 160f, 120f, 180f });  // then set the column's __relative__ widths
            Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                //nested.AddCell(" ");
                //PdfPCell blankCell = new PdfPCell(new Phrase(" "));
                //blankCell.Border = Rectangle.NO_BORDER;                
                //nested.AddCell(blankCell);
                PdfPCell poCell1 = new PdfPCell(new Phrase("PURCHASE ORDER", poFont));
                poCell1.Border = Rectangle.NO_BORDER;
                nested.AddCell(poCell1);
                nested.AddCell(" ");
                nested.AddCell(" ");
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO", titleFontWhite));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.BackgroundColor = headerBackColor;
                nextPostCell1.VerticalAlignment = -2;
                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorName"].ToString(), titleFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorAddress"].ToString(), bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                PdfPCell nextPostCell4 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorEmail"].ToString(), EmailFont));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);
                PdfPCell nextPostCell5 = new PdfPCell(new Phrase("GST NO. " + PODetailsDT.Rows[0]["VendorGSTNo"].ToString(), bodyFont));
                nextPostCell5.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell5);
                PdfPCell nextPostCell6 = new PdfPCell(new Phrase("MOB - " + PODetailsDT.Rows[0]["VendorMobileNo"].ToString(), bodyFont));
                nextPostCell6.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell6);
                PdfPCell nextPostCell7 = new PdfPCell(new Phrase("VENDOR CODE - ", poFont3));
                nextPostCell7.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell7);
                PdfPCell nextPostCell8 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorCode"].ToString(), EmailFont));
                nextPostCell8.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell8);
                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                Invoicetable.AddCell(nesthousing);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                //middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //middlecell.BorderWidthBottom = 1f;
                Invoicetable.AddCell(middlecell);
            }


            {

                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                // nested.AddCell(" ");
                // nested.AddCell(" ");
                DateTime date = Convert.ToDateTime(PODetailsDT.Rows[0]["PurchaseDate"]);
                string formatted = date.ToString("dd-MM-yyyy");

                PdfPCell poDateCell1 = new PdfPCell(new Phrase("DATE - " + formatted, poFont2));
                poDateCell1.Border = Rectangle.NO_BORDER;
                poDateCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                nested.AddCell(poDateCell1);

                PdfPCell poNoCell1 = new PdfPCell(new Phrase("PO NO. – " + PODetailsDT.Rows[0]["PurchaseNo"].ToString(), poFont2));
                poNoCell1.Border = Rectangle.NO_BORDER;
                poNoCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                nested.AddCell(poNoCell1);
                nested.AddCell(" ");

                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("SHIP TO", titleFontWhite));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.BackgroundColor = headerBackColor;

                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["ShipTo"].ToString(), titleFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["ShipToAddress"].ToString(), bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                nested.AddCell("");
                nested.AddCell(" ");
                PdfPCell nextPostCell10 = new PdfPCell(new Phrase("Bill TO", titleFontWhite));
                nextPostCell10.Border = Rectangle.NO_BORDER;
                nextPostCell10.BackgroundColor = headerBackColor;
                nested.AddCell(nextPostCell10);

                PdfPCell nextPostCell4 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["BillTo"].ToString(), titleFont));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);
                PdfPCell nextPostCell5 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["BillToAddress"].ToString(), bodyFont));
                nextPostCell5.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell5);
                PdfPCell nextPostCell6 = new PdfPCell(new Phrase("GST NO. " + PODetailsDT.Rows[0]["BillToGSTNo"].ToString(), bodyFont));
                nextPostCell6.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell6);
                nested.AddCell("");

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                Invoicetable.AddCell(nesthousing);
            }


            //pdfDoc.Add(headertable);
            Invoicetable.PaddingTop = 10f;

            pdfDoc.Add(Invoicetable);
            #endregion

            //#region Items Table
            //Create body table

            bool IsTotalRow = false;
            int FromRowId = 0, ToRowId = 11,TotalRowCount=0;
            TotalRowCount = OrderItemDT.Rows.Count;
            //decimal TotalRowCount2 = (TotalRowCount / 15);
            

           

            if (TotalRowCount>10)
            {
                if(TotalRowCount>15)
                {
                    FromRowId = 0; ToRowId = 16;
                }
                AddTable(writer, pdfDoc, OrderItemDT, boldTableFont, bodyFont, EmailFont, TabelHeaderBackGroundColor, FromRowId, ToRowId, IsTotalRow, totalAmount);
                //AddTermsConditions(writer, pdfDoc, bodyFont, titleFont, PODetailsDT);
                AddFooter(writer, pdfDoc, footerFont, footerFont2);
                pdfDoc.NewPage();

                if (TotalRowCount > 15)
                {
                    FromRowId = 15; ToRowId = 35;
                }
                else
                {
                    FromRowId = 10; ToRowId = 31; 
                }

                IsTotalRow = true;
                AddHeader(writer, pdfDoc);
                AddBlankRow(writer, pdfDoc);
                AddTable(writer, pdfDoc, OrderItemDT, boldTableFont, bodyFont, EmailFont, TabelHeaderBackGroundColor, FromRowId, ToRowId, IsTotalRow, totalAmount);
                AddTermsConditions(writer, pdfDoc, bodyFont, titleFont, PODetailsDT, footerFont2, poFont);
                AddFooter(writer, pdfDoc, footerFont, footerFont2);

            }
            else 
            {
                IsTotalRow=true;
                AddTable(writer, pdfDoc, OrderItemDT, boldTableFont, bodyFont, EmailFont, TabelHeaderBackGroundColor, FromRowId, ToRowId, IsTotalRow, totalAmount);
                AddTermsConditions(writer, pdfDoc, bodyFont, titleFont, PODetailsDT, footerFont2, poFont);
                AddFooter(writer, pdfDoc, footerFont, footerFont2);
               
            }   

            


            pdfDoc.Close();

            

           // pdfDoc.Close();
            //DownloadFile(PDFData);
            //GetReport(PDFData);
            // PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("Sample-PDF-File.pdf"), FileMode.Create));
            //Export HTML String as PDF.
            // StringReader sr = new StringReader(cb.ToString());
            //Document pdfDoc1 = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            //Microsoft.AspNetCore.Http.HttpContext httpContext;
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter writer1 = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.ContentType = "application/pdf";
            //httpContext.Response.Headers ("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Write(pdfDoc);
            //Response.End();

            byte[] bytes = PDFData.ToArray();
            // System.IO.File.WriteAllBytes(@"D:\" + DateTime.Now.ToString("ddMMyyyyHHss") + ".pdf", bytes);

            //System.IO.File.WriteAllBytes(@"D:\test" + ".pdf", bytes);
            //PDFData.Close();
           // return File(bytes, "application/pdf");

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(bytes, "application/pdf");
            fileResult.FileDownloadName = "PO-"+DateTime.Now.ToString("ddMMyyyyHHss") + ".pdf";
            return fileResult;
        }

        protected void DownloadPDF(System.IO.MemoryStream PDFData)
        {
            Microsoft.AspNetCore.Http.HttpContext context;
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var stream = new System.IO.FileStream(@"C:\Users\shoba_eswar\Documents\REquest.pdf", System.IO.FileMode.Open);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "NewTab";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            //return File(stream);

            //  string filename = "\\cas1.cds.intranet\zone1\reg10\projA\Sc1.asd";
            Response.Clear();


            Response.ContentType = "application/octet-stream";
            
            //Response.Headers.Add("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filename));
            //Response.TransmitFile(filename);
            // Response.fl;

            //var httpContext = HttpContext.Current;
            //httpContext.Response.Clear();
            //httpContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            //httpContext.Response.BinaryWrite(GetFileAsByteArray(Server.MapPath(fileName)));
            //httpContext.Response.Flush();
            //httpContext.Response.SuppressContent = false;
            //httpContext.ApplicationInstance.CompleteRequest();
        }

        public FileResult DownloadFile(System.IO.MemoryStream PDFData)
        {
            string TransactionID = "1234";
            //GenerateInvoice(TransactionID);

            // var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            // var stream = new System.IO.FileStream(@"D:\REquest.pdf", System.IO.FileMode.Open);
            // response.Content = new StreamContent(stream);
            // response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            // response.Content.Headers.ContentDisposition.FileName = "NewTab";
            // response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            //// var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload/Task", filename);
            // //var path = Path.Combine(_hostingEnvironment.WebRootPath, "Sample.xlsx");
            // //var fs = new FileStream(path, FileMode.Open);

            // // Return the file. A byte array can also be used instead of a stream
            // return File(stream, "application/octet-stream", "abc.pdf");
            // // call GetFile Method in service and return

            // var dd= FileDownload(filename, "Upload");


            //string ddlValue = ddlFont.SelectedItem.Text.ToLower();
            //Document ds = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
            // System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(ds, memoryStream);
            //Font font = null;
            //ds.Open();

            //Paragraph p2 = new Paragraph(new Chunk("This is test", font));
            //ds.Add(p2);

            //Paragraph p = new Paragraph(new Chunk(new LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            ////LineSeparator line1 = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
            //ds.Add(p);
            //ds.Close();
            byte[] bytes = PDFData.ToArray();
            // System.IO.File.WriteAllBytes(@"D:\" + DateTime.Now.ToString("ddMMyyyyHHss") + ".pdf", bytes);
            System.IO.File.WriteAllBytes(@"D:\test" + ".pdf", bytes);
            
            PDFData.Close();

            return File(bytes, "application/pdf");

        }

        public void AddFooter(PdfWriter writer, Document document, Font footerFont, Font footerFont2)
        {
            int FIRST_ROW = 0;
            int LAST_ROW = -1;

            PdfPTable footertable = new PdfPTable(1);
            footertable.HorizontalAlignment = 0;
            //footertable.WidthPercentage = 100;
            //footertable.TotalHeight = 123F;
            footertable.TotalWidth = 500F;
           
            footertable.DefaultCell.Border = Rectangle.NO_BORDER;
            iTextSharp.text.Image footertablelogo = iTextSharp.text.Image.GetInstance("wwwroot/images/footer.png");
            footertablelogo.ScaleToFit(600f, 200f);
            footertablelogo.SetAbsolutePosition(0f, 0f);
            {
                PdfPCell pdfCelllogo = new PdfPCell(footertablelogo);
                pdfCelllogo.Border = Rectangle.NO_BORDER;
                pdfCelllogo.PaddingLeft = -12f;
               
                //pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //footertable.BorderWidthBottom = 1f;
                footertable.AddCell(pdfCelllogo);
            }
           //footertable.WriteSelectedRows(0, -1, document.Left, document.Bottom + footertable.TotalHeight, writer.DirectContent);
           footertable.WriteSelectedRows(0, -1, document.Left, document.Bottom +100, writer.DirectContent);


            //base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(2);
            PdfPTable tabFot2 = new PdfPTable(1);
            PdfPTable tabFot3 = new PdfPTable(1);
            PdfPCell cellSig;
            PdfPCell cellSig2;
            PdfPCell cell;
            PdfPCell cell2;
            PdfPCell cell3;
            PdfPCell cell4;
            tabFot.TotalWidth = 570F;
            tabFot.DefaultCell.Border = Rectangle.NO_BORDER;
            tabFot2.DefaultCell.Border = Rectangle.NO_BORDER;
            tabFot3.DefaultCell.Border = Rectangle.NO_BORDER;

            //tabFot.WidthPercentage = 100f;

            //cellSig2 = new PdfPCell(new Phrase("For, Secure Infratech FinServ And Securities Limited", footerFont2));
            cellSig2 = new PdfPCell(new Phrase(" "));
            cellSig2.Border = Rectangle.NO_BORDER;
            //cellSig = new PdfPCell(new Phrase("Authorized Signatory", footerFont2));
            cellSig = new PdfPCell(new Phrase(" ", footerFont2));
            cellSig.Border = Rectangle.NO_BORDER;
            cell = new PdfPCell(new Phrase("Regd.Office: 68 Jessore Road,Diamond Arcade,5th Floor,Suit No. 511,Kolkata 700055", footerFont));
            cell.Border = Rectangle.NO_BORDER;
            //cell.VerticalAlignment = 5;
            cell.PaddingTop = 12f;
            cell.PaddingBottom = -8f;
            cell2 = new PdfPCell(new Phrase("Phone- +91 8420105927", footerFont));
            cell2.Border = Rectangle.NO_BORDER;
            cell2.PaddingTop = 20f;
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell3 = new PdfPCell(new Phrase("E-mail- kolkata@sfisltd.in", footerFont));
            cell3.Border = Rectangle.NO_BORDER;
            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell4 = new PdfPCell(new Phrase("Website- www.sfisltd.com", footerFont));
            cell4.Border = Rectangle.NO_BORDER;
            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;

            tabFot2.AddCell(cellSig2);
            tabFot2.AddCell(cellSig);
            tabFot2.AddCell(cell);
            tabFot3.AddCell(cell2);
            tabFot3.AddCell(cell3);
            tabFot3.AddCell(cell4);
            tabFot.AddCell(tabFot2);
            tabFot.AddCell(tabFot3);
            //document.AddSubject("Test Subject");
            //tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
            tabFot.WriteSelectedRows(FIRST_ROW, LAST_ROW, document.Left, document.Bottom + tabFot.TotalHeight, writer.DirectContent);

            //PdfPTable Invoicetable = new PdfPTable(new float[] { 1F });
            //Invoicetable.HorizontalAlignment = 0;
            //Invoicetable.WidthPercentage = 100;
            //Invoicetable.TotalWidth = 300F;
            // //Invoicetable.SetWidths(new float[] { 160f, 180f });  // then set the column's __relative__ widths
            //Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;
            //{
            //    PdfPTable nested = new PdfPTable(1);
            //    nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //    nested.AddCell(" ");
            //    //PdfPCell blankCell = new PdfPCell(new Phrase(" "));
            //    //blankCell.Border = Rectangle.NO_BORDER;                
            //    //nested.AddCell(blankCell);
            //    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO:"));
            //    nextPostCell1.Border = Rectangle.NO_BORDER;
            //    //nextPostCell1.BackgroundColor = ;
            //    nested.AddCell(nextPostCell1);
            //    PdfPCell nesthousing = new PdfPCell(nested);
            //    nesthousing.Border = Rectangle.NO_BORDER;

            //    //nesthousing.Rowspan = 5;
            //    //nesthousing.PaddingBottom = 10f;
            //    Invoicetable.AddCell(nesthousing);               
            //}
            //{
            //    PdfPTable nested = new PdfPTable(1);
            //    nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //    nested.AddCell(" ");
            //    //PdfPCell blankCell = new PdfPCell(new Phrase(" "));
            //    //blankCell.Border = Rectangle.NO_BORDER;                
            //    //nested.AddCell(blankCell);
            //    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE FORM:"));
            //    nextPostCell1.Border = Rectangle.NO_BORDER;
            //    //nextPostCell1.BackgroundColor = ;
            //    nested.AddCell(nextPostCell1);
            //    PdfPCell nesthousing = new PdfPCell(nested);
            //    nesthousing.Border = Rectangle.NO_BORDER;

            //    nesthousing.Rowspan = 5;
            //    nesthousing.PaddingBottom = 10f;
            //    Invoicetable.AddCell(nesthousing);
            //}


            //Invoicetable.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);

        }

        private void AddHeader(PdfWriter writer, Document document)
        {
            PdfPTable headertable = new PdfPTable(1);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            // headertable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing   

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("wwwroot/images/header.png");
            logo.ScaleToFit(600f, 200f);
            logo.SetAbsolutePosition(0f, 0f);
            {
                PdfPCell pdfCelllogo = new PdfPCell(logo);
                pdfCelllogo.Border = Rectangle.NO_BORDER;
                pdfCelllogo.PaddingLeft = -12f;
                //pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //pdfCelllogo.BorderWidthBottom = 1f;
                headertable.AddCell(pdfCelllogo);
            }
            document.Add(headertable);
        }


        private void AddTable(PdfWriter writer, Document document, DataTable OrderItemDT, Font boldTableFont, Font bodyFont, Font EmailFont, BaseColor TabelHeaderBackGroundColor, int FromRowId, int ToRowId, bool IsTotalRow,decimal totalAmount)
        {
            decimal AdvancePaymentPercent = 10;
            decimal AdvancePaymentAmount = 100;
            PdfPTable itemTable = new PdfPTable(8);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            //itemTable.PaddingTop = 150f;
            //itemTable.SetWidths(new float[] { 5, 40, 10, 20, 25 });  // then set the column's __relative__ widths
            itemTable.SetWidths(new float[] { 5, 40, 10, 10, 5, 10, 10, 10 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;

            PdfPCell cell1 = new PdfPCell(new Phrase("NO", boldTableFont));
            cell1.BackgroundColor = TabelHeaderBackGroundColor;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = 5;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("DESCRIPTION", boldTableFont));
            cell2.BackgroundColor = TabelHeaderBackGroundColor;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = 5;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("QUANTITY", boldTableFont));
            cell3.BackgroundColor = TabelHeaderBackGroundColor;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = 5;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("UNIT PRICE", boldTableFont));
            cell4.BackgroundColor = TabelHeaderBackGroundColor;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = 5;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("UOM", boldTableFont));
            cell5.BackgroundColor = TabelHeaderBackGroundColor;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = 5;
            itemTable.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase("AMOUNT", boldTableFont));
            cell6.BackgroundColor = TabelHeaderBackGroundColor;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            cell6.VerticalAlignment = 5;
            itemTable.AddCell(cell6);
            PdfPCell cell7 = new PdfPCell(new Phrase("GST AMOUNT(%)", boldTableFont));
            cell7.BackgroundColor = TabelHeaderBackGroundColor;
            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
            cell7.VerticalAlignment = 5;
            itemTable.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase("Total AMOUNT", boldTableFont));
            cell8.BackgroundColor = TabelHeaderBackGroundColor;
            cell8.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell8);



            foreach (DataRow row in OrderItemDT.Select().Where(e => Convert.ToInt32(e.ItemArray[0]) > FromRowId && Convert.ToInt32(e.ItemArray[0]) < ToRowId))
            {
                // RowId = RowId + 1;

                PdfPCell numberCell = new PdfPCell(new Phrase(row["RowId"].ToString(), bodyFont));
                numberCell.HorizontalAlignment = 1;
                numberCell.VerticalAlignment = 5;
                numberCell.PaddingLeft = 5f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                numberCell.FixedHeight = 18f;
                itemTable.AddCell(numberCell);

                var _phrase = new Phrase();
                _phrase.Add(new Chunk(row["SubProductName"].ToString(), bodyFont));

                //_phrase.Add(new Chunk("New Signup Subscription Plan\n", EmailFont));
                //_phrase.Add(new Chunk("Subscription Plan description will add here.", bodyFont));
                PdfPCell descCell = new PdfPCell(_phrase);
                descCell.HorizontalAlignment = Element.ALIGN_CENTER;
                descCell.VerticalAlignment = 5;
                descCell.PaddingLeft = 5f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                descCell.FixedHeight = 18f;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(row["Quantity"].ToString(), bodyFont));
                qtyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                qtyCell.VerticalAlignment = 5;
                qtyCell.PaddingLeft = 5f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                qtyCell.FixedHeight = 18f;
                itemTable.AddCell(qtyCell);

                PdfPCell unitPriceCell = new PdfPCell(new Phrase("₹" + row["UnitPrice"].ToString(), bodyFont));
                unitPriceCell.HorizontalAlignment = 1;
                unitPriceCell.VerticalAlignment = 5;
                unitPriceCell.PaddingLeft = 5f;
                unitPriceCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                unitPriceCell.FixedHeight = 18f;
                itemTable.AddCell(unitPriceCell);

                PdfPCell uomCell = new PdfPCell(new Phrase(row["UnitName"].ToString(), bodyFont));
                uomCell.HorizontalAlignment = 1;
                uomCell.VerticalAlignment = 5;
                uomCell.PaddingLeft = 5f;
                uomCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                uomCell.FixedHeight = 18f;
                itemTable.AddCell(uomCell);

                PdfPCell amountCell = new PdfPCell(new Phrase("₹" + row["Amount"].ToString(), bodyFont));
                amountCell.HorizontalAlignment = 1;
                amountCell.VerticalAlignment = 5;
                amountCell.PaddingLeft = 5f;
                amountCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                amountCell.FixedHeight = 18f;
                itemTable.AddCell(amountCell);

                PdfPCell gstamountCell = new PdfPCell(new Phrase("₹" + row["GSTAmount"].ToString() + "(" + row["GSTPercen"].ToString() + "%)", bodyFont));
                gstamountCell.HorizontalAlignment = 1;
                gstamountCell.VerticalAlignment = 5;
                gstamountCell.PaddingLeft = 5f;
                gstamountCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                gstamountCell.FixedHeight = 18f;
                itemTable.AddCell(gstamountCell);

                PdfPCell totalamtCell = new PdfPCell(new Phrase("₹" + row["TotalAmount"].ToString(), bodyFont));
                totalamtCell.HorizontalAlignment = 1;
                totalamtCell.VerticalAlignment = 5;
                totalamtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                totalamtCell.FixedHeight = 18f;
                itemTable.AddCell(totalamtCell);

            }
            //Sum Total Amount ..
            if (IsTotalRow==true)
            {
                //Decimal TotalPrice = Convert.ToDecimal(OrderItemDT.Compute("SUM(Convert.ToDecimal(TotalAmount))", "RowId > 0"));
                //Decimal total = OrderItemDT.AsEnumerable().Where(row => !DBNull.Value.Equals(row[7])).Sum(row => Convert.ToDecimal(row[7]));
                //{
                //    PdfPCell cell = new PdfPCell(new Phrase("Advance Payment (If any) : " + AdvancePaymentPercent + " % ", bodyFont));
                //    cell.Colspan = 7;
                //    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //    cell.FixedHeight = 18f;
                //    //cell.HorizontalAlignment = 1;
                //    cell.VerticalAlignment = 5;
                //    itemTable.AddCell(cell);

                //    PdfPCell cell10 = new PdfPCell(new Phrase(AdvancePaymentAmount.ToString(), bodyFont));
                //    cell10.Colspan = 1;
                //    cell10.FixedHeight = 18f;
                //    cell10.HorizontalAlignment = 1;
                //    cell10.VerticalAlignment = 5;
                //    itemTable.AddCell(cell10);

                //}
                {
                    PdfPCell cell = new PdfPCell(new Phrase("Total Amount ", bodyFont));
                    cell.Colspan = 7;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.FixedHeight = 18f;
                    //cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 5;
                    itemTable.AddCell(cell);

                    PdfPCell cell10 = new PdfPCell(new Phrase(totalAmount.ToString(), bodyFont));
                    cell10.Colspan = 1;                    
                    cell10.FixedHeight = 18f;
                    cell10.HorizontalAlignment = 1;
                    cell10.VerticalAlignment = 5;
                    itemTable.AddCell(cell10);

                }
                
            }
           
            
           document.Add(itemTable);
        }

        private void writeFooterTable(PdfWriter writer, Document document, PdfPTable table)
        {
            int FIRST_ROW = 0;
            int LAST_ROW = -1;
            //Table must have absolute width set.header
            if (table.TotalWidth == 0)
                // table.SetTotalWidth((document.Right - document.Left) * table.WidthPercentage / 100f);
                table.WriteSelectedRows(FIRST_ROW, LAST_ROW, document.Left, document.Bottom + table.TotalHeight, writer.DirectContent);
        }

        private void AddTermsConditions(PdfWriter writer, Document document, Font bodyFont, Font titleFont, DataTable PODetailsDT, Font footerFont2,Font poFont)
        {
            PdfPTable TCtable = new PdfPTable(1);
            TCtable.HorizontalAlignment = 0;
            TCtable.TotalWidth = 500F;
            //TCtable.WidthPercentage = 100;
            //TCtable.SetWidths(new float[] { 160f, 120f, 180f });  // then set the column's __relative__ widths
            TCtable.DefaultCell.Border = Rectangle.NO_BORDER;
            //TCtable.PaddingTop = 10f;

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
               // nested.AddCell(" ");
                //PdfPCell blankCell = new PdfPCell(new Phrase(" "));
                //blankCell.Border = Rectangle.NO_BORDER;                
                //nested.AddCell(blankCell);
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("TERMS & CONDITIONS", poFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                //nextPostCell1.Border = Rectangle.BOTTOM_BORDER;
                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["TermsConditions"].ToString(), bodyFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                nested.AddCell(" ");
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase("For, Secure Infratech FinServ And Securities Limited", footerFont2));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                nested.AddCell(" ");
                //nested.AddCell(" ");
                PdfPCell nextPostCell4 = new PdfPCell(new Phrase("Authorized Signatory", footerFont2));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                //nesthousing.Rowspan = 5;
                //nesthousing.PaddingBottom = 10f;
                TCtable.AddCell(nesthousing);
            }

            //TCtable.WriteSelectedRows(0, -40, pdfDoc.Left, 60 + TCtable.TotalHeight, writer.DirectContent);

            document.Add(TCtable);
        }

        private void AddBlankRow(PdfWriter writer, Document document)
        {

            PdfPTable table = new PdfPTable(1);
            table.HorizontalAlignment = 0;
            table.TotalWidth = 500F;
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPTable nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.NO_BORDER;
            nested.AddCell(" ");

            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Border = Rectangle.NO_BORDER;
            table.AddCell(nesthousing);

            document.Add(table);

        }


        //public FileResult GetReport()
        //{


        //    string ReportURL = "D:/test.pdf";
        //    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
        //    //byte[] FileBytes = PDFData.ToArray();
        //    return File(FileBytes, "application/pdf");
        //}

        public IActionResult Abc()
        {
            string TransactionID = "1234";
            //GenerateInvoice(TransactionID);

            return View();
        }

        [HttpGet]
        public IActionResult QuatationToPayment()
        {
           
            List<QuatationToPaymentDetailsViewModel> _quatationToPaymentList = new List<QuatationToPaymentDetailsViewModel>();
            _quatationToPaymentList = _dataAccessLayer.GetQuatationToPaymentListData(5, 1, 2).ToList();

            return View(_quatationToPaymentList);
        }

        [HttpGet]
        public IActionResult QuatationToPaymentDetails(int QuatationToPaymentId)
        {
            
           QuatationToPaymentDetailsViewModel _quatationToPaymentDetailsViewModel = new QuatationToPaymentDetailsViewModel();
            _quatationToPaymentDetailsViewModel = _dataAccessLayer.GetQuatationToPaymentListData(6, QuatationToPaymentId, 2).FirstOrDefault();

            return View(_quatationToPaymentDetailsViewModel);
        }

        public IActionResult DeleteQuatationToPaymentDetails(int QuatationToPaymentId)
        {
            int Response = 0;           
            QuatationToPaymentDetailsViewModel viewModel=new    QuatationToPaymentDetailsViewModel();
            viewModel.QuatationToPaymentId= QuatationToPaymentId;
             Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(14, viewModel);           

            return RedirectToAction("QuatationToPayment");
        }

        [HttpPost]
        public IActionResult AddQuatation(QuatationToPaymentDetailsViewModel viewModel)
        {
            int Response = 0;
           // int Id = viewModel.QuatationToPaymentId;
            if (viewModel.QuatationFile != null)
            {
                UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                _uploadFileResponse = UploadFileWithName(viewModel.QuatationFile, "Upload/Quatation", viewModel.QuatationToPaymentId.ToString()+ "_QuatationFile");
                if (_uploadFileResponse != null)
                {
                    if (_uploadFileResponse.UploadSuccess == true)
                    {
                        viewModel.QuatationFileName = _uploadFileResponse.FileName;
                    }
                }

                Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(2, viewModel);
            }
            else
            {
                if (viewModel.QuatationFileName != null && viewModel.QuatationAmount>0)
                {

                    Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(2, viewModel);

                }


            }

            return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });

        }

        [HttpPost]
        public IActionResult AddPI(QuatationToPaymentDetailsViewModel viewModel)
        {
            int Response = 0;
            if (viewModel.PIFile != null)
            {
                UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                _uploadFileResponse = UploadFileWithName(viewModel.PIFile, "Upload/PI", viewModel.QuatationToPaymentId.ToString() + "_PIFile");
                if (_uploadFileResponse != null)
                {
                    if (_uploadFileResponse.UploadSuccess == true)
                    {
                        viewModel.PIFileName = _uploadFileResponse.FileName;
                    }
                }
                Response= _dataAccessLayer.AddUpdateQuatationToPaymentData(3, viewModel);
            }
            else
            {
                if (viewModel.PIFileName != null && viewModel.PIAmount > 0)
                {

                    Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(3, viewModel);

                }
            }

            return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });

        }

        [HttpPost]
        public IActionResult AddVendorFinalBill(QuatationToPaymentDetailsViewModel viewModel)
        {
            int Response = 0;
            if (viewModel.VFinelBillFile != null)
            {
                UploadFileResponseViewModel _uploadFileResponse = new UploadFileResponseViewModel();
                _uploadFileResponse = UploadFileWithName(viewModel.VFinelBillFile, "Upload/VFinalBill", viewModel.QuatationToPaymentId.ToString() + "_VFinalBillFile");
                if (_uploadFileResponse != null)
                {
                    if (_uploadFileResponse.UploadSuccess == true)
                    {
                        viewModel.VFinelBillFileName = _uploadFileResponse.FileName;
                    }
                }
                Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(11, viewModel);
            }
            else
            {
                if (viewModel.VFinelBillFileName != null && viewModel.FinalAmount > 0)
                {

                    Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(11, viewModel);

                }

            }

            return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });
        }

        [HttpGet]
        public IActionResult AddQuatationToPayment()
        {
            int Response = 0;
            QuatationToPaymentDetailsViewModel _quatationToPayment = new QuatationToPaymentDetailsViewModel();
            _quatationToPayment.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
            //_quatationToPayment.Date = DateTime.Now.ToShortDateString();
            
           // Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(1, _quatationToPayment);

            return View(_quatationToPayment);
            // return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = Response });
        }

        [HttpPost]
        public IActionResult AddQuatationToPayment(QuatationToPaymentDetailsViewModel _quatationToPayment)
        {
            int Response = 0;           
            _quatationToPayment.Date =Convert.ToDateTime(_quatationToPayment.Date).ToShortDateString();
            Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(1, _quatationToPayment);
            
            return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = Response });
        }

        public IActionResult AddPOQuatationToPayment(int Id,int POId,int QuatationToPaymentId)
        {
            int Response = 0;
            if(POId>0)
            {
                Response = POId;
            }
            else
            {
                QuatationToPaymentDetailsViewModel _quatationToPayment = new QuatationToPaymentDetailsViewModel();
                _quatationToPayment.Date = DateTime.Now.ToShortDateString();
                _quatationToPayment.UserId = Id;
                _quatationToPayment.CreatedBy = 1;
                Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(7, _quatationToPayment);
            }
           

            return RedirectToAction("AddEditPurchaseOrder", new { Id = Response, QuatationToPaymentId= QuatationToPaymentId });

        }

        public IActionResult AddEditPurchaseOrder(int Id,int QuatationToPaymentId)
        {

            PurchaseOrderViewModel viewModel = new PurchaseOrderViewModel();
            viewModel.PurchaseOrderDetails = new List<PurchaseOrderDetailsViewModel>();
            viewModel = _dataAccessLayer.GetPurchaseOrderListData(8, Id, 0).FirstOrDefault();
            viewModel.QuatationToPaymentId=QuatationToPaymentId;
            viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
            viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
            //viewModel.PurchaseDate = DateTime.Now;
            if (viewModel.PurchaseDate == Convert.ToDateTime("01-01-0001 00:00:00"))
            {
                //{01-01-0001 00:00:00}
                viewModel.PurchaseDate= DateTime.Now;
            }

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult AddEditPurchaseOrder(PurchaseOrderViewModel viewModel)
        {
            int PurchaseOrderId = 0;
            try
            {
                PurchaseOrderId = _dataAccessLayer.AddUpdatePurchaseOrderData(viewModel, 9);

                if (PurchaseOrderId > 0)
                {
                    QuatationToPaymentDetailsViewModel viewModel2=new QuatationToPaymentDetailsViewModel();
                    viewModel2.POId = PurchaseOrderId;
                    viewModel2.QuatationToPaymentId = viewModel.QuatationToPaymentId;
                    _dataAccessLayer.AddUpdateQuatationToPaymentData(8, viewModel2);
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }

            return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });
        }


        public IActionResult _AddEditPurchaseItemPartial(int PurchaseOrderDetailsId, int PurchaseOrderId,int QuatationToPaymentId)
        {

            PurchaseOrderDetailsViewModel viewModel = new PurchaseOrderDetailsViewModel();
            if (PurchaseOrderDetailsId > 0)
            {
                viewModel = _dataAccessLayer.GetPurchaseOrderItemListData(5, 0, PurchaseOrderDetailsId, 0).FirstOrDefault();
                viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
                viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
                viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
                viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
                viewModel.QuatationToPaymentId = QuatationToPaymentId;
            }
            else
            {
                viewModel.PurchaseOrderId = PurchaseOrderId;
                viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
                viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
                viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
                viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
                viewModel.QuatationToPaymentId = QuatationToPaymentId;
            }


            return PartialView(viewModel);

        }

        [HttpPost]
        public IActionResult _AddEditPurchaseItemPartial(PurchaseOrderDetailsViewModel viewModel)
        {
            int Response = 0;
            int PurchaseOrderId = viewModel.PurchaseOrderId;
            int PurchaseOrderDetailsId = viewModel.PurchaseOrderDetailsId;
            if (PurchaseOrderDetailsId > 0)
            {
                Response = _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(6, PurchaseOrderId, viewModel);
            }
            else
            {
                Response = _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(2, PurchaseOrderId, viewModel);
            }

            if (Response > 0)
            {
                TempData["successmessage"] = "Your data has been saved successfully";
                return RedirectToAction(nameof(AddEditPurchaseOrder), new { id = PurchaseOrderId, QuatationToPaymentId = viewModel.QuatationToPaymentId });
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction(nameof(AddEditPurchaseOrder), new { id = PurchaseOrderId , QuatationToPaymentId =viewModel.QuatationToPaymentId });

        }


        public IActionResult DownloadQuatationFile(string filename)
        {           
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Upload/Quatation", filename);            
            var fs = new FileStream(path, FileMode.Open);           
            return File(fs, "application/octet-stream", filename);            

        }
        public IActionResult DownloadPIFile(string filename)
        {
            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Upload/PI", filename);
            var fs = new FileStream(path, FileMode.Open);
            return File(fs, "application/octet-stream", filename);

        }

        public IActionResult DownloadVFinalBillFile(string filename)
        {
            try
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Upload/VFinalBill", filename);
                var fs = new FileStream(path, FileMode.Open);

                return File(fs, "application/octet-stream", filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public IActionResult AddPaymentInvoiceQuatationToPayment(int Id, int PaymentId, int POId, int QuatationToPaymentId)
        {
            int Response = 0;
            if (POId > 0)
            {
                if (PaymentId > 0)
                {
                    Response = PaymentId;
                }
                else
                {
                    QuatationToPaymentDetailsViewModel _quatationToPayment = new QuatationToPaymentDetailsViewModel();
                    _quatationToPayment.Date = DateTime.Now.ToShortDateString();
                    _quatationToPayment.UserId = Id;
                    _quatationToPayment.CreatedBy = 1;
                    _quatationToPayment.PaymentId = PaymentId;
                    _quatationToPayment.POId = POId;
                    Response = _dataAccessLayer.AddUpdateQuatationToPaymentData(9, _quatationToPayment);
                }
            }else
            {
                TempData["errormessage"] = "Something went wrong! Please create PO first.";
                return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = QuatationToPaymentId });

            }
            return RedirectToAction("AddEditInvoicePayment", new { Id = Response, QuatationToPaymentId = QuatationToPaymentId });

        }

        public IActionResult AddEditInvoicePayment(int Id, int QuatationToPaymentId)
        {

            PaymentInvoiceViewModel viewModel = new PaymentInvoiceViewModel();
            viewModel.InvoiceItemDetails = new List<InvoiceItemDetailsViewModel>();
            viewModel = _dataAccessLayer.GetInvoiceListData(8, Id, 0).FirstOrDefault();
            viewModel.QuatationToPaymentId = QuatationToPaymentId;
            viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
            viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
            viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
            viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult AddEditInvoicePayment(PaymentInvoiceViewModel viewModel)
        {
            int InvoiceId = 0;
            try
            {
                InvoiceId = _dataAccessLayer.AddUpdateInvoiceData(viewModel, 9);

                if (InvoiceId > 0)
                {
                    QuatationToPaymentDetailsViewModel viewModel2 = new QuatationToPaymentDetailsViewModel();
                    viewModel2.PaymentId = InvoiceId;
                    viewModel2.QuatationToPaymentId = viewModel.QuatationToPaymentId;
                    _dataAccessLayer.AddUpdateQuatationToPaymentData(10, viewModel2);
                    TempData["successmessage"] = "Your data has been saved successfully";
                    return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });
                }
                else
                {
                    TempData["errormessage"] = "Something went wrong!";
                }

            }
            catch (Exception ex)
            {
                TempData["errormessage"] = "Error: Something went wrong! -" + ex.Message;
                throw ex;

            }

            return RedirectToAction("QuatationToPaymentDetails", new { QuatationToPaymentId = viewModel.QuatationToPaymentId });
        }


        public IActionResult _AddEditInvoiceItemPartial(int InvoiceItemDetailsId, int InvoiceId, int QuatationToPaymentId)
        {

            InvoiceItemDetailsViewModel viewModel = new InvoiceItemDetailsViewModel();
            if (InvoiceItemDetailsId > 0)
            {
                viewModel = _dataAccessLayer.GetInvoiceItemListData(5, 0, InvoiceItemDetailsId, 0).FirstOrDefault();
                viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
                viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
                viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
                viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
                viewModel.QuatationToPaymentId = QuatationToPaymentId;
            }
            else
            {
                viewModel.InvoiceId = InvoiceId;
                viewModel.ProductList = _dataAccessLayerLinq.GetDropDownListData("Product", 0);
                viewModel.SubProductList = _dataAccessLayerLinq.GetDropDownListData("AllSubProduct", 0);
                viewModel.UnitList = _dataAccessLayerLinq.GetDropDownListData("Unit", 0);
                viewModel.VendorList = _dataAccessLayerLinq.GetDropDownListData("Vendor&Supplier", 0);
                viewModel.QuatationToPaymentId = QuatationToPaymentId;
            }


            return PartialView(viewModel);

        }

        [HttpPost]
        public IActionResult _AddEditInvoiceItemPartial(InvoiceItemDetailsViewModel viewModel)
        {
            int Response = 0;
            int InvoiceId = viewModel.InvoiceId;
            int InvoiceItemDetailsId = viewModel.InvoiceItemDetailsId;
            if (InvoiceItemDetailsId > 0)
            {
                Response = _dataAccessLayer.AddUpdateInvoiceItemData(6, InvoiceId, viewModel);
            }
            else
            {
                Response = _dataAccessLayer.AddUpdateInvoiceItemData(2, InvoiceId, viewModel);
            }

            if (Response > 0)
            {
                TempData["successmessage"] = "Your data has been saved successfully";
                return RedirectToAction(nameof(AddEditInvoicePayment), new { id = InvoiceId, QuatationToPaymentId = viewModel.QuatationToPaymentId });
            }
            else
            {
                TempData["errormessage"] = "Something went wrong!";
            }

            return RedirectToAction(nameof(AddEditInvoicePayment), new { id = InvoiceId, QuatationToPaymentId = viewModel.QuatationToPaymentId });

        }

        public FileResult GenerateInvoicePdf(int InvoiceId)
        {
            decimal totalAmount = 0;
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

            //**************************
            DataTable PODetailsDT = new DataTable();
            PODetailsDT = GetInvoiceDetailsData(InvoiceId);
            DataTable OrderItemDT = new DataTable();
            OrderItemDT = GetInvoiceItemList(InvoiceId);
            InvoiceDataForPdfViewModel _invoiceDataForPdfViewModel = new InvoiceDataForPdfViewModel();
            _invoiceDataForPdfViewModel = GetInvoiceItemTotalAmountList(InvoiceId);
            //************************

            BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#3a4e86");
            BaseColor headerBackColor = WebColors.GetRGBColor("#3a4e86");
            //BaseColor footerFontColor = WebColors.GetRGBColor("#86899f");
            BaseColor footerFontColor = WebColors.GetRGBColor("#66677a");
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, true);

            var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            var titleFontWhite = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.WHITE);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            var footerFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, footerFontColor);
            var footerFont2 = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            var poFont = FontFactory.GetFont("Arial", 9, Font.BOLD | Font.UNDERLINE);
            var poFont2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
            var poFont3 = FontFactory.GetFont("Arial", 8, Font.BOLD | Font.UNDERLINE);
            // BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");


            var EmailFont1 = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            //*************** Add Header *************
            AddHeader(writer, pdfDoc);

            #region Top table



            PdfPTable Invoicetable = new PdfPTable(3);
            Invoicetable.HorizontalAlignment = 0;
            Invoicetable.WidthPercentage = 100;
            Invoicetable.SetWidths(new float[] { 160f, 120f, 180f });  // then set the column's __relative__ widths
            Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                //nested.AddCell(" ");
                //PdfPCell blankCell = new PdfPCell(new Phrase(" "));
                //blankCell.Border = Rectangle.NO_BORDER;                
                //nested.AddCell(blankCell);
                PdfPCell poCell1 = new PdfPCell(new Phrase("INVOICE", poFont));
                poCell1.Border = Rectangle.NO_BORDER;
                nested.AddCell(poCell1);
                nested.AddCell(" ");
                nested.AddCell(" ");
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO", titleFontWhite));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.BackgroundColor = headerBackColor;
                nextPostCell1.VerticalAlignment = -2;
                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorName"].ToString(), titleFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorAddress"].ToString(), bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                PdfPCell nextPostCell4 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorEmail"].ToString(), EmailFont));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);
                PdfPCell nextPostCell5 = new PdfPCell(new Phrase("GST NO. " + PODetailsDT.Rows[0]["VendorGSTNo"].ToString(), bodyFont));
                nextPostCell5.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell5);
                PdfPCell nextPostCell6 = new PdfPCell(new Phrase("MOB - " + PODetailsDT.Rows[0]["VendorMobileNo"].ToString(), bodyFont));
                nextPostCell6.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell6);
                PdfPCell nextPostCell7 = new PdfPCell(new Phrase("VENDOR CODE - ", poFont3));
                nextPostCell7.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell7);
                PdfPCell nextPostCell8 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["VendorCode"].ToString(), EmailFont));
                nextPostCell8.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell8);
                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                Invoicetable.AddCell(nesthousing);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                //middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //middlecell.BorderWidthBottom = 1f;
                Invoicetable.AddCell(middlecell);
            }


            {

                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                // nested.AddCell(" ");
                // nested.AddCell(" ");
                DateTime date = Convert.ToDateTime(PODetailsDT.Rows[0]["InvoiceDate"]);
                string formatted = date.ToString("dd-MM-yyyy");

                PdfPCell poDateCell1 = new PdfPCell(new Phrase("DATE - " + formatted, poFont2));
                poDateCell1.Border = Rectangle.NO_BORDER;
                poDateCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                nested.AddCell(poDateCell1);

                PdfPCell poNoCell1 = new PdfPCell(new Phrase("Invoice NO. – " + PODetailsDT.Rows[0]["InvoiceNo"].ToString(), poFont2));
                poNoCell1.Border = Rectangle.NO_BORDER;
                poNoCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                nested.AddCell(poNoCell1);
                nested.AddCell(" ");

                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("SHIP TO", titleFontWhite));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.BackgroundColor = headerBackColor;

                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["ShipTo"].ToString(), titleFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["ShipToAddress"].ToString(), bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                nested.AddCell("");
                nested.AddCell(" ");
                PdfPCell nextPostCell10 = new PdfPCell(new Phrase("Bill TO", titleFontWhite));
                nextPostCell10.Border = Rectangle.NO_BORDER;
                nextPostCell10.BackgroundColor = headerBackColor;
                nested.AddCell(nextPostCell10);

                PdfPCell nextPostCell4 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["BillTo"].ToString(), titleFont));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);
                PdfPCell nextPostCell5 = new PdfPCell(new Phrase(PODetailsDT.Rows[0]["BillToAddress"].ToString(), bodyFont));
                nextPostCell5.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell5);
                PdfPCell nextPostCell6 = new PdfPCell(new Phrase("GST NO. " + PODetailsDT.Rows[0]["BillToGSTNo"].ToString(), bodyFont));
                nextPostCell6.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell6);
                nested.AddCell("");

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                Invoicetable.AddCell(nesthousing);
            }


            //pdfDoc.Add(headertable);
            Invoicetable.PaddingTop = 10f;

            pdfDoc.Add(Invoicetable);
            #endregion

            //#region Items Table
            //Create body table

            bool IsTotalRow = false;
            int FromRowId = 0, ToRowId = 11, TotalRowCount = 0;
            TotalRowCount = OrderItemDT.Rows.Count;
            //decimal TotalRowCount2 = (TotalRowCount / 15);




            if (TotalRowCount > 10)
            {
                if (TotalRowCount > 15)
                {
                    FromRowId = 0; ToRowId = 16;
                }
                AddTableInvoice(writer, pdfDoc, OrderItemDT, boldTableFont, bodyFont, EmailFont, TabelHeaderBackGroundColor, FromRowId, ToRowId, IsTotalRow, _invoiceDataForPdfViewModel);
                //AddTermsConditions(writer, pdfDoc, bodyFont, titleFont, PODetailsDT);
                AddFooter(writer, pdfDoc, footerFont, footerFont2);
                pdfDoc.NewPage();

                if (TotalRowCount > 15)
                {
                    FromRowId = 15; ToRowId = 35;
                }
                else
                {
                    FromRowId = 10; ToRowId = 31;
                }

                IsTotalRow = true;
                AddHeader(writer, pdfDoc);
                AddBlankRow(writer, pdfDoc);
                AddTableInvoice(writer, pdfDoc, OrderItemDT, boldTableFont, bodyFont, EmailFont, TabelHeaderBackGroundColor, FromRowId, ToRowId, IsTotalRow, _invoiceDataForPdfViewModel);
                AddTermsConditions(writer, pdfDoc, bodyFont, titleFont, PODetailsDT, footerFont2, poFont);
                AddFooter(writer, pdfDoc, footerFont, footerFont2);

            }
            else
            {
                IsTotalRow = true;
                AddTableInvoice(writer, pdfDoc, OrderItemDT, boldTableFont, bodyFont, EmailFont, TabelHeaderBackGroundColor, FromRowId, ToRowId, IsTotalRow, _invoiceDataForPdfViewModel);
                AddTermsConditions(writer, pdfDoc, bodyFont, titleFont, PODetailsDT, footerFont2, poFont);
                AddFooter(writer, pdfDoc, footerFont, footerFont2);

            }

            pdfDoc.Close();



            // pdfDoc.Close();
            //DownloadFile(PDFData);
            //GetReport(PDFData);
            // PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("Sample-PDF-File.pdf"), FileMode.Create));
            //Export HTML String as PDF.
            // StringReader sr = new StringReader(cb.ToString());
            //Document pdfDoc1 = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            //Microsoft.AspNetCore.Http.HttpContext httpContext;
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter writer1 = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.ContentType = "application/pdf";
            //httpContext.Response.Headers ("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Write(pdfDoc);
            //Response.End();

            byte[] bytes = PDFData.ToArray();
            // System.IO.File.WriteAllBytes(@"D:\" + DateTime.Now.ToString("ddMMyyyyHHss") + ".pdf", bytes);

            //System.IO.File.WriteAllBytes(@"D:\test" + ".pdf", bytes);
            //PDFData.Close();
            // return File(bytes, "application/pdf");

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(bytes, "application/pdf");
            fileResult.FileDownloadName = "Invoice-" + DateTime.Now.ToString("ddMMyyyyHHss") + ".pdf";
            return fileResult;
        }

        private void AddTableInvoice(PdfWriter writer, Document document, DataTable OrderItemDT, Font boldTableFont, Font bodyFont, Font EmailFont, BaseColor TabelHeaderBackGroundColor, int FromRowId, int ToRowId, bool IsTotalRow, InvoiceDataForPdfViewModel _invoiceDataForPdfViewModel)
        {
           
            PdfPTable itemTable = new PdfPTable(8);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            //itemTable.PaddingTop = 150f;
            //itemTable.SetWidths(new float[] { 5, 40, 10, 20, 25 });  // then set the column's __relative__ widths
            itemTable.SetWidths(new float[] { 5, 40, 10, 10, 5, 10, 10, 10 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;

            PdfPCell cell1 = new PdfPCell(new Phrase("NO", boldTableFont));
            cell1.BackgroundColor = TabelHeaderBackGroundColor;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = 5;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("DESCRIPTION", boldTableFont));
            cell2.BackgroundColor = TabelHeaderBackGroundColor;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = 5;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("QUANTITY", boldTableFont));
            cell3.BackgroundColor = TabelHeaderBackGroundColor;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = 5;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("UNIT PRICE", boldTableFont));
            cell4.BackgroundColor = TabelHeaderBackGroundColor;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = 5;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("UOM", boldTableFont));
            cell5.BackgroundColor = TabelHeaderBackGroundColor;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = 5;
            itemTable.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase("AMOUNT", boldTableFont));
            cell6.BackgroundColor = TabelHeaderBackGroundColor;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            cell6.VerticalAlignment = 5;
            itemTable.AddCell(cell6);
            PdfPCell cell7 = new PdfPCell(new Phrase("GST AMOUNT(%)", boldTableFont));
            cell7.BackgroundColor = TabelHeaderBackGroundColor;
            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
            cell7.VerticalAlignment = 5;
            itemTable.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase("Total AMOUNT", boldTableFont));
            cell8.BackgroundColor = TabelHeaderBackGroundColor;
            cell8.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell8);



            foreach (DataRow row in OrderItemDT.Select().Where(e => Convert.ToInt32(e.ItemArray[0]) > FromRowId && Convert.ToInt32(e.ItemArray[0]) < ToRowId))
            {
                // RowId = RowId + 1;

                PdfPCell numberCell = new PdfPCell(new Phrase(row["RowId"].ToString(), bodyFont));
                numberCell.HorizontalAlignment = 1;
                numberCell.VerticalAlignment = 5;
                numberCell.PaddingLeft = 5f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                numberCell.FixedHeight = 18f;
                itemTable.AddCell(numberCell);

                var _phrase = new Phrase();
                _phrase.Add(new Chunk(row["SubProductName"].ToString(), bodyFont));

                //_phrase.Add(new Chunk("New Signup Subscription Plan\n", EmailFont));
                //_phrase.Add(new Chunk("Subscription Plan description will add here.", bodyFont));
                PdfPCell descCell = new PdfPCell(_phrase);
                descCell.HorizontalAlignment = Element.ALIGN_CENTER;
                descCell.VerticalAlignment = 5;
                descCell.PaddingLeft = 5f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                descCell.FixedHeight = 18f;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(row["Quantity"].ToString(), bodyFont));
                qtyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                qtyCell.VerticalAlignment = 5;
                qtyCell.PaddingLeft = 5f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                qtyCell.FixedHeight = 18f;
                itemTable.AddCell(qtyCell);

                PdfPCell unitPriceCell = new PdfPCell(new Phrase("₹" + row["UnitPrice"].ToString(), bodyFont));
                unitPriceCell.HorizontalAlignment = 1;
                unitPriceCell.VerticalAlignment = 5;
                unitPriceCell.PaddingLeft = 5f;
                unitPriceCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                unitPriceCell.FixedHeight = 18f;
                itemTable.AddCell(unitPriceCell);

                PdfPCell uomCell = new PdfPCell(new Phrase(row["UnitName"].ToString(), bodyFont));
                uomCell.HorizontalAlignment = 1;
                uomCell.VerticalAlignment = 5;
                uomCell.PaddingLeft = 5f;
                uomCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                uomCell.FixedHeight = 18f;
                itemTable.AddCell(uomCell);

                PdfPCell amountCell = new PdfPCell(new Phrase("₹" + row["Amount"].ToString(), bodyFont));
                amountCell.HorizontalAlignment = 1;
                amountCell.VerticalAlignment = 5;
                amountCell.PaddingLeft = 5f;
                amountCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                amountCell.FixedHeight = 18f;
                itemTable.AddCell(amountCell);

                PdfPCell gstamountCell = new PdfPCell(new Phrase("₹" + row["GSTAmount"].ToString() + "(" + row["GSTPercen"].ToString() + "%)", bodyFont));
                gstamountCell.HorizontalAlignment = 1;
                gstamountCell.VerticalAlignment = 5;
                gstamountCell.PaddingLeft = 5f;
                gstamountCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                gstamountCell.FixedHeight = 18f;
                itemTable.AddCell(gstamountCell);

                PdfPCell totalamtCell = new PdfPCell(new Phrase("₹" + row["TotalAmount"].ToString(), bodyFont));
                totalamtCell.HorizontalAlignment = 1;
                totalamtCell.VerticalAlignment = 5;
                totalamtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                totalamtCell.FixedHeight = 18f;
                itemTable.AddCell(totalamtCell);

            }
            //Sum Total Amount ..
            if (IsTotalRow == true)
            {
                //Decimal TotalPrice = Convert.ToDecimal(OrderItemDT.Compute("SUM(Convert.ToDecimal(TotalAmount))", "RowId > 0"));
                //Decimal total = OrderItemDT.AsEnumerable().Where(row => !DBNull.Value.Equals(row[7])).Sum(row => Convert.ToDecimal(row[7]));
                {
                    PdfPCell cell = new PdfPCell(new Phrase("Advance Payment (If any) : " + _invoiceDataForPdfViewModel.AdvancePaymentPercentage + " % of "+ _invoiceDataForPdfViewModel.Amount, bodyFont));
                    cell.Colspan = 7;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.FixedHeight = 18f;
                    //cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 5;
                    itemTable.AddCell(cell);

                    PdfPCell cell10 = new PdfPCell(new Phrase(_invoiceDataForPdfViewModel.AdvancePaymentAmount.ToString(), bodyFont));
                    cell10.Colspan = 1;
                    cell10.FixedHeight = 18f;
                    cell10.HorizontalAlignment = 1;
                    cell10.VerticalAlignment = 5;
                    itemTable.AddCell(cell10);

                }
                {
                    PdfPCell cell = new PdfPCell(new Phrase("Total Amount ", bodyFont));
                    cell.Colspan = 7;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.FixedHeight = 18f;
                    //cell.HorizontalAlignment = 1;
                    cell.VerticalAlignment = 5;
                    itemTable.AddCell(cell);

                    PdfPCell cell10 = new PdfPCell(new Phrase(_invoiceDataForPdfViewModel.TotalAmount.ToString(), bodyFont));
                    cell10.Colspan = 1;
                    cell10.FixedHeight = 18f;
                    cell10.HorizontalAlignment = 1;
                    cell10.VerticalAlignment = 5;
                    itemTable.AddCell(cell10);

                }

            }


            document.Add(itemTable);
        }


        [HttpGet]
        public IActionResult VendorQuotationReport()
        {

            List<QuatationToPaymentDetailsViewModel> _quatationToPaymentList = new List<QuatationToPaymentDetailsViewModel>();
            _quatationToPaymentList = _dataAccessLayer.GetVendorQuotationReportData(12, 1, 2).ToList();

            return View(_quatationToPaymentList);
        }

        [HttpGet]
        public IActionResult VendorQuotationReportDetails(int UserId)
        {

            List<QuatationToPaymentDetailsViewModel> _quatationToPaymentList = new List<QuatationToPaymentDetailsViewModel>();
            _quatationToPaymentList = _dataAccessLayer.GetQuatationToPaymentListData(13, 1, UserId).ToList();

            return View(_quatationToPaymentList);
        }



    }
}
