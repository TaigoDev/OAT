using Microsoft.AspNetCore.Authorization;
using static Enums;

namespace OAT.Utilities
{
    public class Permissions
    {
        /// <summary>
        /// Имеет ли пользователь право на здание
        /// </summary>
        /// <param name="RoleBuilding">Строение пользователя из бд</param>
        /// <param name="BuildingName">Строение из фронта</param>
        /// <returns>Возвращает false если у пользователя нет права на здание</returns>
        public static bool HaveBuildingByName(string RoleBuilding, string BuildingName)
        {
            var building = Enum.Parse<Building>(RoleBuilding);
            return building switch
            {
                Building.all => true,
                _ => RoleBuilding == BuildingName,
            };
        }

        /// <summary>
        /// Имеет ли пользователь право на здание
        /// </summary>
        /// <param name="RoleBuilding">Строение пользователя из бд</param>
        /// <param name="BuildingName">Строение из фронта</param>
        /// <returns>Возвращает false если у пользователя нет права на здание</returns>
        public static bool HaveBuildingByName(string RoleBuilding, Enums.Building BuildingName) =>
            HaveBuildingByName(RoleBuilding, BuildingName.ToString());


        public static bool HaveBuildingById(string RoleBuilding, string BuildingId)
        {
            var building = Enum.Parse<Building>(RoleBuilding);
            return building switch
            {
                Building.all => true,
                Building.ul_lenina_24 => BuildingId == "b1",
                Building.ul_b_khmelnickogo_281a => BuildingId == "b2",
                Building.pr_kosmicheskij_14a => BuildingId == "b3",
                Building.ul_volkhovstroya_5 => BuildingId == "b4",
                _ => false
            };
        }
    }


    
}
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] allowedRoles)
    {
        var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(Role), x));
        Roles = string.Join(",", allowedRolesAsStrings);
    }
}
public static class Enums
{

    /*
    admin - все ниже, а также управление пользователями и MySql
    reporter - новости, новости професионалитета 
    schedule_manager - расписание, изменение расписания, документы сессии
     */

    public enum Role
    {
        admin,
        reporter,
        schedule_manager,
        files_manager
    }

    public enum Building
    {
        all,
        ul_lenina_24,
        ul_b_khmelnickogo_281a,
        pr_kosmicheskij_14a,
        ul_volkhovstroya_5
    }

}