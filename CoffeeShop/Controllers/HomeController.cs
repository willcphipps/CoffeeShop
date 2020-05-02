using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using Stripe;

namespace CoffeeShop.Controllers {
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }
        private IHostingEnvironment _HostingEnv;

        public HomeController(MyContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _HostingEnv = hostingEnvironment;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userLogin)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.FirstOrDefault(u => u.Email == userLogin.LoginEmail);
                if (customer == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userLogin, customer.Password, userLogin.LoginPassword);
                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                        return View("Index");
                    }
                    else
                    {
                        if (customer.UserType == null)
                        {
                            HttpContext.Session.SetInt32("userid", customer.UserId);
                            return Redirect("/LandingPage");
                        }
                        else
                        {
                            HttpContext.Session.SetInt32("userid", customer.UserId);
                            return Redirect("/Dashboard");
                        }
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }
        
        [HttpPost("Register")]
        public IActionResult Register(Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (_context.Customers.Any(u => u.Email == customer.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<Customer> Hasher = new PasswordHasher<Customer>();
                    customer.Password = Hasher.HashPassword(customer, customer.Password);
                    _context.Customers.Add(customer);
                    // customer.UserType = "Admin";
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", customer.UserId);
                    return Redirect("/LandingPage");
                }
            }
            return View("Index");
        }

        [HttpGet("LandingPage")]
        public IActionResult LandingPage()
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
            {
                return View("Index");
            }
            ViewBag.Cart = _context.OrderItems.Where(o => o.OrderId == (int)HttpContext.Session.GetInt32("orderid")).ToList();
            Order current = _context.Orders.Where(o => o.UserId == (int)userid).LastOrDefault();
            if (current == null || current.HasPaid == true)
            {
                Order newOrder = new Order();
                newOrder.UserId = (int)userid;
                _context.Orders.Add(newOrder);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("orderid", newOrder.OrderId);

                for (var i = 0; i < 20; i++)
                {
                    Console.WriteLine("orderid is ", newOrder.OrderId);
                    Console.WriteLine("sessionid is ", HttpContext.Session.GetInt32("orderid"));
                }
            }
            else
            {
                for (var i = 0; i < 20; i++)
                {
                    Console.WriteLine("orderid is ", current.OrderId);
                    Console.WriteLine("sessionid is ", HttpContext.Session.GetInt32("orderid"));
                }
                HttpContext.Session.SetInt32("orderid", current.OrderId);
                //Add orderid to current user session
            }
            return View();
        }

        [HttpGet("Menu")]
        public IActionResult Menu()
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
            {
                return View("Index");
            }
            List<Product> products = _context.Products.Include(p => p.inCategory).ThenInclude(p => p.NavCat).ToList();
            ViewBag.Cart = _context.OrderItems.Where(o => o.OrderId == (int)HttpContext.Session.GetInt32("orderid")).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(products);
            }
        [HttpGet("AddCart/FromCart/{prodid}")]
        public IActionResult AddFromCart(int prodid)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
            {
                return View("Index");
            }
            for (var i = 0; i < 20; i++)
            {
                Console.WriteLine((int)HttpContext.Session.GetInt32("orderid"));
                Console.WriteLine("prod" + prodid);
            }
            OrderItem itemInCart = new OrderItem();
            itemInCart.OrderId = (int)HttpContext.Session.GetInt32("orderid");
            itemInCart.ProductId = prodid;
            _context.OrderItems.Add(itemInCart);
            _context.SaveChanges();
            return Redirect("/Cart");        
}
        [HttpGet("AddCart/{prodid}")]
        public IActionResult AddCart(int prodid)
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
            {
                return View("Index");
            }
            for (var i = 0; i < 20; i++)
            {
                Console.WriteLine((int)HttpContext.Session.GetInt32("orderid"));
                Console.WriteLine("prod" + prodid);
            }
            OrderItem itemInCart = new OrderItem();
            itemInCart.OrderId = (int)HttpContext.Session.GetInt32("orderid");
            itemInCart.ProductId = prodid;
            _context.OrderItems.Add(itemInCart);
            _context.SaveChanges();
            return Redirect("/Menu");
}
        [HttpGet("Cart")]
        public IActionResult Cart()
        {
            int? userid = HttpContext.Session.GetInt32("userid");
            if (userid == null)
            {
                return View("Index");
            }
            ViewBag.Cart = _context.OrderItems.Where(o => o.OrderId == (int)HttpContext.Session.GetInt32("orderid")).ToList();
            Order topass = _context.Orders.Include(o => o.prodInOrder).ThenInclude(p => p.ProdinOrder).FirstOrDefault(o => o.OrderId == (int)HttpContext.Session.GetInt32("orderid"));
            return View(topass);
        }

        [HttpGet("CartRemove/{orderitemid}")]
        public IActionResult DeleteFromCart(int orderitemid)
        {
            var check = _context.OrderItems.FirstOrDefault(o => o.OrderItemId == orderitemid);
            for (var i = 0; i < 20; i++){
                Console.WriteLine("order item # ", orderitemid);
                Console.WriteLine("orderid " , check.OrderId);
            }
            OrderItem todelete = _context.OrderItems.FirstOrDefault(i => i.OrderItemId == orderitemid);
            _context.OrderItems.Remove(todelete);
            _context.SaveChanges();
            return Redirect("/Cart");
            
        }

        [HttpGet("Checkout/{orderid}")]
        public IActionResult Checkout(int orderid){
            List<OrderItem> todelete = _context.OrderItems.Include(o => o.ProdinOrder).Where(o => o.OrderId == orderid).ToList();
       
                if (todelete.Count <= 0)
                {
                    return Redirect("/Cart");
                }
                else
                {
                    decimal tocharge = 0;
                    for (var i = 0; i < todelete.Count; i++)
                    {
                        tocharge += todelete[i].ProdinOrder.UnitPrice;
                        _context.OrderItems.Remove(todelete[i]);
                        _context.SaveChanges();
                    }
                    tocharge += tocharge * (decimal).0852;
                    Order toprocess = _context.Orders.FirstOrDefault(o => o.OrderId == (int)HttpContext.Session.GetInt32("orderid"));
                    //stripe payment integration here
                    //charge card tocharge
                    toprocess.HasPaid = true;

                    return Redirect($"/Success/{tocharge}");
                }
        }

        [HttpGet("Success/{charged}")]
        public IActionResult Success(decimal charged){
            ViewBag.Cart = _context.OrderItems.Where(o => o.OrderId == (int)HttpContext.Session.GetInt32("orderid")).ToList();
            ViewBag.Charge = charged;
            return View("Success");
        }

