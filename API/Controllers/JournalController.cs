
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using _2122_Senior_Project_06.Types;
using _2122_Senior_Project_06.SqlDatabase;
using System;
namespace _2122_Senior_Project_06.Controllers
{
    /// <summary>
    /// The API's journal controller.
    /// </summary>
    ///  <remarks> Paired programmed by Andrew and Sarah. </remarks>
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("[controller]")]
    public class JournalController: ControllerBase
    {
        // Get list of journals
        [HttpGet("GetAll")]
        public List<JournalEntry> GetAll(string userID)
        {
            if(Sys_Security.VerifySQL(userID))
            {
                if(UserAccountsDataTable.UIDInUse(userID))
                {
                    return JournalsDataTable.GetAllJournals(userID);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
                /*
                SQL injection, return some form of error
                */
            }
            
        }

        //Get selected journal
        [HttpGet("GetJournal")]
        public JournalEntry GetJournal(string journalID)
        {
            if(Sys_Security.VerifySQL(journalID))
            {
                if(JournalsDataTable.JournalIDInUse(journalID))
                {
                    return JournalsDataTable.GetJournalEntry(journalID);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
                /*
                SQL injection, return some form of error
                */
            }
        }
        
        // CREATE
        [HttpPost("Create")]
        public IActionResult Create([FromBody] JournalEntry potentialJournal)
        {
            
            Console.WriteLine(string.Format("Test ID: {0}", potentialJournal.UserID));
            try
            {
                potentialJournal.LastUpdated = DateTime.Now;
                JournalEntry newEntry = new JournalEntry(potentialJournal.Title, potentialJournal.Body, 
                                                        potentialJournal.UserID, potentialJournal.LastUpdated);
                JournalsDataTable.AddNewEntry(newEntry);
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(418);
            }
        }

        // UPDATE
        [HttpPut("Update")]
        public IActionResult Update([FromBody]JournalEntry updatedEntry)
        {
            try
            {
                updatedEntry.LastUpdated = DateTime.Now;
                JournalsDataTable.UpdateJournalEntry(updatedEntry);
                return Ok();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(418);
            }
        }

        // DELETE
        [HttpDelete("Delete")]
        public IActionResult Delete(string journalID)
        {
            if(Sys_Security.VerifySQL(journalID))
            {
                try
                {
                    JournalsDataTable.DeleteEntry(journalID);
                    return Ok();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return StatusCode(418);
                }
            }
            else
            {
                return StatusCode(418); //Im a teapot
                /*
                SQL injection, return some form of error
                */
            }
        }
    }
}