using System;
using System.Threading;

namespace ProcessSimulator;

internal class Program
{
    // 1. Delegat definieren
    public delegate void ProgressReporter(string stepName, int percent);

    private static void Main()
    {
        Console.CursorVisible = false;
        Console.WriteLine("=== Process Simulator ===");
        Console.WriteLine();

        string[] steps =
        {
            "Downloading data",
            "Validating input",
            "Processing records",
            "Generating report",
            "Publishing results",
            "Cleaning up"
        };

        // 4. Delegat mit konkreter UI-Methode verdrahten
        ProgressReporter reporter = DisplayProgress;

        foreach (string step in steps)
        {
            // Startmeldung (UI) bleibt in Main – nicht in der Prozesslogik
            Console.WriteLine($"Starting: {step}");

            // 3. Prozesssimulation über delegatgestützte Methode
            SimulateStep(step, reporter);

            // Abschlussmeldung (UI)
            Console.WriteLine($"Completed: {step}");
            Console.WriteLine();
        }

        Console.WriteLine("All process steps completed.");
        Console.CursorVisible = true;
    }

    // 3. Kernlogik ohne Console-Aufrufe – meldet Fortschritt nur über den Delegaten
    private static void SimulateStep(string stepName, ProgressReporter reporter)
    {
        for (int percent = 0; percent <= 100; percent += 5)
        {
            reporter(stepName, percent);   // Übergabe an die UI-Schicht
            Thread.Sleep(80);
        }
    }

    // 2. Konkrete Fortschrittsbalken-Ausgabe, entspricht der Delegat-Signatur
    private static void DisplayProgress(string stepName, int percent)
    {
        const int width = 30;
        const char filledChar = '█';
        const char emptyChar = '░';
        const char barStartChar = '⟦';
        const char barEndChar = '⟧';

        int filled = percent * width / 100;
        string bar = new string(filledChar, filled) + new string(emptyChar, width - filled);

        Console.Write($"\r{stepName,-22} {barStartChar}{bar}{barEndChar} {percent,3}%");

        if (percent == 100)
        {
            Console.WriteLine();
        }

        // Warnung bei 50 % – jetzt Teil der Darstellung, nicht der Prozesslogik
        if (percent == 50)
        {
            Console.WriteLine();                                  // Zeilenumbruch nach Balken
            Console.WriteLine($"  Warning: {stepName} is only halfway done.");
        }
    }
}