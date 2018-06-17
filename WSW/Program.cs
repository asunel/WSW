namespace WSW
{
    using System;
    using System.ServiceProcess;

    internal class Program : WswService
    {
        private static void Main(string[] args)
        {
            var service = new Program();

            if (Environment.UserInteractive || args.Length > 0)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($@"Starting {service.ServiceName}");

                service.OnStart(args);
                Console.WriteLine($@"{service.ServiceName} has started.");

                Console.WriteLine(@"Press [ENTER] to call stop and exit.");
                Console.ReadLine();
                service.OnStop();
                Console.ResetColor();
                return;
            }

            var servicesToRun = new ServiceBase[]
            {
                    new WswService()
            };

            Run(servicesToRun);
        }
    }
}
