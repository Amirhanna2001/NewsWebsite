namespace NewsApp.Helper
{
    public class DateValidation
    {
        public static bool IsValidDateTime(DateTime dateTime)
        {
            DateTime today = DateTime.Today;
            DateTime weekFromToday = today.AddDays(7);

            return dateTime.Date >= today && dateTime.Date <= weekFromToday;
        }

    }
}
