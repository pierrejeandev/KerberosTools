# KerberosDelegationControl.ps1

Powershell script to check kerberos & delegation configuration on AD account and computer.

It shows:
- The account or computer delegation confiugration, 
- The list of SPN attachd to the account or computer
- The list of services SPN the account is Allowed To Delegate To
- the list of services SPN the account is Allowed To Act On Behalf Of Other Identity

Usage :
```Powershell
.\KerberosDelegationControl.ps1 <account od computer name
```

Requirements:
- Active directory powershell Module


![Screenshoot of KerberosAuthenticationTester](https://github.com/pierrejeandev/KerberosTools/raw/master/KerberosAuthenticationTester/screenshoot1.png)


