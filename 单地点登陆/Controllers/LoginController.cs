
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace 单地点登陆.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginStart()
        {
            return View();
        }

        public ActionResult LoginTwo()
        {
            return View();
        }

        [HttpGet]
        public string FillWrite()
        {
            string str = "佀同欢\r\n刘廷伟";
            string result1 = @"E:\日志文件"+DateTime.Now.ToString("yyyyMMddHHmmss")+".txt";//结果保存到E盘
            FileStream fs = new FileStream(result1, FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine(str);
            wr.Close();
            return "成功";
        }

        [HttpPost]
        public string LoginDeng(string userId)
        {
            try {
                HttpContext httpContext = System.Web.HttpContext.Current;
                var userOnline = (Hashtable)httpContext.Application["Online"];
                if (userOnline != null)
                {
                    IDictionaryEnumerator enumerator = userOnline.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Value != null && enumerator.Value.ToString().Equals(userId.ToString()))
                        {
                            userOnline[enumerator.Key.ToString()] = "_offline_";
                            break;
                        }
                    }
                }
                else
                {
                    userOnline = new Hashtable();
                }
                userOnline[Session.SessionID] = userId.ToString();
                httpContext.Application.Lock();
                httpContext.Application["Online"] = userOnline;
                httpContext.Application.UnLock();
                return "登录成功";
            }
            catch (Exception ex) {
                return ex.ToString();
            }

        }

        [HttpPost]
        public string CheckIsForcedLogout()
        {
            try
            {
                HttpContext httpContext = System.Web.HttpContext.Current;
                Hashtable userOnline = (Hashtable)httpContext.Application["Online"];
                if (userOnline != null)
                {
                    if (userOnline.ContainsKey(httpContext.Session.SessionID))
                    {
                        var value = userOnline[httpContext.Session.SessionID];
                        //判断当前session保存的值是否为被注销值
                        if (value != null && "_offline_".Equals(value))
                        {
                            //验证被注销则清空session
                            userOnline.Remove(httpContext.Session.SessionID);
                            httpContext.Application.Lock();
                            httpContext.Application["online"] = userOnline;
                            httpContext.Application.UnLock();

                            string msg = "下线通知：当前账号另一地点登录， 您被迫下线。若非本人操作，您的登录密码很可能已经泄露，请及时改密。";

                            //登出，清除cookie
                            FormsAuthentication.SignOut();

                            //return Json(new { OperateResult = "Success", OperateData = msg }, JsonRequestBehavior.AllowGet);
                            return msg;
                        }
                    }
                }
                //return Json(new { OperateResult = "Failed" }, JsonRequestBehavior.AllowGet);
                return "Failed";
            }
            catch (Exception ex)
            {
                //return Json(new { OperateResult = "Failed" }, JsonRequestBehavior.AllowGet);
                return ex.ToString();
            }
        }
    }
}