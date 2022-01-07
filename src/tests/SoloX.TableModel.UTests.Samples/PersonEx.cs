// ----------------------------------------------------------------------
// <copyright file="PersonEx.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Impl;
using System;
using System.Globalization;

namespace SoloX.TableModel.UTests.Samples
{
#pragma warning disable CA1711 // Les identificateurs ne doivent pas avoir un suffixe incorrect
#pragma warning disable CA1024 // Utiliser des propriétés quand cela est approprié
    public static class PersonEx
    {
        public static ITableStructure<Person, int> GetTableStructure()
        {
            return new TableStructure<Person, int>(
                "TableStructureId",
                new Column<Person, int>(nameof(Person.Id), p => p.Id, canSort: false),
                new Column<Person, string>(nameof(Person.FirstName), p => p.FirstName),
                new Column<Person, string>(nameof(Person.LastName), p => p.LastName),
                new Column<Person, string>(nameof(Person.Email), p => p.Email),
                new Column<Person, DateTime>(nameof(Person.BirthDate), p => p.BirthDate)
                );
        }

        public static ITableDecorator<Person, string> GetTableDecorator(ITableStructure<Person, int> tableStructure)
        {
            var decorator = new TableDecorator<Person, string>("TableDecoratorId", tableStructure);

            decorator.RegisterDefault(v => v.ToString(), c => c.Id);
            decorator.TryRegister<string>(nameof(Person.LastName), n => n.ToUpper(CultureInfo.InvariantCulture), () => "LastName");
            decorator.TryRegister<DateTime>(nameof(Person.BirthDate), date => date.ToString("D", CultureInfo.InvariantCulture), () => "BirthDate");

            return decorator;
        }
    }
#pragma warning restore CA1024 // Utiliser des propriétés quand cela est approprié
#pragma warning restore CA1711 // Les identificateurs ne doivent pas avoir un suffixe incorrect
}
