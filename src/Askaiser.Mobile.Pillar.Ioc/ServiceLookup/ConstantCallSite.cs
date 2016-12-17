// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// Modifications copyright (c) 2016 Anthony Simmon
using System.Linq.Expressions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class ConstantCallSite : IServiceCallSite
    {
        private readonly object _defaultValue;

        public ConstantCallSite(object defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public object Invoke(ServiceProvider provider)
        {
            return _defaultValue;
        }

        public Expression Build(Expression provider)
        {
            return Expression.Constant(_defaultValue);
        }
    }
}
