# Exemple: .\Kerberos_Delegation_ContrôlePS.ps1 svc-iisprddev01

# Piste pour ajouter des délégation sspn en powershell:
#  $AllowedToDelegateTo = $account.'msDS-AllowedToDelegateTo'
#  $AllowedToDelegateTo.add("HTTP/fobar.domaine.local")
#  Set-ADObject -Identity $accountName -Replace @{ 'msDS-AllowedToDelegateTo' = $AllowedToDelegateTo } 

param(
  [string]
  $AccountName
)

try
{
  $accountData =  Get-ADUser -Identity $accountName -prop samAccountName,msDS-AllowedToDelegateTo,servicePrincipalName,userAccountControl
}
catch {} #just ignore

if($accountData -eq $null)
{
  try
  {
    $accountData =  Get-ADComputer -Identity $accountName -prop samAccountName,msDS-AllowedToDelegateTo,servicePrincipalName,userAccountControl
  }
  catch {} #just ignore
}
if($accountData -eq $null)
{
  write-output ("No account or computer found with name '$accountName'" )
  exit
}

$account = ( Select-Object -InputObject $accountData -Property DistinguishedName,msDS-AllowedToDelegateTo,msDS-AllowedToActOnBehalfOfOtherIdentity,ObjectClass,samAccountName,servicePrincipalName, `
    @{name='DelegationStatus';expression={if($_.UserAccountControl -band 0x80000){'AllServices'}else{'SpecificServices'}}}, `
    @{name='AllowedProtocols';expression={if($_.UserAccountControl -band 0x1000000){'Any'}else{'Kerberos'}}} )

Write-Host ("Account Properties:")
Write-Host ("  samAccountName:       " + $account.samAccountName )
Write-Host ("  ObjectClass:          " + $account.ObjectClass )
Write-Host ("  DelegationStatus:     " + $account.DelegationStatus )
Write-Host ("  AllowedProtocols:     " + $account.AllowedProtocols )
Write-Host ("  DistinguishedName:    " + $account.DistinguishedName )
Write-Host ("Account Service Principal Name (SPN): ")
if($account.servicePrincipalName.Count -eq 0)
{
    Write-Host "  ** Empty **"
}
else
{
    foreach($spn in ( $account.servicePrincipalName | Sort-Object ))
    {
        Write-Host ("  " + $spn)
    }
}

Write-Host ("Allowed To Delegate To (DelegationStatus: " + $account.DelegationStatus + "):")
if($account.'msDS-AllowedToDelegateTo'.Count -eq 0)
{
    Write-Host "  ** Empty **"
}
else
{
    foreach($spn in ( $account.'msDS-AllowedToDelegateTo' | Sort-Object ))
    {
        Write-Host ("  " + $spn)
    }
}

Write-Host ("Allowed To Act On Behalf Of Other Identity:")
if($account.'msDS-AllowedToActOnBehalfOfOtherIdentity'.Count -eq 0)
{
    Write-Host "  ** Empty **"
}
else
{
    foreach($spn in ( $account.'msDS-AllowedToActOnBehalfOfOtherIdentity' | Sort-Object ))
    {
        Write-Host ("  " + $spn)
    }
}