// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// Modifications copyright (c) 2016 Anthony Simmon
using System.Collections.Generic;

namespace Askaiser.Mobile.Pillar.Ioc.Abstractions
{
    /// <summary>
    /// Specifies the contract for a collection of service descriptors.
    /// </summary>
    public interface IServiceCollection : IList<ServiceDescriptor>
    { }
}
