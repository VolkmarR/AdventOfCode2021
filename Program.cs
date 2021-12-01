namespace AdventOfCode;

class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {

            Console.WriteLine("Which day do you want to initialize? Press enter to cancel.");
            var dayString = Console.ReadLine();
            if (string.IsNullOrEmpty(dayString))
                return;

            if (int.TryParse(dayString, out var day))
            {
                DayInitialization.Execute(day);
                return;
            }
        }

    }
}
