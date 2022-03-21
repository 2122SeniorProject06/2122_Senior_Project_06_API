namespace  _2122_Senior_Project_06.Types
{
    public class Badge
    {
        public string BadgeID {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public string Icon {get; set;}

        public Badge () {}

        public string ToSqlString()
        {
            string returnResult = string.Format("{0}, {1}, {2}, {3}",                                                
                                                BadgeID,
                                                Sys_Security.Encoder(Name),
                                                Sys_Security.Encoder(Description),
                                                Sys_Security.Encoder(Icon));
            return returnResult;
        }
    }
}