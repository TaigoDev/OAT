using CsvHelper.Configuration.Attributes;

namespace OMAVIAT.Entities;

public class Contract
{
	[Name("NomKontrakt")] public string documentId { get; set; }

	[Name("DataKontrakt")] public string documentDate { get; set; }

	[Name("FullName")] public string studentFullName { get; set; }

	[Name("Gruppa")] public string Group { get; set; }

	[Name("Zakazchik")] public string FullName { get; set; }
}