// ----------------------------------------------------------------------
// <copyright file="Person.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.TableModel.UTests.Samples
{
    public class Person
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }

        public string? Email { get; set; }

        public DateTime BirthDate { get; set; }

        public Family Family { get; set; }

        public Guid? SomeGuid { get; set; }

        public static Person[] GetSomePersons()
        {
            return new Person[]
            {
                new Person()
                {
                    FirstName = "ANAKIN",
                    LastName = "SKYWALKER",
                    BirthDate = new DateTime(3054, 5, 24),
                },
                new Person()
                {
                    FirstName = "PADME",
                    LastName = "AMIDALA",
                    BirthDate = new DateTime(3051, 4, 12),
                },
                new Person()
                {
                    FirstName = "QI-GON",
                    LastName = "JINN",
                    BirthDate = new DateTime(2954, 1, 4),
                },
                new Person()
                {
                    FirstName = "JAR JAR",
                    LastName = "BINKS",
                    BirthDate = new DateTime(2989, 12, 25),
                },
                new Person()
                {
                    FirstName = "OBI-WAN",
                    LastName = "KENOBI",
                    BirthDate = new DateTime(3067, 10, 24),
                },
                new Person()
                {
                    FirstName = "YODA",
                    LastName = "",
                    BirthDate = new DateTime(2101, 2, 6),
                },
                new Person()
                {
                    FirstName = "DARK",
                    LastName = "MAUL",
                    BirthDate = new DateTime(2814, 1, 31),
                },
            };
        }

#pragma warning disable CA1024 // Utiliser des propriétés quand cela est approprié
        public static Person GetJohnDoe()
#pragma warning restore CA1024 // Utiliser des propriétés quand cela est approprié
        {
            return new Person()
            {
                Id = 1,
                BirthDate = new DateTime(2001, 10, 28),
                FirstName = "John",
                LastName = "Doe"
            };
        }
    }
}
