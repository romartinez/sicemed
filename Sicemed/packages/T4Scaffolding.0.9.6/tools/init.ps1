﻿param($rootPath, $toolsPath, $package, $project)

$dllPath = Join-Path $toolsPath T4Scaffolding.dll
$tabExpansionPath = Join-Path $toolsPath "scaffoldingTabExpansion.psm1"
$packagesRoot = [System.IO.Path]::GetDirectoryName($rootPath)

if (Test-Path $dllPath) {
	# Enable shadow copying so the package can be uninstalled/upgraded later
	[System.AppDomain]::CurrentDomain.SetShadowCopyFiles()

	# First import the module into the local scope where init.ps1 runs...
	Import-Module $dllPath
	[T4Scaffolding.NuGetServices.Services.ScaffoldingPackagePathResolver]::SetPackagesRootDirectory($packagesRoot)
	Set-Alias Scaffold Invoke-Scaffolder -Option AllScope -scope Global
	Update-FormatData -PrependPath (Join-Path $toolsPath T4Scaffolding.Format.ps1xml)

	# The following applies to NuGet 1.0 only
	if ([NuGet.PackageManager].Assembly.GetName().Version -lt 1.1) {
		# ...then promote it to global scope. Note that this is a workaround for the fact that NuGet doesn't
		# currently have a native way for packages to export PowerShell module members into the Package
		# Manager Console scope. When this is resolved in a future version of NuGet, there will be no further
		# need for GlobalCommandRunner and it will be removed. Don't call it from your own package!	
		[T4Scaffolding.Cmdlets.GlobalCommandRunner]::Run(`
			"Import-Module -Force -Global ""$dllPath"";"`
		  + "Set-Alias Scaffold Invoke-Scaffolder -Option AllScope -scope Global;"`
		  + "Import-Module -Force -Global ""$tabExpansionPath"";"`
		  + "[T4Scaffolding.NuGetServices.Services.ScaffoldingPackagePathResolver]::SetPackagesRootDirectory(""$packagesRoot"");"`
		  , $true)
	} else {
		# For NuGet 1.1 and later we can load everything immediately
		Import-Module -Force -Global $tabExpansionPath
	}
} else {
	Write-Warning ("Could not find T4Scaffolding module. Looked for: " + $dllPath)
}