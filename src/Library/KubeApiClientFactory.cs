﻿using KubeClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Contrib.KubeClient.CustomResources
{
    public class KubeApiClientFactory
    {
        private readonly ILogger<KubeApiClientFactory> _logger;
        private readonly string _connectionString;

        public KubeApiClientFactory(ILogger<KubeApiClientFactory> logger, IOptions<KubernetesConfigurationStoreOptions> options)
        {
            _logger = logger;
            _connectionString = options.Value.ConnectionString;
        }

        public IKubeApiClient Build()
        {
            KubeClientOptions options;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                options = KubeClientOptions.FromPodServiceAccount();
                _logger.LogInformation($"Using cluster-internal kubernetes connection ({options.ApiEndPoint}).");
            }
            else
            {
                options = new KubeClientOptions(_connectionString);
                _logger.LogInformation($"Using remote kubernetes connection ({options.ApiEndPoint}).");
            }

            return KubeApiClient.Create(options);
        }
    }
}
