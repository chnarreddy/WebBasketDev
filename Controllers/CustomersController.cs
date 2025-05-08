using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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


            //ViewBag
            ViewBag.TestData= "Test Data";//String, Int, Numeric,etc..

            //ViewData
            ViewData["TestViewData"] = "Test View DATA";//string

            //TempData
            TempData["TempData"] = "Test Temp Data";



            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.MobileSortParm = sortOrder == "MobileNumber" ? "mobile_desc" : "MobileNumber";
            ViewBag.IsActiveSortParm = sortOrder == "IsActive" ? "isactive_desc" : "IsActive";
            ViewBag.CreatedDateSortParm = sortOrder == "CreatedDate" ? "created_desc" : "CreatedDate";
            ViewBag.UpdatedDateSortParm = sortOrder == "UpdatedDate" ? "updated_desc" : "UpdatedDate";
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //select * from Customers order by Name asc -- SQL
            var customer = db.Customers.ToList();//LinQ


            var orders = db.Orders.ToList();//Tolist();

            var user = db.Users.ToList();//Tolist(); It will return the all records from the table, if records are there it eill return null.

            var isValideUser1 = db.Users.Where(u => u.UserName == "Srivatsav" && u.Password == "admin123").FirstOrDefault();//If data presnt in table, it will return  the 1 row of data otherwise return the "null"
            var isValideUser = db.Users.Where(u => u.UserName == "Anoushka" && u.Password == "admin123").First();//If data presnt in table, it will return the 1 row of data otherwise trow the "error"


            var isValideUser2 = db.Users.Where(u => u.UserName == "Hariteja" && u.Password == "admin123").SingleOrDefault();//Here no 0f rows should be present in the table, if 0 rows are present it will return "null" if 2 rows present it will throw an error.
            var isValideUser3 = db.Users.Where(u => u.UserName == "srivatsav2" && u.Password == "admin123").Single();//Here with data combination atleast 1 row should  be present in the table, if 0 rows are present it will throw an error if 2 rows present it will throw an error.


            //Practice for Linq with OrderBy, ThenBy

            //Topic for Group By + Order By
            var userGroupBy = db.Users.GroupBy(u => u.Gender).ToList();//Plain Group By
            var userGroupByOrderByASC = db.Users.GroupBy(u => u.Gender).OrderBy(y => y.Key).ToList();//ASC

            var userGroupByOrderByDesc = db.Users.GroupBy(u => u.Gender).OrderByDescending(y => y.Key).ToList();//Desc

            //SQL Query for same above Linq Query
//            select TOP 1 * from Users order by UserName--ASC
//select TOP 1 * from Users order by UserName desc--DESC
            //            select DISTINCT UserName from Users

            //Select   u.Gender from Users u
            //GROUP By u.Gender
            //ORDER by u.UserName ASC

            //var orderaSingleOrDefault=.

            //var customers1 = db.Customers               
            //    .OrderBy(c => c.Name)                  // Sort customers by name (alphabetically) 
            //    .Skip((pageNumber - 1) * pageSize)     // Skip records from previous pages
            //    .Take(pageSize)                        // Take only the number of records for the current page
            //    .ToList();                             // Execute the query and convert results to a list

            if (!String.IsNullOrEmpty(search))
            {
                customer = customer.Where(c => c.Name.Contains(search) || c.MobileNUmber.Contains(search)).ToList();
            }
            //If, esle if, else
            switch (sortOrder)
            {
                case "name_desc":
                    customer = customer.OrderByDescending(c => c.Name).ToList();
                    break;
                case "MobileNumber":
                    customer = customer.OrderBy(c => c.MobileNUmber).ToList();
                    break;
                case "mobile_desc":
                    customer = customer.OrderByDescending(c => c.MobileNUmber).ToList();
                    break;
                case "IsActive":
                    customer = customer.OrderBy(c => c.IsActive).ToList();
                    break;
                case "isactive_desc":
                    customer = customer.OrderByDescending(c => c.IsActive).ToList();
                    break;
                case "CreatedDate":
                    customer = customer.OrderBy(c => c.CreatedDate).ToList();
                    break;
                case "created_desc":
                    customer = customer.OrderByDescending(c => c.CreatedDate).ToList();
                    break;
                case "Updatede":
                    customer = customer.OrderBy(c => c.UpdatedDate).ToList();
                    break;
                case "updated_desc":
                    customer = customer.OrderByDescending(c => c.UpdatedDate).ToList();
                    break;
                default:
                    customer = customer.OrderBy(c => c.Name).ToList();
                    break;
            }

            if (sortOrder == "name_desc")
            {
                customer = customer.OrderByDescending(c => c.Name).ToList();
            }
            else if (sortOrder == "MobileNumber")
            {
                customer = customer.OrderBy(c => c.MobileNUmber).ToList();
            }
            else if (sortOrder == "mobile_desc")
            {
                customer = customer.OrderByDescending(c => c.MobileNUmber).ToList();
            }
            else if (sortOrder == "IsActive")
            {
                customer = customer.OrderBy(c => c.IsActive).ToList();
            }
            else if (sortOrder == "isactive_desc")
            {
                customer = customer.OrderByDescending(c => c.IsActive).ToList();
            }
            else if (sortOrder == "CreatedDate")
            {
                customer = customer.OrderBy(c => c.CreatedDate).ToList();
            }
            else if (sortOrder == "created_desc")
            {
                customer = customer.OrderByDescending(c => c.CreatedDate).ToList();
            }
            else if (sortOrder == "Updatede")
            {
                customer = customer.OrderBy(c => c.UpdatedDate).ToList();
            }
            else if (sortOrder == "updated_desc")
            {
                customer = customer.OrderByDescending(c => c.UpdatedDate).ToList();
            }
            else
            {
                customer = customer.OrderBy(c => c.Name).ToList();
            }
            

            return View(customer.ToPagedList(pageNumber, pageSize));
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
