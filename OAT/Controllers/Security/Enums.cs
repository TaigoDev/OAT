public static class Enums
{

	/*
	admin - все ниже, а также управление пользователями и MySql
	reporter - новости, новости професионалитета 
	schedule_manager - расписание, изменение расписания, документы сессии
	 */
	public static readonly int campus_count = 4;
	public enum Role
	{
		www_admin,
		www_reporter_news,
		www_reporter_prof_news,
		www_reporter_demoexams,

		www_manager_schedule_ALL,
		www_manager_schedule_campus_1,
		www_manager_schedule_campus_2,
		www_manager_schedule_campus_3,
		www_manager_schedule_campus_4,

		www_manager_changes_ALL,
		www_manager_changes_campus_1,
		www_manager_changes_campus_2,
		www_manager_changes_campus_3,
		www_manager_changes_campus_4,

		www_manager_files_sessions_ALL,
		www_manager_files_sessions_campus_1,
		www_manager_files_sessions_campus_2,
		www_manager_files_sessions_campus_3,
		www_manager_files_sessions_campus_4,

		www_manager_files_practice_ALL,
		www_manager_files_practice_campus_1,
		www_manager_files_practice_campus_2,
		www_manager_files_practice_campus_3,
		www_manager_files_practice_campus_4,

		www_techrem_owner,
		www_POIT_owner,
		www_KTZS_owner,
		www_TehREM_owner,
		www_TehSvar_owner,
		www_TehARS_owner,
		www_EKUP_owner,
		www_Tehmash_owner,
		www_TehREO_owner,
		www_SGD_owner,
		www_GD_owner,
		www_END_owner,
		www_TehOPit_owner,
		www_FKD_owner,
		www_informatiki_owner,
		www_inostYaz_owner,
		www_matematiki_owner,
		www_russYaz_owner,
	}
	public static List<string> ConvertToString(this List<Role> roles)
	{
		var _roles = new List<string>();
		foreach (var role in roles)
			_roles.Add(role.ToString());
		return _roles;
	}
	public enum Building
	{
		all,
		ul_lenina_24,
		ul_b_khmelnickogo_281a,
		pr_kosmicheskij_14a,
		ul_volkhovstroya_5
	}

	public enum AuthResult
	{
		success,
		token_expired,
		fail
	}
}