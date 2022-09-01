// ----------------------------------------------------------------------
// <copyright file="Family.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SoloX.TableModel.UTests.Samples
{
    public class Family
    {
        public int Id { get; set; }
        //public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Person> Members { get; set; }

        public static Family[] GetSomeFamilies()
        {
            return new[]
            {
                new Family()
                {
                    Id = 1,
                    Name = "Addams",
                    Members = new []
                    {
                        new Person()
                        {
                            Id = 1,
                            FirstName = "Gomez",
                            LastName = "Addams",
                        },
                        new Person()
                        {
                            Id = 2,
                            FirstName = "Morticia",
                            LastName = "Addams",
                        },
                        new Person()
                        {
                            Id = 3,
                            FirstName = "Pugsley",
                            LastName = "Addams",
                        },
                        new Person()
                        {
                            Id = 4,
                            FirstName = "Wednesday",
                            LastName = "Addams",
                        },
                        new Person()
                        {
                            Id = 5,
                            FirstName = "Uncle",
                            LastName = "Fester",
                        },
                        new Person()
                        {
                            Id = 6,
                            FirstName = "Grandmama",
                        },
                        new Person()
                        {
                            Id = 7,
                            FirstName = "Lurch",
                        },
                        new Person()
                        {
                            Id = 8,
                            FirstName = "Thing",
                        },
                        new Person()
                        {
                            Id = 9,
                            FirstName = "Cousin",
                            LastName = "Itt",
                        },
                    },
                },
                new Family()
                {
                    Id = 2,
                    Name = "Dolittle",
                    Members = new []
                    {
                        new Person()
                        {
                            Id = 10,
                            FirstName = "Dr",
                            LastName = "Dolittle",
                        },
                        new Person()
                        {
                            Id = 11,
                            FirstName = "Lisa",
                            LastName = "Dolittle",
                        },
                        new Person()
                        {
                            Id = 12,
                            FirstName = "Maya",
                            LastName = "Dolittle",
                        },
                        new Person()
                        {
                            Id = 13,
                            FirstName = "Charisse",
                            LastName = "Dolittle",
                            SomeGuid = new Guid("f45132ed-e1cf-4ddf-b8f9-62e660d2b4cb")
                        },
                    },
                },
            };
        }
    }
}
