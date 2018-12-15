using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSS.Areas.CaiWuGuanLi.Models;
using System.Web.Script.Serialization;
using System.Data.Entity;

namespace PSS.Areas.CaiWuGuanLi.Controllers
{
    public class BankController : Controller
    {
        // GET: CaiWuGuanLi/Bank
        public ActionResult Bank()
        {
            return View();
        }

        public ActionResult BindData(string name, int type=-1, int limit=0,int page=0)
        {
            MyEF ef = new MyEF();
            bool c = Convert.ToBoolean(type);
            var dr = (from p in ef.Bank select new
            {
                BankID =p.BankID,
                BankName = p.BankName,
                OpenAccount = p.OpenAccount,
                OpenBank = p.OpenBank,
                Account =p.Account,
                BankMoney =p.BankMoney,
                IsStop =p.IsStop
            }).ToList();
            if (!string.IsNullOrEmpty(name))
            {
                dr = dr.Where(p => p.BankName.Contains(name)).ToList();
            }
            if (type != -1)
            {
                dr = dr.Where(p => p.IsStop == c).ToList();
            }
            var list = dr.Skip(limit * (page - 1)).Take(limit).ToList();
            return Json(ListToDic(list, dr.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        public int AddBank(Bank newbank)
        {
            MyEF ef = new MyEF();
            ef.Bank.Add(newbank);
            return ef.SaveChanges();
           
        }

        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// 修改传值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetBankData(int id)
        {
            MyEF ef = new MyEF();
            var list = (from p in ef.Bank where p.BankID == id select p).First();
            return Json(list);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            MyEF ef = new MyEF();
            var list = (from p in ef.Bank where p.BankID == id select p).SingleOrDefault();
            ef.Bank.Remove(list);
            int i= ef.SaveChanges();
            return Json(i);
        }
        public int Update(Bank newbank)
        {
            MyEF ef = new MyEF();
            ef.Entry(newbank).State = EntityState.Modified;
            int i = ef.SaveChanges();
            return i ;
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

    }
}