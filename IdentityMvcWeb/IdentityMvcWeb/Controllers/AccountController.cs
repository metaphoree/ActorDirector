using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Repository.Identity;
/// <summary>
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Kloud21.DataModel.Identity;
using IdentityMvcWeb.App_Start;
using IdentityMvcWeb;
using DataModel.UnitOfWork;
using BusinessServices;
using BusinessEntities;
/// </summary>
namespace IdentityMvcWeb.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        UnitOfWork_Identity UnitOfWork_Identity = new UnitOfWork_Identity();
        //DashboardService _dashboardService;
        public AccountController()
        {
            // _dashboardService = new DashboardService();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                SessionVariableHandler.ReturnURL = returnUrl;
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
                //Add this to check if the email was confirmed.
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    ModelState.AddModelError("", "You need to confirm your email.");
                    return View(model);
                }
                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return View("Lockout");
                }
                else
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, change to shouldLockout: true
                    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            //   var currentUserId =Convert.ToInt64(User.Identity.GetUserId());
                            //IList<IdentityUserRole> userRole =UnitOfWork_Identity.UserRoleRepository.GetAllRolesByUserID(currentUserId);
                            // IQueryable<IdentityRole> roles = RoleManager.Roles;
                            //   IList<string> iroles = new List<string>();

                            //returnUrl = returnUrl.Trim();
                            if (string.IsNullOrEmpty(returnUrl) || returnUrl.Equals(""))
                            {
                                if (User.IsInRole(CustomRoles.Admin))
                                {
                                    return RedirectToAction("Index", "Admin");
                                }
                                else if (User.IsInRole(CustomRoles.Actor))
                                {
                                    return RedirectToAction("Index", "Actor");
                                }
                                else if (User.IsInRole(CustomRoles.Director))
                                {
                                    return RedirectToAction("Index", "Director");
                                }
                            }
                            else
                            {

                                return RedirectToLocal(returnUrl);

                            }
                            break;
                        case SignInStatus.LockedOut:
                            return View("Lockout");
                        case SignInStatus.RequiresVerification:
                            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "Invalid login attempt.");
                            return View(model);
                    }
                }
            }
            return View(model);
        }

        //[AllowAnonymous]
        //public ActionResult BackdoorLogin()
        //{

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    SignInManager.PasswordSignInAsync("showme777@hotmail.com", "W3lc@m3", true, shouldLockout: false);
        //    string currentUserId = User.Identity.GetUserId();
        //    IdentityUser currentUser = UserManager.FindByName("showme777@hotmail.com");
        //    DoctorService _doctorService = new DoctorService();

        //    if (UserManager.IsInRole(currentUser.Id, CustomRoles.Doctor))
        //    {
        //        DoctorDTO doctor = new DoctorDTO();
        //        doctor = _doctorService.GetDoctorByUserId(currentUser.Id);
        //        SessionVariableHandler.DoctorId = doctor.Id;
        //        IList<Medical_CenterDTO> MedicalCenterList = _doctorService.GetMedicalCenterList_By_DoctorId(SessionVariableHandler.DoctorId);

        //        if (doctor.StatusID_FK == 1)
        //        {
        //            if (MedicalCenterList != null)
        //            {
        //                if (MedicalCenterList.Count > 0)
        //                {
        //                    return RedirectToLocal("~/Doctor/Doctor/SelectMedicalCentre");
        //                }
        //                else
        //                {
        //                    var e = MedicalCenterList.FirstOrDefault();
        //                    SessionVariableHandler.ClinicId = e.Id;
        //                    return RedirectToLocal("~/Doctor/Dashboard/Dashboard");
        //                }
        //            }
        //            else {
        //                return RedirectToLocal("~/Doctor/Doctor/DoctorProfile?doctorid=" + SessionVariableHandler.DoctorId.ToString());
        //            }

        //        }
        //        else
        //        {
        //            return RedirectToLocal("~/Doctor/Doctor/DoctorProfile?doctorid=" + SessionVariableHandler.DoctorId.ToString());
        //        }


        //    }
        //    else
        //    {
        //        return RedirectToLocal("~/Doctor/Dashboard/Dashboard");
        //    }

        //}

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string message = "")
        {
            var model = new RegisterViewModel();
            var roleList = PopulateRoleList(RoleManager.Roles);
            var result = new List<SelectListItem> { new SelectListItem { Value = null, Text = "Select Role" } };
            result.AddRange(roleList);
            model.RoleList = result;
           // to display user why the registration fail
            if (!String.IsNullOrEmpty(message)) { model.Message = message; }

            return View(model);
        }

        public List<SelectListItem> PopulateRoleList(IQueryable<IdentityRole> identityRoles)
        {
            var x = identityRoles;
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var value in x)
            {
                list.Add(new SelectListItem
                {
                    Text = Convert.ToString(value.Name),
                    Value = Convert.ToString(value.Id)
                });
            }
            return list;
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {

                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user = await UserManager.FindByNameAsync(model.Email);
                    #region(Adding To Role)
                    string selectedRoleName = RoleManager.FindById(model.SelectedUserRoleId).Name;
                    UserManager.AddToRole(user.Id, selectedRoleName);
                    #endregion
                    #region(Add Profile)
                    #region(Add Profile)
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    //DoctorService doctorService = new DoctorService();
                    //if (1==1)//(selectedRoleName == CustomRoles.Doctor)
                    //{
                    //    DoctorDTO doctor = new DoctorDTO();
                    //    doctor.FirstName = "";
                    //    doctor.LastName = "";
                    //    doctor.Description = "";
                    //    doctor.DOB = new DateTime(1900, 1, 1);
                    //    doctor.LicenseNumber = "";
                    //    doctor.StatusID_FK = 0;
                    //    doctor.User_FK_Id = user.Id;
                    //    var docId = doctorService.AddDoctor(doctor);
                    //}
                    #endregion
                    MemberProfileService memberProfileService = new MemberProfileService();
                    MemberProfileDTO member = new MemberProfileDTO();
                    member.FirstName = "";
                    member.LastName = "";
                    member.HighestEducation = "";
                    member.HomeDistrict = "";
                    member.MobilePhone = "";
                    member.Experience = 0;
                    member.DOB = new DateTime(1900, 1, 1);
                    member.FK_FROM_IdentityUser = user.Id;
                    memberProfileService.AddMember(member);
                    #endregion

                    #region(Sending Email)
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action(
                       "ConfirmEmail", "Account",
                       new { userId = user.Id, code = code },
                       protocol: Request.Url.Scheme);

                    await UserManager.SendEmailAsync(user.Id,
                       "Confirm your account",
                       "Please confirm your account by clicking this link: <a href=\""
                                                       + callbackUrl + "\">link</a>");

                    //await UserManager.SendEmailAsync(20,
                    //    "New Doctor Registration",
                    //    "His Email is " + model.Email + ". Please confirm his account by clicking this link: <a href=\""
                    //                                    + callbackUrl + "\">link</a>   Go to <a href='https://amardoctors.com'>Amardoctors</a>");
                    #endregion

                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });

                }

                foreach (var item in result.Errors)
                {
                    message += item + " || ";
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register", new { message = message });
        }

        public ActionResult SendProfileRequestEmailToUser(string body, long userId)
        {
            string message = "";
            try
            {
                UserManager.SendEmail(userId, "Profile Fiil Up Request From AmarDoctors", body);
                message = "Successfully Sent";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return RedirectToAction("SuccessMesssage", "Admin", new { area = "Admin", message = message });
        }
        //public static string StaticMessage { get; set; }
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email; return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(long userId, string code)
        {
            if (userId == default(long) || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (!(user == null) && (await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");

                    // Don't reveal that the user does not exist or is not confirmed
                    //return View("ForgotPasswordConfirmation");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == default(long))
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("DoctorSearch", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion









    }



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