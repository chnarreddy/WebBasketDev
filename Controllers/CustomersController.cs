using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBasketDev.Data;

namespace WebBasketDev.Controllers
{
    public class CustomersController : Controller
    {
        private BasketDBEntities db = new BasketDBEntities();

        // GET: Customers
        public ActionResult Index(string sortOrder, string search, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.MobileSortParm = sortOrder == "MobileNumber" ? "mobile_desc" : "MobileNumber";
            ViewBag.IsActiveSortParm = sortOrder == "IsActive" ? "isactive_desc" : "IsActive";
            ViewBag.CreatedDateSortParm = sortOrder == "CreatedDate" ? "created_desc" : "CreatedDate";
            ViewBag.UpdatedDateSortParm = sortOrder == "UpdatedDate" ? "updated_desc" : "UpdatedDate";

            var customers = db.Customers.ToList();

            if(!String.IsNullOrEmpty(search))
            {
                customers = customers.Where(c => c.Name.Contains(search) || c.MobileNUmber.Contains(search)).ToList();
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(c => c.Name).ToList();
                    break;
                case "MobileNumber":
                    customers = customers.OrderBy(c => c.MobileNUmber).ToList();
                    break;
                case "mobile_desc":
                    customers = customers.OrderByDescending(c => c.MobileNUmber).ToList();
                    break;
                case "IsActive":
                    customers = customers.OrderBy(c => c.IsActive).ToList();
                    break;
                case "isactive_desc":
                    customers = customers.OrderByDescending(c => c.IsActive).ToList();
                    break;
                case "CreatedDate":
                    customers = customers.OrderBy(c => c.CreatedDate).ToList();
                    break;
                case "created_desc":
                    customers = customers.OrderByDescending(c => c.CreatedDate).ToList();
                    break;
                case "Updatede":
                    customers = customers.OrderBy(c => c.UpdatedDate).ToList();
                    break;
                case "updated_desc":
                    customers = customers.OrderByDescending(c => c.UpdatedDate).ToList();
                    break;
                default:
                    customers = customers.OrderBy(c => c.Name).ToList();
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(customers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,MobileNUmber,IsActive,CreatedDate,UpdatedDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,MobileNUmber,IsActive,CreatedDate,UpdatedDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
