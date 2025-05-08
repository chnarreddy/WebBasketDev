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
            var customers = db.Customers.ToList();//LinQ


            var orders = db.Orders.ToList();//Tolist();

            var user = db.Users.ToList();//Tolist(); It will return the all records from the table, if records are there it eill return null.

            var isValideUser1 = db.Users.Where(u => u.UserName == "naresh3" && u.Password == "admin123").FirstOrDefault();//If data presnt in table, it will return  the 1 row of data otherwise return the "null"
            //var isValideUser = db.Users.Where(u => u.UserName == "naresh" && u.Password == "admin123").First();//If data presnt in table, it will return the 1 row of data otherwise trow the "error"


            var isValideUser2 = db.Users.Where(u => u.UserName == "naresh4" && u.Password == "admin123").SingleOrDefault();//Here no 0f rows should be present in the table, if 0 rows are present it will return "null" if 2 rows present it will throw an error.
            //var isValideUser3 = db.Users.Where(u => u.UserName == "naresh4" && u.Password == "admin123").Single();//Here with data combination atleast 1 row should  be present in the table, if 0 rows are present it will throw an error if 2 rows present it will throw an error.


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
            //    .OrderBy(c => c.Name)
            //    .Skip((pageNumber - 1) * pageSize)
            //    .Take(pageSize)
            //    .ToList();

            if (!String.IsNullOrEmpty(search))
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
        

        //    if (sortOrder == "name_desc")
        //    {
        //        customers = customers.OrderByDescending(c => c.Name).ToList();
        //    }
        //    else if (sortOrder == "MobileNumber")
        //    {
        //        customers = customers.OrderBy(c => c.MobileNUmber).ToList();
        //    }
        //    else if (sortOrder == "mobile_desc")
        //    {
        //        customers = customers.OrderByDescending(c => c.MobileNUmber).ToList();
        //    }
        //    else if (sortOrder == "IsActive")
        //    {
        //        customers = customers.OrderBy(c => c.IsActive).ToList();
        //    }
        //    else if (sortOrder == "isactive_desc")
        //    {
        //        customers = customers.OrderByDescending(c => c.IsActive).ToList();
        //    }
        //    else if (sortOrder == "CreatedDate")
        //    {
        //        customers = customers.OrderBy(c => c.CreatedDate).ToList();
        //    }
        //    else if (sortOrder == "created_desc")
        //    {
        //        customers = customers.OrderByDescending(c => c.CreatedDate).ToList();
        //    }
        //    else if (sortOrder == "Updatede")
        //    {
        //        customers = customers.OrderBy(c => c.UpdatedDate).ToList();
        //    }
        //    else if (sortOrder == "updated_desc")
        //    {
        //        customers = customers.OrderByDescending(c => c.UpdatedDate).ToList();
        //    }
        //    else
        //    {
        //        customers = customers.OrderBy(c => c.Name).ToList();
        //    }
            

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
