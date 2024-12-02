//using DevExpress.XtraReports.UI;
//using System.Data;

//namespace ApiDoctorCare.Services
//{
//    public class ReportPrint
//    {
//        public static XtraReport PrintReport(object datasource, string repx)
//        {
//            XtraReport report = XtraReport.FromFile(repx, true);
//            if (datasource != null)
//            {
//                report.DataSource = datasource;
//            }
//            if (datasource is DataSet)
//            {
//                DataSet ds = datasource as DataSet;
//                if (ds.Tables.Count > 0)
//                {
//                    report.DataMember = ds.Tables[0].TableName;
//                }
//            }
//            report.CreateDocument();
//            return report;
//        }
//    }
//}
