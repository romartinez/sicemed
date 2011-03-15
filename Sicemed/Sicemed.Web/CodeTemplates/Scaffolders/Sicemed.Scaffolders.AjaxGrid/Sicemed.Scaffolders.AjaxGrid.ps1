[T4Scaffolding.Scaffolder(Description = "Creates an Ajax Grid fro CRUD Operations")][CmdletBinding()]
param(        
	[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true)]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Scaffold MvcScaffolding.Controller $ModelType -Project $Poject `
	-CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders `
	-NoChildItems -Force:$Force

$controllerName = Get-PluralizedWord $ModelType
Scaffold MvcScaffolding.RazorView $controllerName Index -ModelType $ModelType `
	-Template jQGridView -Project $Project -CodeLanguage $CodeLanguage `
	-OverrideTemplateFolders $TemplateFolders -Force:$Force