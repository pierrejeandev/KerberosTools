# Kerberos Authentication Tester

Source: http://blog.michelbarneveld.nl/michel/archive/2009/12/05/kerberos-authentication-tester.aspx

Tool to test kerberos on web application.

# Original description from the author

Posted 12-05-2009 10:25 PM by Michel Barneveld

Quite often I am wondering if a site is using Kerberos or NTLM. You can use tools like Fiddler, Network Monitor and such for that. But sometimes I just want to have a simple tool without installation like when working on computers where you can't install such software but are allowed to run executables. For that I have created a tool: Kerberos Authentication Tester.

Kerberos Authentication Tester Features:

- It shows what authentication method is used in a web request: None, Basic, NTLM or Kerberos
- It shows the SPN used in case of Kerberos
- It shows the HTTP status
- It shows the HTTP Headers of the request.
- It shows the version of NTLM used (v1 or v2)
- It has a detailed view with a complete breakdown of the Authorization header. (Yep, went through all the RFCs to dissect the Kerberos and NTLM packages)
- It shows your current Kerberos tickets and allows you to remove them (like klist.exe)

# Improvement from the 2009 version

- Support for TLS 1.1 and 1.2
- Detailed messages on exception
