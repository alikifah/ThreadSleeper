// ====================================================================================
//    Author: Al-Khafaji, Ali Kifah
//    Date:   14.12.2024
//    Description: A High percision thread sleeper class
// =====================================================================================

using System;
using System.Diagnostics;
using System.Threading;

public class ThreadSleeper
{
    private Stopwatch stopWatch = new Stopwatch();
    private float nextTrigger = 0f;
    private bool isRunning;

    // Tick time length in [ms]
    private readonly double tickLength = 1000f / Stopwatch.Frequency;

    public void Sleep(float delayMsec)
    {
        nextTrigger += delayMsec;
        double elapsed;

        while (isRunning) // sleeping loop that blocks the thread till the value of timeLeftToSleep reaches 0
        {
            elapsed = stopWatch.ElapsedTicks * tickLength;
            double timeLeftToSleep = nextTrigger - elapsed;

            if (timeLeftToSleep <= 0f)
                break;
            if (timeLeftToSleep < 1f)
                Thread.SpinWait(10);
            else if (timeLeftToSleep < 5f)
                Thread.SpinWait(100);
            else if (timeLeftToSleep < 15f)
                Thread.Sleep(1);
            else
                Thread.Sleep(10);

            // restart the stopWatch to correct potential clock drift!
            if (stopWatch.Elapsed.TotalMinutes >= 60)
            {
                stopWatch.Restart();
                nextTrigger = 0f;
            }
        }
    }
    public void Start()
    {
        nextTrigger = 0f;
        stopWatch.Start();
        isRunning = true;
    }
    public void Stop()
    {
        isRunning = false;
        stopWatch.Stop();
    }
} // end class 
