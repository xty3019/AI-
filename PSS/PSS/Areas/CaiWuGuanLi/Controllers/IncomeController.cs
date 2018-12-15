using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSS.Areas.CaiWuGuanLi.Models;
using System.Data.Entity;

namespace PSS.Areas.CaiWuGuanLi.Controllers
{
    public class IncomeController : Controller
    {
        // GET: CaiWuGuanLi/Income
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BindData(string name, string type, DateTime? date, DateTime? dateend, int limit, int page)
        {

            MyEF ef = new MyEF();
            var dr = (from p in ef.Income
                      join bank in ef.Bank on p.BankID equals bank.BankID
                      join user in ef.Users on p.UsersID equals user.UsersID
                      join ep in ef.Employee on p.Employee equals ep.Eid
                      where 1 > 0 && (!string.IsNullOrEmpty(date.ToString()) ? (p.IncomeDate >= date) : true)
                      && (!string.IsNullOrEmpty(dateend.ToString()) ? (p.IncomeDate <= dateend) : true)
                      select new
                      {
                          IncomeID = p.IncomeID,
                          IncomeName = p.IncomeName,
                          IncomeDate = p.IncomeDate,
                          IncomeMoney = p.IncomeMoney,
                          IncomeRemark = p.IncomeRemark,
                          IncomeType = p.IncomeType,
                          BankName = bank.BankName,
                          BankID = p.BankID,
                          UsersName = user.UsersName,
                          EName = ep.EName,


                      }).ToList();

            if (!string.IsNullOrEmpty(name))
            {
                dr = dr.Where(p => p.IncomeName.Contains(name)).ToList();
            }
            if (!string.IsNullOrEmpty(type))
            {
                dr = dr.Where(p => p.IncomeType == type).ToList();
            }
            //if (!string.IsNullOrEmpty(date.ToString()) && !string.IsNullOrEmpty(dateend.ToString()))
            //{
            //    dr = dr.Where(p => p.IncomeDate >= date)
            //}
            var list = dr.OrderByDescending(p => p.IncomeID).Skip(limit * (page - 1)).Take(limit).ToList();
            return Json(ListToDic(list, dr.Count()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddIncome(Income newincome)
        {
            MyEF ef = new MyEF();
            Income Income = new Income();
            Income.IncomeDate = newincome.IncomeDate;
            Income.IncomeID = newincome.IncomeID;
            Income.IncomeName = newincome.IncomeName;
            Income.IncomeRemark = newincome.IncomeRemark;
            Income.IncomeMoney = newincome.IncomeMoney;
            Income.IncomeType= newincome.IncomeType;
            Income.Employee = newincome.Employee;
            Income.UsersID = newincome.UsersID;
            Income.BankID = newincome.BankID;
            ef.Income.Add(Income);
            int i=ef.SaveChanges();
            if (i > 0)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }


        public int AddMoney(int BankID,decimal BankMoney)
        {
            MyEF ef = new MyEF();
            Bank b = (from p in ef.Bank where p.BankID == BankID select p).SingleOrDefault();
            b.BankMoney += BankMoney;
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
        public ActionResult GetData(string id)
        {
            MyEF ef = new MyEF();
            var list = (from p in ef.Income
                        join bank in ef.Bank on p.BankID equals bank.BankID
                        join user in ef.Users on p.UsersID equals user.UsersID
                        join ep in ef.Employee on p.Employee equals ep.Eid
                        where p.IncomeID == id
                        select new
                        {
                            IncomeID = p.IncomeID,
                            IncomeName = p.IncomeName,
                            IncomeDate = p.IncomeDate,
                            IncomeMoney = p.IncomeMoney,
                            IncomeRemark = p.IncomeRemark,
                            IncomeType = p.IncomeType,
                            BankName = bank.BankName,
                            BankID=bank.BankID,
                            UsersName = user.UsersName,
                            UsersID=user.UsersID,
                            EName = ep.EName,
                            Eid=ep.Eid,


                        }).ToList();
            return Json(list);
        }


        public int Update(Income editincome)
        {
            MyEF ef = new MyEF();
            ef.Entry(editincome).State = EntityState.Modified;
            int i = ef.SaveChanges();
            return i;
        }

        /// <summary>
        /// 审核，反审
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="incomemoney"></param>
        /// <param name="bankid"></param>
        /// <returns></returns>
        public int Istype(string id,string type,decimal incomemoney=0, int bankid=0)
        {
            MyEF ef = new MyEF();
            Income income= (from p in ef.Income where p.IncomeID == id select p).SingleOrDefault();
            Bank b= (from p in ef.Bank where p.BankID == bankid select p).SingleOrDefault();
            if (income.IncomeType == "未审")
            {
                income.IncomeType = "已审";
                b.BankMoney += incomemoney;
                
            }
            else {

                income.IncomeType = "未审";
                b.BankMoney -= incomemoney;
            }
            //ef.Entry(income).State = EntityState.Modified;
            int i = ef.SaveChanges();
            return i;
        }

       

        /// <summary>
        /// 经手人下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult Emptype()
        {
            MyEF ef = new MyEF();
            var list = from p in ef.Employee select new { Eid=p.Eid, EName=p.EName };
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 银行账户下拉框
        /// </summary>
        /// <returns></returns>

        public ActionResult BankType()
        {
            MyEF ef = new MyEF();
            var list = from p in ef.Bank select new { BankID=p.BankID, BankName=p.BankName};
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Delete(string id)
        {
            MyEF ef = new MyEF();
            var list = (from p in ef.Income where p.IncomeID ==id select p).FirstOrDefault();
            ef.Income.Remove(list);
            int i = ef.SaveChanges();
            return Json(i);
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