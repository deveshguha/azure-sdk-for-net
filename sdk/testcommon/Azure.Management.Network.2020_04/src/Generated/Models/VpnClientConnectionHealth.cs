// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.Management.Network.Models
{
    /// <summary> VpnClientConnectionHealth properties. </summary>
    public partial class VpnClientConnectionHealth
    {
        /// <summary> Initializes a new instance of VpnClientConnectionHealth. </summary>
        public VpnClientConnectionHealth()
        {
        }

        /// <summary> Initializes a new instance of VpnClientConnectionHealth. </summary>
        /// <param name="totalIngressBytesTransferred"> Total of the Ingress Bytes Transferred in this P2S Vpn connection. </param>
        /// <param name="totalEgressBytesTransferred"> Total of the Egress Bytes Transferred in this connection. </param>
        /// <param name="vpnClientConnectionsCount"> The total of p2s vpn clients connected at this time to this P2SVpnGateway. </param>
        /// <param name="allocatedIpAddresses"> List of allocated ip addresses to the connected p2s vpn clients. </param>
        internal VpnClientConnectionHealth(long? totalIngressBytesTransferred, long? totalEgressBytesTransferred, int? vpnClientConnectionsCount, IList<string> allocatedIpAddresses)
        {
            TotalIngressBytesTransferred = totalIngressBytesTransferred;
            TotalEgressBytesTransferred = totalEgressBytesTransferred;
            VpnClientConnectionsCount = vpnClientConnectionsCount;
            AllocatedIpAddresses = allocatedIpAddresses;
        }

        /// <summary> Total of the Ingress Bytes Transferred in this P2S Vpn connection. </summary>
        public long? TotalIngressBytesTransferred { get; }
        /// <summary> Total of the Egress Bytes Transferred in this connection. </summary>
        public long? TotalEgressBytesTransferred { get; }
        /// <summary> The total of p2s vpn clients connected at this time to this P2SVpnGateway. </summary>
        public int? VpnClientConnectionsCount { get; set; }
        /// <summary> List of allocated ip addresses to the connected p2s vpn clients. </summary>
        public IList<string> AllocatedIpAddresses { get; set; }
    }
}
