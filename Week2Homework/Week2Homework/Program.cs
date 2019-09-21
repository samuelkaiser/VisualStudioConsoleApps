using System;
using System.IO;
/**
 * @title Week2Homework
 * @author Sam Kaiser
 * @version 1.0
 * 
 * Read the sleep file and display the data as defined
 * in the presentation.
 *
 * For extra credit include the average hours of sleep each week.
 *
 * The if (resp == "1") {} code block was provided by the instructor.
 *
 * The if (resp == "2") {} code block is the solution to the assignment.
 * 
 **/
namespace Week2Homework
{
    class Program
    {
        static void Main(string[] args)
        {
            // ask for input
            Console.WriteLine("Enter 1 to create data file.");
            Console.WriteLine("Enter 2 to parse data.");
            Console.WriteLine("Enter anything else to quit.");
            // input response
            string resp = Console.ReadLine();

            // specify path for data file
            string file = "data.txt";

            if (resp == "1")
            {
                // create data file

                // ask a question
                Console.WriteLine("How many weeks of data is needed?");
                // input the response (convert to int)
                int weeks = int.Parse(Console.ReadLine());

                // determine start and end date
                DateTime today = DateTime.Now;
                // we want full weeks sunday - saturday
                DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
                // subtract # of weeks from endDate to get startDate
                DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));

                // random number generator
                Random rnd = new Random();

                // create file
                StreamWriter sw = new StreamWriter(file);
                // loop for the desired # of weeks
                while (dataDate < dataEndDate)
                {
                    // 7 days in a week
                    int[] hours = new int[7];
                    for (int i = 0; i < hours.Length; i++)
                    {
                        // generate random number of hours slept between 4-12 (inclusive)
                        hours[i] = rnd.Next(4, 13);
                    }
                    // M/d/yyyy,#|#|#|#|#|#|#
                    //Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
                    sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
                    // add 1 week to date
                    dataDate = dataDate.AddDays(7);
                }
                sw.Close();
            }
            else if (resp == "2")
            {
                // TODO: parse data file
                // just here to add a new line after user's input. looks nicer.
                Console.Write("\n");

                try {
                    using (StreamReader sr = new StreamReader(file)) {
                        while (sr.Peek() >= 0) {
                            // split date and sleep data
                            var fullData = sr.ReadLine().Split(',');

                            // parse out date values
                            int month = Convert.ToInt32(fullData[0].Split('/')[0]);
                            int day = Convert.ToInt32(fullData[0].Split('/')[1]);
                            int year = Convert.ToInt32(fullData[0].Split('/')[2]);

                            DateTime date = new DateTime(year, month, day);

                            var weeklyData = fullData[1].Split('|');

                            int totalHoursOfSleep = 0;

                            // count the hours of sleep
                            foreach (string value in weeklyData) {
                                totalHoursOfSleep += Convert.ToInt32(value);
                            }

                            // calculate the average
                            decimal averageHoursOfSleep = Convert.ToDecimal(totalHoursOfSleep) / 7;

                            // spit out the formatted results
                            Console.WriteLine($"{"Week of "}{date.ToString("MMM")}{", "}{date.ToString("dd")}{", "}{date.ToString("yyyy")}");
                            Console.WriteLine($"{"Su",3}{"Mo",3}{"Tu",3}{"We",3}{"Th",3}{"Fr",3}{"Sa",3}{"Tot",4}{"Avg",4}");
                            Console.WriteLine($"{"--",3}{"--",3}{"--",3}{"--",3}{"--",3}{"--",3}{"--",3}{"---",4}{"---",4}");
                            Console.WriteLine($"{weeklyData[0],3}{weeklyData[1],3}{weeklyData[2],3}{weeklyData[3],3}{weeklyData[4],3}{weeklyData[5],3}{weeklyData[6],3}{totalHoursOfSleep,4}{Math.Round(averageHoursOfSleep,1),4}\n");
                        }
                    } 
                }
                catch (FileNotFoundException ex) {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
