using System;
using System.Threading;

namespace ConsoleStopwatch
{
    // Delegate for stopwatch events
    public delegate void StopwatchEventHandler(string message);

    public class Stopwatch
    {
        private int _timeElapsedSeconds; // Time in seconds
        private int _timeElapsedMilliseconds; // Time in milliseconds
        private bool _isRunning;
        private Timer _timer;

        // Events
        public event StopwatchEventHandler OnStarted;
        public event StopwatchEventHandler OnStopped;
        public event StopwatchEventHandler OnReset;

        public Stopwatch()
        {
            _timeElapsedSeconds = 0;
            _timeElapsedMilliseconds = 0;
            _isRunning = false;
        }

        // Method to start the stopwatch
        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _timer = new Timer(Tick, null, 0, 10); // Tick every 10 milliseconds
                OnStarted?.Invoke("Stopwatch Started!");
            }
            else
            {
                Console.WriteLine("Stopwatch is already running!");
            }
        }

        // Method to stop the stopwatch
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _timer?.Dispose();
                OnStopped?.Invoke("Stopwatch Stopped!");
            }
            else
            {
                Console.WriteLine("Stopwatch is not running!");
            }
        }

        // Method to reset the stopwatch
        public void Reset()
        {
            _isRunning = false;
            _timer?.Dispose();
            _timeElapsedSeconds = 0;
            _timeElapsedMilliseconds = 0;
            OnReset?.Invoke("Stopwatch Reset!");
        }

        // Tick method to increment time
        private void Tick(object state)
        {
            _timeElapsedMilliseconds += 10;

            if (_timeElapsedMilliseconds >= 1000)
            {
                _timeElapsedMilliseconds = 0;
                _timeElapsedSeconds++;
            }
        }

        // Get the formatted elapsed time
        public string GetFormattedTime()
        {
            int minutes = _timeElapsedSeconds / 60;
            int seconds = _timeElapsedSeconds % 60;
            return $"{minutes}:{seconds:D2}:{_timeElapsedMilliseconds / 10:D2}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            // Subscribe to events
            stopwatch.OnStarted += HandleStopwatchEvent;
            stopwatch.OnStopped += HandleStopwatchEvent;
            stopwatch.OnReset += HandleStopwatchEvent;

            bool exit = false;

            Console.WriteLine("Stopwatch Application");
            Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit");

            while (!exit)
            {
                if (stopwatch.GetFormattedTime() != "0:00:00")
                {
                    Console.Write($"\rTime Elapsed: {stopwatch.GetFormattedTime()}");
                }

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    switch (key)
                    {
                        case ConsoleKey.S:
                            stopwatch.Start();
                            break;
                        case ConsoleKey.T:
                            stopwatch.Stop();
                            break;
                        case ConsoleKey.R:
                            stopwatch.Reset();
                            break;
                        case ConsoleKey.Q:
                            exit = true;
                            stopwatch.Stop();
                            break;
                        default:
                            // Display invalid input message and prompt user to enter a valid option
                            Console.WriteLine("\nInvalid input! Please enter S, T, R, or Q.");
                            Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit");
                            break;
                    }
                }

                Thread.Sleep(10); // Small delay to allow the timer to increment
            }

            Console.WriteLine("\nExiting Stopwatch...");
        }

        // Event handler method
        static void HandleStopwatchEvent(string message)
        {
            Console.WriteLine($"\n{message}");
        }
    }
}