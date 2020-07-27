using System;
using System.Net;

namespace CheckTlsSupport
{
    class Program
    {
        static void Main(string[] args)
        {
            // print initial status
            Console.WriteLine("Runtime: " + System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(int).Assembly.Location).ProductVersion);
            Console.WriteLine("Enabled protocols:   " + ServicePointManager.SecurityProtocol);
            Console.WriteLine("Available protocols: ");
            Boolean platformSupportsTls12 = false;
            foreach (SecurityProtocolType protocol in Enum.GetValues(typeof(SecurityProtocolType)))
            {
                Console.WriteLine(String.Concat("  ", protocol.ToString(), " (", protocol.GetHashCode(), ")"));
                if (protocol.GetHashCode() == 3072)
                {
                    platformSupportsTls12 = true;
                }
            }
            //Console.WriteLine("Is Tls12 enabled: " + ServicePointManager.SecurityProtocol.HasFlag((SecurityProtocolType)3072));


            //// enable Tls12, if possible
            //if (!ServicePointManager.SecurityProtocol.HasFlag((SecurityProtocolType)3072))
            //{
            //    if (platformSupportsTls12)
            //    {
            //        Console.WriteLine("Platform supports Tls12, but it is not enabled. Enabling it now.");
            //        ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            //    }
            //    else
            //    {
            //        Console.WriteLine("Platform does not supports Tls12.");
            //    }
            //}

            //// disable ssl3
            //if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Ssl3))
            //{
            //    Console.WriteLine("Ssl3SSL3 is enabled. Disabling it now.");
            //    // disable SSL3. Has no negative impact if SSL3 is already disabled. The enclosing "if" if just for illustration.
            //    System.Net.ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            //}
            //Console.WriteLine("Enabled protocols:   " + ServicePointManager.SecurityProtocol);

            Console.WriteLine("Press enter to Quit.");
            Console.ReadLine();

        }
    }
}
