using PSS.Areas.CaiWuGuanLi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSS.Areas.CaiWuGuanLi.Controllers
{
    public class AccessController : Controller
    {
        // GET: CaiWuGuanLi/Access
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BindData(string type,string cuntype, DateTime? date, DateTime? dateend, int limit, int page)
        {
            MyEF ef = new MyEF();
            var dr = (from p in ef.Access
                      join bank in ef.Bank on p.BankID equals bank.BankID
                      join user in ef.Users on p.UsersID equals user.UsersID
                      join ep in ef.Employee on p.Eid equals ep.Eid
                      where 1 > 0 && (!string.IsNullOrEmpty(date.ToString()) ? (p.AccessDate >= date) : true)
                      && (!string.IsNullOrEmpty(dateend.ToString()) ? (p.AccessDate <= dateend) : true)
                      select new
                      {
                          AccessID = p.AccessID,
                          AccessDate = p.AccessDate,
                          AccessMoeny = p.AccessMoeny,
                          AccessRemark = p.AccessRemark,
                          AccessState=p.AccessState,
                          AccessType = p.AccessType,
                          BankName = bank.BankName,
                          BankID = p.BankID,
                          UsersName = user.UsersName,
                          EName = ep.EName,


                      }).ToList();

            if (!string.IsNullOrEmpty(type))
            {
                dr = dr.Where(p => p.AccessType.Contains(type)).ToList();
            }
            if (!string.IsNullOrEmpty(cuntype))
            {
                dr = dr.Where(p => p.AccessState == cuntype).ToList();
            }
            //if (!string.IsNullOrEmpty(date.ToString()) && !string.IsNullOrEmpty(dateend.ToString()))
            //{
            //    dr = dr.Where(p => p.IncomeDate >= date)
            //}
            var list = dr.OrderByDescending(p => p.AccessID).Skip(limit * (page - 1)).Take(limit).ToList();
            return Json(ListToDic(list, dr.Count()), JsonRequestBehavior.AllowGet);
        }






        public static Dictionary<string, object> ListToDic<T>(List<T> list, int count) where T : class
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", "0");
            dic.Add("msg", "");
            dic.Add("count", count);
            dic.Add("data", list);
            return dic;
        }


        /// <summary>
        /// 得到当天最大的单号值
        /// </summary>
        /// <returns></returns>
        public string GetMaxDHtoday()
        {
            MyEF ef = new MyEF();
            string Orderno = null;
            string maxOrderno = ((from t in ef.Income orderby t.IncomeID descending select t.IncomeID).FirstOrDefault()).ToString();//得到最大的单号
            string nowdate = DateTime.Today.ToString("yyyyMMdd");
            string uid_pfix = "SR" + nowdate;
            if (maxOrderno != null && maxOrderno.Contains(uid_pfix))
            {
                string uid_end = maxOrderno.Substring(9); // 截取字符串最后四位，结果:0001
                int endNum = Convert.ToInt32(uid_end); // 把string类型的0001转化为int类型的1
                int tmpNum = 10000 + endNum + 1; // 结果10002
                Orderno = uid_pfix + subStr("" + tmpNum, 1);// 把10002首位的1去掉，再拼成NO201601260002字符串
            }
            else
            {
                Orderno = uid_pfix + "0001";
            }
            return Orderno;
        }

        //合成单号
        public string subStr(string str, int start)
        {
            if (str == null || str.Equals("") || str.Length == 0)
                return "";
            if (start < str.Length)
            {
                return str.Substring(start);
            }
            else
            {
                return "";
            }

        }
    }



}