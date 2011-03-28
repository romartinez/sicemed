[T4Scaffolding.Scaffolder(Description = "Creates an Ajax Grid fro CRUD Operations")][CmdletBinding()]
param(        
	[Parameter(Mandatory=$true, ValueFromPipelineByPropertyName=$true)]$ModelType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$inflector = new-object ConfOrm.Shop.Inflectors.SpanishInflector

$controllerName = $inflector.Pluralize($ModelType)

Scaffold MvcScaffolding.Controller ($controllerName+"Controller") -Project $Poject `
	-Template GridController -CodeLanguage $CodeLanguage -OverrideTemplateFolders $TemplateFolders `
	-NoChildItems -Force:$Force

Scaffold MvcScaffolding.RazorView $controllerName Index -ModelType $ModelType `
	-Template GridView  -Project $Project -CodeLanguage $CodeLanguage `
	-OverrideTemplateFolders $TemplateFolders -Force:$Force