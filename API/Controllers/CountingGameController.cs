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
        [HttpGet("GetVal")]
        public int GenerateValue(int startingValue)
        {
            return GenCountingVal(startingValue);
        }

        private static int GenCountingVal(int str_val)
        {
            Random rnd = new Random();
            
            if (str_val <= 100)
            {
                int counting_val = rnd.Next(2, 15);
                return counting_val;
            }
            else if(str_val > 100 && str_val <=200)
            {
                int counting_val = rnd.Next(15, 35);
                return counting_val;
            }
            return 7;

        }
    }
}