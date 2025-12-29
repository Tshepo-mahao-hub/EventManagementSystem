// File: Program.cs
using System;
using System.Collections.Generic;

namespace EventManagementSystem
{
    // Custom exception for capacity validation
    public class InvalidCapacityException : ArgumentException
    {
        public InvalidCapacityException(string message) : base(message) { }
    }

    // Base class
    public abstract class Event
    {
        private static int nextId = 1; // unique id generator

        public int EventID { get; }
        public string EventName { get; protected set; }
        public int Capacity { get; protected set; }

        // Default constructor (overload)
        protected Event()
        {
            EventID = nextId++;
            EventName = "Unnamed Event";
            Capacity = 0;
        }

        // Parameterized constructor (overload) - validates capacity
        protected Event(string name, int capacity)
        {
            if (capacity < 0) throw new InvalidCapacityException("Capacity cannot be negative.");
            EventID = nextId++;
            EventName = string.IsNullOrWhiteSpace(name) ? "Unnamed Event" : name.Trim();
            Capacity = capacity;
        }

        // Polymorphic summary display
        public virtual void Display()
        {
            Console.WriteLine($"[{EventID}] {EventName} (Capacity: {Capacity})");
        }

        // Overloaded display for detailed view (method overloading)
        public virtual void Display(bool verbose)
        {
            if (verbose)
            {
                Console.WriteLine($"Event ID : {EventID}");
                Console.WriteLine($"Name     : {EventName}");
                Console.WriteLine($"Capacity : {Capacity}");
            }
            else
            {
                Display();
            }
        }
    }

    // Derived class: Workshop
    public class Workshop : Event
    {
        public string Topic { get; private set; }
        public string Company { get; private set; }

        public Workshop() : base()
        {
            Topic = "General";
            Company = "Unknown";
        }

        // Parameterized constructor (overload)
        public Workshop(string name, int capacity, string topic, string company) : base(name, capacity)
        {
            Topic = string.IsNullOrWhiteSpace(topic) ? "General" : topic.Trim();
            Company = string.IsNullOrWhiteSpace(company) ? "Unknown" : company.Trim();
        }

        public override void Display()
        {
            Console.WriteLine($"Workshop [{EventID}] {EventName} | Topic: {Topic} | Company: {Company} | Capacity: {Capacity}");
        }

        public override void Display(bool verbose)
        {
            if (verbose)
            {
                Console.WriteLine("Type     : Workshop");
                Console.WriteLine($"Event ID : {EventID}");
                Console.WriteLine($"Name     : {EventName}");
                Console.WriteLine($"Topic    : {Topic}");
                Console.WriteLine($"Company  : {Company}");
                Console.WriteLine($"Capacity : {Capacity}");
            }
            else
            {
                Display();
            }
        }
    }

    // Derived class: Seminar
    public class Seminar : Event
    {
        public string Speaker { get; private set; }

        public Seminar() : base()
        {
            Speaker = "TBD";
        }

        // Parameterized constructor (overload)
        public Seminar(string name, int capacity, string speaker) : base(name, capacity)
        {
            Speaker = string.IsNullOrWhiteSpace(speaker) ? "TBD" : speaker.Trim();
        }

        public override void Display()
        {
            Console.WriteLine($"Seminar  [{EventID}] {EventName} | Speaker: {Speaker} | Capacity: {Capacity}");
        }

        public override void Display(bool verbose)
        {
            if (verbose)
            {
                Console.WriteLine("Type     : Seminar");
                Console.WriteLine($"Event ID : {EventID}");
                Console.WriteLine($"Name     : {EventName}");
                Console.WriteLine($"Speaker  : {Speaker}");
                Console.WriteLine($"Capacity : {Capacity}");
            }
            else
            {
                Display();
            }
        }
    }

    // Program with menu
    class Program
    {
        static void Main()
        {
            var events = new List<Event>();
            bool running = true;

            Console.WriteLine("=== Conference Center Event Management System ===");

            while (running)
            {
                try
                {
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1. Add a Workshop");
                    Console.WriteLine("2. Add a Seminar");
                    Console.WriteLine("3. View all events");
                    Console.WriteLine("4. Exit");

                    int choice = ReadInt("Choose an option (1-4): ");

                    switch (choice)
                    {
                        case 1:
                            AddWorkshop(events);
                            break;
                        case 2:
                            AddSeminar(events);
                            break;
                        case 3:
                            ViewAllEvents(events);
                            break;
                        case 4:
                            Console.WriteLine("Exiting program. Goodbye!");
                            running = false;
                            break;
                        default:
                            Console.WriteLine($"Invalid menu choice: {choice}. Please choose 1-4.");
                            break;
                    }
                }
                catch (InvalidCapacityException icex)
                {
                    Console.WriteLine($"Capacity error: {icex.Message}");
                }
                catch (FormatException fex)
                {
                    Console.WriteLine($"Input format error: {fex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }

        // Read integer with validation (re-prompts until valid)
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (input is null)
                    throw new Exception("Input stream closed.");

                if (int.TryParse(input.Trim(), out int value))
                    return value;

                Console.WriteLine("Please enter a valid integer.");
            }
        }

        // Read a string (returns trimmed string; allows empty but not null)
        private static string ReadString(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input is null) throw new Exception("Input stream closed.");
            return input.Trim();
        }

        private static void AddWorkshop(List<Event> events)
        {
            try
            {
                string name = ReadString("Enter workshop name: ");
                int capacity = ReadInt("Enter capacity (integer >= 0): ");
                if (capacity < 0) throw new InvalidCapacityException("Capacity cannot be negative.");
                string topic = ReadString("Enter workshop topic: ");
                string company = ReadString("Enter company: ");

                var w = new Workshop(name, capacity, topic, company);
                events.Add(w);
                Console.WriteLine("Workshop added successfully.");
            }
            catch (InvalidCapacityException)
            {
                throw; // bubble up to top-level handler for consistent messaging
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add workshop: {ex.Message}");
            }
        }

        private static void AddSeminar(List<Event> events)
        {
            try
            {
                string name = ReadString("Enter seminar name: ");
                int capacity = ReadInt("Enter capacity (integer >= 0): ");
                if (capacity < 0) throw new InvalidCapacityException("Capacity cannot be negative.");
                string speaker = ReadString("Enter speaker name: ");

                var s = new Seminar(name, capacity, speaker);
                events.Add(s);
                Console.WriteLine("Seminar added successfully.");
            }
            catch (InvalidCapacityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add seminar: {ex.Message}");
            }
        }

        private static void ViewAllEvents(List<Event> events)
        {
            if (events.Count == 0)
            {
                Console.WriteLine("No events registered yet.");
                return;
            }

            Console.WriteLine("\n--- All Events (summary) ---");
            foreach (var ev in events)
            {
                ev.Display(); // polymorphic call
            }
            Console.WriteLine("-----------------------------");

            string resp = ReadString("Show detailed view of each event? (y/n): ");
            if (!string.IsNullOrWhiteSpace(resp) && (resp[0] == 'y' || resp[0] == 'Y'))
            {
                Console.WriteLine("\n--- Detailed Events ---");
                foreach (var ev in events)
                {
                    ev.Display(true); // overloaded detailed display
                    Console.WriteLine("---------------------");
                }
            }
        }
    }
}
