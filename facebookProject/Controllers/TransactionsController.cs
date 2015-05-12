using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using facebookProject.Models;
using Facebook;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Collections;

namespace facebookProject.Controllers
{
    public class TransactionsController : Controller
    {
        private NosebookContext db = new NosebookContext();
        FacebookClient fb;
        //
        private bool isLoggedIn(string accessToken)
        {
            if (!String.IsNullOrEmpty(accessToken))
            {
                //var accessToken = Session["AccessToken"].ToString();
                fb = new FacebookClient(accessToken);
                dynamic user = fb.Get("me");
                var created = createUser(user.id, user.first_name, user.last_name, user.email);
                return (created != null);
            }
            else
            {
                return false;
            }
        }


        public User createUser(string id, string first_name, string last_name,
            string email)
        {
            User user = new User();
            user.user_id = id;
            user.first_name = first_name;
            user.last_name = last_name;
            user.fb_username = email;
            if (db.Users.ToList().Where(p => p.user_id == id).Count() == 0)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
            else
            {
                return db.Users.Find(id);
            }
        }
        // GET: /Transactions/
        public ActionResult Index()
        {
            var accessToken = "";
            try{
            accessToken = HttpContext.ApplicationInstance.Request.Cookies["jumbleUP"].Value;
            }
            catch( NullReferenceException e){
                accessToken = "";
            }
                if (isLoggedIn(accessToken))
                {

                    dynamic user = fb.Get("me");
                    return View(db.Transactions.ToList().Where(tr => tr.user_id == user.id).OrderBy(tr => tr.datetime));
                }
                return Redirect("/Home/Index");
            
        }
        public ActionResult BuySell(string searchString)
        {
            var token = HttpContext.ApplicationInstance.Request.Cookies["jumbleUP"].Value;
            if (isLoggedIn(token))
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    Dictionary<string, string> st = getStockData(searchString);

                    return View(st);
                }
                Dictionary<string, string> dict = new Dictionary<string, string>();
                return View(dict);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        //
        // GET: /Transactions/Details/5
        
        public void BuyStock( string stockAmount, string symbol, string price)
        {
            var token = HttpContext.ApplicationInstance.Request.Cookies["jumbleUP"].Value;
            if (isLoggedIn(token) && !String.IsNullOrEmpty(symbol))
            {
                if (!String.IsNullOrEmpty(stockAmount))
                {
                    dynamic user = fb.Get("me");
                    string user_id = user.id;
                    string stock_id = symbol;
                    decimal priceDec = Decimal.Parse(price);
                    int amount = Convert.ToInt32(stockAmount);
                    createTransaction(user_id, stock_id,
                    priceDec, amount, true);
                    Response.Redirect("Index");
                }
            }
        }
        public ActionResult SellStock(string stock_id, string token)
        {
            
            if (isLoggedIn(token))
            {
                dynamic user = fb.Get("me");
                var lst = db.Transactions.ToList().Where(t => t.user_id == user.id).
                    Where(t => t.stock_id == stock_id).
                    Where(t => t.buy == true);
                
                return View(lst);
            }
            return Redirect("/Home/Index");
        }
        public ActionResult SellStock2(string stock_id, string amount)
        {
            
            if (getNumberOwned(stock_id) > 0
                && getNumberOwned(stock_id) >= Convert.ToInt32(amount))
            {
                
                Transaction tr = new Transaction();
                tr.stock_id = stock_id;
                tr.amount = Convert.ToInt32(amount);
                tr.datetime = DateTime.Now;
                tr.price = Convert.ToDecimal(getStockPrice(stock_id));
                dynamic user = fb.Get("me");
                tr.user_id = user.id;
                tr.buy = false;
                db.Transactions.Add(tr);
                db.SaveChanges();
                return Redirect("Index");
            }
            return Redirect("Index");
        }
        public ActionResult Details(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //
        // GET: /Transactions/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Transactions/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            transaction.datetime = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transaction);
        }

