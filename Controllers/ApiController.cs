using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT150Site.Models;

namespace MSIT150Site.Controllers
{
    public class ApiController : Controller
    {
        private readonly DemoContext _context;

        private readonly IWebHostEnvironment _host;
        public ApiController(DemoContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        //public ApiController(DemoContext context)
        //{
        //    _context = context;
        //}

        public IActionResult Index()
        {
            //System.Threading.Thread.Sleep(3000);  //睡3秒鐘 再往下執行
            return Content("Hello Fetch!!");
        }

        public IActionResult GetDemo(UserViewModel user)  //string name,int age = 30
        {
            if (string.IsNullOrEmpty(user.name))
            {
                user.name = "guest";
            }
            return Content($"Hello {user.name} , you are {user.age} years old.");
        }

        public IActionResult Register(Members member, IFormFile file)
        {
            //_context.Members.Add(member);
            //_context.SaveChanges();
            //return Content("新增成功!!");

            string filePath = Path.Combine(_host.WebRootPath, "uploads", file.FileName); //C:\shared\Ajax\MSIT150Site\wwwroot\uploads\walk.gif

            //上傳檔案到uploads資料夾中
            using (var fileStream = new FileStream(filePath, FileMode.Create))  
            {
                file.CopyTo(fileStream);
            }

            //將圖轉成二進位
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);          //fs 上傳 ms 定義路徑
                imgByte = memoryStream.ToArray();  //路徑定義為照片
            }
            
            //存回member資料表
            member.FileName = file.FileName;
            member.FileData = imgByte;
            _context.Members.Add(member);
            _context.SaveChanges();

            //return Content($"上傳檔案到 {filePath}");
            return Content("新增成功!!");
        }

        public IActionResult GetImageByte(int id = 1)
        {
            Members? member = _context.Members.Find(id);  //使用find方法會直接找PrimaryKey 不需要再用Where . FirstorDefault
            byte[]? img = member.FileData;
            return File(img, "image/jpeg");
        }

        public IActionResult Cities()
        {
            var cities = _context.Address.Select(c => c.City).Distinct();
            //var cities = _context.Address.Select(c => new
            //{
            //    c.City
            //}).Distinct();
            return Json(cities);
        }

        //根據城市名稱，回傳城市的鄉鎮區JSON資料
        public IActionResult Districts(string city)
        {
            var districts = _context.Address.Where(a => a.City == city).Select(c => c.SiteId).Distinct();

            return Json(districts);
        }

        //根據鄉鎮區名稱，回傳鄉鎮區的路名JSON資料
        public IActionResult Roads(string siteId)
        {
            var roads = _context.Address.Where(a => a.SiteId == siteId).Select(c => c.Road).Distinct();

            return Json(roads);
        }

        public async Task<IActionResult> CheckAccount(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Json(new { /*isValid = false,*/ message = "請提供姓名" });
            }

            var memberExists = await _context.Members.AnyAsync(m => m.Name == Name);

            if (memberExists)
            {
                return Json(new { /*isValid = false,*/ message = "該姓名已存在" });
            }

            return Json(new { /*isValid = true,*/ message = "" });
        }
    }
}
