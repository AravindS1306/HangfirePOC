namespace HangfirePOC
{
    public class HangfireTask
    {
        public async Task DoWorkAsync()
        {
            Console.WriteLine("Doing work A...");
            await Task.Delay(6000);
            Console.WriteLine("Work A completed.");
        }
    }
}
