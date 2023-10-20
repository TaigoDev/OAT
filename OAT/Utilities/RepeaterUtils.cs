namespace OAT.Utilities
{
    public class RepeaterUtils
    {
        public static void Repeat(Func<Task> method, int time) =>
            new Task(async () =>
            {
                while (true)
                {
                    try
                    {
                        await method.Invoke();
                        await Task.Delay(time);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.ToString());
                    }
                }
            }).Start();

        public static Task RepeatAsync(Func<Task> repeat, int minutes)
        {
            new Thread(async () =>
            {
                try
                {
                    await repeat.Invoke();
                    await Task.Delay(minutes * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }).Start();
            return Task.CompletedTask;
        }

        public static async Task RepeatOnce(Func<Task> repeat, int count)
        {
            try
            {
                for (int i = 0; i < count; i++)
                    await repeat.Invoke();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

        }
    }
}
