﻿// Copyright (c) 2019-2020 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using ICSharpCode.Decompiler.Metadata;

namespace Pharmacist.Core.Comparers
{
    internal class AssemblyReferenceNameComparer : IEqualityComparer<IAssemblyReference>
    {
        public static AssemblyReferenceNameComparer Default { get; } = new();

        /// <inheritdoc />
        public bool Equals(IAssemblyReference? x, IAssemblyReference? y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return StringComparer.InvariantCulture.Equals(x.FullName, y.FullName);
        }

        /// <inheritdoc />
        public int GetHashCode(IAssemblyReference obj)
        {
            return StringComparer.InvariantCulture.GetHashCode(obj.FullName);
        }
    }
}
