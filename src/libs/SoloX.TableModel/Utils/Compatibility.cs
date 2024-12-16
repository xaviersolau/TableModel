// ----------------------------------------------------------------------
// <copyright file="Compatibility.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel.Utils
{
    internal static class ArgumentNullException
    {
        internal static void ThrowIfNull(object param, string paramName)
        {
            if (param == null)
            {
                throw new System.ArgumentNullException(paramName);
            }
        }
    }
}