        //
        // GET: /Transactions/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //
        // POST: /Transactions/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        //
        // GET: /Transactions/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        //
        // POST: /Transactions/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /*
         * All the method stubs for the transactions part of the 
         * Nosebook project are here.
         * The stubs will be filled as we go along
         * */
        public IEnumerable<Transaction> getListOfTransactions(string userID){
             return db.Transactions.ToList().Where(t => t.user_id == userID);
        }
        public IEnumerable<Transaction> getListOfTransactions(string userID, string stocksymbol)
        {
            return db.Transactions.ToList().Where(t => t.user_id == userID).Where(t => t.stock_id == stocksymbol);
        }
        public int deleteTransaction(int transaction_id)
        {
            Transaction tr = db.Transactions.First(t => t.transaction_id == transaction_id);
            db.Transactions.Remove(tr);
            int success = db.SaveChanges();
            return success;
        }
        public void createTransaction(string user_id, string stock_id,
            decimal price, int amount, bool buy)
        {
            DateTime datetime = DateTime.Now;
            Transaction tr = new Transaction();
            tr.user_id = user_id;
            tr.stock_id = stock_id;
            tr.price = price;
            tr.amount = amount;
            tr.datetime = datetime;
            tr.buy = buy;
            db.Transactions.Add(tr);
            db.SaveChanges();
        }
        private Dictionary<string, string> getStockData(String ticker)
        {
            String url = "http://dev.markitondemand.com/Api/v2/Quote/xml?symbol=";
            url += ticker;
            String response = RequestResponse(url);
            XDocument doc = XDocument.Parse(response);
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

            foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;

                while (dataDictionary.ContainsKey(keyName))
                {
                    keyName = element.Name.LocalName + "_" + keyInt++;
                }

                dataDictionary.Add(keyName, element.Value);
            }
            //string[] attributes = new string[]{"Status", "Name",
            //"Symbol",
            //"LastPrice",
            //"Change" ,
            //"ChangePercent",
            //"Timestamp",
            //"MSDate",
            //"MarketCap",
            //"Volume",
            //"ChangeYTD",
            //"ChangePercentYTD",
            //"High",
            //"Low",
            //"Open"};
            //foreach ( string attribute in attributes){

            //}
            ////XmlNode retrievedData = xmlDoc.SelectSingleNode(attribute);
            ////String data = retrievedData.InnerText;

            return dataDictionary;
        }
        public string getStockName(String stock_id)
        {
            var dict = getStockData(stock_id);
            return dict["Name"];
        }
        public string getStockPrice(string stock_id)
        {
            var dict = getStockData(stock_id);
            return dict["LastPrice"];
        }
        private string RequestResponse(string pUrl)
        {
            HttpWebRequest webRequest = System.Net.WebRequest.Create(pUrl) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.Timeout = 20000;

            Stream responseStream = null;
            StreamReader responseReader = null;
            string responseData = "";
            try
            {
                WebResponse webResponse = webRequest.GetResponse();
                responseStream = webResponse.GetResponseStream();
                responseReader = new StreamReader(responseStream);
                responseData = responseReader.ReadToEnd();
            }
            catch (Exception exc)
            {
                Response.Write("<br /><br />ERROR : " + exc.Message);
            }
            finally
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseReader.Close();
                }
            }

            return responseData;
        }
        public int getNumberOwned( string stocksymbol)
        {
            var token = Request.Cookies["jumbleUP"].Value;
            if (isLoggedIn(token))
            {
                dynamic user = fb.Get("me");
                string userID = user.id;
                IEnumerable<Transaction> bought = db.Transactions.ToList().Where(t => t.user_id == userID && t.stock_id==stocksymbol && t.buy == true);
                IEnumerable<Transaction> sold = db.Transactions.ToList().Where(t => t.user_id == userID && t.stock_id==stocksymbol && t.buy == false);
                int boughtCount = 0;
                int soldCount = 0;
                foreach (Transaction t in bought)
                {
                    boughtCount += t.amount;
                }
                foreach (Transaction t in sold)
                {
                    soldCount += t.amount;
                }
                return boughtCount - soldCount;
            }
            return -1;
            
        }
        public decimal getProfit(string stocksymbol)
        {
            var token = Request.Cookies["jumbleUP"].Value;
            if (isLoggedIn(token))
            {

                dynamic user = fb.Get("me");
                string userID = user.id;
                var bought = db.Transactions.ToList().Where(t => t.user_id == userID).Where(t => t.stock_id == stocksymbol).Where(t => t.buy == true);
                var sold = db.Transactions.ToList().Where(t => t.user_id == userID).Where(t => t.stock_id == stocksymbol).Where(t => t.buy == false);
                decimal spent = 0;
                decimal gained = 0;
                foreach (Transaction t in bought)
                {
                    spent += t.amount * t.price;
                }
                foreach (Transaction t in sold)
                {
                    gained += t.amount * t.price;
                }
                return gained - spent;
            }
            return -1;
        }
    }
}