// Admin Section of Controller ------------------------------

        [HttpGet ("Dashboard")]
        public IActionResult Dashboard () {
            int? userid = HttpContext.Session.GetInt32 ("userid");
            if (userid == null) {
                return View ("Index");
            }
            Customer customer = _context.Customers.FirstOrDefault(u => u.UserId == userid);
            if (customer.UserType != "Admin"){
                Redirect("/LandingPage");
            }
            List<Product> products = _context.Products.Include (p => p.inCategory).ThenInclude (p => p.NavCat).ToList ();
            return View (products);
        }

        [HttpGet ("AddProduct")]
        public IActionResult AddProduct () {
            int? userid = HttpContext.Session.GetInt32 ("userid");
            if (userid == null) {
                return View ("Index");
            }
            Customer customer = _context.Customers.FirstOrDefault(u => u.UserId == userid);
            if (customer.UserType != "Admin"){
                Redirect("/LandingPage");
            }
            ViewBag.Category = _context.Categories.ToList ();
            return View ();
        }

        [HttpPost ("CreateProduct")]
        public IActionResult CreateProduct (CreateProduct model) {
            if (ModelState.IsValid) {
                string uniqueFileName = null;
                if (model.ImagePath != null) {
                    string UploadedFolder = Path.Combine (_HostingEnv.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid ().ToString () + "_" + model.ImagePath.FileName;
                    string filePath = Path.Combine (UploadedFolder, uniqueFileName);
                    model.ImagePath.CopyTo (new FileStream (filePath, FileMode.Create));
                }

                Product newProduct = new Product {
                    ProductName = model.ProductName,
                    Description = model.Description,
                    UnitPrice = model.UnitPrice,
                    ImagePath = uniqueFileName
                };
                _context.Add (newProduct);
                Collection newCollection = new Collection ();
                newCollection.CategoryId = model.CategoryId;
                newCollection.ProductId = newProduct.ProductId;
                _context.Add (newCollection);
                _context.SaveChanges ();
                return Redirect ("/Dashboard");
            } else {
                return View ("AddProduct");
            }
        }

        [HttpGet ("AddCategory")]
        public IActionResult AddCategory () {
            int? userid = HttpContext.Session.GetInt32 ("userid");
            if (userid == null) {
                return View ("Index");
            }
            Customer customer = _context.Customers.FirstOrDefault(u => u.UserId == userid);
            if (customer.UserType != "Admin"){
                Redirect("/LandingPage");
            }
            return View ();
        }

        [HttpPost ("NewCategory")]
        public IActionResult CreateCategory (Category newCat) {
            if (ModelState.IsValid) {
                _context.Categories.Add (newCat);
                _context.SaveChanges ();
                return Redirect ("/Dashboard");
            } else {
                return View ("AddCategory");
            }
        }
        [HttpGet ("/Delete/{itemid}")]
        public IActionResult DestroyProduct (int itemid) {
            Product toDestroy = _context.Products.FirstOrDefault (p => p.ProductId == itemid);
            _context.Products.Remove (toDestroy);
            _context.SaveChanges ();
            return Redirect ("/Dashboard");
        }
        [HttpGet("Details/{productid}")]
        public IActionResult ProductDetails(int prodid){
            ViewBag.Product = _context.Products.FirstOrDefault(p => p.ProductId == prodid);
            return View();
        }

        [HttpGet ("Logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index");
        }
    }
}