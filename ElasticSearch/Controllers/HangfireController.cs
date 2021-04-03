using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ElasticSearch.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        public HangfireController()
        {

        }
        [HttpPost("welcome")]
        public IActionResult Welcome(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"Welcome to our application, {userName}"));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }
        //public void SendWelcomeMail(string userName)
        //{
        //    //Logic to Mail the user
        //    Console.WriteLine($"Welcome to our application, {userName}");
        //}
        [HttpPost]
        [Route("delayedWelcome")]
        public IActionResult DelayedWelcome(string userName)
        {
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Welcome to our application, {userName}"), TimeSpan.FromMinutes(1));
            return Ok($"Job Id {jobId} Completed. Delayed Welcome Mail Sent!");
        }
        //public void SendDelayedWelcomeMail(string userName)
        //{
        //    //Logic to Mail the user
        //    Console.WriteLine($"Welcome to our application, {userName}");
        //}

        [HttpPost]
        [Route("invoice")]
        public IActionResult Invoice(string userName)
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine($"Here is your invoice, {userName}"), Cron.MinuteInterval(2));
            return Ok($"Recurring Job Scheduled. Invoice will be mailed Monthly for {userName}!");
        }
        //public void SendInvoiceMail(string userName)
        //{
        //    //Logic to Mail the user
        //    Console.WriteLine($"Here is your invoice, {userName}");
        //}

        [HttpPost]
        [Route("Thankyou")]
        public IActionResult Thankyou(string userName)
        {
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Welcome to our application, {userName}"), TimeSpan.FromMinutes(1));
            BackgroundJob.ContinueJobWith(
               jobId,
               () => Console.WriteLine($"Thankyou! {userName}"));
            return Ok($"Recurring Job Scheduled. Invoice will be mailed Monthly for {userName}!");
        }
    }
}
