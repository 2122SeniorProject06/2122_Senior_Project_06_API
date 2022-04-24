
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

        [HttpPost("SuperSecretBaseValueGeneration")]
        public void CreateJournals(string password)
        {
            if(password == "ReusingPasswordsIsAWorseIdeaThanGettingYourTwoMonthGirlfriendsNameTattooed")
            {
                string HugosID = UserAccountsDataTable.GetUIDFromEmail("email2@gmail.com");
                string AndrewsID = UserAccountsDataTable.GetUIDFromEmail("email3@gmail.com");
                string[] activity = { ActivityItems.Encourage, ActivityItems.Focus, ActivityItems.Relax, ActivityItems.Ground,
                                      ActivityItems.Relax, ActivityItems.Other, ActivityItems.Focus, ActivityItems.Focus,
                                      ActivityItems.Ground, ActivityItems.Encourage };
                bool[] hadAttack = {true, true, true, true, true, false, true, true, true, false};
                for(int i = 0; i < 10; i++)
                {
                    string userID = i % 2 == 0 ? HugosID : AndrewsID;
                    JournalEntry newEntry = new JournalEntry(string.Format("Journal #{0}", i), string.Format("{0} x 10 = {1}", i, i*10), userID, DateTime.Now,
                                                             hadAttack[i], activity[i], !(i % 5 == 0));
                    JournalsDataTable.AddNewEntry(newEntry);
                }
            }
        }

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
                JournalEntry newEntry = new JournalEntry(potentialJournal.Title, potentialJournal.Body, potentialJournal.UserID, 
                                                         potentialJournal.LastUpdated, potentialJournal.ActivityMetric);
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
        /*
            Possible Error handling for HTML injections, attacker send false UID or JID
            Obtained from: https://docs.microsoft.com/en-us/aspnet/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/aspnet-error-handling
            try
            {
                file.ReadBlock(buffer, index, buffer.Length);
            }
            catch (FileNotFoundException e)
            {
                Server.Transfer("NoFileErrorPage.aspx", true);
            }
            catch (System.IO.IOException e)
            {
                Server.Transfer("IOErrorPage.aspx", true);
            }

            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        */
    }
}