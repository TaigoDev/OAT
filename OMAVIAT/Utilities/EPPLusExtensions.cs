using System.Reflection;
using OfficeOpenXml;
using OMAVIAT.Entities;

namespace OMAVIAT.Utilities;

public static class EPPLusExtensions
{
	public static IEnumerable<T> Fetch<T>(this ExcelWorksheet worksheet, int start = 0, int end = 300) where T : new()
	{
		static bool columnOnly(CustomAttributeData y)
		{
			return y.AttributeType == typeof(ColumnEPPlus);
		}

		var columns = typeof(T)
			.GetProperties()
			.Where(x => x.CustomAttributes.Any(columnOnly))
			.Select(p => new
			{
				Property = p,
				Column = p.GetCustomAttributes<ColumnEPPlus>().First().ColumnIndex //safe because if where above
			}).ToList();


		var rows = worksheet.Cells
			.Select(cell => cell.Start.Row)
			.Distinct()
			.OrderBy(x => x);

		var collection = rows.Where(e => e >= start && e <= end)
			.Select(row =>
			{
				var tnew = new T();
				columns.ForEach(col =>
				{
					var val = worksheet.Cells[row, col.Column];
					if (val.Value == null)
					{
						col.Property.SetValue(tnew, null);
						return;
					}

					if (col.Property.PropertyType == typeof(int))
					{
						col.Property.SetValue(tnew, val.GetValue<int>());
						return;
					}

					if (col.Property.PropertyType == typeof(double))
					{
						col.Property.SetValue(tnew, val.GetValue<double>());
						return;
					}

					col.Property.SetValue(tnew, val.GetValue<string>());
				});

				return tnew;
			});


		return collection;
	}
}