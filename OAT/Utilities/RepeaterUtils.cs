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

        public static void Try(Func<Task> action, TimeSpan timeSpan, int attempt = 2) =>
            new Task(async () => await Try(action, timeSpan, attempt, 0));

        private static async Task Try(Func<Task> action, TimeSpan timeSpan, int attempt, int count)
        {
            if (count >= attempt)
                return;
            try
            {
                await action.Invoke();
            }
            catch
            {
                await Task.Delay(timeSpan);
                await Try(action, timeSpan, attempt, count + 1);
            }
        }
    }
}
