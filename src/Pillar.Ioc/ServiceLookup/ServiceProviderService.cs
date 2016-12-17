// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// Modifications copyright (c) 2016 Anthony Simmon

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Pillar.Ioc.Abstractions;

namespace Pillar.Ioc.ServiceLookup
{
    internal class ServiceProviderService : IService, IServiceCallSite
    {
        public IService Next { get; set; }

        public ServiceLifetime Lifetime
        {
            get { return ServiceLifetime.Transient; }
        }

        public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type> callSiteChain)
        {
            return this;
        }

        public object Invoke(ServiceProvider provider)
        {
            return provider;
        }

        public Expression Build(Expression provider)
        {
            return provider;
        }
    }
}
