using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Data
{
	/// <summary>
	/// Clase para mapear un DataTable
	/// </summary>
	public static class DataTableExtension
	{
		public static List<T> MapDataTableToList<T>(this DataTable dt)
		{
			try
			{
				if (dt == null | dt.Rows.Count < 0)
				{
					return null;
				}

				List<T> list = new List<T>();

				foreach (DataRow dr in dt.Rows)
				{
					list.Add(MapDataRowToObject<T>(dr));
				}

				return list;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		public static T MapDataRowToObject<T>(this DataRow dr)
		{
			T instance = Activator.CreateInstance<T>();
			try
			{

				if (dr == null)
				{
					return instance;
				}

				PropertyInfo[] properties = instance.GetType().GetProperties();

				if ((properties.Length > 0))
				{
					foreach (PropertyInfo propertyObject in properties)
					{

						bool valueSet = false;

						foreach (object attributeObject in propertyObject.GetCustomAttributes(false))
						{

							if (object.ReferenceEquals(attributeObject.GetType(), typeof(MapperColumn)))
							{
								MapperColumn columnAttributeObject = (MapperColumn)attributeObject;

								if ((columnAttributeObject.ColumnName != string.Empty))
								{
									if (dr.Table.Columns.Contains(columnAttributeObject.ColumnName) && !(dr[columnAttributeObject.ColumnName] is DBNull))
									{

										propertyObject.SetValue(instance, dr[columnAttributeObject.ColumnName], null);


										valueSet = true;

									}
								}
							}
						}

						if (!valueSet)
						{
							if (dr.Table.Columns.Contains(propertyObject.Name) && !(dr[propertyObject.Name] is DBNull))
							{
								propertyObject.SetValue(instance, dr[propertyObject.Name], null);
							}

						}
					}
				}

				return instance;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	internal class MapperColumn : Attribute
	{

		private string mColumnName;

		public MapperColumn(string columnName)
		{
			mColumnName = columnName;
		}

		public string ColumnName
		{
			get { return mColumnName; }
			set { mColumnName = value; }
		}
	}
}
