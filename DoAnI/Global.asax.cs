using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace DoAnI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application["LuongTruyCap"] = 0;
            Application["LuongTruyCapOnline"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock(); //dùng để đồng bộ hóa
            Application["LuongTruyCap"] = (int)Application["LuongTruyCap"] + 1;
            Application["LuongTruyCapOnline"] = (int)Application["LuongTruyCapOnline"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();//dùng để đồng bộ hóa
            Application["LuongTruyCapOnline"] = (int)Application["LuongTruyCapOnline"] - 1;
            Application.UnLock();
        }
        protected void Application_End()
        {
            Application.Lock();//dùng để đồng bộ hóa
            Application["LuongTruyCapOnline"] = (int)Application["LuongTruyCapOnline"] - 1;
            Application.UnLock();
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var TaiKhoanCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (TaiKhoanCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(TaiKhoanCookie.Value);
                var Quyen = authTicket.UserData.Split(new char[] { ',' });
                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), Quyen);
                Context.User = userPrincipal;
            }
        }
    }
}
