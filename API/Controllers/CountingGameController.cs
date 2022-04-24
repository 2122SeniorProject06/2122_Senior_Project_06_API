using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2122_Senior_Project_06.Models;
using _2122_Senior_Project_06.SqlDatabase;

namespace _2122_Senior_Project_06.Controllers
{
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[controller]")]
    public class CountingGameController : ControllerBase
    {
        [HttpGet("GetCountVal")]
        public int GenerateCountValue(int countingValue)
        {
            return GenCountingVal();
        }

        private static int GenCountingVal()
        {
            var random = new Random();

            var list = new List<int>{ 3,5,15,20,30};

            int index = random.Next(list.Count);

            int couting_val = (list[index]);

            return couting_val;

        }
    }   
}