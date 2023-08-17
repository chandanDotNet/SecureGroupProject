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

namespace SecureGroup.Controllers 
{
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
                        _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(2,PurchaseOrderId, item);
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
            List<PurchaseOrderViewModel> _purchaseOrderList = new List<PurchaseOrderViewModel>();
            _purchaseOrderList = _dataAccessLayer.GetPurchaseOrderListData(3, 0, UserId).ToList();

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

        public IActionResult _PurchaseItemPartial(int PurchaseOrderDetailsId,int PurchaseOrderId)
        {

            PurchaseOrderDetailsViewModel viewModel = new PurchaseOrderDetailsViewModel();         
            if(PurchaseOrderDetailsId>0)
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
            if(PurchaseOrderDetailsId > 0)
            {
                Response= _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(6, PurchaseOrderId, viewModel);
            }
            else
            {
                Response= _dataAccessLayer.AddUpdatePurchaseOrderProductItemData(2, PurchaseOrderId, viewModel);
            }

            if (Response > 0)
            {
                TempData["successmessage"] = "Your data has been saved successfully";
                return RedirectToAction(nameof(EditPurchaseOrder), new {id= PurchaseOrderId });
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

        public IActionResult DeletePurchaseOrder( int PurchaseOrderId)
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


        public DataTable GetPurchaseOrderItemList()
        {
            DataTable result = new DataTable();
            List<PurchaseOrderDetailsViewModel> _purchaseOrderItemList = new List<PurchaseOrderDetailsViewModel>();
            int PurchaseOrderId = 1;
            _purchaseOrderItemList = _dataAccessLayer.GetPurchaseOrderItemListData(4, PurchaseOrderId,0, 0).ToList();
            result= ToDataTable(_purchaseOrderItemList);
            return result;
        }
        public DataTable GetPurchaseOrderDetailsData()
        {
            DataTable result = new DataTable();
            List<PurchaseOrderViewModel> _purchaseOrderList = new List<PurchaseOrderViewModel>();
            int PurchaseOrderId = 1;
            _purchaseOrderList = _dataAccessLayer.GetPurchaseOrderListData(8, PurchaseOrderId, 0).ToList();
            result = ToDataTable(_purchaseOrderList);
            return result;
        }

        public void GenerateInvoice(string TransactionID)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

            //**************************
            DataTable PODetailsDT = new DataTable();
            PODetailsDT = GetPurchaseOrderDetailsData();
            DataTable OrderItemDT = new DataTable();
            OrderItemDT = GetPurchaseOrderItemList();

           //************************
            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
            var titleFontWhite = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.WHITE);
            var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD, BaseColor.WHITE);
            var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
           // BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");
            BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#3a4e86");
            BaseColor headerBackColor = WebColors.GetRGBColor("#3a4e86");
            Font fontRupee = FontFactory.GetFont("Arial", "₹", true, 12);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1257, true);
            
            var EmailFont1 =FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            #region Top table
            //New Changes
            PdfPTable headertable = new PdfPTable(1);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
           // headertable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing   

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("wwwroot/images/header.png");
            logo.ScaleToFit(600f,200f);
            logo.SetAbsolutePosition(0f, 0f);
            {
                PdfPCell pdfCelllogo = new PdfPCell(logo);               
                pdfCelllogo.Border = Rectangle.NO_BORDER;
                pdfCelllogo.PaddingLeft = -12f;
                //pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //pdfCelllogo.BorderWidthBottom = 1f;
                headertable.AddCell(pdfCelllogo);
            }

            

            // Create the header table 
            //PdfPTable headertable = new PdfPTable(3);
            //headertable.HorizontalAlignment = 0;
            //headertable.WidthPercentage = 100;
            //headertable.SetWidths(new float[] { 100f, 320f, 100f });  // then set the column's __relative__ widths
            //headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            ////headertable.DefaultCell.Border = Rectangle.BOX; //for testing           

            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("wwwroot/images/logo2.png");
            //logo.ScaleToFit(100, 15);

            //{
            //    PdfPCell pdfCelllogo = new PdfPCell(logo);
            //    pdfCelllogo.Border = Rectangle.NO_BORDER;
            //    pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //    pdfCelllogo.BorderWidthBottom = 1f;
            //    headertable.AddCell(pdfCelllogo);
            //}

            //{
            //    PdfPCell middlecell = new PdfPCell();
            //    middlecell.Border = Rectangle.NO_BORDER;
            //    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //    middlecell.BorderWidthBottom = 1f;
            //    headertable.AddCell(middlecell);
            //}

            //{
            //    PdfPTable nested = new PdfPTable(1);
            //    nested.DefaultCell.Border = Rectangle.NO_BORDER;
            //    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Company Name", titleFont));
            //    nextPostCell1.Border = Rectangle.NO_BORDER;
            //    nested.AddCell(nextPostCell1);
            //    PdfPCell nextPostCell2 = new PdfPCell(new Phrase("xxx City Heights, AZ 8xxx4, US,", bodyFont));
            //    nextPostCell2.Border = Rectangle.NO_BORDER;
            //    nested.AddCell(nextPostCell2);
            //    PdfPCell nextPostCell3 = new PdfPCell(new Phrase("(xxx) xxx-xxx", bodyFont));
            //    nextPostCell3.Border = Rectangle.NO_BORDER;
            //    nested.AddCell(nextPostCell3);
            //    PdfPCell nextPostCell4 = new PdfPCell(new Phrase("company@example.com", EmailFont));
            //    nextPostCell4.Border = Rectangle.NO_BORDER;
            //    nested.AddCell(nextPostCell4);
            //    nested.AddCell("");
            //    PdfPCell nesthousing = new PdfPCell(nested);
            //    nesthousing.Border = Rectangle.NO_BORDER;
            //    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            //    nesthousing.BorderWidthBottom = 1f;
            //    nesthousing.Rowspan = 5;
            //    nesthousing.PaddingBottom = 10f;
            //    headertable.AddCell(nesthousing);
            //}


            PdfPTable Invoicetable = new PdfPTable(3);
            Invoicetable.HorizontalAlignment = 0;
            Invoicetable.WidthPercentage = 100;
            Invoicetable.SetWidths(new float[] { 160f, 120f, 180f });  // then set the column's __relative__ widths
            Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                nested.AddCell(" ");
                //PdfPCell blankCell = new PdfPCell(new Phrase(" "));
                //blankCell.Border = Rectangle.NO_BORDER;                
                //nested.AddCell(blankCell);
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE TO:", titleFontWhite));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nextPostCell1.BackgroundColor=headerBackColor; 
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
                PdfPCell nextPostCell5 = new PdfPCell(new Phrase("GST NO. "+PODetailsDT.Rows[0]["VendorGSTNo"].ToString(), bodyFont));
                nextPostCell5.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell5);
                PdfPCell nextPostCell6 = new PdfPCell(new Phrase("MOB - " + PODetailsDT.Rows[0]["VendorMobileNo"].ToString(), bodyFont));
                nextPostCell6.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell6);
                PdfPCell nextPostCell7 = new PdfPCell(new Phrase("VENDOR CODE - ", EmailFont));
                nextPostCell7.Border = Rectangle.BOTTOM_BORDER;
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
                //PdfPTable nested = new PdfPTable(1);
                //nested.DefaultCell.Border = Rectangle.NO_BORDER;
                //PdfPCell nextPostCell1 = new PdfPCell(new Phrase("INVOICE 3-2-1", titleFontBlue));
                //nextPostCell1.Border = Rectangle.NO_BORDER;
                //nested.AddCell(nextPostCell1);
                //PdfPCell nextPostCell2 = new PdfPCell(new Phrase("Date of Invoice: " + DateTime.Now.ToShortDateString(), bodyFont));
                //nextPostCell2.Border = Rectangle.NO_BORDER;
                //nested.AddCell(nextPostCell2);
                //PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Due Date: " + DateTime.Now.AddDays(30).ToShortDateString(), bodyFont));
                //nextPostCell3.Border = Rectangle.NO_BORDER;
                //nested.AddCell(nextPostCell3);
                //nested.AddCell("");

                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                nested.AddCell(" ");
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("SHIP TO:", titleFontWhite));
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
                PdfPCell nextPostCell10 = new PdfPCell(new Phrase("Bill TO:", titleFontWhite));
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


            pdfDoc.Add(headertable);
            Invoicetable.PaddingTop = 10f;

            pdfDoc.Add(Invoicetable);
            #endregion

            #region Items Table
            //Create body table
           
            PdfPTable itemTable = new PdfPTable(8);

            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            //itemTable.SetWidths(new float[] { 5, 40, 10, 20, 25 });  // then set the column's __relative__ widths
            itemTable.SetWidths(new float[] { 5, 40, 10, 10, 5,10,10,10 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("NO", boldTableFont));
            cell1.BackgroundColor = TabelHeaderBackGroundColor;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("DESCRIPTION", boldTableFont));
            cell2.BackgroundColor = TabelHeaderBackGroundColor;
            //cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("QUANTITY", boldTableFont));
            cell3.BackgroundColor = TabelHeaderBackGroundColor;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("UNIT PRICE", boldTableFont));
            cell4.BackgroundColor = TabelHeaderBackGroundColor;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("UOM", boldTableFont));
            cell5.BackgroundColor = TabelHeaderBackGroundColor;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell5);
            PdfPCell cell6 = new PdfPCell(new Phrase("AMOUNT", boldTableFont));
            cell6.BackgroundColor = TabelHeaderBackGroundColor;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell6);
            PdfPCell cell7 = new PdfPCell(new Phrase("GST AMOUNT(%)", boldTableFont));
            cell7.BackgroundColor = TabelHeaderBackGroundColor;
            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell7);
            PdfPCell cell8 = new PdfPCell(new Phrase("Total AMOUNT", boldTableFont));
            cell8.BackgroundColor = TabelHeaderBackGroundColor;
            cell8.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell8);
            foreach (DataRow row in OrderItemDT.Rows)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase("1", bodyFont));
                numberCell.HorizontalAlignment = 1;
                numberCell.PaddingLeft = 5f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(numberCell);

                var _phrase = new Phrase();
                _phrase.Add(new Chunk(row["SubProductName"].ToString(), EmailFont));
                //_phrase.Add(new Chunk("New Signup Subscription Plan\n", EmailFont));
                //_phrase.Add(new Chunk("Subscription Plan description will add here.", bodyFont));
                PdfPCell descCell = new PdfPCell(_phrase);
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 5f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(row["Quantity"].ToString(), bodyFont));
                qtyCell.HorizontalAlignment = 1;
                qtyCell.PaddingLeft = 5f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(qtyCell);

                PdfPCell unitPriceCell = new PdfPCell(new Phrase("₹"+row["UnitPrice"].ToString(), bodyFont));
                unitPriceCell.HorizontalAlignment = 1;
                unitPriceCell.PaddingLeft = 5f;
                unitPriceCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(unitPriceCell);

                PdfPCell uomCell = new PdfPCell(new Phrase(row["UnitName"].ToString(), bodyFont));
                uomCell.HorizontalAlignment = 1;
                uomCell.PaddingLeft = 5f;
                uomCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(uomCell);

                PdfPCell amountCell = new PdfPCell(new Phrase("₹" + row["Amount"].ToString(), bodyFont));
                amountCell.HorizontalAlignment = 1;
                amountCell.PaddingLeft = 5f;
                amountCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(amountCell);

                PdfPCell gstamountCell = new PdfPCell(new Phrase("₹" + row["GSTAmount"].ToString()+"("+ row["GSTPercen"].ToString()+"%)", bodyFont));
                gstamountCell.HorizontalAlignment = 1;
                gstamountCell.PaddingLeft = 5f;
                gstamountCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(gstamountCell);

                PdfPCell totalamtCell = new PdfPCell(new Phrase("₹" + row["TotalAmount"].ToString(), bodyFont));
                totalamtCell.HorizontalAlignment = 1;
                totalamtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                itemTable.AddCell(totalamtCell);

            }
            // Table footer
            PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
            totalAmtCell1.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell1);
            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtCell3 = new PdfPCell(new Phrase(""));
            totalAmtCell3.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell3);
            PdfPCell totalAmtCell4 = new PdfPCell(new Phrase(""));
            totalAmtCell4.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell4);
            PdfPCell totalAmtCell5 = new PdfPCell(new Phrase(""));
            totalAmtCell5.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell5);
            PdfPCell totalAmtCell6 = new PdfPCell(new Phrase(""));
            totalAmtCell6.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell6);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Total Amount", boldTableFont));
            totalAmtStrCell.Border = Rectangle.TOP_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);

            

            PdfPCell totalAmtCell = new PdfPCell(new Phrase("₹"  ,bodyFont));
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);

            PdfPCell cell = new PdfPCell(new Phrase("***NOTICE: A finance charge of 1.5% will be made on unpaid balances after 30 days. ***", bodyFont));
            cell.Colspan = 8;
            cell.HorizontalAlignment = 1;
            itemTable.AddCell(cell);
            pdfDoc.Add(itemTable);
            #endregion

            //******************* Footer
            PdfPTable footertable = new PdfPTable(1);
            footertable.HorizontalAlignment = 0;
            footertable.WidthPercentage = 100;            
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
            //pdfDoc.Add(footertable);




            PdfContentByte cb = new PdfContentByte(writer);


            //*****************
            //PdfTemplate tmpFooter = cb.CreateTemplate(580, 70);
            //// Move to the bottom left corner of the template
            //tmpFooter.MoveTo(1, 1);
            //// Place the footer content
            //tmpFooter.Stroke();
            //// Begin writing the footer
            //tmpFooter.BeginText();
            //// Set the font and size
            //tmpFooter.SetFontAndSize(bf, 8);
            //// Write out details from the payee table
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 0, 53, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 0, 45, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 0, 37, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 0, 29, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString() + " " + PODetailsDT.Rows[0]["BillTo"].ToString() + " " + PODetailsDT.Rows[0]["BillTo"].ToString(), 0, 21, 0);
            //// Bold text for ther headers
            //tmpFooter.SetFontAndSize(bf, 8);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Phone", 215, 53, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mail", 215, 45, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Web", 215, 37, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Legal info", 400, 53, 0);
            //// Regular text for infomation fields
            //tmpFooter.SetFontAndSize(bf,8);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 265, 53, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 265, 45, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 265, 37, 0);
            //tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["BillTo"].ToString(), 400, 45, 0);
            //// End text
           // tmpFooter.EndText();
            //// Stamp a line above the page footer
            //cb.SetLineWidth(0f);
            //cb.MoveTo(30, 60);
            //cb.LineTo(570, 60);
            //cb.Stroke();

            //***********

            //cb = new PdfContentByte(writer);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetColorStroke(TabelHeaderBackGroundColor);
            cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            cb.AddImage(footertablelogo);
           // cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "aaaaaaaaaaaaaaaaa ", 50, 50, 0);
           // cb.ShowText("Invoice was created on a computer and is valid without the signature and seal. ");
           // cb.ShowText("Invoice was created on a computer and is valid without the signature and seal. ");
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, PODetailsDT.Rows[0]["VendorAddress"].ToString(), 5, 10, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,  " Phone- 9876543210", 480, 10, 0);
            cb.SetLineWidth(19);
            //cb.MoveTo(0, 0);
            cb.EndText();
            
            //


            //Move the pointer and draw line to separate footer section from rest of page
            //cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            //cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            cb.Stroke();

            pdfDoc.Close();
            DownloadFile(PDFData);
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

        public IActionResult DownloadFile(System.IO.MemoryStream PDFData)
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
            System.IO.File.WriteAllBytes(@"D:\test"  + ".pdf", bytes);
            PDFData.Close();

            return null;

        }

        public IActionResult Abc()
        {
            string TransactionID = "1234";
            GenerateInvoice(TransactionID);

            return View();
        }

        }
    }
