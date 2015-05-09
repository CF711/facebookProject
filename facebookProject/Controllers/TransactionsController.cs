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
        private bool isLoggedIn()
        {
            if (Session["AccessToken"] != null)
            {

                var accessToken = Session["AccessToken"].ToString();
                fb = new FacebookClient(accessToken);
                return true;
            }
            else
            {
                return false;
            }
        }
        // GET: /Transactions/
        public ActionResult Index()
        {
            if (isLoggedIn())
            {
                dynamic user = fb.Get("me");
                return View(db.Transactions.ToList().Where(tr => tr.user_id == user.id).OrderBy(tr => tr.datetime));
            }
            return Redirect("/Home/Index");
        }
        public ActionResult BuySell(string searchString)
        {
            if (isLoggedIn())
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
            decimal price, int amount)
        {
            DateTime datetime = DateTime.Now;
            Transaction tr = new Transaction();
            tr.user_id = user_id;
            tr.stock_id = stock_id;
            tr.price = price;
            tr.amount = amount;
            tr.datetime = datetime;
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


    }
}