/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Apache.Ignite.Core.Impl.Compute.Closure
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Apache.Ignite.Core.Compute;

    /// <summary>
    /// Closure-based task producing multiple jobs and returning a collection of job results.
    /// </summary>
    [ComputeTaskNoResultCache]
    internal class ComputeMultiClosureTask<TA, T, TR> : ComputeAbstractClosureTask<TA, T, TR> 
        where TR : ICollection<T>
    {
        /** Result. */
        private readonly ICollection<T> _res;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="size">Expected results count.</param>
        public ComputeMultiClosureTask(int size)
        {
            _res = new List<T>(size);
        }

        /** <inheritDoc /> */
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods")]
        protected override ComputeJobResultPolicy Result0(IComputeJobResult<T> res)
        {
            _res.Add(res.Data);

            return ComputeJobResultPolicy.Wait;
        }

        /** <inheritDoc /> */
        public override TR Reduce(IList<IComputeJobResult<T>> results)
        {
            return (TR) _res;
        }
    }
}
