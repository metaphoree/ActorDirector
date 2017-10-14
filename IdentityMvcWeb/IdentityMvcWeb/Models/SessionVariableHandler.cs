using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityMvcWeb
{
    public class SessionVariableHandler
    {
        #region "Private Constants"
        private static string _memberId = "MemberId";
        private static string _clinicId = "ClinicId";
        private static string _doctorName = "DoctorName";
        private static string _clinicName = "ClinicName";
        private static string _returnUrl = "ReturnURL";
        #endregion

        #region "Public Properties"
        //<summary>
        //    ZoneID is used to store the current selection for dispatched Zone.
        //    We set this varibale DispatchGrid.aspx and used in different pages ConfirmDispatchGrid.aspx, DataGrid.aspx, ConfirmDispatchGrid.aspx
        //</summary>
        public static long MemberId
        {
            get
            {
                // Check for null first
                if ((HttpContext.Current.Session[_memberId] == null))
                {
                    // Return an empty string if session variable is null
                    //return 0;
                    // HttpContext.Current.Response.Redirect("/Account/Login", true);
                    return 0;
                }
                else
                {
                    return Convert.ToInt64(HttpContext.Current.Session[_memberId]);
                }
            }
            set
            {
                HttpContext.Current.Session[_memberId] = Convert.ToString(value);
            }
        }
        public static long ClinicId
        {
            get
            {
                // Check for null first
                if ((HttpContext.Current.Session[_clinicId] == null))
                {
                    // Return an empty string if session variable is null
                    //return 0;
                    // HttpContext.Current.Response.Redirect("/Account/Login", true);
                    return 0;
                }
                else
                {

                    return Convert.ToInt64(HttpContext.Current.Session[_clinicId]);
                }
            }
            set
            {
                HttpContext.Current.Session[_clinicId] = Convert.ToString(value);
            }
        }
        public static string ClinicName
        {
            get
            {
                // Check for null first
                if ((HttpContext.Current.Session[_clinicName] == null))
                {
                    return "";
                }
                else
                {

                    return HttpContext.Current.Session[_clinicName].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session[_clinicName] = Convert.ToString(value);
            }
        }
        public static string DoctorName
        {
            get
            {
                // Check for null first
                if ((HttpContext.Current.Session[_doctorName] == null))
                {
                    return "";
                }
                else
                {

                    return HttpContext.Current.Session[_doctorName].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session[_doctorName] = Convert.ToString(value);
            }
        }
        public static string ReturnURL
        {
            get
            {
                // Check for null first
                if ((HttpContext.Current.Session[_returnUrl] == null))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Session[_returnUrl].ToString();
                }
            }
            set
            {
                HttpContext.Current.Session[_returnUrl] = Convert.ToString(value);
            }
        }


        #endregion










    }
}