using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace IbrahKit
{
    public static class Network_Utilities
    {
        public static string GetLocalIPv4()
        {
            string localIP = string.Empty;
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localIP = ip.Address.ToString();
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(localIP))
                        break;
                }
            }
            return localIP;
        }

        public static bool IsValidIPv4(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out IPAddress parsedAddress))
            {
                return parsedAddress.AddressFamily == AddressFamily.InterNetwork;
            }
            return false;
        }

        // Function to shorten IPv4 address to hexadecimal format
        public static string ShortenIPv4(string ipAddress)
        {
            string[] octets = ipAddress.Split('.');
            string shortenedIP = "";

            foreach (string octet in octets)
            {
                shortenedIP += int.Parse(octet).ToString("X2"); // Convert each octet to hexadecimal
            }

            return shortenedIP;
        }

        // Function to expand shortened IPv4 address back to dotted-decimal format
        public static string ExpandIPv4(string shortenedIPAddress)
        {
            string expandedIP = "";

            for (int i = 0; i < shortenedIPAddress.Length; i += 2)
            {
                string hexOctet = shortenedIPAddress.Substring(i, 2); // Get two characters representing an octet
                expandedIP += Convert.ToInt32(hexOctet, 16).ToString(); // Convert hexadecimal to decimal
                if (i < shortenedIPAddress.Length - 2)
                    expandedIP += "."; // Add dot between octets
            }

            return expandedIP;
        }
    }
}