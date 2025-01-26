using Ganss.Excel;

namespace OMAVIAT.Entities.Models;

public class ManagementModel
{
	[Column(1)] public string FullName { get; set; }

	[Column(2)] public string StructureDivision { get; set; }

	[Column(3)] public string Telephone { get; set; }

	[Column(4)] public string Email { get; set; }
}