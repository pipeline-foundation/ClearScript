// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Runtime.CompilerServices;

namespace Microsoft.ClearScript.Util
{
    internal class CollateralObject<THolder, TValue>
        where THolder : class
        where TValue : class
    {
        private readonly ConditionalWeakTable<THolder, TValue> table = new();

        public TValue Get(THolder holder)
        {
            return table.TryGetValue(holder, out var value) ? value : null;
        }

        public TValue GetOrCreate(THolder holder)
        {
            return table.GetOrCreateValue(holder);
        }

        public virtual void Set(THolder holder, TValue value)
        {
            Clear(holder);
            if (value is not null)
            {
                table.Add(holder, value);
            }
        }

        public void Clear(THolder holder)
        {
            table.Remove(holder);
        }
    }

    internal sealed class CollateralArray<THolder, TElement> : CollateralObject<THolder, TElement[]> where THolder : class
    {
        public override void Set(THolder holder, TElement[] value)
        {
            if (value is null)
            {
                Clear(holder);
            }
            else if (value.Length > 0)
            {
                base.Set(holder, value);
            }
            else
            {
                base.Set(holder, ArrayHelpers.GetEmptyArray<TElement>());
            }
        }
    }
}
