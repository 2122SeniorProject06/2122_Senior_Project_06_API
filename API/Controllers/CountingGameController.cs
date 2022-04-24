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
            return GenCountingVal(countingValue);
        }

        private static int GenCountingVal(int cnt_val)
        {
            var random = new Random();

            var list = new List<int>{ 5,10,20,25};

            int index = random.Next(list.Count);

            int couting_val = (list[index]);

            return couting_val;

        }

        [HttpGet("GetStartVal")]
        public int GenerateStartValue(int startingValue)
        {
            return GenStartingVal(startingValue);
        }

         private static int GenStartingVal(int str_val)
        {
            var random = new Random();

            var list = new List<int>{ 50,100,200,500};

            int index = random.Next(list.Count);

            int starting_val = (list[index]);

            return starting_val;

        }   
    }
}