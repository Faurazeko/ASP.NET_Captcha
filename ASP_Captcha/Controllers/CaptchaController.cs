using Microsoft.AspNetCore.Mvc;

//i dont give a shit that all this staff works only on windows xd lmao
#pragma warning disable CA1416

namespace ASP_Captcha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        IWebHostEnvironment _env;

        Thread CaptchaCleanerThread;
        public CaptchaController(IWebHostEnvironment environment)
        {
            _env = environment;

            CaptchaCleanerThread = new Thread(DeleteExpiredCaptchasLoop);
            CaptchaCleanerThread.Start();
        }

        [HttpGet]
        public IActionResult GetCaptcha()
        {
            var captcha = CaptchaEngine.GenerateCaptcha(out string captchaContent);
            var captchaPath = $"/Captchas/{DateTime.Now.Ticks}.png";
            captcha.Save(_env.WebRootPath + captchaPath);

            var captchaObj = new Models.Captcha(captchaContent, captchaPath);

            using (var db = new AppDbContext())
            {
                db.Captchas.Add(captchaObj);
                db.SaveChanges();
            }

            return new JsonResult(new { Id = captchaObj.Id.ToString(), ImageLink = $"https://localhost:7150{captchaPath}" });
        }

        [HttpPost]
        public IActionResult CheckCaptcha(string guess, int id)
        {
            var result = "incorrect";
            guess = guess.ToUpper();
            using (var db = new AppDbContext())
            {
                foreach (var item in db.Captchas)
                {
                    if(item.Id == id)
                    {
                        if (item.Content == guess && item.ExpirationTime >= DateTime.Now)
                            result = "correct";

                        System.IO.File.Delete(_env.WebRootPath + item.Path);
                        db.Captchas.Remove(item);

                        break;
                    }
                }
                db.SaveChanges();
            }
            return new JsonResult(new { Result = result });
        }

        [NonAction]
        private void DeleteExpiredCaptchasLoop()
        {
            while (true)
            {
                using (var db = new AppDbContext())
                {
                    foreach (var item in db.Captchas)
                    {
                        if (item.ExpirationTime < DateTime.Now)
                        {
                            System.IO.File.Delete(_env.WebRootPath + item.Path);
                            db.Captchas.Remove(item);
                        }
                    }
                }

                Thread.Sleep(3600000); // 1 hour
            }
        }
    }
}
