using Quartz;
using Quartz.Impl;

namespace OMAVIAT.Services;

public class ScheduleService
{
	public static IScheduler Scheduler { get; set; } = null!;
	internal static async Task ShutdownAsync() => await Scheduler.Shutdown();

	internal static async Task RegisterAsync() => 
		Scheduler = await StdSchedulerFactory.GetDefaultScheduler(); 
}


