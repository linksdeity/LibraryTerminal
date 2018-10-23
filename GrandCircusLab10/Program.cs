using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GrandCircusLab10
{
    class Program
    {
        static void Main(string[] args)
        {

            int bookBorrowIndex;

            //here is the list and index

            //0 - Author
            //1 - Title
            //2 - 0 or Checked Out By
            //3 - 0 or Check Out Date
            //4 - 0 or Return Date
            //5 - Donated By


            List<string[]> availableBooks = new List<string[]>()
            {
                new string[6] { "Bradbury, Ray", "Martian Chronicles, the", "0", "0", "0", "Guy Montag" },
                new string[6] { "Asimov, Isaac", "I Robot", "0", "0", "0", "Susan Calvin" },
                new string[6] { "Wells, HG", "Time Machine, the", "0", "0", "0", "Mr. Hillyer" },
                new string[6] { "King, Steven", "Dark Tower, the", "0", "0", "0", "Roland Deschain" },
                new string[6] { "Adams, Douglas", "Hitchiker's Guide to the Galaxy, the", "0", "0", "0", "Ford Prefect" },
                new string[6] { "Herbert, Frank", "DUNE", "0", "0", "0", "Paul Atreides" },
                new string[6] { "Clarke, Arthur C.", "Rendezvous with Rama", "0", "0", "0", "Bill Norton" },
                new string[6] { "Orwell, George", "Animal Farm", "0", "0", "0", "Mr. Whymper" },
                new string[6] { "Verne, Jules", "Journey to the Center of the Earth", "0", "0", "0", "Professor Lidenbrock" },
                new string[6] { "Vonnegut, Kurt", "Sirens of Titan, the", "0", "0", "0", "Stony Stevenson" },
                new string[6] { "Pratchett, Terry", "Color of Magic, the", "0", "0", "0", "Rincewind" },
                new string[6] { "Jordan, Robert", "Eye of the World, the", "0", "0", "0", "Perrin Aybara" }

            };

            //prepare books by organizing alphabetically by author to begin
            availableBooks = availableBooks.OrderBy(arr => arr[0]).ToList();
            int sort = 0;

            while (true)
            { 
                //greet user, ask them if they want to check out a book, return a book, or donate a book
                int userChoice = GetNumber("Hello! Welcome to the library! What would you like to do today? Please type the number of the option below...\n\n" +
                               "1) Check out a book\n2) Return a book\n3) Donate a book\n4) Switch book list sort by Title/Author\n5) See book donation credits\n\n", 1, 5);


                switch (userChoice)
                {
                    //borrowing a book
                    case 1:
                        Console.Clear();
                        ListBooks(availableBooks);


                        bookBorrowIndex = CheckList("\n\nWhich book would you like to borrow? (You can type book name keywords, author name, or 'go back' to leave...", availableBooks);

                        if (bookBorrowIndex > 0)
                        {
                            DateTime myValue = DateTime.Today;
                            Console.WriteLine("What is your name? (First Last)");

                            string nameBorrow = GetString ("Please enter your First and Last name, no symbols, first letters capitalized.", @"^[A-Z][a-z]+\s[A-Z][a-z]+$");

                            Console.WriteLine("\nYou are borrowing '{0}", availableBooks[bookBorrowIndex][1] + "'  on  " + myValue.ToShortDateString());
                            Console.WriteLine("It is due back on: {0}", myValue.AddDays(14).ToShortDateString());

                            availableBooks[bookBorrowIndex][2] = nameBorrow;
                            availableBooks[bookBorrowIndex][3] = myValue.ToShortDateString();
                            availableBooks[bookBorrowIndex][4] = myValue.AddDays(14).ToShortDateString();

                            Console.WriteLine("\npress anything to continue...");
                            Console.ReadKey(true);

                        }

                        break;
                    //Return a book
                    case 2:
                        Console.Clear();

                        string nameReturn = GetString("Please enter your First and Last name, no symbols, first letters capitalized.", @"^[A-Z][a-z]+\s[A-Z][a-z]+$");

                        BookReturn(availableBooks, nameReturn);

                        Console.Clear();
                        break;

                    //Donate a book
                    case 3:

                        DonateBook(availableBooks);

                        Console.Clear();
                        break;

                    //Sort books by title or author
                    case 4:

                        if (sort == 0)
                        {
                            availableBooks = availableBooks.OrderBy(arr => arr[1]).ToList();
                            sort = 1;
                            Console.WriteLine("\nSorted by book title!");
                        }
                        else if (sort == 1)
                        {
                            availableBooks = availableBooks.OrderBy(arr => arr[0]).ToList();
                            sort = 0;
                            Console.WriteLine("\nSorted by author name!");
                        }

                        Console.WriteLine("\n\n(press anything to continue...)");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;

                    case 5:
                        Console.Clear();
                        Console.WriteLine("Here are the book donation credits...\n");
                        foreach(string[ ]book in availableBooks)
                        {
                            Console.WriteLine(book[1] + " was donated by... " + book[5]);
                        }

                        Console.WriteLine("\n\n(press any key to continue...)");
                        Console.ReadKey(true);

                        break;
                }


                Console.Clear();
            }

        }

        static int GetNumber(string message, int minValue, int maxValue)
        {
            //verify input is a number and within boundaries
            while (true)

            {
                Console.WriteLine(message);

                int number;

                try
                {
                    number = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number, no letters or symbols!");
                    continue;
                }

                if(number < minValue || number > maxValue)
                {
                    continue;
                }
                else
                {
                    return number;
                }
            }
        }



        static string GetString(string message, string regExpect)
        {
            //verify that a string is formatted as expected

            while (true)

            {
                Console.WriteLine(message);

                string words = Console.ReadLine();

                if (Regex.IsMatch(words, regExpect))
                {
                    return words;
                    
                }
                else
                {
                    continue;
                }
            }
        }



        static void ListBooks (List<string[]> availableBooks)
        {
            Console.WriteLine("The following books are available...\n\n");

            foreach(string[] book in availableBooks)
            {
                if (book[2] == "0")
                {
                    Console.WriteLine(String.Format("{0,-40} -- {1,5}", book[1], "by " + book[0]));
                }
            }

            Console.WriteLine("\nThe following books are currently checked out...");

            int checkedOut = 0;

            foreach (string[] book in availableBooks)
            {
                if (book[2] != "0")
                {
                    checkedOut++;
                    Console.WriteLine(String.Format("'{0}',  checked out by:  {1}  and due back on {2}", book[1],  book[2], book[4]));
                }
            }


            if (checkedOut == 0)
            {
                Console.WriteLine("\n*  All books currently available! :)\n");
            }

        }



        static int CheckList(string message, List<string[]> availableBooks)
        {

            while (true)
            {//0 author
             //1 title

                Console.WriteLine(message);

                string userChoice = Console.ReadLine();

                //check if the user wants to go back by passing negative value
                if (userChoice.ToLower() == "go back" || userChoice.ToLower() == "goback")
                {
                    Console.Clear();
                    return -1;
                }

                int bookHit = 0;
                int authorHit = 0;
                int counter = 0;
                int selectionIndex = 0;





                List<int> bookIndexer = new List<int>();
                List<int> authorIndexer = new List<int>();

                //search for keywords in title
                foreach (string[] book in availableBooks)
                {
                    counter++;

                    if (book[1].ToLower().Contains(userChoice.ToLower()))
                    {

                        bookIndexer.Add(counter - 1);

                        bookHit++;

                    }

                    //search for matching author names
                    else if (book[0].ToLower().Contains(userChoice.ToLower()))
                    {

                        authorIndexer.Add(counter - 1);

                        authorHit++;

                    }

                }

                Console.Clear();

                //display all hits from the search
                Console.WriteLine("{0} books found with matching titles:\n", bookHit);

                foreach (int location in bookIndexer)
                {
                    Console.WriteLine("  *  " + availableBooks[location][1]);
                }


                Console.WriteLine("\n{0} books found with matching authors:\n", authorHit);

                foreach (int location in authorIndexer)
                {
                    Console.WriteLine("  *  " + availableBooks[location][0] + "  --  " + availableBooks[location][1]);
                }

                Console.WriteLine("\nIs the book you want listed? (press 'y' for yes, anything else for no)");

                char response = Console.ReadKey(true).KeyChar;


                //narrow down the selection...
                if (response == 'y' || response == 'Y')
                {

                    Console.Clear();

                    //if there is only one match and they say yes we return that
                    if (bookIndexer.Count == 1 && authorIndexer.Count == 0)
                    {

                        selectionIndex = bookIndexer[0];

                    }
                    else if (bookIndexer.Count == 0 && authorIndexer.Count == 1)
                    {

                        selectionIndex = authorIndexer[0];

                    }
                    else
                    {
                        //or else we build an array of the matches and have them pick one
                        int bookCounter = 0;
                        int[] bookNumberAndIndex = new int[(bookIndexer.Count + authorIndexer.Count)];

                        Console.WriteLine("\nWhich book would you like to checkout? (enter the number for the selection...)\n\n");

                        foreach (int index in bookIndexer)
                        {
                            bookNumberAndIndex[bookCounter] = index;
                            bookCounter++;
                            Console.WriteLine(bookCounter + " -- " + availableBooks[index][1]);

                        }

                        foreach (int index in authorIndexer)
                        {
                            bookNumberAndIndex[bookCounter] = index;
                            bookCounter++;
                            Console.WriteLine(bookCounter + " -- " + availableBooks[index][1]);
                        }


                        int userSelection = GetNumber("\nPlease enter the number of the book you want to borrow!", 1, bookCounter);

                        selectionIndex = bookNumberAndIndex[userSelection - 1];


                    }
                }
                else
                {
                    Console.Clear();
                    ListBooks(availableBooks);
                    continue;
                }

                Console.Clear();
                return selectionIndex;

            }


        }


        static void BookReturn(List<string[]> availableBooks, string name)
        {
            int counter = -1;
            List<int> indexer = new List<int>();
            DateTime myValue = DateTime.Today;


            foreach (string[] book in availableBooks)
            {

                counter++;

                if (book[2] == name)
                {
                    indexer.Add(counter);
                }

            }

            if (indexer.Count == 0)
            {
                Console.WriteLine("\n\nYou have no books out currently! (press any key to continue...)");
                Console.ReadKey(true);
                return;
            }

            counter = 0;

            Console.WriteLine("\n\nYou have the following books out...");

            foreach(int number in indexer)
            {
                counter++;
                Console.WriteLine("\n\nYou currently have:   '" + availableBooks[number][1] + "'");
                Console.WriteLine("checked out on:   {0}", availableBooks[number][3]);
                Console.WriteLine("due back by:   {0}", availableBooks[number][4]);

                Console.WriteLine("\nAre you returning this book? ('y' for YES, anything else for NO...)");

                char answer = Console.ReadKey(true).KeyChar;

                if (answer == 'y' || answer == 'Y')
                {
                    availableBooks[number][2] = "0";
                    availableBooks[number][3] = "0";
                    availableBooks[number][4] = "0";

                    Console.WriteLine("\n\nThank you! {0}", availableBooks[number][1] + " has been returned\n(press anything to continue...)");
                    Console.ReadKey(true);
                }

            }

        }



        static void DonateBook(List<string[]> availableBooks)
        {
            Console.Clear();

            Console.WriteLine("Please enter the book information below...\n");

            string bookName = GetString("Please enter the name of the book in Title Case", @"^([A-Z0-9]+[a-z0-9]+\s*)+$");

            string authorName = GetString("Please enter the name of the author as Firstname Lastname in Title Case", @"^[A-Z][a-z]+\s[A-Z][a-z]+$");

            string donatedBy = GetString("Please enter your name in Title Case", @"^[A-Z][a-z]+\s[A-Z][a-z]+$");

            //move "THE" from the start of a book title to the back...
            if (bookName[0] == 'T' && bookName[1] == 'h' && bookName[2] == 'e')
            {
                bookName = bookName.Remove(0, 4);
                bookName = bookName + ", the";
            }

            //move author last name in front of author first name...
            string[] authorArray = authorName.Split(' ');

            authorName = authorArray[1] + ", " + authorArray[0];

            availableBooks.Add(new string[6] { authorName, bookName, "0", "0", "0", donatedBy });

            Console.WriteLine("\n\nTHANK YOU! Your book has been added!\n(press anything to continue...)");
            Console.ReadKey(true);


        }




    }
}
