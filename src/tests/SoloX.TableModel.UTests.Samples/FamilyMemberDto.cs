// ----------------------------------------------------------------------
// <copyright file="FamilyMemberDto.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.TableModel.UTests.Samples
{
    public class FamilyMemberDto
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public Guid? SomeGuid { get; set; }
    }
}
