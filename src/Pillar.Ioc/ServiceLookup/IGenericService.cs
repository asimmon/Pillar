// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// Modifications copyright (c) 2016 Anthony Simmon

using System;

namespace Pillar.Ioc
{
    internal interface IGenericService
    {
        ServiceLifetime Lifetime { get; }

        IService GetService(Type closedServiceType);
    }
}
