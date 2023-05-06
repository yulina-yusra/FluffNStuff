using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using FluffNStuff.Models;
using System.Data.Entity.Validation;

namespace FluffNStuff.Controllers
{
    public class AdminController : Controller
    {
        FluffNStuffEntities db = new FluffNStuffEntities();
        [HttpGet]
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(admintable admin)
        {
            admintable ad = db.admintables.Where(x => x.admin_name == admin.admin_name && x.admin_password == admin.admin_password).SingleOrDefault();
            if(ad!= null)
            {
                Session["admin_id"]=ad.admin_id.ToString();
                return RedirectToAction("Category");
            }
            else
            {
                ViewBag.error = "Invalid Username or Password";
            }
            return View();
        }
        public ActionResult Category()
        {
            if (Session["admin_id"] == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Category(category cat, HttpPostedFileBase imgfile)
        {

            admintable ad = new admintable();
            string path = uploadimage(imgfile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "Image Could Not Be Uploaded";

            }
            else
            {
                category ca = new category();
                ca.cat_name = cat.cat_name;
                ca.cat_image = path;
                ca.cat_status = 1;
                ca.admin_id_fk = Convert.ToInt32(Session["admin_id"].ToString());
                


                db.categories.Add(ca);


                db.SaveChanges();


                return RedirectToAction("ViewCategory");
            }


            return View();

        }


        public ActionResult ViewCategory(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.categories.Where(x => x.cat_status == 1).OrderBy(x => x.cat_id).ToList();
            IPagedList<category> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }
        //public ActionResult Index(int? page)
        //{
        //    int pagesize = 9, pageindex = 1;
        //    pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    var list = db.categories.Where(x => x.cat_status == 1).OrderBy(x => x.cat_id).ToList();
        //    IPagedList<category> cate = list.ToPagedList(pageindex, pagesize);
        //    return View(cate);

        //}

        [HttpGet]
        public ActionResult CreateAdd()
        {
            List<category> li = db.categories.ToList();
            ViewBag.categoryList = new SelectList(li, "cat_id", "cat_name");
            return View();
        }

        [HttpPost]
        public ActionResult CreateAdd(sub_category c, HttpPostedFileBase imgfile)
        {
            List<category> li = db.categories.ToList();
            ViewBag.categoryList = new SelectList(li, "cat_id", "cat_name");


            string path = uploadimage(imgfile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "Image Could Not Be Uploaded";

            }
            else
            {
                sub_category sc = new sub_category();
                sc.subcat_name = c.subcat_name;
                sc.subcat_price = c.subcat_price;
                sc.subcat_image = path;
                sc.cat_id_fk = c.admin_id_fk;
                sc.subcat_desc = c.subcat_desc;
                sc.admin_id_fk = Convert.ToInt32(Session["admin_id"].ToString());
                //db.sub_category.Add(sc);
                //db.SaveChanges();
                try
                {
                    db.sub_category.Add(sc);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                Response.Redirect("ViewCategory");
            }
            return View();
        }

        public ActionResult DisplayAdd(int? id, int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.sub_category.Where(x => x.cat_id_fk == id).OrderBy(x => x.subcat_id).ToList();
            IPagedList<sub_category> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }


        [HttpPost]
        public ActionResult DisplayAdd(int? id, int? page, string search)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.sub_category.Where(x => x.subcat_name.Contains(search)).OrderByDescending(x => x.subcat_id).ToList();
            IPagedList<sub_category> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }

        





        public string uploadimage(HttpPostedFileBase file)

        {

            Random r = new Random();

            string path = "-1";

            int random = r.Next();

            if (file != null && file.ContentLength > 0)

            {

                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))

                {

                    try

                    {



                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);



                        //    ViewBag.Message = "File uploaded successfully";

                    }

                    catch (Exception ex)

                    {

                        path = "-1";

                    }

                }

                else

                {

                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");

                }

            }



            else

            {

                Response.Write("<script>alert('Please select a file'); </script>");

                path = "-1";

            }







            return path;


        }
        public ActionResult SignOut()
        {


            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("ViewCategory");
        }

        public ActionResult ViewDetail(int? id)
        {

            detail_model adm = new detail_model();

            sub_category p = db.sub_category.Where(x => x.subcat_id == id).SingleOrDefault();
            adm.subcat_id = p.subcat_id;
            adm.subcat_name = p.subcat_name;
            adm.subcat_image = p.subcat_image;
            adm.subcat_price = p.subcat_price;
            adm.subcat_desc = p.subcat_desc;

            category cat = db.categories.Where(x => x.cat_id == p.cat_id_fk).SingleOrDefault();
            adm.cat_name = cat.cat_name;
            admintable u = db.admintables.Where(x => x.admin_id == p.admin_id_fk).SingleOrDefault();
            adm.admin_name = u.admin_name;
            //adm.u_image = u.u_image;
           // adm.u_contact = u.u_contact;
            adm.admin_id_fk = u.admin_id;
            return View(adm);



        }

        public ActionResult Add_Delete(int? id)
        {
            sub_category p = db.sub_category.Where(x => x.subcat_id == id).SingleOrDefault();
            db.sub_category.Remove(p);
            db.SaveChanges();

            return RedirectToAction("ViewCategory");
        }


    }
}