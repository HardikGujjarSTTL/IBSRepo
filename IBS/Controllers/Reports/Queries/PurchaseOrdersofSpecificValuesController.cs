using Humanizer;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Repositories;
using IBS.Repositories.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace IBS.Controllers.Reports.Queries
{
    public class PurchaseOrdersofSpecificValuesController : BaseController
    {
        private readonly IPurchaseOrdersofSpecificValuesRepository purchaseOrdersofSpecificValuesRepository;

        public PurchaseOrdersofSpecificValuesController(IPurchaseOrdersofSpecificValuesRepository _purchaseOrdersofSpecificValuesRepository)
        {
            purchaseOrdersofSpecificValuesRepository = _purchaseOrdersofSpecificValuesRepository;
        }

        #region Detailed Report - P O of Specific Values
        public IActionResult DetailedReport()
        {
            return View();
        }
        public IActionResult TableDetailed(string p_wAgency, DateTime p_frmDt, DateTime p_toDt, string p_SelCriteria, string p_wClient, string p_wFrmAmt, string p_wToAmt)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                string Regiontext = "";
                if (Region == "N")
                    Regiontext = "Northern Region";
                if (Region == "S")
                    Regiontext = "Southern Region";
                if (Region == "E")
                    Regiontext = "Eastern Region";
                if (Region == "W")
                    Regiontext = "Westrern Region";
                if (Region == "C")
                    Regiontext = "Central Region";
                if (Region == "Q")
                    Regiontext = "QA Corporate";

                string wPoValue = "";
                if (Convert.ToInt32(p_wFrmAmt) > 0)
                { wPoValue = "between " + p_wFrmAmt + " and " + p_wToAmt; }
                else
                { wPoValue = " less than or equal to " + p_wToAmt; }

                ViewBag.Region = Regiontext;
                ViewBag.Title = p_wClient + " (" + Regiontext + ") Purchase Orders of Value " + wPoValue + " for the Period : " + p_frmDt.ToString("dd/MM/yyy") + " TO " + p_toDt.ToString("dd/MM/yyy");
                List<PurchaseOrdersofSpecificValueModel> model = purchaseOrdersofSpecificValuesRepository.GetDataList(p_wAgency, p_frmDt, p_toDt, p_SelCriteria, p_wClient, p_wFrmAmt, p_wToAmt, Region);
                //return PartialView("_TableDetailed", model);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PurchaseOrdersofSpecificValues", "TableDetailed", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion

        #region Summary Report - P O of Specific Values
        public IActionResult SummaryReport()
        {
            return View();
        }

        public IActionResult TableSummary(string p_wAgency, DateTime p_frmDt, DateTime p_toDt, string p_SelCriteria, string p_wFrmAmt, string p_wToAmt)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                string Regiontext = "";
                if (Region == "N")
                    Regiontext = "Northern Region";
                if (Region == "S")
                    Regiontext = "Southern Region";
                if (Region == "E")
                    Regiontext = "Eastern Region";
                if (Region == "W")
                    Regiontext = "Westrern Region";
                if (Region == "C")
                    Regiontext = "Central Region";
                if (Region == "Q")
                    Regiontext = "QA Corporate";

                string wlAgency = "";

                if (p_wAgency == "R")
                {
                    wlAgency = "Railway";
                }
                else if (p_wAgency == "P")
                {
                    wlAgency = "Private";
                }
                else if (p_wAgency == "U")
                {
                    wlAgency = "PSU";
                }
                else if (p_wAgency == "F")
                {
                    wlAgency = "Foreign Railway";
                }
                else if (p_wAgency == "S")
                {
                    wlAgency = "State Government";
                }

                string wPoValue = "";
                if (Convert.ToInt32(p_wFrmAmt) > 0)
                { wPoValue = "between " + p_wFrmAmt + " and " + p_wToAmt; }
                else
                { wPoValue = " less than or equal to " + p_wToAmt; }

                ViewBag.Region = Regiontext;
                ViewBag.Title = wlAgency + " Purchase Orders of Value " + wPoValue + " for the Period : " + p_frmDt.ToString("dd/MM/yyy") + " TO " + p_toDt.ToString("dd/MM/yyy");
                List<PurchaseOrdersofSummaryModel> model = purchaseOrdersofSpecificValuesRepository.GetSummaryDataList(p_wAgency, p_frmDt, p_toDt, p_SelCriteria, p_wFrmAmt, p_wToAmt, Region);
                return View(model);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PurchaseOrdersofSpecificValues", "TableSummary", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        #endregion

        #region Item Wise Inspections
        public IActionResult ItemWiseInspectionsReport()
        {
            return View();
        }

        public IActionResult TableItemWiseInspections(string ItemDesc1, string ItemDesc2, string ItemDesc3, string ItemDesc4, string ItemDesc5, DateTime frmDt, DateTime toDt, string OneRegion)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                ItemWiseInspectionsParamModel model = new ItemWiseInspectionsParamModel();
                model.ItemDesc1 = ItemDesc1;
                model.ItemDesc2 = ItemDesc2;
                model.ItemDesc3 = ItemDesc3;
                model.ItemDesc4 = ItemDesc4;
                model.ItemDesc5 = ItemDesc5;
                model.OneRegion = OneRegion;
                model.Region = Region;
                model.frmDt = frmDt;
                model.toDt = toDt;
                List<InspectionDataModel> model1 = purchaseOrdersofSpecificValuesRepository.GetItemWiseInspectionsList(model);

                string html = "";
                string wRegion = "";
                if (Region == "N")
                    wRegion = "Northern Region";
                if (Region == "S")
                    wRegion = "Southern Region";
                if (Region == "E")
                    wRegion = "Eastern Region";
                if (Region == "W")
                    wRegion = "Westrern Region";
                if (Region == "C")
                    wRegion = "Central Region";
                if (Region == "Q")
                    wRegion = "QA Corporate";

                string items_searched = "";
                if (ItemDesc1 != null)
                    items_searched = ItemDesc1.Trim().ToUpper();
                if (ItemDesc2 != null)
                    items_searched = items_searched + ", " + ItemDesc2.Trim().ToUpper();
                if (ItemDesc3 != null)
                    items_searched = items_searched + ", " + ItemDesc3.Trim().ToUpper();
                if (ItemDesc4 != null)
                    items_searched = items_searched + ", " + ItemDesc4.Trim().ToUpper();
                if (ItemDesc5 != null)
                    items_searched = items_searched + ", " + ItemDesc5.Trim().ToUpper();

                items_searched = items_searched + ".";

                if (OneRegion != "true") { wRegion = "Northern,Western,Eastern & Southern Region"; }

                ViewBag.Region = wRegion;
                ViewBag.Title = "Item Wise Inspection Details For the Period : " + model.frmDt.ToString("dd / MM / yyy") + " to " + model.toDt.ToString("dd / MM / yyy");
                ViewBag.Searched = "Items Searched: " + items_searched;
                return View(model1);
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PurchaseOrdersofSpecificValues", "TableItemWiseInspections", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        #endregion

        #region Item Wise Inspections For Tender Queries
        public IActionResult ItemWiseInspectionsForTenderQueriesReport()
        {
            return View();
        }
        public IActionResult TableItemWiseInspectionsForTenderQueries(string ItemDesc1, string ItemDesc2, string ItemDesc3, string ItemDesc4, string ItemDesc5, DateTime frmDt, DateTime toDt, string OneRegion,string Client, string RCode)
        {
            try
            {
                string Region = SessionHelper.UserModelDTO.Region;
                ItemWiseInspectionsParamModel model = new ItemWiseInspectionsParamModel();
                model.ItemDesc1 = ItemDesc1;
                model.ItemDesc2 = ItemDesc2;
                model.ItemDesc3 = ItemDesc3;
                model.ItemDesc4 = ItemDesc4;
                model.ItemDesc5 = ItemDesc5;
                model.OneRegion = OneRegion;
                model.Region = Region;
                model.frmDt = frmDt;
                model.toDt = toDt;
                model.Client = Client;
                model.RCode = RCode;
                DataTable dt = purchaseOrdersofSpecificValuesRepository.GetItemWiseInspectionsForTenderQueriesList(model);

                string html = "";
                string wRegion = "";
                if (Region == "N")
                    wRegion = "Northern Region";
                if (Region == "S")
                    wRegion = "Southern Region";
                if (Region == "E")
                    wRegion = "Eastern Region";
                if (Region == "W")
                    wRegion = "Westrern Region";
                if (Region == "C")
                    wRegion = "Central Region";
                if (Region == "Q")
                    wRegion = "QA Corporate";

                
                string wClient = "", wIcPeriod = "";
                string pClient = "", pIcPeriod = "";
                double w_tot_VALUE = 0;
                int w_tot_NO_OF_INSP = 0;
                int wSno = 0;
                string items_searched = "";
                if (ItemDesc1 != null)
                    items_searched = ItemDesc1.Trim().ToUpper();
                if (ItemDesc2 != null)
                    items_searched = items_searched + ", " + ItemDesc2.Trim().ToUpper();
                if (ItemDesc3 != null)
                    items_searched = items_searched + ", " + ItemDesc3.Trim().ToUpper();
                if (ItemDesc4 != null)
                    items_searched = items_searched + ", " + ItemDesc4.Trim().ToUpper();
                if (ItemDesc5 != null)
                    items_searched = items_searched + ", " + ItemDesc5.Trim().ToUpper();

                items_searched = items_searched + ".";
                if (OneRegion == "true") { wRegion = "Northern,Western,Eastern & Southern Region"; }
                html+="<br><table border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse;' bordercolor='#111111' width='100%'>";
                html+="<tr><td width='100%' colspan='8'>";
                html+="<H5 align='center'><font face='Verdana'>" + wRegion + "</font><br></p>";
                html+="<H5 align='center'><font face='Verdana'>Client Wise Inspections For the Period : " + model.frmDt.ToString("dd/MM/yyy") + " to " + model.toDt.ToString("dd/MM/yyy") + "</font><br></p>";
                html+="</td>";
                html+="</tr>";
                html+="<tr><td width='100%' colspan='8'>";
                html+="<H5 align='center'><font face='Verdana'>Item(s) Searched: " + items_searched + " </font><br></p>";
                html+="</td>";
                html+="</tr>";
                html+="<tr>";
                html+="<th width='5%' valign='top'><b><font size='1' face='Verdana'>S.NO.</font></b></th>";
                html+="<th width='10%' valign='top'><b><font size='1' face='Verdana'>CLIENT</font></b></th>";
                html+="<th width='7%' valign='top'><b><font size='1' face='Verdana'>INSPECTION PERIOD</font></b></th>";
                html+="<th width='15%' valign='top'><b><font size='1' face='Verdana'>ITEM</font></b></th>";
                html+="<th width='5%' valign='top'><b><font size='1' face='Verdana'>QTY PASSED</font></b></th>";
                html+="<th width='5%' valign='top'><b><font size='1' face='Verdana'>QTY REJECTED</font></b></th>";
                html+="<th width='7%' valign='top'><b><font size='1' face='Verdana'>MATERIAL VALUE</font></b></th>";
                html+="<th width='7%' valign='top'><b><font size='1' face='Verdana'>NO OF INSPECTIONS</font></b></th>";
                html+="</tr></font>";
                var reader = dt.CreateDataReader();
                while (reader.Read())
                {
                    wSno = wSno + 1;
                    if (reader["BPO_RLY"].ToString() == wClient)
                    {
                        pClient = "";
                        if (reader["IC_PERIOD"].ToString() == wIcPeriod)
                        {
                            pIcPeriod = "";
                        }
                        else
                        {
                            html+="<tr>";
                            html+="<td width='5%' valign='top' align='center' colspan=6> <font size='1' face='Verdana'><b><font size='1' face='Verdana'>Total For The Inspection Period: " + wIcPeriod + "</font></b></td>";
                            html+="<td width='7%' valign='top' align='right'> <font size='1' face='Verdana'>"; html += w_tot_VALUE; html += "</td>";
                            html+="<td width='7%' valign='top' align='center'> <font size='1' face='Verdana'>"; html += w_tot_NO_OF_INSP; html += "</td>";
                            html+="</tr>";
                            w_tot_NO_OF_INSP = 0;
                            w_tot_VALUE = 0;
                            wIcPeriod = reader["IC_PERIOD"].ToString();
                            pIcPeriod = reader["IC_PERIOD"].ToString();
                        }
                    }
                    else
                    {
                        wClient = reader["BPO_RLY"].ToString();
                        wIcPeriod = reader["IC_PERIOD"].ToString();
                        pClient = reader["BPO_RLY"].ToString();
                        pIcPeriod = reader["IC_PERIOD"].ToString();
                    }
                    html+="<tr>";
                    html+="<td width='5%' valign='top' align='center'> <font size='1' face='Verdana'>" + wSno + "</td>";
                    html+="<td width='10%' valign='top' align='center'> <font size='1' face='Verdana'>"+ pClient +"</td>";
                    html+="<td width='7%' valign='top' align='center'> <font size='1' face='Verdana'>" + pIcPeriod + "</td>";
                    html+="<td width='15%' valign='top' align='left'> <font size='1' face='Verdana'>"+ reader["ITEM_DESC"].ToString()+"</td>";
                    html+="<td width='5%' valign='top' align='right'> <font size='1' face='Verdana'>"+ reader["QTY_PASS"].ToString()+" "+ reader["UOM_S_DESC"].ToString()+"</td>";
                    html+="<td width='5%' valign='top' align='right'> <font size='1' face='Verdana'>"+ reader["QTY_REJ"].ToString()+" "+ reader["UOM_S_DESC"].ToString()+"</td>";
                    html+="<td width='7%' valign='top' align='right'> <font size='1' face='Verdana'>"+ reader["VALUE"].ToString() +"</td>";
                    html+="<td width='7%' valign='top' align='center'> <font size='1' face='Verdana'>"+ reader["NO_OF_INSP"].ToString() +"</td>";
                    w_tot_VALUE = w_tot_VALUE + Convert.ToDouble(reader["VALUE"].ToString());
                    w_tot_NO_OF_INSP = w_tot_NO_OF_INSP + Convert.ToInt32(reader["NO_OF_INSP"].ToString());
                    html+="</tr>";
                };
                html+="<tr>";
                html+="<td width='5%' valign='top' align='center' colspan=6> <font size='1' face='Verdana'><b><font size='1' face='Verdana'>Total For The Inspection Period: " + wIcPeriod + "</font></b></td>";
                html+="<td width='7%' valign='top' align='right'> <font size='1' face='Verdana'>" + w_tot_VALUE + "</td>";
                html+="<td width='7%' valign='top' align='center'> <font size='1' face='Verdana'>" + w_tot_NO_OF_INSP +"</td>";
                html+="</tr>";
                html+="</table>";

                ViewBag.html = html;

                return View();
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "PurchaseOrdersofSpecificValues", "TableItemWiseInspectionsForTenderQueries", 1, GetIPAddress());
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        #endregion
    }
}
