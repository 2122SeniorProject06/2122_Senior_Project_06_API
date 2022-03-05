namespace  _2122_Senior_Project_06
{
    /// <summary>
    /// Interface for accessing the account database.
    /// </summary>
    /// <remarks> Made by Hugo Mazariego. Last update 12/09/2021. </remarks>
    public interface IAccountDatabase
    {
        UserAccount GetByUsername (string username);
    }
}