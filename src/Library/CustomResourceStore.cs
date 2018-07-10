﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contrib.KubeClient.CustomResources
{
    public class CustomResourceStore<TResource> : ICustomResourceStore<TResource>
    {
        private readonly IEnumerable<CustomResource<TResource>> _resources;

        public CustomResourceStore(ICustomResourceWatcher<TResource> watcher)
        {
            _resources = watcher.RawResources;
            watcher.StartWatching();
        }

        public Task<IEnumerable<CustomResource<TResource>>> FindAsync(Func<CustomResource<TResource>, bool> query)
            => Task.FromResult(_resources.Where(query));

        public Task<IEnumerable<CustomResource<TResource>>> FindByNamespaceAsync(string @namespace)
            => FindAsync(res => res.Metadata.Namespace.Equals(@namespace, StringComparison.InvariantCultureIgnoreCase));

        public Task<CustomResource<TResource>> FindByNameAsync(string name)
            => Task.FromResult(_resources.First(res => res.Metadata.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));

        public Task<long> Count() => Task.FromResult(_resources.LongCount());
    }
}
