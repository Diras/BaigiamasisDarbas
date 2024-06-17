using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Enums;
using BaigiamasisDarbas.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Services
{
    public class MeetingConsoleUI
    {
        private readonly IWorkerService _workerService;
        private readonly IMeetingService _meetingService;

        public MeetingConsoleUI(IWorkerService workerService, IMeetingService meetingService)
        {
            _workerService = workerService;
            _meetingService = meetingService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Worker and Meeting Management");
                Console.WriteLine("1. Manage Workers");
                Console.WriteLine("2. Manage Meetings");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ManageWorkers();
                        break;
                    case "2":
                        ManageMeetings();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void ManageWorkers()
        {
            while (true)
            {
                Console.WriteLine("Manage Workers");
                Console.WriteLine("1. List Workers");
                Console.WriteLine("2. Add Worker");
                Console.WriteLine("3. Update Worker");
                Console.WriteLine("4. Delete Worker");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListWorkers().Wait();
                        break;
                    case "2":
                        AddWorker().Wait();
                        break;
                    case "3":
                        UpdateWorker().Wait();
                        break;
                    case "4":
                        DeleteWorker().Wait();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void ManageMeetings()
        {
            while (true)
            {
                Console.WriteLine("Manage Meetings");
                Console.WriteLine("1. List Meetings");
                Console.WriteLine("2. Add Meeting");
                Console.WriteLine("3. Update Meeting");
                Console.WriteLine("4. Delete Meeting");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListMeetings().Wait();
                        break;
                    case "2":
                        AddMeeting().Wait();
                        break;
                    case "3":
                        UpdateMeeting().Wait();
                        break;
                    case "4":
                        DeleteMeeting().Wait();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
       
        private async Task ListWorkers()
        {
            List<Worker> workers = (await _workerService.GetAllWorkersAsync()).ToList();
            foreach (Worker worker in workers)
            {
                if (worker is Admin)
                {

                }
                else
                {
                    Console.WriteLine(worker.ToString());
                }
                
            }
        }

        private async Task AddWorker()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter surname: ");
            string surname = Console.ReadLine();
            Console.Write("Enter department: ");
            string department = Console.ReadLine();

            var worker = new Worker
            {
                Name = name,
                Surname = surname,
                Department = department
            };

            await _workerService.AddWorkerAsync(worker);
            Console.WriteLine("Worker added successfully.");
        }
        
        private async Task UpdateWorker()
        {
            ListWorkers().Wait();
            Console.Write("Enter worker Id to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool workerFound = false;
                Worker existingWorker = new Worker();
                List<Worker> workers = (await _workerService.GetAllWorkersAsync()).ToList();
                foreach(Worker worker in workers)
                {
                    if(worker.Id == id)
                    {
                        workerFound = true;
                        existingWorker = worker;
                    }
                }
                if (workerFound == false)
                {
                    Console.WriteLine($"Worker with Id {id} not found.");
                    return;
                }

                Console.Write("Enter name (leave empty to keep current): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                    existingWorker.Name = name;

                Console.Write("Enter surname (leave empty to keep current): ");
                string surname = Console.ReadLine();
                if (!string.IsNullOrEmpty(surname))
                    existingWorker.Surname = surname;

                Console.Write("Enter department (leave empty to keep current): ");
                string department = Console.ReadLine();
                if (!string.IsNullOrEmpty(department))
                    existingWorker.Department = department;

                await _workerService.UpdateWorkerAsync(existingWorker, id);
                Console.WriteLine("Worker updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid Id format.");
            }
        }
        
        
        private async Task DeleteWorker()
        {
            ListWorkers().Wait();
            List<Admin> admins = (await _workerService.GetAdminsInfoAsync()).ToList();
            Console.Write("Enter worker Id to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool workerFound = false;
                List<Worker> workers = (await _workerService.GetAllWorkersAsync()).ToList();
                foreach (Worker worker in workers)
                {
                    if (worker.Id == id)
                    {
                       
                        if (admins.Any(admin => admin.Id == worker.Id+1))
                        {
                            Console.WriteLine("Cannot delete worker, person responsible of meeting!");
                            return;
                        }

                        workerFound = true;
                        break; 
                    }
                }
                if (workerFound == false)
                {
                    Console.WriteLine($"Worker with Id {id} not found.");
                    return;
                }

                await _workerService.DeleteWorkerAsync(id);
                Console.WriteLine("Worker deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid Id format.");
            }
        }
        
        
        private async Task ListMeetings()
        {
            List<Meeting> meetings = (await _meetingService.GetAllMeetingsAsync()).ToList();
            foreach (Meeting meeting in meetings)
            {
                Console.WriteLine(meeting.ToString());
            }
        }

        private async Task AddMeeting()
        {
            Console.Write("Enter meeting name: ");
            string name = Console.ReadLine();

            Console.Write("Enter description: ");
            string description = Console.ReadLine();
            string category;
            while (true)
            {
                Console.WriteLine("Select category:");
                Console.WriteLine("1. CodeMonkey");
                Console.WriteLine("2. Hub");
                Console.WriteLine("3. Short");
                Console.WriteLine("4. TeamBuilding");
                int categorySelection = Convert.ToInt32(Console.ReadLine());

                switch (categorySelection)
                {
                    case 1:
                        category = "CodeMonkey";
                        break;
                    case 2:
                        category = "Hub";
                        break;
                    case 3:
                        category = "Short";
                        break;
                    case 4:
                        category = "TeamBuilding";
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        continue;
                }
                break;
            }

            string type;
            while (true)
            {
                Console.WriteLine("Select type:");
                Console.WriteLine("1. Live");
                Console.WriteLine("2. InPerson");
                int typeSelection = Convert.ToInt32(Console.ReadLine());

                switch (typeSelection)
                {
                    case 1:
                        type = "Live";
                        break;
                    case 2:
                        type = "InPerson";
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        continue;
                }
                break;
            }

            DateTime startDate;
            while (true)
            {
                Console.Write("Enter start date (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid start date format. Please try again.");
                }
            }

            DateTime endDate;
            while (true)
            {
                Console.Write("Enter end date (yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid end date format. Please try again.");
                }
            }

            ListWorkers().Wait();
            Worker workerAdmin = new Worker();
            bool workerFound = false;

            while (!workerFound)
            {
                Console.WriteLine("Enter ID of meeting responsible person: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    List<Worker> workers = (await _workerService.GetAllWorkersAsync()).ToList();
                    foreach (Worker worker in workers)
                    {
                        if (worker.Id == id)
                        {
                            workerFound = true;
                            workerAdmin = worker;
                            break;
                        }
                    }
                    if (!workerFound)
                    {
                        Console.WriteLine($"Worker with Id {id} not found. Please try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Id format. Please try again.");
                }
            }

            Console.Write("Enter password for the new Responsible person: ");
            string password = Console.ReadLine();
            Admin responsiblePerson = new Admin(workerAdmin.Name, workerAdmin.Surname, workerAdmin.Department, password);

            Meeting meeting = new Meeting
            {
                Name = name,
                ResponsiblePerson = responsiblePerson,
                Description = description,
                Category = (MeetingCategory)Enum.Parse(typeof(MeetingCategory), category),
                Type = (MeetingType)Enum.Parse(typeof(MeetingType), type),
                StartDate = startDate,
                EndDate = endDate
            };

            await _meetingService.AddMeetingAsync(meeting);
            Console.WriteLine("Meeting added successfully.");

        }

        private async Task UpdateMeeting()
        {
            Console.Write("Enter meeting Id to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Meeting existingMeeting = await _meetingService.GetMeetingByIdAsync(id);
                if (existingMeeting == null)
                {
                    Console.WriteLine($"Meeting with Id {id} not found.");
                    return;
                }

                Console.Write("Enter meeting name (leave empty to keep current): ");
                string name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name))
                    existingMeeting.Name = name;

                

                Console.Write("Enter description (leave empty to keep current): ");
                string description = Console.ReadLine();
                if (!string.IsNullOrEmpty(description))
                    existingMeeting.Description = description;

                string category;
                while (true)
                {
                    Console.WriteLine("Select category:");
                    Console.WriteLine("1. CodeMonkey");
                    Console.WriteLine("2. Hub");
                    Console.WriteLine("3. Short");
                    Console.WriteLine("4. TeamBuilding");
                    int categorySelection = Convert.ToInt32(Console.ReadLine());

                    switch (categorySelection)
                    {
                        case 1:
                            category = "CodeMonkey";
                            break;
                        case 2:
                            category = "Hub";
                            break;
                        case 3:
                            category = "Short";
                            break;
                        case 4:
                            category = "TeamBuilding";
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            continue;
                    }
                    break;
                }

                string type;
                while (true)
                {
                    Console.WriteLine("Select type:");
                    Console.WriteLine("1. Live");
                    Console.WriteLine("2. InPerson");
                    int typeSelection = Convert.ToInt32(Console.ReadLine());

                    switch (typeSelection)
                    {
                        case 1:
                            type = "Live";
                            break;
                        case 2:
                            type = "InPerson";
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            continue;
                    }
                    break;
                }

                DateTime startDate;
                while (true)
                {
                    Console.Write("Enter start date (yyyy-MM-dd HH:mm): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid start date format. Please try again.");
                    }
                }

                DateTime endDate;
                while (true)
                {
                    Console.Write("Enter end date (yyyy-MM-dd HH:mm): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid end date format. Please try again.");
                    }
                }

                Worker workerAdmin = null;
                Admin admin = null;
                bool workerFound = false;
                string password = null;
                while (!workerFound)
                {
                    Console.WriteLine("Enter ID of meeting responsible person (leave empty to keep current): ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                    {
                        workerFound = true; // Exit the loop if the user wants to keep the current responsible person
                        admin = existingMeeting.ResponsiblePerson;
                    }
                    else if (int.TryParse(input, out int responsiblePersonId))
                    {
                        List<Worker> workers = (await _workerService.GetAllWorkersAsync()).ToList();
                        foreach (Worker worker in workers)
                        {
                            if (worker.Id == responsiblePersonId)
                            {
                                workerFound = true;
                                workerAdmin = worker;
                                break;
                            }
                        }
                        if (!workerFound)
                        {
                            Console.WriteLine($"Worker with Id {responsiblePersonId} not found. Please try again.");
                        }
                        else
                        {
                            Console.WriteLine("Enter password for responsible person");
                            password = Console.ReadLine();
                            admin = new Admin(workerAdmin.Name, workerAdmin.Surname, workerAdmin.Department, password);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Id format. Please try again.");
                    }
                }
                existingMeeting.ResponsiblePerson = admin;
                existingMeeting.Category = (MeetingCategory)Enum.Parse(typeof(MeetingCategory), category);
                existingMeeting.Type = (MeetingType)Enum.Parse(typeof(MeetingType), type);
                existingMeeting.StartDate = startDate;
                existingMeeting.EndDate = endDate;

                await _meetingService.UpdateMeetingAsync(existingMeeting, id);
                Console.WriteLine("Meeting updated successfully.");
            }
            else
            {
                Console.WriteLine("Invalid Id format.");
            }
        }


        private async Task DeleteMeeting()
        {
            Console.Write("Enter meeting Id to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Meeting existingMeeting = await _meetingService.GetMeetingByIdAsync(id);
                if (existingMeeting == null)
                {
                    Console.WriteLine($"Meeting with Id {id} not found.");
                    return;
                }
                existingMeeting.ResponsiblePerson.ToString();
                Console.WriteLine("Enter responsible person password to delete meeting...");
                string password = Console.ReadLine();
                if(existingMeeting.ResponsiblePerson.Password == password)
                {
                    await _meetingService.DeleteMeetingAsync(id);
                    Console.WriteLine($"Meeting with Id {id} deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Password incorect! Meeting was not deleted!");
                }
                
            }
            else
            {
                Console.WriteLine("Invalid meeting Id format.");
            }
        }

    }
}
