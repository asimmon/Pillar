// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// 
// Modifications copyright (c) 2016 Anthony Simmon
using System;

namespace Askaiser.Mobile.Pillar.Ioc.Abstractions
{
    /// <summary>
    /// The <see cref="System.IDisposable.Dispose"/> method ends the scope lifetime. Once Dispose
    /// is called, any scoped services that have been resolved from
    /// <see cref="ServiceProvider"/> will be
    /// disposed.
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// The <see cref="System.IServiceProvider"/> used to resolve dependencies from the scope.
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}
