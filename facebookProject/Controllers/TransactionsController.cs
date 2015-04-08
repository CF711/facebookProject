using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using facebookProject.Models;

namespace facebookProject.Controllers
{
    public class TransactionsController : Controller
    {
        private NosebookContext db = new NosebookContext();
        

        //
        // GET: /Transactions/
        public ActionResult Index()
        {
            return View(db.Transactions.ToList().OrderBy(p => p.datetime));
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
    }
}