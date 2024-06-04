namespace OAT.Utilities
{
	public class Runs<T>
	{
		public delegate Task Method(T parametr);
		public delegate Task Method2(T parametr, bool IsRepeat);

		public static async Task InTasks(Method method, List<T> parametrs)
		{
			var tasks = new List<Task>();
			foreach (var parametr in parametrs)
				tasks.Add(method.Invoke(parametr));
			await Task.WhenAll(tasks.Where(t => t != null).ToArray());
		}
		public static async Task InTasks(Method2 method, List<T> parametrs, bool IsRepeat)
		{
			var tasks = new List<Task>();
			foreach (var parametr in parametrs)
				tasks.Add(method.Invoke(parametr, IsRepeat));
			await Task.WhenAll(tasks.Where(t => t != null).ToArray());
		}


		public static async Task InTask(Method method, List<T> parametrs)
		{
			foreach (var parametr in parametrs)
				await method.Invoke(parametr);
		}

	}

	public class Runs
	{
		public static async void StartModules(params Func<Task>[] modules)
		{
			bool IsError = false;
			foreach (var module in modules)
			{
				try
				{
					await module.Invoke();
				}
				catch (Exception ex)
				{
					IsError = true;
					Logger.Error($"❌ Ошибка загрузки модуля {GetMethodName(module)}. Продолжаю запуск...\nОшибка: {ex}");
				}
			}

			Logger.Info(IsError ? "⚠️ Сайт был запущен, но не все модули были загружены успешно" : "✅ Все модули сайта были успешно загружены");
		}

		private static string GetMethodName(Func<Task> module) =>
			$"{module.Method.DeclaringType!.Name}.{module.Method.Name}";

		public static void InThread(Action action) =>
			new Task(() => { action.Invoke(); }).Start();
	}
}
