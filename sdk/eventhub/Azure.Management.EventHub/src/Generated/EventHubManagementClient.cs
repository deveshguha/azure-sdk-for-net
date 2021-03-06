// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Management.EventHub;

namespace Azure.Management.EventHub
{
    /// <summary> EventHub service management client. </summary>
    public class EventHubManagementClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        private readonly string _subscriptionId;
        private readonly Uri _endpoint;

        /// <summary> Initializes a new instance of EventHubManagementClient for mocking. </summary>
        protected EventHubManagementClient()
        {
        }

        /// <summary> Initializes a new instance of EventHubManagementClient. </summary>
        /// <param name="subscriptionId"> Subscription credentials that uniquely identify a Microsoft Azure subscription. The subscription ID forms part of the URI for every service call. </param>
        /// <param name="tokenCredential"> The OAuth token for making client requests. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public EventHubManagementClient(string subscriptionId, TokenCredential tokenCredential, EventHubManagementClientOptions options = null) : this(subscriptionId, null, tokenCredential, options)
        {
        }
        /// <summary> Initializes a new instance of EventHubManagementClient. </summary>
        /// <param name="subscriptionId"> Subscription credentials that uniquely identify a Microsoft Azure subscription. The subscription ID forms part of the URI for every service call. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="tokenCredential"> The OAuth token for making client requests. </param>
        /// <param name="options"> The options for configuring the client. </param>
        /// <exception cref="ArgumentNullException"> This occurs when one of the required arguments is null. </exception>
        public EventHubManagementClient(string subscriptionId, Uri endpoint, TokenCredential tokenCredential, EventHubManagementClientOptions options = null)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            endpoint ??= new Uri("https://management.azure.com");

            options ??= new EventHubManagementClientOptions();
            _clientDiagnostics = new ClientDiagnostics(options);
            _pipeline = ManagementPipelineBuilder.Build(tokenCredential, endpoint, options);
            _subscriptionId = subscriptionId;
            _endpoint = endpoint;
        }

        /// <summary> Creates a new instance of ClustersClient. </summary>
        public virtual ClustersClient GetClustersClient()
        {
            return new ClustersClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }

        /// <summary> Creates a new instance of NamespacesClient. </summary>
        public virtual NamespacesClient GetNamespacesClient()
        {
            return new NamespacesClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }

        /// <summary> Creates a new instance of ConfigurationClient. </summary>
        public virtual ConfigurationClient GetConfigurationClient()
        {
            return new ConfigurationClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }

        /// <summary> Creates a new instance of DisasterRecoveryConfigsClient. </summary>
        public virtual DisasterRecoveryConfigsClient GetDisasterRecoveryConfigsClient()
        {
            return new DisasterRecoveryConfigsClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }

        /// <summary> Creates a new instance of EventHubsClient. </summary>
        public virtual EventHubsClient GetEventHubsClient()
        {
            return new EventHubsClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }

        /// <summary> Creates a new instance of ConsumerGroupsClient. </summary>
        public virtual ConsumerGroupsClient GetConsumerGroupsClient()
        {
            return new ConsumerGroupsClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }

        /// <summary> Creates a new instance of OperationsClient. </summary>
        public virtual OperationsClient GetOperationsClient()
        {
            return new OperationsClient(_clientDiagnostics, _pipeline, _endpoint);
        }

        /// <summary> Creates a new instance of RegionsClient. </summary>
        public virtual RegionsClient GetRegionsClient()
        {
            return new RegionsClient(_clientDiagnostics, _pipeline, _subscriptionId, _endpoint);
        }
    }
}
