using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using pers1102.Models;
using System.Diagnostics;



namespace pers1102.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            var mm = new MimeMessage();

            string message = $"You have received a new email from your site's contact form!<br/>" +
                $"Sender: {cvm.Name}<br/>Email: {cvm.Email}<br/>Subject: {cvm.Subject}<br/>" +
            $"Message: {cvm.Message}";

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));
            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));
            mm.Subject = cvm.Subject;
            mm.Body = new TextPart("HTML") { Text = message };
            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            using (var client = new SmtpClient())
            {
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"));
                client.Authenticate(
                _config.GetValue<string>("Credentials:Email:User"),
                    _config.GetValue<string>("Credentials:Email:Password"));

                try
                {
                    client.Send(mm);
                }
                catch (Exception ex)
                {

                    ViewBag.ErrorMessage = $"There was an error processing your request." +
                        $"Please try again later.<br/>" +
                        $"Error Message: {ex.StackTrace}";
                    return View(cvm);

                }
            }

            return View("EmailConfirmation", cvm);
        }

        public IActionResult PortfolioDetail()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}