using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

/*********************************************************
 * @author Sam Kaiser
 * @title Movie Reader/Writer
 * @version 1.0
 * 
 * @description
 * This application will read movie data from a 
 * CSV file and output that data to the console.
 * 
 * Based on a user's input it will also be able to write
 * additional movie data to this CSV file.
 * 
 * This application is modeled after a previous 
 * assignment.
 * 
 * @status incomplete
 *********************************************************/
namespace movieDataFileHwSk
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Test log file
            /// @status passed
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Hello World");

            /// Enlarge console window
            Console.SetWindowSize(160, 30);
            /**
             * Initialize file path to movies.csv 
             **/

            string movieDataFile = "movies.csv";

            /*********************************
             * If selection == 1, try to
             * open the movies.csv file
             * and print out the data from 
             * each movie.
             * 
            *********************************/
            int selection;
            do
            {
                Console.WriteLine("1) Read movies and display info.");
                Console.WriteLine("2) Add movie to file.");
                Console.WriteLine("3) Exit");
                selection = Convert.ToInt32(Console.ReadLine());

                /***************************************************************
                 * If selection is equal to 1 open the StreamReader object.
                 ***************************************************************/
                if (selection == 1)
                {
                    /***************************************************************
                    * If selection is equal to 2 open the StreamWriter object.
                    ***************************************************************/
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(movieDataFile);

                        /************************************************
                         * Delimiter description:
                         * 
                         * The commas in movie titles are handled
                         * by replacing ", " with "*" so that commas
                         * followed by spaces aren't used to delimit 
                         * new lines of data.
                         * 
                         * This assumes that typos aren't involved with
                         * movie titles.
                         * 
                         * *********************************************/

                        while (!sr.EndOfStream) {
                            string line = sr.ReadLine().Replace(", ", "*");
                            string[] previousMovieData = {"", ""};
                            string[] parsedMovieData = line.Split(',');

                            //try(int.Parse(parsedMovieData[0])){

                            //}catch (Exception e) {
                            //    logger.Info("Non-integer found as movie Id");
                            //}
                            //finally {

                            //};
                            // Check to see if data x is equal to data y
                            if (!string.Equals(parsedMovieData[0], previousMovieData[0])) 
                            {
                                Console.WriteLine("{0, -8}{1, -30}{2, -30}", parsedMovieData[0], parsedMovieData[1].Replace("*", ", "), parsedMovieData[2]);
                                previousMovieData[0] = parsedMovieData[0];
                            }
                            else if(string.Equals(parsedMovieData[0], previousMovieData[0])) {
                                Console.WriteLine("Duplicate found");
                                logger.Info("Duplicate record found: " + parsedMovieData[0]);
                            }
                                       
                        }
                        /// Log successful output
                        logger.Info("Data successfully parsed and outputted to console.");
                    } /// Catch the exception if the file is not found
                    catch (Exception e)
                    {
                        /// Log any issues
                        logger.Info(e);
                        Console.WriteLine(e.Message);
                    }
                    finally {
                        /// Close up the reader
                        sr.Close();
                        //Console.ReadLine();
                    }

                }
                else if (selection == 2)
                {
                    /// The second parameter makes sure to append rather than to overwrite
                    StreamWriter sw = null;

                    /********************************************************************
                     * Prompt and assign variables to the following: 
                     * TicketID, Summary, Status, Priority, Submitter, Assigned, Watching
                     ********************************************************************/

                    string movieId;
                    Boolean duplicate = false;
                    do
                    {
                        Console.WriteLine("Enter Movie Id: ");
                        movieId = Console.ReadLine();
                        StreamReader sr = null;
                        try
                        {
                            sr = new StreamReader(movieDataFile);

                            /************************************************
                             * Delimiter description:
                             * 
                             * The commas in movie titles are handled
                             * by replacing ", " with "*" so that commas
                             * followed by spaces aren't used to delimit 
                             * new lines of data.
                             * 
                             * This assumes that typos aren't involved with
                             * movie titles.
                             * 
                             * *********************************************/
                            
                            while (!sr.EndOfStream || !(duplicate))
                            {

                                string line = sr.ReadLine().Replace(", ", "*");
                                string[] parsedMovieData = line.Split(',');
                                if (Convert.ToInt32(parsedMovieData[0]) == Convert.ToInt32(movieId)) {
                                    duplicate = true;
                                    logger.Info("Duplicate movie attempt.");
                                }

                            }
                            /// Log successful output
                            logger.Info("Data successfully parsed and outputted to console.");
                        } /// Catch the exception if the file is not found
                        catch (Exception e)
                        {
                            /// Log any issues
                            logger.Info(e);
                            Console.WriteLine(e.Message);
                        }
                        finally
                        {
                            /// Close up the reader
                            sr.Close();
                            //Console.ReadLine();
                        }
                    } while (!Regex.IsMatch(movieId, @"^\d+$") && (duplicate==true));


                    Console.WriteLine("Enter Title: ");
                    string title = Console.ReadLine();

                    Console.WriteLine("Enter Year: ");
                    string year = Console.ReadLine();

                    /// Enter Genres
                    int count = 0;
                    string genre="";
                    string moreGenres = "";
                    do
                    {
                        Console.WriteLine("Enter Genre: ");
                        /************************************************************
                         * Only add pipe character if there is more than one genre
                         ************************************************************/
                        if (count == 0)
                        {
                            genre += Console.ReadLine();
                        }
                        else
                        {
                            genre += "|" + Console.ReadLine();
                        }

                        /// Increase the count so it knows to insert the "|"
                        count++;
                        Console.WriteLine("Enter more genres?");
                        moreGenres = Console.ReadLine();
                        moreGenres = moreGenres.ToUpper();
                    } while (moreGenres == "Y");
                    
                    

                    /************************************************************************************************************
                    * Enter new movie
                    * 
                    * Will check for duplicate movie ID before writing.
                    *************************************************************************************************************/

                    sw = new StreamWriter(movieDataFile, true);
                    sw.WriteLine(movieId + "," + title + " (" + year + ")" + "," + genre);
                    /// Ensure closure of StreamWriter after
                    sw.Close();
                }
            } while (selection != 3);
            

        }
    }
}
