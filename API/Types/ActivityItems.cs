namespace _2122_Senior_Project_06.Types
{
    public class ActivityItems
    {
        public static string Focus = "Focus";
        public static string Ground = "Ground";
        public static string Relax = "Relax";
        public static string Breathe = "Breathe";
        public static string Encourage = "Encourage";
        public static string CheckIn = "Check-In";
        public static string None = "None";
        public static string Other = "Other";

        public static string MatchToActivity(int num)
        {
            if(num == 0)
            {
                return ActivityItems.Focus;
            }
            else if(num == 1)
            {
                return ActivityItems.Ground;
            }
            else if(num == 2)
            {
                return ActivityItems.Relax;
            }
            else if(num == 3)
            {
                return ActivityItems.Breathe;
            }
            else if(num == 4)
            {
                return ActivityItems.Encourage;
            }
            else if(num == 5)
            {
                return ActivityItems.CheckIn;
            }
            else return null;
        }
    }
}