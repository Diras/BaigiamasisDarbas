using BaigiamasisDarbas.Repositories;
using BaigiamasisDarbas.Services;
using BaigiamasisDarbas.Contracts;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;
using System;

namespace BaigiamasisDarbas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information() // Keičiame minimalų žinučių lygį į Information
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            string mongoConnectionString = "mongodb+srv://Diras:rgd7840Z@learning.0insxlx.mongodb.net/?retryWrites=true&w=majority&appName=Learning";

            // MongoDB Client setu
            var mongoClient = new MongoClient(mongoConnectionString);
            var cacheMeetingRepository = new CacheMeetingRepository(mongoClient);
            var cacheWorkerRepository = new CacheWorkerRepository(mongoClient);

            // Repositories setup (assuming WorkerRepository and MeetingRepository exist)
            var workerRepository = new WorkerRepository();
            var meetingRepository = new MeetingRepository();
           
            // Services setup (assuming WorkerService and MeetingService exist)
            var workerService = new WorkerService(workerRepository,cacheWorkerRepository);  // Inject MongoDBRepository for caching
            var meetingService = new MeetingService(meetingRepository, cacheMeetingRepository);  // Inject MongoDBRepository for caching

            // Console UI setup
            var consoleUI = new MeetingConsoleUI(workerService, meetingService);

            // Run the application
            consoleUI.Run();
        }
    }
}