using SoloX.TableModel.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.UTests.Samples
{
    public static class PersonEx
    {
        public static ITableStructure<Person, int> GetTableStructure()
        {
            return new TableStructure<Person, int>(
                "TableStructureId",
                new Column<Person, int>(nameof(Person.Id), p => p.Id, false),
                new Column<Person, string>(nameof(Person.FirstName), p => p.FirstName),
                new Column<Person, string>(nameof(Person.LastName), p => p.LastName),
                new Column<Person, string>(nameof(Person.Email), p => p.Email),
                new Column<Person, DateTime>(nameof(Person.BirthDate), p => p.BirthDate)
                );
        }

        public static ITableDecorator<Person, string> GetTableDecorator(ITableStructure<Person, int> tableStructure)
        {
            var decorator = new TableDecorator<Person, string>("TableDecoratorId", tableStructure);

            decorator.RegisterDefault(v => v.ToString());
            decorator.Register<string>(nameof(Person.LastName), n => n.ToUpper());
            decorator.Register<DateTime>(nameof(Person.BirthDate), date => date.ToString("D", CultureInfo.InvariantCulture));

            return decorator;
        }
    }
}
