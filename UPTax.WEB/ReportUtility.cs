using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Reflection;
using System.Web;

namespace UPTax.WEB
{
    public static class ReportUtility
    {
        public static byte[] RenderedReportViewer(LocalReport reportViewer, string reportType, out string mimeType)
        {
            try
            {
                Warning[] warnings;

                string[] streamids;

                string encoding;

                string extension;

                string deviceInfo;

                string contentType;

                var rType = reportType.ToUpper();

                switch (rType)
                {
                    case "PDF":
                        deviceInfo =
                            "<DeviceInfo>" +
                            " <OutputFormat>PDF</OutputFormat>" +

                            //" <PageWidth>8.5in</PageWidth>" +

                            //" <PageHeight>11in</PageHeight>" +

                            //" <MarginTop>1in</MarginTop>" +

                            //" <MarginLeft>1in</MarginLeft>" +

                            //" <MarginRight>1in</MarginRight>" +

                            //" <MarginBottom>1in</MarginBottom>" +

                            "</DeviceInfo>";
                        contentType = "application/pdf";
                        break;

                    case "EXCEL":

                        deviceInfo =

                            "<DeviceInfo>" +

                            " <SimplePageHeaders>False</SimplePageHeaders>" +

                            "</DeviceInfo>";

                        contentType = "application/vnd.ms-excel";
                        extension = ".xlsx";

                        break;

                    default:

                        deviceInfo =

                            "<DeviceInfo>" +

                            " <OutputFormat>" + reportType + "</OutputFormat>" +

                            //" <PageWidth>8.5in</PageWidth>" +

                            //" <PageHeight>11in</PageHeight>" +

                            //" <MarginTop>1in</MarginTop>" +

                            //" <MarginLeft>1in</MarginLeft>" +

                            //" <MarginRight>1in</MarginRight>" +

                            //" <MarginBottom>1in</MarginBottom>" +

                            "</DeviceInfo>";

                        contentType = "application/msword";

                        break;

                }
                byte[] bytes = reportViewer.Render(reportType, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

                HttpContext.Current.Response.Clear();

                HttpContext.Current.Response.ClearHeaders();

                HttpContext.Current.Response.ContentType = contentType;

                if (rType == "EXCEL") HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + reportType + ".xlsx");
                if (rType == "WORD") HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + reportType + ".doc");
                //if (rType == "PDF") HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=" + reportType + ".pdf");

                HttpContext.Current.Response.BinaryWrite(bytes);

                HttpContext.Current.Response.End();
                return bytes;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                mimeType = "application/pdf";
                byte[] reBytes = new byte[1];
                return reBytes;
            }
        }
        public static DataTable ToDataTable<TSource>(this TSource data)
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            var props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ??
                                                 prop.PropertyType);
            }

            var values = new object[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(data, null);
            }
            dataTable.Rows.Add(values);

            return dataTable;
        }
    }
}