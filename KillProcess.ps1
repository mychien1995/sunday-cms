Function KillProcess
{
	Write-Host 'abc'
	$processName = 'Sunday.CMS.Interface'
	$processes = Get-Process $processName -ErrorAction SilentlyContinue
	if($processes)
	{
		Stop-Process -Name $processName -Force
	}
}

KillProcess