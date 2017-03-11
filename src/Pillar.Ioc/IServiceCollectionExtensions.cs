// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// Modifications copyright (c) 2016 Anthony Simmon

using System;

namespace Pillar.Ioc
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            return new ServiceProvider(services);
        }
    }
}
